using DEFRA.NE.BNG.Integration.Function.UnitTests;

namespace DEFRA.NE.BNG.Integration.Function.Functions.Tests
{
    public class BNGOperatorEmailNotificationFunctionTests : TestBase
    {
        private readonly Mock<IUkGovNotifyFacade> facade;
        private readonly BngOperatorEmailNotificationFunction systemUnderTest;
        private readonly Mock<ILogger<BngOperatorEmailNotificationFunction>> logger;

        public BNGOperatorEmailNotificationFunctionTests() : base()
        {
            logger = new Mock<ILogger<BngOperatorEmailNotificationFunction>>();
            facade = new Mock<IUkGovNotifyFacade>();

            systemUnderTest = new BngOperatorEmailNotificationFunction(logger.Object, facade.Object);
        }

        [Fact]
        public void BNGOperatorEmailNotificationFunction()
        {
            FluentActions.Invoking(() => new BngOperatorEmailNotificationFunction(logger.Object, facade.Object))
                          .Should()
                          .NotThrow();
        }


        [Fact]
        public async Task Run()
        {
            var data = await File.ReadAllTextAsync("TestData/notification/Notice of intent.json");

            var receivedMessage = ServiceBusModelFactory.ServiceBusReceivedMessage(body: new BinaryData(data),
                                                                                   messageId: Guid.NewGuid().ToString(),
            contentType: "application/json");

            facade.Setup(x => x.EmailNotification(It.IsAny<Guid>()))
                    .ReturnsAsync(true);

            await FluentActions.Invoking(() => systemUnderTest.Run(receivedMessage))
                               .Should()
                               .NotThrowAsync();

            facade.Verify(x => x.EmailNotification(It.IsAny<Guid>()), Times.Once());
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
                               .ThrowAsync<Exception>();

            facade.Verify(x => x.EmailNotification(It.IsAny<Guid>()), Times.Never());
        }

        [Fact]
        public async Task Run_FacadeOrchestrationFails()
        {
            var data = await File.ReadAllTextAsync("TestData/notification/Notice of intent.json");

            var receivedMessage = ServiceBusModelFactory.ServiceBusReceivedMessage(body: new BinaryData(data),
                                                                                   messageId: Guid.NewGuid().ToString(),
            contentType: "text/plain");

            facade.Setup(x => x.EmailNotification(It.IsAny<Guid>()))
                   .Throws<Exception>();

            await FluentActions.Invoking(() => systemUnderTest.Run(receivedMessage))
                               .Should()
                               .ThrowAsync<Exception>();

            facade.Verify(x => x.EmailNotification(It.IsAny<Guid>()), Times.Once());
        }
    }
}