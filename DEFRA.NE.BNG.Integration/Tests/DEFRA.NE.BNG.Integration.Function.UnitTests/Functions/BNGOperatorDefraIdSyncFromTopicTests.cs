using DEFRA.NE.BNG.Integration.Domain.JsonRequestRoot;
using DEFRA.NE.BNG.Integration.Domain.Request;
using DEFRA.NE.BNG.Integration.Function.UnitTests;
using Newtonsoft.Json;


namespace DEFRA.NE.BNG.Integration.Function.Functions.Tests
{
    public class BNGOperatorDefraIdSyncFromTopicTests : TestBase
    {
        private readonly Mock<IDefraIdSyncFacade> facade;
        private readonly BngOperatorDefraIdSyncFromTopic systemUnderTest;
        private readonly DefraIdRequestPayload requestData;
        private readonly Mock<ILogger<BngOperatorDefraIdSyncFromTopic>> logger;

        public BNGOperatorDefraIdSyncFromTopicTests() : base()
        {
            facade = new Mock<IDefraIdSyncFacade>();
            requestData = new DefraIdRequestPayload
            {
                Recorddata = new DefraIdRecorddata()
            };

            logger = new Mock<ILogger<BngOperatorDefraIdSyncFromTopic>>();

            systemUnderTest = new BngOperatorDefraIdSyncFromTopic(logger.Object, facade.Object);
        }

        [Fact]
        public void BNGOperatorDefraIdSyncFromTopic()
        {
            FluentActions.Invoking(() => new BngOperatorDefraIdSyncFromTopic(logger.Object, facade.Object))
                         .Should()
                         .NotThrow();
        }

        [Fact]
        public async Task Run()
        {
            var metaData = new DefraIdMetadata
            {
                Entity = "defra_lobserviceuserlink",
                Operationtype = "create",
                Origin = "origin",
                Recordid = Guid.NewGuid().ToString(),
                TrackingId = Guid.NewGuid().ToString()
            };

            requestData.Metadata = metaData;
            requestData.Recorddata.Account = CreateTestDefraIdAccount();
            var data = JsonConvert.SerializeObject(requestData);

            var receivedMessage = ServiceBusModelFactory.ServiceBusReceivedMessage(body: new BinaryData(data),
                                                                                   messageId: Guid.NewGuid().ToString(),
            contentType: "application/json");

            facade.Setup(x => x.UpdateUserAndAccountFromDefraId(It.IsAny<DefraIdRequestPayload>()));

            await FluentActions.Invoking(() => systemUnderTest.Run(receivedMessage))
                          .Should()
                          .NotThrowAsync();

            facade.Verify(x => x.UpdateUserAndAccountFromDefraId(It.IsAny<DefraIdRequestPayload>()), Times.Once());
        }

        [Fact]
        public async Task Run_InvalidPayload()
        {
            var data = "Invalid data";

            var receivedMessage = ServiceBusModelFactory.ServiceBusReceivedMessage(body: new BinaryData(data),
                                                                                   messageId: Guid.NewGuid().ToString(),
            contentType: "text/plain");

            await FluentActions.Invoking(() => systemUnderTest.Run(receivedMessage))
                         .Should()
                         .ThrowAsync<JsonReaderException>();

            facade.Verify(x => x.UpdateUserAndAccountFromDefraId(It.IsAny<DefraIdRequestPayload>()), Times.Never());
        }

        [Fact]
        public async Task Run_FacadeOrchestrationFails()
        {
            var metaData = new DefraIdMetadata
            {
                Entity = "defra_lobserviceuserlink",
                Operationtype = "create",
                Origin = "origin",
                Recordid = Guid.NewGuid().ToString(),
                TrackingId = Guid.NewGuid().ToString()
            };

            requestData.Metadata = metaData;
            requestData.Recorddata.Account = CreateTestDefraIdAccount();
            var data = JsonConvert.SerializeObject(requestData);

            var receivedMessage = ServiceBusModelFactory.ServiceBusReceivedMessage(body: new BinaryData(data),
                                                                                   messageId: Guid.NewGuid().ToString(),
            contentType: "text/plain");

            facade.Setup(x => x.UpdateUserAndAccountFromDefraId(It.IsAny<DefraIdRequestPayload>()))
                    .Throws<Exception>();

            await FluentActions.Invoking(() => systemUnderTest.Run(receivedMessage))
                               .Should()
                               .ThrowAsync<Exception>();

            facade.Verify(x => x.UpdateUserAndAccountFromDefraId(It.IsAny<DefraIdRequestPayload>()), Times.Once());
        }
    }
}