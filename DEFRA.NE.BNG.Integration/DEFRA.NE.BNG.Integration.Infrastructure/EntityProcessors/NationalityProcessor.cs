using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using DEFRA.NE.BNG.Integration.Domain.Models;
using Microsoft.Extensions.Logging;

namespace DEFRA.NE.BNG.Integration.Infrastructure.EntityProcessors
{
    /// <summary>
    /// Implements IEntityProcessor for an HabitatProcessor
    /// </summary>
    public class NationalityProcessor
    {
        /// <summary>
        /// Creates an Habitat in Dataverse
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <returns></returns>
        public async Task<Guid> Create(Nationality entityToCreate, IDataverseService dataverseService, ILogger logger)
        {
            logger.LogDebug("Executing {method}", nameof(Create));

            Guid creationId;

            var query = DataverseExtensions.GetQuery<bng_Nationality>();
            query.AddEqualOperatorCondition<bng_Nationality>(nameof(bng_Nationality.bng_Name), entityToCreate.Name);

            var entity = await dataverseService.RetrieveFirstRecord<bng_Nationality>(query);

            if (entity != null)
            {
                creationId = entity.Id;
            }
            else
            {
                var nationality = new bng_Nationality
                {
                    bng_Name = entityToCreate.Name
                };
                creationId = await dataverseService.CreateAsync(nationality);
            }

            return creationId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityListToCreate"></param>
        /// <param name="dataverseService"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public async Task<List<Guid>> CreateList(List<Nationality> entityListToCreate, IDataverseService dataverseService, ILogger logger)
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
