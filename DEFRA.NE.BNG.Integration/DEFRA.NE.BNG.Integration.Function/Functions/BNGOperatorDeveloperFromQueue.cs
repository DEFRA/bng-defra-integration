using Azure.Messaging.ServiceBus;
using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using DEFRA.NE.BNG.Integration.Domain.JsonRequestRoot;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace DEFRA.NE.BNG.Integration.Function.Functions
{
    public class BngOperatorDeveloperFromQueue
    {
        private readonly IDeveloperRegistrationFacade facade;
        private readonly ILogger<BngOperatorDeveloperFromQueue> logger;

        public BngOperatorDeveloperFromQueue(ILogger<BngOperatorDeveloperFromQueue> logger, IDeveloperRegistrationFacade facade)
        {
            this.facade = facade;
            this.logger = logger;
        }

        [Function(nameof(BngOperatorDeveloperFromQueue))]
        [ServiceBusOutput("ne.bng.developer.journal", Connection = "ServiceBusConnection")]
        public async Task<string> Run([ServiceBusTrigger("ne.bng.developer.inbound", Connection = "ServiceBusConnection")] ServiceBusReceivedMessage message)
        {
            var errorMessage = "FAILEDMESSAGE";
            var queueMessage = Encoding.UTF8.GetString(message.Body);
            logger.LogInformation("C# Queue trigger function processed: {data}", queueMessage);
            try
            {
                var requestLoData = JsonConvert.DeserializeObject<DeveloperRequestPayload>(queueMessage);

                if (requestLoData?.DeveloperRegistration != null)
                {
                    await facade.OrchestrationBNG(requestLoData.DeveloperRegistration);
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