using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DEFRA.NE.BNG.Integration.Function.Functions
{
    public class BngOperatorEmailNotificationRetry
    {
        private readonly IUKGovNotifyRetriggerFacade facade;
        private readonly ILogger<BngOperatorEmailNotificationRetry> logger;

        public BngOperatorEmailNotificationRetry(ILogger<BngOperatorEmailNotificationRetry> logger, IUKGovNotifyRetriggerFacade facade)
        {
            this.facade = facade;
            this.logger = logger;
        }

        [Function("BNGOperatorEmailNotificationRetry")]
        public async Task Run([TimerTrigger("%NotificationRetryCRON%")] TimerInfo myTimer)
        {
            try
            {
                logger.LogInformation("C# Timer trigger function executed at: {time} {paramter}", DateTime.Now, myTimer);
                await facade.RetrySendingMail();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{error}", ex.Message);
            }
        }
    }
}