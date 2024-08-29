using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Domain.Request;
using Microsoft.Extensions.Logging;

namespace DEFRA.NE.BNG.Integration.Infrastructure.EntityProcessors
{
    /// <summary>
    /// Implements IEntityProcessor for an HabitatProcessor
    /// </summary>
    public class HabitatProcessor
    {

        /// <summary>
        /// Creates an Habitat in Dataverse
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <returns></returns>
        public async Task<Guid> Create(Habitat entityToCreate, IDataverseService dataverseService, ILogger logger)
        {
            logger.LogDebug("Executing {method}", nameof(Create));
            Guid creationId = Guid.Empty;

            var query = DataverseExtensions.GetQuery<bng_HabitatType>();
            query.AddEqualOperatorCondition<bng_HabitatType>(nameof(bng_HabitatType.bng_HabitatName), entityToCreate.Id);

            var entity = await dataverseService.RetrieveFirstRecord<bng_HabitatType>(query);

            if (entity != null)
            {
                creationId = entity.Id;
            }

            return creationId;
        }

        public async Task<List<Guid>> CreateList(List<Habitat> entityListToCreate, IDataverseService dataverseService, ILogger logger)
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
