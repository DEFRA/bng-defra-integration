using System.Text;
using Azure.Messaging.ServiceBus;
using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using DEFRA.NE.BNG.Integration.Domain.JsonRequestRoot;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DEFRA.NE.BNG.Integration.Function.Functions
{
    public class BngOperatorCombinedFromQueue
    {
        private readonly ILogger<BngOperatorCombinedFromQueue> logger;
        private readonly ICombinedRegistrationFacade facade;

        public BngOperatorCombinedFromQueue(ILogger<BngOperatorCombinedFromQueue> logger, ICombinedRegistrationFacade facade)
        {
            this.logger = logger;
            this.facade = facade;
        }

        [Function(nameof(BngOperatorCombinedFromQueue))]
        [ServiceBusOutput("ne.bng.combined.journal", Connection = "ServiceBusConnection")]
        public async Task<string> Run([ServiceBusTrigger("ne.bng.combined.inbound", Connection = "ServiceBusConnection")] ServiceBusReceivedMessage message)
        {
            var errorMessage = "FAILEDMESSAGE";
            var queueMessage = Encoding.UTF8.GetString(message.Body);
            logger.LogInformation("C# Queue trigger function processed: {data}", queueMessage);
            try
            {
                var requestLoData = JsonConvert.DeserializeObject<CombinedRequestPayload>(queueMessage);

                if (requestLoData?.combinedCase != null)
                {
                    await facade.OrchestrationBNG(requestLoData.combinedCase);
                    errorMessage = "";
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{error}", ex.Message);
            }
            return $"{errorMessage}{queueMessage}";
        }
    }
}
