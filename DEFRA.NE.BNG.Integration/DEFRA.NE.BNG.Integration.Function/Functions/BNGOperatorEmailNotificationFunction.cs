using Azure.Messaging.ServiceBus;
using DEFRA.NE.BNG.Integration.Domain.Constants;
using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using DEFRA.NE.BNG.Integration.Infrastructure.Utilities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;
using System.Text;

namespace DEFRA.NE.BNG.Integration.Function.Functions
{
    public class BngOperatorEmailNotificationFunction
    {
        private readonly IUkGovNotifyFacade facade;
        private readonly ILogger<BngOperatorEmailNotificationFunction> logger;

        public BngOperatorEmailNotificationFunction(ILogger<BngOperatorEmailNotificationFunction> logger, IUkGovNotifyFacade facade)
        {
            this.facade = facade;
            this.logger = logger;
        }

        [Function(nameof(BngOperatorEmailNotificationFunction))]
        public async Task Run([ServiceBusTrigger("ne.bng.operatornotification.outbound", Connection = "ServiceBusConnectionPowerapps")] ServiceBusReceivedMessage message)
        {
            var notificationQueueItem = "";
            try
            {
                notificationQueueItem = Encoding.UTF8.GetString(message.Body);
                logger.LogInformation("C# ServiceBus queue trigger function processed message: {Message}", notificationQueueItem);

                var emailNotificationContext = SerializationHelper.DeserializeRemoteContextTypeString<RemoteExecutionContext>(notificationQueueItem);

                var targetEntity = (Entity)emailNotificationContext.InputParameters[EnvironmentConstants.ExecutionContextTarget];

                await facade.EmailNotification(targetEntity.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed message: {Message} {NotificationQueueItem}", ex.Message, notificationQueueItem);
                throw;
            }
        }
    }
}