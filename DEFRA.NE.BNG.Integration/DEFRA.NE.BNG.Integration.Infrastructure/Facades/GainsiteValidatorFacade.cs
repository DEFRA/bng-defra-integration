using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Exceptions;
using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using DEFRA.NE.BNG.Integration.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk.Query;
using Response = DEFRA.NE.BNG.Integration.Domain.Response;

namespace DEFRA.NE.BNG.Integration.Infrastructure.Facades
{
    public class GainsiteValidatorFacade : FacadeBase, IGainsiteValidatorFacade
    {
        private readonly ILogger<GainsiteValidatorFacade> logger;
        public GainsiteValidatorFacade(ILogger<GainsiteValidatorFacade> logger,
                                       IConfigurationReader environmentVariableReader,
                                       IMailServiceAgent mailService,
                                       IDataverseService dataverseService)
            : base(environmentVariableReader, mailService, dataverseService)
        {
            this.logger = logger;
        }

        public async Task<Response.GainsiteRegistrationIdValidation> ValidateGainsiteFromId(string gainsiteId)
        {
            logger.LogInformation("Inside Facade with GainSite Id: {gainsiteId} ", gainsiteId);
            return await ValidateGainsite(gainsiteId);
        }

        private async Task<Response.GainsiteRegistrationIdValidation> ValidateGainsite(string gainsiteId)
        {
            if (string.IsNullOrWhiteSpace(gainsiteId))
            {
                throw new ArgumentException("Gainsite is npt provided!");
            }

            var query = DataverseExtensions.GetQuery<bng_GainSiteRegistration>();
            query.AddEqualOperatorCondition<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.bng_GainSiteReference), gainsiteId);
            var gainsite = await dataverseService.RetrieveFirstRecord<bng_GainSiteRegistration>(query);

            if (gainsite != null)
            {
                var gainsiteValidation = new Response.GainsiteRegistrationIdValidation
                {
                    gainsiteNumber = gainsiteId,
                    gainsiteStatus = Enum.GetName(typeof(bng_gainsiteregistration_statuscode), gainsite.statuscode.Value),
                    habitats = RetriveHabitatByGainsiteId(gainsite.Id).Result
                };
                return gainsiteValidation;
            }
            else
            {
                var message = "Gainsite not found";
                logger.LogInformation("{message}", message);
                throw new DataNotFoundException(message);
            }
        }

        private async Task<List<Response.Habitat>> RetriveHabitatByGainsiteId(Guid gainsiteId)
        {
            var query = DataverseExtensions.GetQuery<bng_HabitatType>();
            const string gainsiteregId = "bng_gainsiteregistrationid";
            LinkEntity gainsiteLink = new(
                                            bng_HabitatType.EntityLogicalName,
                                            bng_GainSiteRegistration.EntityLogicalName,
                                            gainsiteregId,
                                            gainsiteregId,
                                            JoinOperator.Inner);
            query.LinkEntities.Add(gainsiteLink);
            ConditionExpression condition = new(gainsiteregId, ConditionOperator.Equal, $"{{{gainsiteId}}}");
            gainsiteLink.LinkCriteria.AddCondition(condition);

            var habitatFromQuery = await dataverseService.RetrieveMultipleAsync<bng_HabitatType>(query);

            if (habitatFromQuery != null)
            {
                var habitats = habitatFromQuery.Select(MapHabitatTypeToResponse).ToList();
                return habitats;
            }
            else
            {
                var message = "Habitat not found";
                logger.LogInformation("{message}", message);
                throw new DataNotFoundException(message);
            }
        }

        private Response.Habitat MapHabitatTypeToResponse(bng_HabitatType habitatType)
        {
            var result = new Response.Habitat
            {
                HabitatId = habitatType.bng_HabitatName,
                ProposedHabitatType = habitatType.bng_ProposedHabitatSubTypeLookup?.Name,
                AreaLength = habitatType.bng_Size?.ToString(),
                TotalAllocated = habitatType.bng_Allocated?.ToString(),
                Remaining = habitatType.bng_Remaining?.ToString()
            };

            if (habitatType.bng_HabitatType1 != null && habitatType.bng_HabitatType1.HasValue)
            {
                result.HabitatModule = habitatType.bng_HabitatType1.Value.ToString();
            }

            if (habitatType.bng_Condition != null && habitatType.bng_Condition.HasValue)
            {
                result.Condition = habitatType.bng_Condition.Value.ToString();
            }

            return result;
        }
    }
}
