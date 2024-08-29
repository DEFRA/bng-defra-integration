using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DEFRA.NE.BNG.Integration.Function.Functions
{
    public class BNGOperatorGainsiteValidator
    {
        private readonly ILogger<BNGOperatorGainsiteValidator> logger;
        private readonly IGainsiteValidatorFacade facade;
        public BNGOperatorGainsiteValidator(ILogger<BNGOperatorGainsiteValidator> logger, IGainsiteValidatorFacade facade)
        {
            this.logger = logger;
            this.facade = facade;
        }

        [Function("BNGOperatorGainsiteValidator")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/allocation/gainsite/{gainSiteNumber}")] HttpRequest req, string gainSiteNumber)
        {
            try
            {
                logger.LogInformation("Processing request for Gainsite Id :{gainSiteNumber}", gainSiteNumber);
                if (!string.IsNullOrEmpty(gainSiteNumber))
                {
                    var regValidation = await facade.ValidateGainsiteFromId(gainSiteNumber);
                    return new OkObjectResult(regValidation);
                }
                else
                {
                    logger.LogInformation("Gainsite number is null or empty");
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{error}", ex.Message);
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }

        }
    }
}
