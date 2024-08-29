using DEFRA.NE.BNG.Integration.Domain.Constants;
using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using DEFRA.NE.BNG.Integration.Domain.JsonRequestRoot;
using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Domain.Request;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace DEFRA.NE.BNG.Integration.Infrastructure.Facades
{
    public class DefraIdSyncFacade : IDefraIdSyncFacade
    {
        private readonly ILogger<DefraIdSyncFacade> logger;
        private readonly IDataverseService dataverseService;

        public DefraIdSyncFacade(ILogger<DefraIdSyncFacade> logger, IDataverseService dataverseService)
        {
            this.logger = logger;
            this.dataverseService = dataverseService;
        }

        /// <summary>
        /// UpdateUserAndAccountFromDefraId
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public async Task UpdateUserAndAccountFromDefraId(DefraIdRequestPayload requestData)
        {
            logger.LogInformation("Inside UpdateUserAndAccountFromDefraId method");
            if (requestData == null || requestData.Recorddata == null || requestData.Metadata == null)
            {
                return;
            }


            if (string.Equals(requestData.Metadata.Entity, EnvironmentConstants.DefraIdMetaDataLOBEntity,
                StringComparison.OrdinalIgnoreCase) && string.Equals(requestData.Metadata.Operationtype,
                EnvironmentConstants.DefraIdMetaDataCreateOperation, StringComparison.OrdinalIgnoreCase))
            {
                logger.LogInformation("Inside UpdateUserAndAccountFromDefraId Creating Contact and/or Account");

                if (requestData.Recorddata.Account != null)
                {
                    logger.LogInformation("Inside UpdateUserAndAccountFromDefraId  Creating Account");
                    var account = ConvertToEntity(false, true, requestData);
                    await dataverseService.UpsertEntity(account);
                }

                if (requestData.Recorddata.Contact != null)
                {
                    logger.LogInformation("Inside UpdateUserAndAccountFromDefraId  Creating Contact");
                    var contact = ConvertToEntity(true, true, requestData);
                    await dataverseService.UpsertEntity(contact);
                }
            }
            else if (string.Equals(requestData.Metadata.Entity, Contact.EntityLogicalName,
                StringComparison.OrdinalIgnoreCase) && string.Equals(requestData.Metadata.Operationtype,
                EnvironmentConstants.DefraIdMetaDataUpdateOperation, StringComparison.OrdinalIgnoreCase))
            {
                logger.LogInformation("Inside UpdateUserAndAccountFromDefraId  Updating Contact");
                if (requestData.Recorddata.Contact != null)
                {
                    var contactEntityId = await GetPlatformDataById<Contact>(
                                                            Guid.Parse(requestData.Recorddata.Contact.contactid),
                                                            DataverseExtensions.AttributeLogicalName<Contact>(nameof(Contact.ContactId))
                                                    );

                    if (contactEntityId != Guid.Empty)
                    {
                        var contact = ConvertToEntity(true, false, requestData);

                        await dataverseService.UpdateAsync(contact);
                        logger.LogInformation("Existing Contact Updated with Id: {id}", contactEntityId);
                    }
                }
            }
            else if (string.Equals(requestData.Metadata.Entity, Account.EntityLogicalName,
                StringComparison.OrdinalIgnoreCase) && string.Equals(requestData.Metadata.Operationtype,
                EnvironmentConstants.DefraIdMetaDataUpdateOperation, StringComparison.OrdinalIgnoreCase))
            {
                logger.LogInformation("Inside UpdateUserAndAccountFromDefraId  Updating Account");
                if (requestData.Recorddata.Account != null)
                {
                    var accountEntityId = await GetPlatformDataById<Account>(Guid.Parse(requestData.Recorddata.Account.accountid), DataverseExtensions.AttributeLogicalName<Account>(nameof(Account.AccountId)));
                    if (accountEntityId == Guid.Empty)
                    {
                        return;
                    }

                    var account = ConvertToEntity(false, false, requestData);
                    await dataverseService.UpdateAsync(account);
                    logger.LogInformation("Existing Account Updated with Id: {id}", accountEntityId);
                }
            }
        }

        public async Task<Guid> GetOrganisationType(string orgType)
        {
            Guid OrganisationType = Guid.Empty;
            try
            {
                var query = new QueryExpression(bng_OrganisationType.EntityLogicalName)
                {
                    ColumnSet = new ColumnSet(
                        DataverseExtensions.AttributeLogicalName<bng_OrganisationType>(nameof(bng_OrganisationType.bng_Name)),
                        DataverseExtensions.AttributeLogicalName<bng_OrganisationType>(nameof(bng_OrganisationType.bng_OrganisationTypeId))
                    ),
                    Criteria = new FilterExpression(LogicalOperator.And)
                };
                query.Criteria.AddCondition(
                    DataverseExtensions.AttributeLogicalName<bng_OrganisationType>(nameof(bng_OrganisationType.bng_OrgTypePicklist)), ConditionOperator.Equal,
                            orgType);

                var results = await dataverseService.RetrieveMultipleAsync(query);

                if (results?.Entities?.Count > 0)
                {
                    OrganisationType = results.Entities[0].Id;
                    logger.LogInformation("Get GetOrganisationType Id: {id}", OrganisationType);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{error}", ex.Message);
            }
            return OrganisationType;
        }

        public async Task<Guid> GetPlatformDataById<T>(Guid entityId, params string[] columns) where T : Entity
        {
            logger.LogInformation("Inside {methodname}", nameof(GetPlatformDataById));
            var returnValue = Guid.Empty;
            try
            {
                var result = await dataverseService.RetrieveAsync<T>(entityId, new ColumnSet(columns));
                if (result != null)
                {
                    returnValue = result.Id;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{error}", ex.Message);
            }
            return returnValue;
        }


        private Entity ConvertToEntity(bool isContact, bool forCreate, DefraIdRequestPayload requestData)
        {
            logger.LogInformation("Inside ConvertToEntity method");
            Entity entity;

            if (isContact)
            {
                var contact = new Contact() { Id = Guid.Parse(requestData.Recorddata.Contact.contactid) };

                if (forCreate)
                {
                    SetContactAttributesInCreateMode(requestData.Recorddata.Contact, contact);
                }
                else
                {
                    SetContactAttributesInUpdateMode(requestData.Recorddata.Contact, contact);
                }

                SetContactAddressAttributes(requestData.Recorddata.Contact, contact);

                entity = contact;
            }
            else
            {
                var account = new Account() { Id = Guid.Parse(requestData.Recorddata.Account.accountid) };
                if (forCreate)
                {
                    SetAccountAttributesInCreateMode(requestData.Recorddata.Account, account);
                }
                else
                {
                    SetAccountAttributesInUpdateMode(requestData.Recorddata.Account, account);
                }
                SetOrganisationAddressAttributes(requestData.Recorddata.Account, account);
                entity = account;
            }

            return entity;
        }

        private static void SetContactAttributesInCreateMode(DefraIdContact contact, Contact entity)
        {
            if (contact.firstname != null)
            {
                entity.FirstName = contact.firstname;
            }

            if (contact.lastname != null)
            {
                entity.LastName = contact.lastname;
            }

            if (contact.middlename != null)
            {
                entity.MiddleName = contact.middlename;
            }

            if (contact.birthdate != null)
            {
                entity.BirthDate = Convert.ToDateTime(contact.birthdate);
            }

            if (contact.emailaddress1 != null)
            {
                entity.EMailAddress1 = contact.emailaddress1;
            }

            if (contact.contactid != null)
            {
                entity.bng_DefraID = contact.contactid;
            }

            if (contact.telephone1 != null)
            {
                entity.Telephone1 = contact.telephone1;
            }

            if (contact.nationalityid1 != null &&
                Guid.TryParse(contact.nationalityid1, out Guid nationality1)
                )
            {
                entity.bng_NationalityId1 = nationality1.GetEntityReference<bng_Nationality>();
            }

            if (
                Guid.TryParse(contact.nationalityid2, out Guid nationalityid2)
                )
            {
                entity.bng_NationalityId2 = nationalityid2.GetEntityReference<bng_Nationality>();
            }

            if (
                Guid.TryParse(contact.nationalityid3, out Guid nationalityid3))
            {
                entity.bng_NationalityId3 = nationalityid3.GetEntityReference<bng_Nationality>();
            }

            if (
                Guid.TryParse(contact.nationalityid4, out Guid nationalityid4))
            {
                entity.bng_NationalityId4 = nationalityid4.GetEntityReference<bng_Nationality>();
            }
        }

        private static void SetContactAttributesInUpdateMode(DefraIdContact contact, Contact entity)
        {
            if (contact.firstname_hasvalue)
            {
                entity.FirstName = contact.firstname;
            }

            if (contact.lastname_hasvalue)
            {
                entity.LastName = contact.lastname;
            }

            if (contact.middlename_hasvalue)
            {
                entity.MiddleName = contact.middlename;
            }

            if (contact.birthdate_hasvalue)
            {
                entity.BirthDate = Convert.ToDateTime(contact.birthdate);
            }

            if (contact.emailaddress1_hasvalue)
            {
                entity.EMailAddress1 = contact.emailaddress1;
            }

            if (contact.contactid_hasvalue)
            {
                entity.bng_DefraID = contact.contactid;
            }

            if (contact.telephone1_hasvalue)
            {
                entity.Telephone1 = contact.telephone1;
            }

            if (Guid.TryParse(contact.nationalityid1, out Guid nationalityid1))
            {
                entity.bng_NationalityId1 = nationalityid1.GetEntityReference<bng_Nationality>();
            }

            if (Guid.TryParse(contact.nationalityid2, out Guid nationalityid2))
            {
                entity.bng_NationalityId2 = nationalityid2.GetEntityReference<bng_Nationality>();
            }

            if (Guid.TryParse(contact.nationalityid3, out Guid nationalityid3))
            {
                entity.bng_NationalityId3 = nationalityid3.GetEntityReference<bng_Nationality>();
            }

            if (Guid.TryParse(contact.nationalityid4, out Guid nationalityid4))
            {
                entity.bng_NationalityId4 = nationalityid4.GetEntityReference<bng_Nationality>();
            }
        }

        private void SetAccountAttributesInCreateMode(DefraIdAccount account, Account entity)
        {
            if (account.accountid != null)
            {
                entity.bng_DefraID = account.accountid;
            }

            if (account.name != null)
            {
                entity.Name = account.name;
            }

            if (account.telephone1 != null)
            {
                entity.Telephone1 = account.telephone1;
            }

            if (account.emailaddress1 != null)
            {
                entity.EMailAddress1 = account.emailaddress1;
            }

            if (account.defra_dateofincorporation != null)
            {
                entity.bng_DateofIncorporation = DateTime.Parse(account.defra_dateofincorporation, EnvironmentConstants.DefaultCultureInfo);
            }

            if (account.defra_type != null)
            {
                var orgnisationType = GetOrganisationType(account.defra_type).Result;

                if (orgnisationType != Guid.Empty)
                {
                    entity.bng_OrganisationTypeID = orgnisationType.GetEntityReference<bng_OrganisationType>();
                }
            }

            if (account.defra_cmcrn != null)
            {
                entity.bng_UniqueRegistrationNumber = account.defra_cmcrn;
            }
        }

        private void SetAccountAttributesInUpdateMode(DefraIdAccount account, Account entity)
        {
            if (account.name_hasvalue)
            {
                entity.Name = account.name;
            }

            if (account.telephone1_hasvalue)
            {
                entity.Telephone1 = account.telephone1;
            }

            if (account.emailaddress1_hasvalue)
            {
                entity.EMailAddress1 = account.emailaddress1;
            }

            if (account.defra_dateofincorporation_hasvalue)
            {
                entity.bng_DateofIncorporation = DateTime.Parse(account.defra_dateofincorporation, EnvironmentConstants.DefaultCultureInfo);
            }

            if (account.defra_type_hasvalue)
            {
                var orgnisationType = GetOrganisationType(account.defra_type).Result;

                if (orgnisationType != Guid.Empty)
                {
                    entity.bng_OrganisationTypeID = orgnisationType.GetEntityReference<bng_OrganisationType>();
                }
            }

            if (account.defra_cmcrn_hasvalue)
            {
                entity.bng_UniqueRegistrationNumber = account.defra_cmcrn;
            }
        }

        private static void SetContactAddressAttributes(DefraIdContact contact, Contact entity)
        {
            if (contact.defra_addrcorsubbuildingname != null)
            {
                entity.Address1_Line1 = contact.defra_addrcorsubbuildingname;
            }

            if (contact.defra_addrcorbuildingname != null)
            {
                entity.Address1_Name = contact.defra_addrcorbuildingname;
            }

            if (contact.defra_addrcorbuildingnumber != null)
            {
                entity.Address1_Line2 = contact.defra_addrcorbuildingnumber;
            }

            if (contact.defra_addrcorstreet != null)
            {
                entity.Address1_Line3 = contact.defra_addrcorstreet;
            }

            if (contact.defra_addrcortown != null)
            {
                entity.Address1_City = contact.defra_addrcortown;
            }

            if (contact.defra_addrcorlocality != null)
            {
                entity.Address1_County = contact.defra_addrcorlocality;
            }

            if (contact.defra_addrcordependentlocality != null)
            {
                entity.bng_DependentLocality = contact.defra_addrcordependentlocality;
            }

            if (contact.defra_addrcorcounty != null)
            {
                entity.Address1_StateOrProvince = contact.defra_addrcorcounty;
            }

            if (contact.defra_addrcorpostcode != null)
            {
                entity.Address1_PostalCode = contact.defra_addrcorpostcode;
            }

            if (contact.defra_addrcorinternationalpostalcode != null)
            {
                entity.Address1_PostOfficeBox = contact.defra_addrcorinternationalpostalcode;
            }

            if (contact.defra_addrcorcountryid != null && Guid.TryParse(contact.defra_addrcorcountryid, out Guid tempGuid))
            {
                entity.bng_CountryID = tempGuid.GetEntityReference<bng_Country>();
            }
        }

        private static void SetOrganisationAddressAttributes(DefraIdAccount account, Account entity)
        {
            if (account.defra_addrregsubbuildingname != null)
            {
                entity.Address1_Line1 = account.defra_addrregsubbuildingname;
            }

            if (account.defra_addrregbuildingname != null)
            {
                entity.Address1_Name = account.defra_addrregbuildingname;
            }

            if (account.defra_addrregbuildingnumber != null)
            {
                entity.Address1_Line2 = account.defra_addrregbuildingnumber;
            }

            if (account.defra_addrregstreet != null)
            {
                entity.Address1_Line3 = account.defra_addrregstreet;
            }

            if (account.defra_addrregtown != null)
            {
                entity.Address1_City = account.defra_addrregtown;
            }

            if (account.defra_addrreglocality != null)
            {
                entity.Address1_County = account.defra_addrreglocality;
            }

            if (account.defra_addrregdependentlocality != null)
            {
                entity.bng_DependentLocality = account.defra_addrregdependentlocality;
            }

            if (account.defra_addrregcounty != null)
            {
                entity.Address1_StateOrProvince = account.defra_addrregcounty;
            }

            if (account.defra_addrregpostcode != null)
            {
                entity.Address1_PostalCode = account.defra_addrregpostcode;
            }

            if (account.defra_addrreginternationalpostalcode != null)
            {
                entity.Address1_PostOfficeBox = account.defra_addrreginternationalpostalcode;
            }

            if (account.defra_addrregcountryid != null && Guid.TryParse(account.defra_addrregcountryid, out Guid tempGuid))
            {
                entity.bng_CountryID = tempGuid.GetEntityReference<bng_Country>();
            }
        }
    }
}
