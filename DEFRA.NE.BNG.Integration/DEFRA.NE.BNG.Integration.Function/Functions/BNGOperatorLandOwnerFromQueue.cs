using System.Text;
using Azure.Messaging.ServiceBus;
using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using DEFRA.NE.BNG.Integration.Domain.JsonRequestRoot;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DEFRA.NE.BNG.Integration.Function.Functions
{
    public class BngOperatorLandOwnerFromQueue
    {
        private readonly ILandOwnerRegistrationFacade facade;
        private readonly ILogger<BngOperatorLandOwnerFromQueue> logger;

        public BngOperatorLandOwnerFromQueue(ILogger<BngOperatorLandOwnerFromQueue> logger, ILandOwnerRegistrationFacade facade)
        {
            this.facade = facade;
            this.logger = logger;
        }

        [Function(nameof(BngOperatorLandOwnerFromQueue))]
        [ServiceBusOutput("ne.bng.landowner.journal", Connection = "ServiceBusConnection")]
        public async Task<string> Run([ServiceBusTrigger("ne.bng.landowner.inbound", Connection = "ServiceBusConnection")] ServiceBusReceivedMessage message)
        {
            var errorMessage = "FAILEDMESSAGE";
            var queueMessage = Encoding.UTF8.GetString(message.Body);
            logger.LogInformation("C# ServiceBus queue trigger function processed message: {data}", queueMessage);
            try
            {
                var requestLoData = JsonConvert.DeserializeObject<LandOwnerRequestPayload>(queueMessage);

                if (requestLoData?.LandownerGainSiteRegistration != null)
                {
                    await facade.OrchestrationBNG(requestLoData.LandownerGainSiteRegistration, Domain.Models.bng_casetype.Registration);
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