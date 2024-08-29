using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Domain.Request;
using DEFRA.NE.BNG.Integration.Model.Request;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace DEFRA.NE.BNG.Integration.Domain.Interfaces
{
    public interface IDataverseService
    {
        public IBlobClientAccess BlobClientAccess { get; }

        Task<Guid> CreateAsync(Entity entity);
        Task<List<Guid>> CreateListAsync(List<Entity> entityListToCreate);
        Task<T> RetrieveAsync<T>(Guid id, ColumnSet columnSet) where T : Entity;
        Task<Guid> RetrieveFirstRecordForEntity(QueryBase query);
        Task<T> RetrieveFirstRecord<T>(QueryBase query) where T : Entity;
        Task<EntityCollection> RetrieveMultipleAsync(QueryBase query);
        Task<List<T>> RetrieveMultipleAsync<T>(QueryBase query) where T : Entity;
        Task UpdateAsync(Entity entity);
        Task<OrganizationResponse> ExecuteAsync(OrganizationRequest request);
        Task AssosiateTwoEntitiesRecords(EntityReference entity1, string relationshipName, List<EntityReference> entityList);
        Task AssociateAsync(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities);
        Task UpsertEntity(Entity entity);
        Task CreateAttachments(List<FileDetails> files, EntityReference ownerEntity);
        Task CreateAttachment(FileDetails file, EntityReference ownerEntity);
        Task<IList<bng_BNGConfiguration>> RetrieveConfigurations(string configurationValue);
        Task<EntityReference> UpsertClient(Agent agent);
    }
}
