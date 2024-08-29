using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Domain.Request;
using DEFRA.NE.BNG.Integration.Infrastructure.Utilities;
using DEFRA.NE.BNG.Integration.Model.Request;
using Microsoft.Extensions.Logging;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace DEFRA.NE.BNG.Integration.Infrastructure.Services
{
    public class DataverseService : IDataverseService
    {
        private readonly ILogger logger;
        private readonly IOrganizationServiceAsync2 organizationService;

        public DataverseService(ILogger logger, IBlobClientAccess blobClientAccess, IOrganizationServiceAsync2 organizationService)
        {
            this.logger = logger;
            BlobClientAccess = blobClientAccess;
            this.organizationService = organizationService;
        }

        public IBlobClientAccess BlobClientAccess { get; private set; }

        public async Task<Guid> CreateAsync(Entity entity)
        {
            var id = await organizationService.CreateAsync(entity);
            return id;
        }

        public async Task<List<Guid>> CreateListAsync(List<Entity> entityListToCreate)
        {
            var tasks = entityListToCreate.Select(async x =>
             {
                 var result = await organizationService.CreateAsync(x);
                 return result;
             });

            var tasksArray = await Task.WhenAll(tasks);

            var combinedResults = tasksArray.ToList();


            return combinedResults;
        }

        public async Task<T> RetrieveAsync<T>(Guid id, ColumnSet columnSet) where T : Entity
        {
            var entity = (Entity)Activator.CreateInstance(typeof(T));

            var result = await organizationService.RetrieveAsync(entity.LogicalName, id, columnSet);
            var mapped = result.ToEntity<T>();
            return mapped;
        }

        public async Task<Guid> RetrieveFirstRecordForEntity(QueryBase query)
        {
            Guid id = Guid.Empty;

            var resuts = await organizationService.RetrieveMultipleAsync(query);

            if (resuts != null && resuts.Entities.Count > 0)
            {
                id = resuts.Entities[0].Id;
            }

            return id;
        }

        public async Task<T> RetrieveFirstRecord<T>(QueryBase query) where T : Entity
        {
            T mapped = default(T);

            var resuts = await organizationService.RetrieveMultipleAsync(query);

            if (resuts != null && resuts.Entities.Count > 0)
            {
                mapped = resuts.Entities[0].ToEntity<T>();
            }

            return mapped;
        }

        public async Task<EntityCollection> RetrieveMultipleAsync(QueryBase query)
        {
            var entityCollection = await organizationService.RetrieveMultipleAsync(query);
            return entityCollection;
        }

        public async Task<List<T>> RetrieveMultipleAsync<T>(QueryBase query) where T : Entity
        {
            var result = new List<T>();

            var entityCollection = await organizationService.RetrieveMultipleAsync(query);

            if (entityCollection != null && entityCollection.Entities != null)
            {
                foreach (var item in entityCollection.Entities)
                {
                    result.Add(item.ToEntity<T>());
                }
            }

            return result;
        }

        public async Task UpdateAsync(Entity entity)
        {
            await organizationService.UpdateAsync(entity);
        }

        public async Task AssosiateTwoEntitiesRecords(EntityReference entity1, string relationshipName, List<EntityReference> entityList)
        {
            try
            {
                var entityCollection = new EntityReferenceCollection();
                entityList.ForEach(id => entityCollection.Add(id));

                await organizationService.AssociateAsync(entity1.LogicalName, entity1.Id, new Relationship(relationshipName), entityCollection);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{message}", ex.Message);
            }
        }

        public async Task AssociateAsync(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            await organizationService.AssociateAsync(entityName, entityId, relationship, relatedEntities);
        }

        public async Task<OrganizationResponse> ExecuteAsync(OrganizationRequest request)
        {
            return await organizationService.ExecuteAsync(request);
        }

        public async Task UpsertEntity(Entity entity)
        {
            logger.LogInformation("UpsertEntity type: {entitytype}", entity.LogicalName);

            var request = new UpsertRequest() { Target = entity };
            var response = (UpsertResponse)await organizationService.ExecuteAsync(request);
            if (response.RecordCreated)
            {
                logger.LogInformation("New record {id} is created!", entity.Id);
            }
            else
            {
                logger.LogInformation("Existing record {id} is updated!", entity.Id);
            }
        }

        public async Task CreateAttachments(List<FileDetails> files, EntityReference ownerEntity)
        {
            if (files?.Count > 0)
            {

                SerializationHelper.GroupbyAndIndexFileType(files);

                var tasks = files.Select(async file =>
                {
                    await CreateAttachment(file, ownerEntity);
                });

                await Task.WhenAll(tasks);
            }
        }

        public async Task CreateAttachment(FileDetails file, EntityReference ownerEntity)
        {
            try
            {
                var entity = new Annotation
                {
                    MimeType = file.ContentMediaType,
                    DocumentBody = await BlobClientAccess.ReadDataFromBlob(file.FileLocation),
                    FileName = $"{file.FileType}{SerializationHelper.GetExtensionFromFileName(file.FileName)}",
                    Subject = $"LOP_{file.FileName}",
                    ObjectId = ownerEntity
                };
                var id = await CreateAsync(entity);

                logger.LogInformation("Created: {id}", id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{message}", ex.Message);
            }
        }

        public async Task<IList<bng_BNGConfiguration>> RetrieveConfigurations(string configurationValue)
        {
            var query = DataverseExtensions.GetQuery<bng_BNGConfiguration>();
            query.AddEqualOperatorCondition<bng_BNGConfiguration>(nameof(bng_BNGConfiguration.bng_Name), configurationValue);

            var results = await RetrieveMultipleAsync(query);

            List<bng_BNGConfiguration> entities = [];

            if (results != null)
            {
                entities = results.Entities.Select(x => (bng_BNGConfiguration)x).ToList();
            }

            return entities;
        }

        public async Task<EntityReference> UpsertClient(Agent agent)
        {
            EntityReference agentReference = null;

            if (agent.ClientType == "individual")
            {
                var individualQuery = DataverseExtensions.GetQuery<Contact>();

                individualQuery.AddEqualOperatorCondition<Contact>(nameof(Contact.FirstName), agent.ClientNameIndividual.FirstName);
                individualQuery.AddEqualOperatorCondition<Contact>(nameof(Contact.LastName), agent.ClientNameIndividual.LastName);

                if (!string.IsNullOrWhiteSpace(agent.ClientEmail))
                {
                    individualQuery.AddEqualOperatorCondition<Contact>(nameof(Contact.EMailAddress1), agent.ClientEmail);
                }

                var individualId = await RetrieveFirstRecordForEntity(individualQuery);

                if (individualId == Guid.Empty)
                {
                    Contact entity = GenerateContact(agent);

                    individualId = await CreateAsync(entity);
                }

                agentReference = individualId.GetEntityReference<Contact>();
            }
            else if (agent.ClientType == "organisation")
            {
                var organisationQuery = DataverseExtensions.GetQuery<Account>();

                if (!string.IsNullOrWhiteSpace(agent.ClientEmail))
                {
                    organisationQuery.AddEqualOperatorCondition<Account>(nameof(Account.EMailAddress1), agent.ClientEmail);
                }

                if (!string.IsNullOrWhiteSpace(agent.ClientNameOrganisation))
                {
                    organisationQuery.AddEqualOperatorCondition<Account>(nameof(Account.Name), agent.ClientNameOrganisation);
                }

                var organisationId = await RetrieveFirstRecordForEntity(organisationQuery);

                if (organisationId != Guid.Empty)
                {
                    agentReference = organisationId.GetEntityReference<Account>();
                }
                else
                {
                    Entity entity = GenerateOrganisation(agent);

                    organisationId = await CreateAsync(entity);
                    agentReference = organisationId.GetEntityReference<Account>();
                }
            }

            return agentReference;
        }

        private static Account GenerateOrganisation(Agent agent)
        {
            Account entity = new()
            {
                Name = agent.ClientNameOrganisation,
                EMailAddress1 = agent.ClientEmail,
                Telephone1 = agent.ClientPhoneNumber
            };

            if (!string.IsNullOrWhiteSpace(agent.ClientAddress?.Line1))
            {
                entity.Address1_Line1 = agent.ClientAddress.Line1;
            }

            if (!string.IsNullOrWhiteSpace(agent.ClientAddress?.Town))
            {
                entity.Address1_City = agent.ClientAddress.Town;
            }

            if (!string.IsNullOrWhiteSpace(agent.ClientAddress?.Postcode))
            {
                entity.Address1_PostalCode = agent.ClientAddress.Postcode;
            }

            return entity;
        }

        private static Contact GenerateContact(Agent agent)
        {
            var entity = new Contact
            {
                FirstName = agent.ClientNameIndividual.FirstName,
                LastName = agent.ClientNameIndividual.LastName
            };

            if (!string.IsNullOrWhiteSpace(agent.ClientEmail))
            {
                entity.EMailAddress1 = agent.ClientEmail;
            }

            if (!string.IsNullOrWhiteSpace(agent.ClientPhoneNumber))
            {
                entity.Telephone1 = agent.ClientPhoneNumber;
            }

            if (!string.IsNullOrWhiteSpace(agent.ClientAddress?.Line1))
            {
                entity.Address1_Line1 = agent.ClientAddress.Line1;
            }

            if (!string.IsNullOrWhiteSpace(agent.ClientAddress?.Town))
            {
                entity.Address1_City = agent.ClientAddress.Town;
            }

            if (!string.IsNullOrWhiteSpace(agent.ClientAddress?.Postcode))
            {
                entity.Address1_PostalCode = agent.ClientAddress.Postcode;
            }

            return entity;
        }
    }
}