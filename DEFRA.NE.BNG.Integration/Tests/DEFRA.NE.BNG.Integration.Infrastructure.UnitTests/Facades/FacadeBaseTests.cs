using DEFRA.NE.BNG.Integration.Infrastructure.Facades;

namespace DEFRA.NE.BNG.Integration.Infrastructure.UnitTests.Facades
{
    public class FacadeBaseTests : TestBase<UkGovNotifyFacade>
    {
        [Fact]
        public void CanInstantiate()
        {
            FluentActions.Invoking(() => new UkGovNotifyFacade(logger.Object,
                                                               mailService.Object,
                                                               environmentVariableReader.Object,
                                                               dataverseService.Object,
                                                               mappingManager.Object,
                                                               notifyManager.Object,
                                                               emailNotificationRequestGenerator.Object))
                         .Should()
                         .NotThrow();
        }
    }
}