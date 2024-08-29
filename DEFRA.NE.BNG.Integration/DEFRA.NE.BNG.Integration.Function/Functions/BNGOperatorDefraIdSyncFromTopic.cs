using Azure.Messaging.ServiceBus;
using DEFRA.NE.BNG.Integration.Domain.Exceptions;
using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using DEFRA.NE.BNG.Integration.Domain.JsonRequestRoot;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace DEFRA.NE.BNG.Integration.Function.Functions
{
    public class BngOperatorDefraIdSyncFromTopic
    {
        private readonly IDefraIdSyncFacade facade;
        private readonly ILogger<BngOperatorDefraIdSyncFromTopic> logger;

        public BngOperatorDefraIdSyncFromTopic(ILogger<BngOperatorDefraIdSyncFromTopic> logger, IDefraIdSyncFacade facade)
        {
            this.facade = facade;
            this.logger = logger;
        }

        [Function(nameof(BngOperatorDefraIdSyncFromTopic))]
        public async Task Run([ServiceBusTrigger("customer-sync-broker", "%DefraIdSubscription%", Connection = "ServiceBusDefraIDConnection")] ServiceBusReceivedMessage message)
        {
            var defraIdQueueItem = Encoding.UTF8.GetString(message.Body);
            logger.LogInformation("Message Id: {Id}. Inside BNGOperatorDefraIdSync processed: {Message}", message.MessageId, defraIdQueueItem);

            try
            {
                var data = JsonConvert.DeserializeObject<DefraIdRequestPayload>(defraIdQueueItem);

                if (data?.Recorddata != null || data?.Metadata != null)
                {
                    await facade.UpdateUserAndAccountFromDefraId(data);
                }
                else
                {
                    throw new InvalidPayloadException(defraIdQueueItem);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed message: {Message} {DefraIdQueueItem}", ex.Message, defraIdQueueItem);
                throw;
            }
        }
    }
}