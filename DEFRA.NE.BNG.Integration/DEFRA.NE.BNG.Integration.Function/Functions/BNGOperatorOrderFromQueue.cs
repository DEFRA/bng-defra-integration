using Azure.Messaging.ServiceBus;
using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using DEFRA.NE.BNG.Integration.Domain.JsonRequestRoot;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace DEFRA.NE.BNG.Integration.Function.Functions
{
    public class BngOperatorOrderFromQueue
    {
        private readonly ILogger<BngOperatorOrderFromQueue> logger;
        private readonly IOrderFacade facade;
        private readonly IConfigurationReader environmentVariableReader;


        public BngOperatorOrderFromQueue(ILogger<BngOperatorOrderFromQueue> logger, IOrderFacade facade,
            IConfigurationReader environmentVariableReader)
        {
            this.logger = logger;
            this.facade = facade;
            this.environmentVariableReader = environmentVariableReader;
        }

        [Function(nameof(BngOperatorOrderFromQueue))]
        [ServiceBusOutput("ne.bng.order.journal", Connection = "ServiceBusConnection")]
        public async Task<string> Run([ServiceBusTrigger("ne.bng.order.inbound", Connection = "ServiceBusConnection")] ServiceBusReceivedMessage message)
        {
            var errorMessage = "FAILEDMESSAGE";
            var queueMessage = Encoding.UTF8.GetString(message.Body);
            logger.LogInformation("C# Queue trigger function processed: {data}", queueMessage);
            try
            {
                var data = JsonConvert.DeserializeObject<OrderRequestPayload>(queueMessage);
                logger.LogInformation("Payload : {data}", queueMessage);

                if (data?.CreditsPurchase != null)
                {
                    await facade.OrchestrationBNG(data.CreditsPurchase, environmentVariableReader);
                    errorMessage = "";
                }
                logger.LogInformation("Payload : processed");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{error}", ex.Message);
            }
            return $"{errorMessage}{queueMessage}";
        }
    }
}