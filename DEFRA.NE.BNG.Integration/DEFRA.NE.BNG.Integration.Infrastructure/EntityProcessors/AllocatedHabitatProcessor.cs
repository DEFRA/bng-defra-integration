using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using DEFRA.NE.BNG.Integration.Domain.Models;
using Microsoft.Extensions.Logging;

namespace DEFRA.NE.BNG.Integration.Infrastructure.EntityProcessors
{
    /// <summary>
    /// Implements IEntityProcessor for an AllocatedHabitatProcessor
    /// </summary>
    public class AllocatedHabitatProcessor
    {
        /// <summary>
        /// Creates an AllocatedHabitat in Dataverse
        /// </summary>
        /// <param name="allocatedHabitat"></param>
        /// <returns></returns>
        public async Task<Guid> Create(AllocatedHabitat allocatedHabitat,
                                       IDataverseService dataverseService,
                                       ILogger logger)
        {
            logger.LogDebug("Executing {method}", nameof(Create));

            Guid creationId = Guid.Empty;

            var query = DataverseExtensions.GetQuery<bng_HabitatType>();
            query.AddEqualOperatorCondition<bng_HabitatType>(nameof(bng_HabitatType.bng_HabitatName), allocatedHabitat.HabitatId);

            var retrievedEntity = await dataverseService.RetrieveFirstRecord<bng_HabitatType>(query);

            if (retrievedEntity != null)
            {

                var entity = new bng_AllocatedHabitats
                {
                    bng_HabitatID = retrievedEntity.Id.GetEntityReference<bng_HabitatType>(),
                    bng_DeveloperRegistrationID = allocatedHabitat.DeveloperRegistrationId.GetEntityReference<bng_DeveloperRegistration>(),
                    bng_AllocatedtothisDevelopment = allocatedHabitat.Area
                };

                if (allocatedHabitat.GainSiteId != Guid.Empty)
                {
                    entity.bng_GainSiteRegistrationID = allocatedHabitat.GainSiteId.GetEntityReference<bng_GainSiteRegistration>();
                }

                if (allocatedHabitat.CaseId != Guid.Empty)
                {
                    entity.bng_CaseID = allocatedHabitat.CaseId.GetEntityReference<bng_Case>();
                }

                creationId = await dataverseService.CreateAsync(entity);
            }
            return creationId;
        }

        public async Task<List<Guid>> CreateList(List<AllocatedHabitat> entityListToCreate, IDataverseService dataverseService, ILogger logger)
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