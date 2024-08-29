using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using DEFRA.NE.BNG.Integration.Domain.Models;
using Microsoft.Extensions.Logging;

namespace DEFRA.NE.BNG.Integration.Infrastructure.EntityProcessors
{
    /// <summary>
    /// Implements IEntityProcessor for an LocalPlanningAuthorityProcessor
    /// </summary>
    public class LocalPlanningAuthorityProcessor
    {
        public async Task<Guid> FindFirst(LocalPlanningAuthority planningAuthority, IDataverseService dataverseService, ILogger logger)
        {
            logger.LogDebug("Executing {method}", nameof(FindFirst));

            var id = Guid.Empty;

            if (planningAuthority != null && !string.IsNullOrEmpty(planningAuthority.Name))
            {
                var query = DataverseExtensions.GetQuery<bng_LocalPlanningAuthority>();
                query.AddEqualOperatorCondition<bng_LocalPlanningAuthority>(nameof(bng_LocalPlanningAuthority.bng_Name), planningAuthority.Name);

                if (!string.IsNullOrEmpty(planningAuthority.Code))
                {
                    query.AddEqualOperatorCondition<bng_LocalPlanningAuthority>(nameof(bng_LocalPlanningAuthority.bng_lpaid), planningAuthority.Code);
                }

                var result = await dataverseService.RetrieveFirstRecord<bng_LocalPlanningAuthority>(query);

                if (result != null)
                {
                    id = result.Id;
                }
            }

            return id;
        }
    }
}
