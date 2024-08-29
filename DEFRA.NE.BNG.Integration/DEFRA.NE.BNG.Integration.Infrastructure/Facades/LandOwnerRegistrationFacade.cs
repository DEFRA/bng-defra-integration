using System.Text;
using DEFRA.NE.BNG.Integration.Domain.Constants;
using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Domain.Request;
using DEFRA.NE.BNG.Integration.Infrastructure.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace DEFRA.NE.BNG.Integration.Infrastructure.Facades
{
    public class LandOwnerRegistrationFacade : FacadeBase, ILandOwnerRegistrationFacade
    {
        private readonly ILogger<LandOwnerRegistrationFacade> logger;
        private readonly IMappingManager mappingManager;

        public LandOwnerRegistrationFacade(ILogger<LandOwnerRegistrationFacade> logger, IConfigurationReader environmentVariableReader, IMailServiceAgent mailService, IDataverseService dataVerseService, IMappingManager mappingManager) : base(environmentVariableReader, mailService, dataVerseService)
        {
            this.logger = logger;
            this.mappingManager = mappingManager;
        }

        public async Task<Guid> OrchestrationBNG(GainSiteRegistration gainSiteRegistration, bng_casetype caseType)
        {
            Guid applicant = await GenerateApplicant(gainSiteRegistration);

            var agentReference = new EntityReference(Contact.EntityLogicalName, applicant);

            EntityReference clientReference = null;

            if (gainSiteRegistration.Applicant.Role == "agent")
            {
                clientReference = await dataverseService.UpsertClient(gainSiteRegistration.Agent);
            }

            var caseQuery = DataverseExtensions.GetQuery<bng_Case>();
            caseQuery.AddEqualOperatorCondition<bng_Case>(nameof(bng_Case.bng_CaseReference), gainSiteRegistration.GainSiteReference);

            var caseId = await dataverseService.RetrieveFirstRecordForEntity(caseQuery);

            if (caseId != Guid.Empty)
            {
                gainSiteRegistration.GainSiteReference = null;
            }

            Entity organisationEntity = await GenrateOrganisation(gainSiteRegistration);

            var gainSiteId = await CreateGainSiteRegistration(gainSiteRegistration, agentReference, clientReference, organisationEntity, caseType);

            if (gainSiteId != Guid.Empty)
            {
                if (gainSiteRegistration.Files != null)
                {
                    await dataverseService.CreateAttachments(gainSiteRegistration.Files, gainSiteId.GetEntityReference<bng_GainSiteRegistration>());
                }

                if (gainSiteRegistration.LegalAgreementParties != null)
                {
                    await CreateLegalAgreementParties(gainSiteRegistration.LegalAgreementParties, gainSiteId);
                }

                await ManageResponsibleBodies(gainSiteRegistration, gainSiteId);
                await AssociateGainSiteToCustomer(gainSiteRegistration, applicant, clientReference, organisationEntity, gainSiteId);

                if (gainSiteRegistration.LandOwners != null)
                {
                    await CreateLandOwners(gainSiteRegistration.LandOwners, gainSiteId);
                }

                await ProcessHabitats(gainSiteRegistration, gainSiteId);

                var updateEntity = new bng_GainSiteRegistration
                {
                    Id = gainSiteId,
                    bng_ReadyForProcessingOn = DateTime.Now
                };

                await dataverseService.UpdateAsync(updateEntity);
            }

            return gainSiteId;
        }

        /// <summary>
        /// Creates and update gain site registration
        /// </summary>
        /// <param name="gainSiteRegistration"></param>
        /// <returns></returns>
        public async Task<Guid> CreateGainSiteRegistration(GainSiteRegistration gainSiteRegistration, EntityReference applicantId, EntityReference clientReference, Entity organisationEntity, bng_casetype caseType)
        {
            var entity = new bng_GainSiteRegistration
            {
                bng_CaseName = gainSiteRegistration.GainSiteReference,
                bng_AreaInHectares = gainSiteRegistration.LandBoundaryHectares,
                bng_LandBoundaryGridReference = gainSiteRegistration.LandBoundaryGridReference,
                OwnerId = Guid.Parse(EnvironmentConstants.OwnerIdGuid).GetEntityReference<Team>(),
                bng_source = bng_source.Online,
                bng_CaseType = caseType,
            };

            if (!string.IsNullOrEmpty(gainSiteRegistration.EnhancementWorkStartDate))
            {
                entity.bng_HabitatEnhancementWorksStartDate = DateTime.Parse(gainSiteRegistration.EnhancementWorkStartDate, EnvironmentConstants.DefaultCultureInfo);
            }

            if (!string.IsNullOrEmpty(gainSiteRegistration.LegalAgreementStartDate))
            {
                entity.bng_LegalAgreementStartDate = DateTime.Parse(gainSiteRegistration.LegalAgreementStartDate, EnvironmentConstants.DefaultCultureInfo);
            }

            if (!string.IsNullOrEmpty(gainSiteRegistration.LegalAgreementEndDate))
            {
                entity.bng_HabitatEnhancementWorksEndDate = DateTime.Parse(gainSiteRegistration.LegalAgreementEndDate, EnvironmentConstants.DefaultCultureInfo);
                entity.bng_HabitatEnhancementWorksEndDateStatus = bng_gainsiteregistration_bng_habitatenhancementworksenddatestatus.EndDateProvided;
            }
            else
            {
                entity.bng_HabitatEnhancementWorksEndDateStatus = bng_gainsiteregistration_bng_habitatenhancementworksenddatestatus.EndDateinPerpetuity;
            }

            if (!string.IsNullOrEmpty(gainSiteRegistration.ManagementMonitoringStartDate))
            {
                entity.bng_YearManagementMonitoringPeriodStartDate = DateTime.Parse(gainSiteRegistration.ManagementMonitoringStartDate, EnvironmentConstants.DefaultCultureInfo);
            }

            if (!string.IsNullOrEmpty(gainSiteRegistration.LegalAgreementType))
            {
                entity.bng_LegalAgreementType =
                    (bng_legalagreementtype)Enum.ToObject(typeof(bng_legalagreementtype), int.Parse(gainSiteRegistration.LegalAgreementType));
            }

            if (!string.IsNullOrEmpty(gainSiteRegistration.SubmittedOn))
            {
                entity[DataverseExtensions.AttributeLogicalName<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.CreatedOn))
                    ] = DateTime.Parse(gainSiteRegistration.SubmittedOn, EnvironmentConstants.DefaultCultureInfo);
            }

            if (!string.IsNullOrEmpty(gainSiteRegistration.HabitatPlanIncludedLegalAgreementYesNo))
            {
                entity.bng_HabitatPlanIncludedLegalAgreementYesNo = gainSiteRegistration.HabitatPlanIncludedLegalAgreementYesNo == "Yes";
            }

            var jsonForCaseCreation = GenerateJsonForCaseCreation(gainSiteRegistration, clientReference, organisationEntity);

            if (!string.IsNullOrWhiteSpace(jsonForCaseCreation))
            {
                entity.bng_JSONForCaseCreation = jsonForCaseCreation;
            }

            if (applicantId != null)
            {
                entity.bng_ApplicantContactID = applicantId;
            }

            entity.bng_LandownerID = applicantId;

            if (!string.IsNullOrWhiteSpace(gainSiteRegistration.Payment?.Method))
            {
                entity.bng_PaymentMethod = mappingManager.MapPaymentMethod(gainSiteRegistration.Payment?.Method);
            }

            var creationId = await dataverseService.CreateAsync(entity);
            logger.LogInformation("GainSiteRegistration Created Id: {id}", creationId);

            return creationId;
        }

        /// <summary>
        /// Create Legal agreement Party
        /// </summary>
        /// <param name="request"></param>
        /// <param name="gainSiteRegistrationId"></param>
        /// <returns></returns>
        public async Task<Guid> CreateLegalAgreementParties(List<LegalAgreementParty> legalAgreementParties, Guid gainSiteRegistrationId)
        {
            Guid creationId = Guid.Empty;
            try
            {
                var tasks = legalAgreementParties.Select(async legalAgreementParty =>
                {
                    var entity = new bng_LegalAgreementParty
                    {
                        bng_FullNameOrOrganisation = legalAgreementParty.Name,
                        bng_Role = legalAgreementParty.Role,
                        bng_GainSiteRegistrationID = gainSiteRegistrationId.GetEntityReference<bng_GainSiteRegistration>()
                    };
                    creationId = await dataverseService.CreateAsync(entity);
                    logger.LogInformation("Legal Agreement Created Id: {id}", creationId);
                });
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error {message}", ex.Message);
            }
            return creationId;
        }

        public async Task<Guid> CreateOtherLandOwners(List<OtherLandOwner> otherLandOwners, Guid gainSiteRegistrationId)
        {
            Guid creationId = Guid.Empty;

            var tasks = otherLandOwners.Select(async otherLandOwner =>
                {
                    var entity = new bng_GainSitePropertyLandowners
                    {
                        bng_FullName = otherLandOwner.Name,
                        bng_GainSiteRegistrationID = gainSiteRegistrationId.GetEntityReference<bng_GainSiteRegistration>()
                    };
                    creationId = await dataverseService.CreateAsync(entity);
                    logger.LogInformation("Other Landowner Created Id: {id}", creationId);
                });
            await Task.WhenAll(tasks);

            return creationId;
        }

        public async Task CreateLandOwners(LandOwners landOwners, Guid gainSiteRegistrationId)
        {
            try
            {
                if (landOwners?.Individual?.Count > 0)
                {
                    var individualTasks = landOwners.Individual.Select(async contact => await UpsertIndividual(contact));
                    var guidList = await Task.WhenAll(individualTasks);

                    var entityList = guidList.Where(x => x != Guid.Empty).Select(x => new EntityReference(Contact.EntityLogicalName, x));

                    await dataverseService.AssosiateTwoEntitiesRecords(gainSiteRegistrationId.GetEntityReference<bng_GainSiteRegistration>(),
                          DataverseExtensions.GetRelationshipSchemaName<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.bng_GainSiteRegistration_Contact_Contact))
                          , entityList.ToList());
                }

                if (landOwners?.Organisation?.Count > 0)
                {
                    var organisationTasks = landOwners.Organisation.Select(async organisation => await UpsertOrganisation(organisation));
                    var guidList = await Task.WhenAll(organisationTasks);
                    var entityList = guidList.Where(x => x != Guid.Empty).Select(x => new EntityReference(Contact.EntityLogicalName, x));

                    await dataverseService.AssosiateTwoEntitiesRecords(gainSiteRegistrationId.GetEntityReference<bng_GainSiteRegistration>(),
                        DataverseExtensions.GetRelationshipSchemaName<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.bng_GainSiteRegistration_Account_Account))
                        , entityList.ToList());
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{error}", ex.Message);
            }
        }

        public async Task<IDictionary<string, Guid>> CreateBaselineHabitats(Habitats habitats, Guid gainSiteRegistrationId)
        {
            var refranceGuidList = new Dictionary<string, Guid>();

            try
            {
                if (habitats != null && habitats.Baseline != null && habitats.Baseline.Count > 0)
                {
                    foreach (var item in habitats.Baseline)
                    {
                        var entityId = await CreateBaselineHabitat(item, gainSiteRegistrationId);

                        var key = $"{item.BaselineReference}{item.State}";
                        refranceGuidList.TryAdd(key, entityId);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{error}", ex.Message);
            }
            return refranceGuidList;
        }

        public async Task<Guid> CreateBaselineHabitat(BaselineHabitat baselineHabitat, Guid gainSiteRegistrationId)
        {
            var entity = new bng_BaselineHabitat
            {
                bng_BaselineReference = Convert.ToInt32(baselineHabitat.BaselineReference),

                bng_BaselineAreaLength = baselineHabitat.Area.BeforeEnhancement
            };

            if (!string.IsNullOrWhiteSpace(baselineHabitat.Condition))
            {
                entity.bng_BaselineCondition = mappingManager.MapHabitatConditionDic(baselineHabitat.Condition);
            }

            var habitatSubTypeId = await UpsertHabitatSubType(baselineHabitat.HabitatType);

            if (habitatSubTypeId != Guid.Empty)
            {
                entity.bng_BaselineHabitatTypeID = habitatSubTypeId.GetEntityReference<bng_HabitatSubType>();
            }

            entity.bng_GainSiteID = gainSiteRegistrationId.GetEntityReference<bng_GainSiteRegistration>();

            if (!string.IsNullOrWhiteSpace(baselineHabitat.State))
            {
                entity.bng_HabitatModule = mappingManager.MapHabitatState(baselineHabitat.State);
            }

            if (!string.IsNullOrWhiteSpace(baselineHabitat.MeasurementUnits))
            {
                switch (baselineHabitat.MeasurementUnits)
                {
                    case "hectares":
                        entity.bng_BaselineUnitofMeasure = bng_habitatunitofmeasurement.Hectares;
                        break;

                    case "kilometres":
                        entity.bng_BaselineUnitofMeasure = bng_habitatunitofmeasurement.Kilometres;
                        break;
                }
            }

            var creationId = await dataverseService.CreateAsync(entity);

            return creationId;
        }

        public async Task CreateProposedHabitats(Habitats habitats, Guid gainSiteRegistrationId, IDictionary<string, Guid> BaselineHabitatRefranceGuid)
        {
            try
            {
                if (habitats != null && habitats.Proposed != null && habitats.Proposed.Count > 0)
                {
                    var tasks = habitats.Proposed.Select(async proposedHabitat =>
                    {
                        var entity = new bng_HabitatType();

                        var searchKey = $"{proposedHabitat.BaselineReference}{proposedHabitat.State}";
                        if (BaselineHabitatRefranceGuid.TryGetValue(searchKey, out Guid baselineHabitatId))
                        {
                            entity.bng_BaselineReferenceID = baselineHabitatId.GetEntityReference<bng_BaselineHabitat>();
                        }
                        entity.bng_Size = proposedHabitat.Area;

                        if (!string.IsNullOrWhiteSpace(proposedHabitat.Condition))
                        {
                            entity.bng_Condition = mappingManager.MapHabitatConditionDic(proposedHabitat.Condition);
                        }

                        if (!string.IsNullOrWhiteSpace(proposedHabitat.StrategicSignificance))
                        {
                            entity.bng_StrategicSignificance = mappingManager.MapHabitatStrategicSignificanceDic(proposedHabitat.StrategicSignificance);
                        }

                        if (!string.IsNullOrWhiteSpace(proposedHabitat.AdvanceCreation))
                        {
                            entity.bng_AdvanceCreation = mappingManager.MapCreationChoices(Convert.ToInt32(proposedHabitat.AdvanceCreation));
                        }

                        if (!string.IsNullOrWhiteSpace(proposedHabitat.DelayedCreation))
                        {
                            entity.bng_DelayedCreation = mappingManager.MapCreationChoices(Convert.ToInt32(proposedHabitat.DelayedCreation));
                        }

                        if (!string.IsNullOrWhiteSpace(proposedHabitat.EncroachmentExtent))
                        {
                            entity.bng_ExtentofEncroachment = mappingManager.MapExtentOfEncroachment(proposedHabitat.EncroachmentExtent);
                        }

                        if (!string.IsNullOrWhiteSpace(proposedHabitat.EncroachmentExtentBothBanks))
                        {
                            entity.bng_ExtentofEncroachmentforbothbanks = mappingManager.MapExtentOfEncroachmentBothBanks(proposedHabitat.EncroachmentExtentBothBanks);
                        }

                        if (!string.IsNullOrWhiteSpace(proposedHabitat.State))
                        {
                            entity.bng_HabitatType1 = mappingManager.MapHabitatState(proposedHabitat.State);
                        }

                        if (!string.IsNullOrWhiteSpace(proposedHabitat.Module))
                        {
                            entity.bng_HabitatState = mappingManager.MapHabitatInterventionTypes(proposedHabitat.Module);
                        }

                        if (!string.IsNullOrWhiteSpace(proposedHabitat.HabitatId))
                        {
                            entity.bng_HabitatName = proposedHabitat.HabitatId;
                        }

                        var habitatSubTypeId = await UpsertHabitatSubType(proposedHabitat.HabitatType);

                        if (habitatSubTypeId != Guid.Empty)
                        {
                            entity.bng_ProposedHabitatSubTypeLookup = habitatSubTypeId.GetEntityReference<bng_HabitatSubType>();
                        }

                        entity.bng_GainSiteRegistrationID = gainSiteRegistrationId.GetEntityReference<bng_GainSiteRegistration>();
                        var creationId = await dataverseService.CreateAsync(entity);
                        logger.LogInformation("Proposed Habitat Created Id: {id}", creationId);
                    });
                    await Task.WhenAll(tasks);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{error}", ex.Message);
            }
        }

        public async Task<Guid> UpsertHabitatSubType(string name)
        {
            var query = new QueryExpression(bng_HabitatSubType.EntityLogicalName)
            {
                Criteria = new FilterExpression(LogicalOperator.And),
                ColumnSet = new ColumnSet(
                 DataverseExtensions.AttributeLogicalName<bng_HabitatSubType>(nameof(bng_HabitatSubType.bng_HabitatSubTypeId)),
                 DataverseExtensions.AttributeLogicalName<bng_HabitatSubType>(nameof(bng_HabitatSubType.bng_HabitatSubTypeName)))
            };
            query.AddEqualOperatorCondition<bng_HabitatSubType>(nameof(bng_HabitatSubType.bng_HabitatSubTypeName), name);

            var id = await dataverseService.RetrieveFirstRecordForEntity(query);

            if (id == Guid.Empty)
            {
                var entity = new bng_HabitatSubType
                {
                    bng_HabitatSubTypeName = name
                };

                id = await dataverseService.CreateAsync(entity);
            }

            return id;
        }

        public static string GenerateJsonForCaseCreation(GainSiteRegistration gainSiteRegistration, EntityReference clientReference, Entity organisationEntity)
        {
            var jsonStringBuilder = new StringBuilder();

            if (gainSiteRegistration.Applicant != null && gainSiteRegistration.Applicant.Role != null)
            {
                jsonStringBuilder.Append('{');
                if (gainSiteRegistration.Applicant.Role.Equals("agent", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (gainSiteRegistration.Agent != null)
                    {
                        AppendAgentTagsCommonData(jsonStringBuilder, clientReference, gainSiteRegistration.Agent.ClientType);
                        AppendCommonData(jsonStringBuilder, gainSiteRegistration.Agent.ClientAddress, "759150001");
                    }
                }
                else if (gainSiteRegistration.LandownerAddress != null)
                {
                    AppendCommonData(jsonStringBuilder, gainSiteRegistration.LandownerAddress, "759150000");
                }
                else if (gainSiteRegistration.Organisation != null)
                {
                    var clientType = organisationEntity.LogicalName == Account.EntityLogicalName ? "organisation" : organisationEntity.LogicalName;

                    jsonStringBuilder.Append($"\"agentId\":\"{organisationEntity.Id}\",");
                    jsonStringBuilder.Append($"\"clientType\":\"{clientType}\",");
                    jsonStringBuilder.Append($"\"id\":\"{gainSiteRegistration.Organisation.Id}\",");
                    AppendCommonData(jsonStringBuilder, gainSiteRegistration.Organisation.Address, "759150002");
                }
                jsonStringBuilder.Append('}');
            }
            return jsonStringBuilder.ToString();
        }


        private async Task AssociateGainSiteToCustomer(GainSiteRegistration gainSiteRegistration, Guid applicant, EntityReference clientReference, Entity organisationEntity, Guid gainSiteId)
        {
            if (clientReference != null)
            {
                if (clientReference.LogicalName.Equals(Contact.EntityLogicalName))
                {
                    await dataverseService.AssosiateTwoEntitiesRecords(gainSiteId.GetEntityReference<bng_GainSiteRegistration>(),
                   DataverseExtensions.GetRelationshipSchemaName<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.bng_GainSiteRegistration_Contact_Contact))
                                                  ,
                                                  [clientReference]);
                }
                else if (clientReference.LogicalName.Equals(Account.EntityLogicalName))
                {
                    await dataverseService.AssosiateTwoEntitiesRecords(gainSiteId.GetEntityReference<bng_GainSiteRegistration>(),
                        DataverseExtensions.GetRelationshipSchemaName<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.bng_GainSiteRegistration_Account_Account))
                                                  ,
                                                  [clientReference]);
                }
            }
            else if (applicant != Guid.Empty && gainSiteRegistration.Applicant.Role == "landowner")
            {
                await dataverseService.AssosiateTwoEntitiesRecords(gainSiteId.GetEntityReference<bng_GainSiteRegistration>(), DataverseExtensions.GetRelationshipSchemaName<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.bng_GainSiteRegistration_Contact_Contact))
                                              ,
                                              [applicant.GetEntityReference<Contact>()]);
            }
            else if (gainSiteRegistration.Organisation != null && organisationEntity != null)
            {
                await dataverseService.AssosiateTwoEntitiesRecords(gainSiteId.GetEntityReference<bng_GainSiteRegistration>(), DataverseExtensions.GetRelationshipSchemaName<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.bng_GainSiteRegistration_Account_Account)),
                                          [organisationEntity.Id.GetEntityReference<Account>()]);
            }
        }

        private async Task<Entity> GenrateOrganisation(GainSiteRegistration gainSiteRegistration)
        {
            Entity organisationEntity = null;

            if (gainSiteRegistration.Organisation != null)
            {
                var queryOrganisation = DataverseExtensions.GetQuery<Account>();
                queryOrganisation.AddEqualOperatorCondition<Account>(nameof(Account.bng_DefraID), gainSiteRegistration.Organisation.Id);

                organisationEntity = await dataverseService.RetrieveFirstRecord<Account>(queryOrganisation);
            }

            return organisationEntity;
        }

        private async Task<Guid> GenerateApplicant(GainSiteRegistration gainSiteRegistration)
        {
            if (string.IsNullOrWhiteSpace(gainSiteRegistration?.Applicant?.Id))
            {
                throw new InvalidDataException("No valid applicant was supplied in payload!");
            }

            var query = DataverseExtensions.GetQuery<Contact>();

            query.AddEqualOperatorCondition<Contact>(nameof(Contact.bng_DefraID), gainSiteRegistration.Applicant.Id);

            var applicant = await dataverseService.RetrieveFirstRecordForEntity(query);

            if (applicant == Guid.Empty)
            {
                throw new InvalidDataException("Applicant does not exist!");
            }

            return applicant;
        }

        private static void AppendAgentTagsCommonData(StringBuilder jsonStringBuilder, EntityReference clientReference, string clientType)
        {
            jsonStringBuilder.Append(clientReference != null ? $"\"agentId\":\"{clientReference.Id}\"," : $"\"agentId\":null,");
            jsonStringBuilder.Append($"\"clientType\":\"{clientType}\",");
        }

        private static void AppendCommonData(StringBuilder jsonStringBuilder, ClientAddress address, string applicantRole)
        {
            jsonStringBuilder.Append($"\"type\":\"{address.Type}\",");
            jsonStringBuilder.Append($"\"line1\":\"{StringHelper.ConcatenateAddressLines(
                address)}\",");
            jsonStringBuilder.Append($"\"town\":\"{address.Town}\",");
            jsonStringBuilder.Append($"\"postcode\":\"{address.Postcode}\",");
            jsonStringBuilder.Append($"\"county\":\"{address.County}\",");
            jsonStringBuilder.Append($"\"country\":\"{address.Country}\" ,");
            jsonStringBuilder.Append($"\"applicantRole\":\"{applicantRole}\"");
        }

        private async Task<Guid> UpsertIndividual(Individual contact)
        {
            Guid contactId;

            var query = DataverseExtensions.GetQuery<Contact>();

            if (!string.IsNullOrWhiteSpace(contact.Id))
            {
                query.AddEqualOperatorCondition<Contact>(nameof(Contact.bng_DefraID), contact.Id);
            }

            if (!string.IsNullOrWhiteSpace(contact.Firstname))
            {
                query.AddEqualOperatorCondition<Contact>(nameof(Contact.FirstName), contact.Firstname);
            }

            if (!string.IsNullOrWhiteSpace(contact.Lastname))
            {
                query.AddEqualOperatorCondition<Contact>(nameof(Contact.LastName), contact.Lastname);
            }

            if (!string.IsNullOrWhiteSpace(contact.Email))
            {
                query.AddEqualOperatorCondition<Contact>(nameof(Contact.EMailAddress1), contact.Email);
            }

            contactId = await dataverseService.RetrieveFirstRecordForEntity(query);

            if (contactId == Guid.Empty)
            {
                var entity = new Contact();
                entity.FirstName = contact.Firstname;
                entity.LastName = contact.Lastname;

                if (!string.IsNullOrWhiteSpace(contact.Middlenames))
                {
                    entity.MiddleName = contact.Middlenames;
                }

                if (!string.IsNullOrWhiteSpace(contact.Email))
                {
                    entity.EMailAddress1 = contact.Email;
                }

                entity.bng_DefraID = contact.Id;

                contactId = await dataverseService.CreateAsync(entity);
            }

            return contactId;
        }

        private async Task<Guid> UpsertOrganisation(Organisation organisation)
        {
            var query = DataverseExtensions.GetQuery<Account>();

            if (!string.IsNullOrWhiteSpace(organisation.OrganisationName))
            {
                query.AddEqualOperatorCondition<Account>(nameof(Account.Name), organisation.OrganisationName);
            }

            if (!string.IsNullOrWhiteSpace(organisation.Name))
            {
                query.AddEqualOperatorCondition<Account>(nameof(Account.Name), organisation.Name);
            }

            if (!string.IsNullOrWhiteSpace(organisation.Id))
            {
                query.AddEqualOperatorCondition<Account>(nameof(Account.bng_DefraID), organisation.Id);
            }

            if (!string.IsNullOrWhiteSpace(organisation.OrganisationEmail))
            {
                query.AddEqualOperatorCondition<Account>(nameof(Account.EMailAddress1), organisation.OrganisationEmail);
            }

            var organisationId = await dataverseService.RetrieveFirstRecordForEntity(query);

            if (organisationId == Guid.Empty)
            {
                var entity = new Account
                {
                    bng_DefraID = organisation.Id
                };

                if (!string.IsNullOrWhiteSpace(organisation.Name))
                {
                    entity.Name = organisation.Name;
                }
                else if (!string.IsNullOrWhiteSpace(organisation.OrganisationName))
                {
                    entity.Name = organisation.OrganisationName;
                }

                if (!string.IsNullOrWhiteSpace(organisation.OrganisationEmail))
                {
                    entity.EMailAddress1 = organisation.OrganisationEmail;
                }

                organisationId = await dataverseService.CreateAsync(entity);
            }

            return organisationId;
        }

        private async Task ProcessHabitats(GainSiteRegistration gainSiteRegistration, Guid gainSiteId)
        {
            var baselineHabitatRefranceGuid = await CreateBaselineHabitats(gainSiteRegistration.Habitats, gainSiteId);

            if (baselineHabitatRefranceGuid == null)
            {
                baselineHabitatRefranceGuid = new Dictionary<string, Guid>();
            }

            await CreateProposedHabitats(gainSiteRegistration.Habitats, gainSiteId, baselineHabitatRefranceGuid);
        }

        private async Task ManageResponsibleBodies(GainSiteRegistration gainSiteRegistration, Guid gainSiteId)
        {
            if (gainSiteRegistration.PlanningObligationLPAs != null &&
                gainSiteRegistration.LegalAgreementType == ((int)bng_legalagreementtype.PlanningObligation).ToString())
            {
                var listOfLpaId = new List<Guid>();
                foreach (var item in gainSiteRegistration.PlanningObligationLPAs)
                {
                    Guid lpaId = await RetrieveLPA(item);
                    listOfLpaId.Add(lpaId);
                }

                var entityList = listOfLpaId.Where(x => x != Guid.Empty)
                                            .Select(x =>
                                                        x.GetEntityReference<bng_LocalPlanningAuthority>()
                                            ).ToList();
                await dataverseService.AssosiateTwoEntitiesRecords(gainSiteId.GetEntityReference<bng_GainSiteRegistration>(), DataverseExtensions.GetRelationshipSchemaName<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.bng_GainSiteRegistration_bng_LocalPlanning))
                                                    ,
                                                    entityList);
            }
            else if (gainSiteRegistration.ConservationCovernantResponsibleBodies != null &&
                    gainSiteRegistration.LegalAgreementType == ((int)bng_legalagreementtype.ConservationCovenant).ToString())
            {
                var listOfOrganisationId = new List<Guid>();
                foreach (var item in gainSiteRegistration.ConservationCovernantResponsibleBodies)
                {
                    var responsibleBodyId = await CreateResponsibleBody(item, gainSiteId);

                    listOfOrganisationId.Add(responsibleBodyId);
                }

                var entityList = listOfOrganisationId.Where(x => x != Guid.Empty)
                                                     .Select(x =>
                                                                    x.GetEntityReference<bng_ResponsibleBody>())
                                                     .ToList();

                await dataverseService.AssosiateTwoEntitiesRecords(gainSiteId.GetEntityReference<bng_GainSiteRegistration>(), DataverseExtensions.GetRelationshipSchemaName<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.bng_ResponsibleBody_bng_GainSiteRegistrat))
                                                    ,
                                                    entityList);
            }
        }

        private async Task<Guid> CreateResponsibleBody(ResponsibleBody responsibleBody, Guid gainSiteId)
        {
            var id = await RetrieveResponsibleBody(responsibleBody);

            if (id == Guid.Empty)
            {
                var organisationEntity = new bng_ResponsibleBody
                {
                    bng_Name = responsibleBody.ResponsibleBodyName
                };

                id = await dataverseService.CreateAsync(organisationEntity);
            }

            return id;
        }

        private async Task<Guid> RetrieveResponsibleBody(ResponsibleBody responsibleBody)
        {
            Guid entityId = Guid.Empty;

            var query = new QueryExpression(bng_ResponsibleBody.EntityLogicalName)
            {
                Criteria = new FilterExpression(LogicalOperator.And),
                ColumnSet = new ColumnSet(
                    DataverseExtensions.AttributeLogicalName<bng_ResponsibleBody>(nameof(bng_ResponsibleBody.bng_Name))
                    )
            };

            query.AddEqualOperatorCondition<bng_ResponsibleBody>(nameof(bng_ResponsibleBody.bng_Name), responsibleBody.ResponsibleBodyName);

            var result = await dataverseService.RetrieveMultipleAsync(query);

            if (result != null && result.Entities.Count > 0)
            {
                entityId = result.Entities[0].Id;
            }

            return entityId;
        }

        private async Task<Guid> RetrieveLPA(PlanningObligationLpa lpa)
        {
            Guid lpaId = Guid.Empty;

            var query = DataverseExtensions.GetQuery<bng_LocalPlanningAuthority>();
            query.AddEqualOperatorCondition<bng_LocalPlanningAuthority>(nameof(bng_LocalPlanningAuthority.bng_Name), lpa.LpaName);
            query.AddEqualOperatorCondition<bng_LocalPlanningAuthority>(nameof(bng_LocalPlanningAuthority.bng_lpaid), lpa.LpaId);

            var result = await dataverseService.RetrieveMultipleAsync(query);

            if (result != null && result.Entities.Count > 0)
            {
                lpaId = result.Entities[0].Id;
            }

            return lpaId;
        }
    }
}