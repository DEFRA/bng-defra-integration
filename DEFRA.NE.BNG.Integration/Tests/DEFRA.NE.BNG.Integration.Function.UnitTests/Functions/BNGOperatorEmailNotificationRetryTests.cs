using DEFRA.NE.BNG.Integration.Function.UnitTests;
using Microsoft.Azure.Functions.Worker;

namespace DEFRA.NE.BNG.Integration.Function.Functions.Tests
{
    public class BNGOperatorEmailNotificationRetryTests : TestBase
    {
        private readonly Mock<IUKGovNotifyRetriggerFacade> facade;
        private readonly BngOperatorEmailNotificationRetry systemUnderTest;
        private readonly Mock<ILogger<BngOperatorEmailNotificationRetry>> logger;

        public BNGOperatorEmailNotificationRetryTests() : base()
        {
            logger = new Mock<ILogger<BngOperatorEmailNotificationRetry>>();
            facade = new Mock<IUKGovNotifyRetriggerFacade>();

            systemUnderTest = new BngOperatorEmailNotificationRetry(logger.Object, facade.Object);
        }

        [Fact]
        public void BNGOperatorEmailNotificationRetry()
        {
            FluentActions.Invoking(() => new BngOperatorEmailNotificationRetry(logger.Object, facade.Object))
                         .Should()
                         .NotThrow();
        }


        [Fact]
        public async Task Run()
        {
            facade.Setup(x => x.RetrySendingMail());

            await FluentActions.Invoking(() => systemUnderTest.Run(new TimerInfo()))
                               .Should()
                               .NotThrowAsync();

            facade.Verify(x => x.RetrySendingMail(), Times.Once());
        }

        [Fact]
        public async Task Run_FacadeOrchestrationFails()
        {
            facade.Setup(x => x.RetrySendingMail())
                  .Throws<Exception>();

            await FluentActions.Invoking(() => systemUnderTest.Run(new TimerInfo()))
                               .Should()
                               .NotThrowAsync();

            facade.Verify(x => x.RetrySendingMail(), Times.Once());
        }

    }
}