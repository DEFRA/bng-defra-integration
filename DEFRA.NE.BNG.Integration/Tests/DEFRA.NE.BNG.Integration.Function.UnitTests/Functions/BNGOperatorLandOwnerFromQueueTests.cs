using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Domain.Request;
using DEFRA.NE.BNG.Integration.Function.UnitTests;

namespace DEFRA.NE.BNG.Integration.Function.Functions.Tests
{
    public class BNGOperatorLandOwnerFromQueueTests : TestBase
    {
        private readonly Mock<ILandOwnerRegistrationFacade> facade;
        private readonly Mock<ILogger<BngOperatorLandOwnerFromQueue>> logger;
        private readonly BngOperatorLandOwnerFromQueue systemUnderTest;

        public BNGOperatorLandOwnerFromQueueTests() : base()
        {
            logger = new Mock<ILogger<BngOperatorLandOwnerFromQueue>>();
            facade = new Mock<ILandOwnerRegistrationFacade>();

            systemUnderTest = new BngOperatorLandOwnerFromQueue(logger.Object, facade.Object);
        }

        [Fact]
        public void BNGOperatorLandOwnerFromQueue()
        {
            FluentActions.Invoking(() => new BngOperatorLandOwnerFromQueue(logger.Object, facade.Object))
                          .Should()
                          .NotThrow();
        }


        [Fact]
        public async Task Run()
        {
            var data = await File.ReadAllTextAsync("TestData/registration/agent-applyingfor-individual.json");

            var receivedMessage = ServiceBusModelFactory.ServiceBusReceivedMessage(body: new BinaryData(data),
                                                                                   messageId: Guid.NewGuid().ToString(),
            contentType: "application/json");

            facade.Setup(x => x.OrchestrationBNG(It.IsAny<GainSiteRegistration>(), It.IsAny<bng_casetype>()))
                    .ReturnsAsync(Guid.NewGuid());

            var actual = await systemUnderTest.Run(receivedMessage);

            actual.Should().Be(data);
            facade.Verify(x => x.OrchestrationBNG(It.IsAny<GainSiteRegistration>(), It.IsAny<bng_casetype>()), Times.Once());
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
            facade.Verify(x => x.OrchestrationBNG(It.IsAny<GainSiteRegistration>(), It.IsAny<bng_casetype>()), Times.Never());
        }

        [Fact]
        public async Task Run_FacadeOrchestrationFails()
        {
            var data = await File.ReadAllTextAsync("TestData/registration/agent-applyingfor-individual.json");

            var receivedMessage = ServiceBusModelFactory.ServiceBusReceivedMessage(body: new BinaryData(data),
                                                                                   messageId: Guid.NewGuid().ToString(),
            contentType: "text/plain");

            facade.Setup(x => x.OrchestrationBNG(It.IsAny<GainSiteRegistration>(), It.IsAny<bng_casetype>()))
                    .Throws<Exception>();

            var actual = await systemUnderTest.Run(receivedMessage);

            actual.Should().Be($"FAILEDMESSAGE{data}");
            facade.Verify(x => x.OrchestrationBNG(It.IsAny<GainSiteRegistration>(), It.IsAny<bng_casetype>()), Times.Once());
        }

    }
}