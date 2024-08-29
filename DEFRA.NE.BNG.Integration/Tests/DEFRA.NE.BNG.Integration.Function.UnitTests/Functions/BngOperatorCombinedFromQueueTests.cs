using DEFRA.NE.BNG.Integration.Domain.Request;
using DEFRA.NE.BNG.Integration.Function.Functions;

namespace DEFRA.NE.BNG.Integration.Function.UnitTests.Functions
{
    public class BngOperatorCombinedFromQueueTests : TestBase
    {
        private readonly Mock<ICombinedRegistrationFacade> facade;
        private readonly BngOperatorCombinedFromQueue systemUnderTest;
        private readonly Mock<ILogger<BngOperatorCombinedFromQueue>> logger;

        public BngOperatorCombinedFromQueueTests() : base()
        {
            logger = new Mock<ILogger<BngOperatorCombinedFromQueue>>();
            facade = new Mock<ICombinedRegistrationFacade>();

            systemUnderTest = new BngOperatorCombinedFromQueue(logger.Object, facade.Object);
        }

        [Fact]
        public void BNGOperatorDeveloperFromQueue()
        {
            FluentActions.Invoking(() => new BngOperatorCombinedFromQueue(logger.Object, facade.Object))
                         .Should()
                         .NotThrow();
        }

        [Fact]
        public async Task Run()
        {
            var data = await File.ReadAllTextAsync("TestData/combined/combined-case-individual.json");

            var receivedMessage = ServiceBusModelFactory.ServiceBusReceivedMessage(body: new BinaryData(data),
                                                                                   messageId: Guid.NewGuid().ToString(),
            contentType: "application/json");

            facade.Setup(x => x.OrchestrationBNG(It.IsAny<CombinedRegistration>()))
                    .ReturnsAsync(Guid.NewGuid());

            var actual = await systemUnderTest.Run(receivedMessage);

            actual.Should().Be(data);
            facade.Verify(x => x.OrchestrationBNG(It.IsAny<CombinedRegistration>()), Times.Once());
        }
    }
}