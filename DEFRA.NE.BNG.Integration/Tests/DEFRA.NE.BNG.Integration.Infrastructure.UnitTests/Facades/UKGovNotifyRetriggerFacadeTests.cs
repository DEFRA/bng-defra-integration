using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Exceptions;
using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Domain.Response;
using DEFRA.NE.BNG.Integration.Infrastructure.Facades;

namespace DEFRA.NE.BNG.Integration.Infrastructure.UnitTests.Facades
{
    public class UKGovNotifyRetriggerFacadeTests : TestBase<UKGovNotifyRetriggerFacade>
    {
        private UKGovNotifyRetriggerFacade systemUnderTest;

        private Mock<IUkGovNotifyFacade> ukGovNotifyFacade;

        public UKGovNotifyRetriggerFacadeTests() : base()
        {
            ukGovNotifyFacade = new Mock<IUkGovNotifyFacade>();

            systemUnderTest = new UKGovNotifyRetriggerFacade(logger.Object,
                                                             ukGovNotifyFacade.Object,
                                                             mailService.Object,
                                                             environmentVariableReader.Object,
                                                             dataverseService.Object,
                                                             emailNotificationRequestGenerator.Object);
        }

        [Fact]
        public async Task CanRetrySendingMail_GetFailedNotifications_ReturnsNoRecord()
        {
            List<bng_Notify> entityCollection = null;

            ukGovNotifyFacade.Setup(x => x.GetFailedNotifications())
                             .ReturnsAsync(entityCollection);

            await systemUnderTest.Invoking(async x => await x.RetrySendingMail())
                                 .Should()
                                 .NotThrowAsync();

            logger.VerifyAll();

            ukGovNotifyFacade.Verify(x => x.UpdateNotifyEntity(It.IsAny<Entity>(), null, It.IsAny<string>(), false, It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task CanRetrySendingMail_GetFailedNotifications_ReturnsRecord()
        {
            bng_Notify entity = new() { Id = Guid.NewGuid() };
            var notificationEmailService = "";
            var mailNotificationRequestWrapper = new EmailNotificationRequestWrapper();
            var emailNotificationResponse = new EmailNotificationResponse(); ;

            var entityCollection = new List<bng_Notify>();
            entityCollection.Add(entity);

            ukGovNotifyFacade.Setup(x => x.GetFailedNotifications())
                             .ReturnsAsync(entityCollection);

            emailNotificationRequestGenerator.Setup(x => x.PrepairNotificationRequestData(It.IsAny<bng_Notify>()))
                             .ReturnsAsync(mailNotificationRequestWrapper);

            environmentVariableReader.Setup(x => x.Read(It.IsAny<string>())).Returns(notificationEmailService);

            mailService.Setup(x => x.SendMailAsync(It.IsAny<EmailNotificationRequest>(), It.IsAny<string>()))
                       .ReturnsAsync(emailNotificationResponse);

            await systemUnderTest.Invoking(async x => await x.RetrySendingMail())
                                 .Should()
                                 .NotThrowAsync();

            logger.VerifyAll();
            ukGovNotifyFacade.Verify(x => x.GetFailedNotifications(), Times.Once);
            emailNotificationRequestGenerator.Verify(x => x.PrepairNotificationRequestData(It.IsAny<bng_Notify>()), Times.AtLeastOnce);
            environmentVariableReader.Verify(x => x.Read(It.IsAny<string>()), Times.AtLeastOnce);
            mailService.Verify(x => x.SendMailAsync(It.IsAny<EmailNotificationRequest>(), It.IsAny<string>()), Times.AtLeastOnce);

            ukGovNotifyFacade.Verify(x => x.UpdateNotifyEntity(It.IsAny<Entity>(), null, It.IsAny<string>(), false, It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task CanRetrySendingMail_GetFailedNotifications_ReturnsRecord_2()
        {
            bng_Notify entity = new() { Id = Guid.NewGuid() };
            var notificationEmailService = "";
            var mailNotificationRequestWrapper = new EmailNotificationRequestWrapper();
            var emailNotificationResponse = new EmailNotificationResponse(); ;

            var entityCollection = new List<bng_Notify>();
            entityCollection.Add(entity);

            ukGovNotifyFacade.Setup(x => x.GetFailedNotifications())
                             .ReturnsAsync(entityCollection);

            emailNotificationRequestGenerator.Setup(x => x.PrepairNotificationRequestData(It.IsAny<bng_Notify>()))
                             .ReturnsAsync(mailNotificationRequestWrapper);

            environmentVariableReader.Setup(x => x.Read(It.IsAny<string>())).Returns(notificationEmailService);

            mailService.Setup(x => x.SendMailAsync(It.IsAny<EmailNotificationRequest>(), It.IsAny<string>()))
                    .Throws<MailNotificationFailedException>();

            await systemUnderTest.Invoking(async x => await x.RetrySendingMail())
                                 .Should()
                                 .NotThrowAsync();

            logger.VerifyAll();
            ukGovNotifyFacade.Verify(x => x.GetFailedNotifications(), Times.Once);
            emailNotificationRequestGenerator.Verify(x => x.PrepairNotificationRequestData(It.IsAny<bng_Notify>()), Times.AtLeastOnce);
            environmentVariableReader.Verify(x => x.Read(It.IsAny<string>()), Times.AtLeastOnce);
            mailService.Verify(x => x.SendMailAsync(It.IsAny<EmailNotificationRequest>(), It.IsAny<string>()), Times.AtLeastOnce);

            ukGovNotifyFacade.Verify(x => x.UpdateNotifyEntity(It.IsAny<Entity>(), null, It.IsAny<string>(), false, It.IsAny<int>()), Times.Once);
        }
    }
}