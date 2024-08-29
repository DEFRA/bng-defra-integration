using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using DEFRA.NE.BNG.Integration.Domain.Models;
using Microsoft.Extensions.Logging;

namespace DEFRA.NE.BNG.Integration.Infrastructure.EntityProcessors
{
    /// <summary>
    /// Implements IEntityProcessor for an OrganisationProcessor
    /// </summary>
    public class OrganisationProcessor
    {
        /// <summary>
        /// Creates an Organisation in Dataverse
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <returns></returns>
        public async Task<Guid> Create(Organisation entityToCreate, IDataverseService dataverseService, ILogger logger)
        {
            logger.LogDebug("Executing {method}", nameof(Create));

            Guid creationId = Guid.Empty;

            if (entityToCreate != null)
            {
                var query = DataverseExtensions.GetQuery<Account>();
                query.AddEqualOperatorCondition<Account>(nameof(Account.bng_DefraID), entityToCreate.Id);

                var entity = await dataverseService.RetrieveFirstRecord<Account>(query);

                if (entity != null)
                {
                    creationId = entity.Id;
                }
                else
                {
                    throw new InvalidDataException("Organisation does not exist!");
                }
            }

            return creationId;
        }

        public async Task<List<Guid>> CreateList(List<Organisation> entityListToCreate, IDataverseService dataverseService, ILogger logger)
        {
            var tasks = entityListToCreate.Select(async x =>
            {
                var result = await Create(x, dataverseService, logger);
                return result;
            });

            var tasksArray = await Task.WhenAll(tasks);

            var combinedResults = tasksArray.ToList();
            return combinedResults;
        }
    }
}