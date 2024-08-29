using System.Text;
using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Domain.Request;
using DEFRA.NE.BNG.Integration.Infrastructure.EntityProcessors;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace DEFRA.NE.BNG.Integration.Infrastructure.Facades
{
    public class DeveloperRegistrationFacade : FacadeBase, IDeveloperRegistrationFacade
    {
        private readonly ILogger<DeveloperRegistrationFacade> logger;

        public DeveloperRegistrationFacade(ILogger<DeveloperRegistrationFacade> logger,
                                           IConfigurationReader environmentVariableReader,
                                           IMailServiceAgent mailService,
                                           IDataverseService dataverseService) : base(
                                                                                      environmentVariableReader,
                                                                                      mailService,
                                                                                      dataverseService)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Function facilitate the sequence of methods
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Guid> OrchestrationBNG(DeveloperRegistration developerRegistration, bng_casetype caseType = bng_casetype.Allocation)
        {
            EntityReference applicantRef = null;
            EntityReference clientReference = null;

            if (caseType == bng_casetype.Allocation)
            {
                if (developerRegistration?.Applicant == null)
                {
                    throw new InvalidDataException("Applicant is required!");
                }

                var applicantProcessor = new ApplicantProcessor();
                var applicantId = await applicantProcessor.Create(developerRegistration.Applicant, dataverseService, logger);
                applicantRef = applicantId.GetEntityReference<Contact>();

                if (developerRegistration.Applicant?.Role == "agent")
                {
                    clientReference = await dataverseService.UpsertClient(developerRegistration.Agent);
                }
            }

            EntityReference localPlanningAuthority = null;

            if (developerRegistration.Development?.LocalPlanningAuthority != null)
            {
                var planningProcessor = new LocalPlanningAuthorityProcessor();
                var planningId = await planningProcessor.FindFirst(
                                                                developerRegistration.Development.LocalPlanningAuthority,
                                                                dataverseService, logger);
                if (planningId != Guid.Empty)
                {
                    localPlanningAuthority = planningId.GetEntityReference<bng_LocalPlanningAuthority>();
                }
            }

            Guid devRegistrationId = Guid.Empty;

            if (developerRegistration.GainSite != null)
            {
                var developmentProcessor = new DevelopmentProcessor();
                var devRegistrationTuple = await developmentProcessor.Create(
                                                                    developerRegistration.Development,
                                                                    developerRegistration.GainSite.Reference,
                                                                    applicantRef,
                                                                    localPlanningAuthority,
                                                                    dataverseService,
                                                                    logger);

                devRegistrationId = devRegistrationTuple.Id;

                if (devRegistrationId != Guid.Empty)
                {
                    if (developerRegistration.Files != null)
                    {
                        await dataverseService.CreateAttachments(developerRegistration.Files,
                                                    devRegistrationId.GetEntityReference<bng_DeveloperRegistration>());
                    }

                    var gainSiteId = await RetrieveGainSiteWithDeveloper(developerRegistration.GainSite);

                    await TriggerCaseCreation(developerRegistration, caseType, clientReference, devRegistrationId, devRegistrationTuple, gainSiteId);
                }
            }

            return devRegistrationId;
        }

        private async Task TriggerCaseCreation(DeveloperRegistration developerRegistration, bng_casetype caseType, EntityReference clientReference, Guid devRegistrationId, (Guid Id, bool AlreadyLinkedToGainSite) devRegistrationTuple, Guid gainSiteId)
        {
            if (developerRegistration.GainSite != null && !devRegistrationTuple.AlreadyLinkedToGainSite)
            {
                var gainsiteEntityCollection = new EntityReferenceCollection
                                                            {
                                                                gainSiteId.GetEntityReference<bng_GainSiteRegistration>()
                                                            };

                await dataverseService.AssociateAsync(
                        bng_DeveloperRegistration.EntityLogicalName, devRegistrationId,
                        new Relationship(DataverseExtensions.GetRelationshipSchemaName<bng_DeveloperRegistration>(nameof(bng_DeveloperRegistration.bng_DeveloperRegistration_GainSiteReg))),
                        gainsiteEntityCollection);
            }

            List<Guid> allocatedHabitatIds = await CreateAllocatedHabitats(developerRegistration, devRegistrationId, gainSiteId);
            await CreateAllocationCase(developerRegistration, caseType, clientReference, devRegistrationId, gainSiteId, allocatedHabitatIds);
        }

        private async Task<List<Guid>> CreateAllocatedHabitats(DeveloperRegistration developerRegistration, Guid devRegistrationId, Guid gainSiteId)
        {
            List<Guid> allocatedHabitatIds = null;

            if (developerRegistration.Development != null)
            {


                var habitatList = developerRegistration.Habitats.Allocated
                    .Select(habitat =>
                    {
                        habitat.DeveloperRegistrationId = devRegistrationId;
                        habitat.GainSiteId = gainSiteId;
                        return habitat;
                    }
                           )
                    .ToList();

                var habitatProcessor = new AllocatedHabitatProcessor();
                allocatedHabitatIds = await habitatProcessor.CreateList(habitatList, dataverseService, logger);
            }

            return allocatedHabitatIds;
        }

        private async Task CreateAllocationCase(DeveloperRegistration developerRegistration, bng_casetype caseType, EntityReference clientReference, Guid devRegistrationId, Guid gainSiteId, List<Guid> allocatedHabitatIds)
        {
                var caseCreationInstructionsJson = GenerateCaseCreationInstructionsJson(developerRegistration, clientReference, gainSiteId, allocatedHabitatIds, caseType);

                var developerRegistrationEntity = new bng_DeveloperRegistration
                {
                    Id = devRegistrationId,
                    bng_ReadyForProcessingOn = DateTime.Now,
                    bng_JSONForCaseCreation = caseCreationInstructionsJson,
                    
                    bng_PortalReferenceNumber = developerRegistration.AllocationReference
                };

                await dataverseService.UpdateAsync(developerRegistrationEntity);
        }

        public async Task<Guid> RetrieveDevReistrationIdForAllocations(DevelopmentDetails developmentDetails)
        {
            var devRegistrationId = Guid.Empty;

            var query = DataverseExtensions.GetQuery<bng_DeveloperRegistration>();

            query.Criteria.AddCondition(
                DataverseExtensions.AttributeLogicalName<bng_DeveloperRegistration>(nameof(bng_DeveloperRegistration.bng_PlanningReference)), ConditionOperator.Equal,
                                           developmentDetails.PlanningReference);

            LinkEntity linkEntity1 = new LinkEntity(bng_DeveloperRegistration.EntityLogicalName,
                                           "bng_developerregistration_bng_gainsitereg",
                   DataverseExtensions.AttributeLogicalName<bng_DeveloperRegistration>(nameof(bng_DeveloperRegistration.bng_DeveloperRegistrationId)),
                   DataverseExtensions.AttributeLogicalName<bng_DeveloperRegistration>(nameof(bng_DeveloperRegistration.bng_DeveloperRegistrationId))
                   , JoinOperator.Inner);
            LinkEntity linkEntity2 = new LinkEntity(
                "bng_developerregistration_bng_gainsitereg"
                , bng_GainSiteRegistration.EntityLogicalName,
                DataverseExtensions.AttributeLogicalName<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.bng_GainSiteRegistrationId)),
                DataverseExtensions.AttributeLogicalName<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.bng_GainSiteRegistrationId)), JoinOperator.Inner);

            linkEntity1.LinkEntities.Add(linkEntity2);
            query.LinkEntities.Add(linkEntity1);


            var entityCollection = await dataverseService.RetrieveMultipleAsync(query);
            if (entityCollection != null && entityCollection.Entities.Count > 0)
            {
                devRegistrationId = entityCollection.Entities[0].Id;
            }

            return devRegistrationId;
        }

        public async Task<Guid> RetrieveGainSiteWithDeveloper(GainSite gainSite)
        {
            var query = DataverseExtensions.GetQuery<bng_GainSiteRegistration>();
            query.Criteria.AddCondition(
                                    DataverseExtensions.AttributeLogicalName<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.bng_GainSiteReference)),
                                    ConditionOperator.Equal,
                                    gainSite.Reference);
            var gainsite = await dataverseService.RetrieveFirstRecordForEntity(query);

            return gainsite;
        }

        public static string GenerateCaseCreationInstructionsJson(DeveloperRegistration developerRegistration, EntityReference clientReference, Guid gainSiteId, List<Guid> allocatedHabitatIds, bng_casetype caseType)
        {           
            var jsonStringBuilder = new StringBuilder();
            jsonStringBuilder.Append('{');
            jsonStringBuilder.Append($"\"gainSiteId\":\"{gainSiteId}\",");
            jsonStringBuilder.Append($"\"area\":\"{developerRegistration.GainSite.OffSiteUnitChange.Habitat}\",");
            jsonStringBuilder.Append($"\"watercourses\":\"{developerRegistration.GainSite.OffSiteUnitChange.Watercourse}\",");
            jsonStringBuilder.Append($"\"hedgerow\":\"{developerRegistration.GainSite.OffSiteUnitChange.Hedge}\",");           
            jsonStringBuilder.Append($"\"caseType\":\"{caseType}\",");

            if(caseType == bng_casetype.Allocation)
            {
                var islandowner = developerRegistration.IsLandownerLeaseholder.ToLower().Equals("yes", StringComparison.CurrentCultureIgnoreCase);
                jsonStringBuilder.Append($"\"role\":\"{developerRegistration.Applicant.Role}\",");
                switch (developerRegistration.Applicant.Role)
                {
                    case "agent":
                        GenerateJsonForAgent(developerRegistration, clientReference, islandowner, jsonStringBuilder);
                        break;
                    case "individual":
                        GenerateJsonForIndividual(developerRegistration, islandowner, jsonStringBuilder);
                        break;
                    case "organisation":
                        var applicantRole = 759150000;

                        if (!islandowner && !string.IsNullOrWhiteSpace(developerRegistration.Organisation?.Id))
                        {
                            applicantRole = 759150002;
                            jsonStringBuilder.Append($"\"organisationId\":\"{developerRegistration.Organisation.Id}\",");
                        }

                        jsonStringBuilder.Append($"\"applicantRole\":{applicantRole}");
                        break;
                }
                jsonStringBuilder.Append(',');
            }
            GenerateJsonForAllocatedHabitats(allocatedHabitatIds, jsonStringBuilder);

            jsonStringBuilder.Append('}');
            return jsonStringBuilder.ToString();
        }

        private static void GenerateJsonForAllocatedHabitats(List<Guid> allocatedHabitatIds, StringBuilder jsonStringBuilder)
        {
            var filteredList = allocatedHabitatIds.Where(x => x != Guid.Empty).ToList();

            jsonStringBuilder.Append($"\"allocatedHabitats\":[");
            for (int counter = 0; counter < filteredList.Count; counter++)
            {
                var itemseparator = counter == filteredList.Count - 1 ? "" : ",";
                jsonStringBuilder.Append($"{{\"id\":\"{filteredList[counter]}\"}}{itemseparator}");
            }
            jsonStringBuilder.Append(']');
        }

        private static void GenerateJsonForIndividual(DeveloperRegistration developerRegistration, bool islandowner, StringBuilder jsonBuilder)
        {
            int applicantRole = 759150000;
            if (islandowner && developerRegistration.Agent != null && !string.IsNullOrEmpty(developerRegistration.Agent.ClientType) && developerRegistration.Agent.ClientType.ToLower().Equals("organisation", StringComparison.CurrentCultureIgnoreCase))
            {
                applicantRole = 759150002;
            }
            else if (!islandowner)
            {
                applicantRole = 759150003;
            }

            jsonBuilder.Append($"\"applicantRole\":{applicantRole}");
        }

        private static void GenerateJsonForAgent(DeveloperRegistration developerRegistration, EntityReference clientReference, bool islandowner, StringBuilder jsonStringBuilder)
        {
            if (developerRegistration.Agent != null)
            {
                jsonStringBuilder.Append($"\"clientType\":\"{developerRegistration.Agent.ClientType}\",");

                if (clientReference != null)
                {
                    jsonStringBuilder.Append($"\"agentId\":\"{clientReference.Id}\",");
                }
                else
                {
                    jsonStringBuilder.Append($"\"agentId\":null,");
                }

                int applicantRole = 759150001;

                if (developerRegistration.Agent.ClientType == "individual" && islandowner)
                {
                    applicantRole = 759150001;
                }
                else if (developerRegistration.Agent.ClientType == "individual" && !islandowner)
                {
                    applicantRole = 759150004;
                }

                jsonStringBuilder.Append($"\"applicantRole\":{applicantRole}");
            }
        }
    }
}