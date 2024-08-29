using DEFRA.NE.BNG.Integration.Function.UnitTests;

namespace DEFRA.NE.BNG.Integration.Function.Functions.Tests
{
    public class BNGOperatorOrderFromQueueTests : TestBase
    {
        private readonly Mock<IOrderFacade> facade;
        private readonly Mock<ILogger<BngOperatorOrderFromQueue>> logger;
        private readonly BngOperatorOrderFromQueue systemUnderTest;

        public BNGOperatorOrderFromQueueTests() : base()
        {
            logger = new Mock<ILogger<BngOperatorOrderFromQueue>>();
            facade = new Mock<IOrderFacade>();

            systemUnderTest = new BngOperatorOrderFromQueue(logger.Object, facade.Object, environmentVariableReader.Object);
        }

        [Fact]
        public void BNGOperatorOrderFromQueue()
        {
            FluentActions.Invoking(() => new BngOperatorOrderFromQueue(logger.Object, facade.Object, environmentVariableReader.Object))
                         .Should()
                         .NotThrow();
        }

        [Fact]
        public async Task Run()
        {
            var data = await File.ReadAllTextAsync("TestData/credits/credits-purchase-individual.example.json");

            var receivedMessage = ServiceBusModelFactory.ServiceBusReceivedMessage(body: new BinaryData(data),
                                                                                   messageId: Guid.NewGuid().ToString(),
            contentType: "application/json");

            facade.Setup(x => x.OrchestrationBNG(It.IsAny<Order>(), It.IsAny<IConfigurationReader>()))
                    .ReturnsAsync(Guid.NewGuid());

            var actual = await systemUnderTest.Run(receivedMessage);

            actual.Should().Be(data);
            facade.Verify(x => x.OrchestrationBNG(It.IsAny<Order>(), It.IsAny<IConfigurationReader>()), Times.Once());
        }

        [Fact]
        public async Task Run_InvalidPayload()
        {
            var data = "Invalid data";

            var receivedMessage = ServiceBusModelFactory.ServiceBusReceivedMessage(body: new BinaryData(data),
                                                                                   messageId: Guid.NewGuid().ToString(),
            contentType: "text/plain");

            var actual = await systemUnderTest.Run(receivedMessage);

            actual.Should().Be($"FAILEDMESSAGE{data}");
            facade.Verify(x => x.OrchestrationBNG(It.IsAny<Order>(), It.IsAny<IConfigurationReader>()), Times.Never());
        }

        [Fact]
        public async Task Run_FacadeOrchestrationFails()
        {
            var data = await File.ReadAllTextAsync("TestData/credits/credits-purchase-individual.example.json");

            var receivedMessage = ServiceBusModelFactory.ServiceBusReceivedMessage(body: new BinaryData(data),
                                                                                   messageId: Guid.NewGuid().ToString(),
            contentType: "text/plain");

            facade.Setup(x => x.OrchestrationBNG(It.IsAny<Order>(), It.IsAny<IConfigurationReader>()))
                    .Throws<Exception>();

            var actual = await systemUnderTest.Run(receivedMessage);

            actual.Should().Be($"FAILEDMESSAGE{data}");
            facade.Verify(x => x.OrchestrationBNG(It.IsAny<Order>(), It.IsAny<IConfigurationReader>()), Times.Once());
        }
    }
}