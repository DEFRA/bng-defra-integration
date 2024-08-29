using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Domain.Request;
using DEFRA.NE.BNG.Integration.Function.UnitTests;

namespace DEFRA.NE.BNG.Integration.Function.Functions.Tests
{
    public class BNGOperatorDeveloperFromQueueTests : TestBase
    {
        private readonly Mock<IDeveloperRegistrationFacade> facade;
        private readonly BngOperatorDeveloperFromQueue systemUnderTest;
        private readonly Mock<ILogger<BngOperatorDeveloperFromQueue>> logger;


        public BNGOperatorDeveloperFromQueueTests() : base()
        {
            logger = new Mock<ILogger<BngOperatorDeveloperFromQueue>>();
            facade = new Mock<IDeveloperRegistrationFacade>();

            systemUnderTest = new BngOperatorDeveloperFromQueue(logger.Object, facade.Object);
        }

        [Fact]
        public void BNGOperatorDeveloperFromQueue()
        {
            FluentActions.Invoking(() => new BngOperatorDeveloperFromQueue(logger.Object, facade.Object))
                         .Should()
                         .NotThrow();
        }


        [Fact]
        public async Task Run()
        {
            var data = await File.ReadAllTextAsync("TestData/allocation/allocation.agent.individual.json");

            var receivedMessage = ServiceBusModelFactory.ServiceBusReceivedMessage(body: new BinaryData(data),
                                                                                   messageId: Guid.NewGuid().ToString(),
            contentType: "application/json");

            facade.Setup(x => x.OrchestrationBNG(It.IsAny<DeveloperRegistration>(),bng_casetype.Allocation))
                    .ReturnsAsync(Guid.NewGuid());

            var actual = await systemUnderTest.Run(receivedMessage);

            actual.Should().Be(data);
            facade.Verify(x => x.OrchestrationBNG(It.IsAny<DeveloperRegistration>(), bng_casetype.Allocation), Times.Once());
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
            facade.Verify(x => x.OrchestrationBNG(It.IsAny<DeveloperRegistration>(), bng_casetype.Allocation), Times.Never());
        }

        [Fact]
        public async Task Run_FacadeOrchestrationFails()
        {
            var data = await File.ReadAllTextAsync("TestData/allocation/allocation.agent.individual.json");

            var receivedMessage = ServiceBusModelFactory.ServiceBusReceivedMessage(body: new BinaryData(data),
                                                                                   messageId: Guid.NewGuid().ToString(),
            contentType: "text/plain");

            facade.Setup(x => x.OrchestrationBNG(It.IsAny<DeveloperRegistration>(), bng_casetype.Allocation))
                    .Throws<Exception>();

            var actual = await systemUnderTest.Run(receivedMessage);

            actual.Should().Be($"FAILEDMESSAGE{data}");
            facade.Verify(x => x.OrchestrationBNG(It.IsAny<DeveloperRegistration>(), bng_casetype.Allocation), Times.Once());
        }

    }
}