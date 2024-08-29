using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Exceptions;
using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Domain.Response;
using DEFRA.NE.BNG.Integration.Infrastructure.Facades;

namespace DEFRA.NE.BNG.Integration.Infrastructure.UnitTests.Facades
{
    public class UkGovNotifyFacadeTests : TestBase<UkGovNotifyFacade>
    {
        private UkGovNotifyFacade systemUnderTest;

        public UkGovNotifyFacadeTests() : base()
        {
            systemUnderTest = new UkGovNotifyFacade(logger.Object,
                                                    mailService.Object,
                                                    environmentVariableReader.Object,
                                                    dataverseService.Object,
                                                    mappingManager.Object,
                                                    notifyManager.Object,
                                                    emailNotificationRequestGenerator.Object);

            SetupEnvironmentVariables();
        }

        [Fact]
        public void CanInitializeUkGovNotify()
        {
            environmentVariableReader.Setup(x => x.BuildDataVerseConnectionString())
                .Returns(ConnectionString);

            FluentActions.Invoking(() =>
                                    {
                                        var actual = new UkGovNotifyFacade(logger.Object,
                                                                           mailService.Object,
                                                                           environmentVariableReader.Object,
                                                                           dataverseService.Object,
                                                                           mappingManager.Object,
                                                                           notifyManager.Object,
                                                                           emailNotificationRequestGenerator.Object);
                                    })
                        .Should()
                        .NotThrow();
        }

        [Fact]
        public async Task EmailNotification_SendMailAsyncThrowsMailNotificationFailedException()
        {
            var notificationEmailService = "notificationEmailService";

            Guid entityId = Guid.NewGuid();
            var notifyEntity = new bng_Notify()
            {
                Id = entityId,
                bng_ActionType = bng_notifytype.Created,
                bng_ApplicantID = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()),
                RegardingObjectId = Guid.NewGuid().GetEntityReference<SalesOrder>(),
                bng_RetryCount = 2,
                Description = "Description"
            };

            var replyDictionary = new Dictionary<int, string>
            {
                { 1, "test@email.com" },
                { 2, "test@email.com" }
            };

            EmailNotificationRequestWrapper emailNotificationRequestWrapper = new()
            {
                EmailNotificationRequest = new EmailNotificationRequest(),
            };

            emailNotificationRequestGenerator.Setup(x => x.PrepairNotificationRequestData(It.IsAny<bng_Notify>()))
                                             .ReturnsAsync(emailNotificationRequestWrapper);

            mappingManager.Setup(x => x.GetReplyEmailMapping())
                          .Returns(replyDictionary);

            dataverseService.Setup(x => x.RetrieveAsync<bng_Notify>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                .ReturnsAsync(notifyEntity);

            environmentVariableReader.Setup(x => x.Read(EnvironmentConstants.NotificationEmailService))
            .Returns(notificationEmailService);

            mailService.Setup(x => x.SendMailAsync(It.IsAny<EmailNotificationRequest>(), It.IsAny<string>()))
                        .Throws<MailNotificationFailedException>();

            var actual = await systemUnderTest.EmailNotification(entityId);

            actual.Should().BeFalse();
            environmentVariableReader.VerifyAll();
            dataverseService.VerifyAll();
            dataverseService.Verify(x => x.UpdateAsync(It.IsAny<Entity>()), Times.Once());
        }


        [Fact]
        public async Task EmailNotification_SendMailAsyncThrowsException()
        {
            var notificationEmailService = "notificationEmailService";

            Guid entityId = Guid.NewGuid(); var notifyEntity = new bng_Notify()
            {
                Id = entityId,
                bng_ActionType = bng_notifytype.Created,
                bng_ApplicantID = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()),
                RegardingObjectId = Guid.NewGuid().GetEntityReference<SalesOrder>(),
                bng_RetryCount = 2,
                Description = "Description"
            };
            //var notifyEntity = new Entity(NotifyConstants.LogicalName, entityId);
            //notifyEntity.Attributes.Add(NotifyConstants.ActionType, new OptionSetValue(759150000));
            //notifyEntity.Attributes.Add(NotifyConstants.ApplicantId, new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()));
            //notifyEntity.Attributes.Add(NotifyConstants.RegardingObjectId, new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()));
            //notifyEntity.Attributes.Add(NotifyConstants.RetryCount, 2);
            //notifyEntity.Attributes.Add(NotifyConstants.Description, "Description");

            var replyDictionary = new Dictionary<int, string>
            {
                { 1, "test@email.com" },
                { 2, "test@email.com" }
            };
            EmailNotificationRequestWrapper emailNotificationRequestWrapper = new()
            {
                EmailNotificationRequest = new EmailNotificationRequest(),
            };

            emailNotificationRequestGenerator.Setup(x => x.PrepairNotificationRequestData(It.IsAny<bng_Notify>()))
                                             .ReturnsAsync(emailNotificationRequestWrapper);

            mappingManager.Setup(x => x.GetReplyEmailMapping())
                          .Returns(replyDictionary);

            dataverseService.Setup(x => x.RetrieveAsync<bng_Notify>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                .ReturnsAsync(notifyEntity);

            environmentVariableReader.Setup(x => x.Read(EnvironmentConstants.NotificationEmailService))
            .Returns(notificationEmailService);

            mailService.Setup(x => x.SendMailAsync(It.IsAny<EmailNotificationRequest>(), It.IsAny<string>()))
                        .Throws<Exception>();

            var actual = await systemUnderTest.EmailNotification(entityId);

            actual.Should().BeFalse();
            environmentVariableReader.VerifyAll();
            dataverseService.VerifyAll();
            dataverseService.Verify(x => x.UpdateAsync(It.IsAny<Entity>()), Times.Never());
        }


        [Fact]
        public async Task EmailNotification_ActionType_Create()
        {
            var notificationEmailService = "notificationEmailService";

            Guid entityId = Guid.NewGuid(); var notifyEntity = new bng_Notify()
            {
                Id = entityId,
                bng_ActionType = bng_notifytype.Created,
                bng_ApplicantID = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()),
                RegardingObjectId = Guid.NewGuid().GetEntityReference<SalesOrder>(),
                bng_RetryCount = 2,
                Description = "Description"
            };
            //var notifyEntity = new Entity(NotifyConstants.LogicalName, entityId);
            //notifyEntity.Attributes.Add(NotifyConstants.ActionType, new OptionSetValue(759150000));
            //notifyEntity.Attributes.Add(NotifyConstants.ApplicantId, new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()));
            //notifyEntity.Attributes.Add(NotifyConstants.RegardingObjectId, new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()));
            //notifyEntity.Attributes.Add(NotifyConstants.RetryCount, 2);
            //notifyEntity.Attributes.Add(NotifyConstants.Description, "Description");

            var replyDictionary = new Dictionary<int, string>
            {
                { 1, "test@email.com" },
                { 2, "test@email.com" }
            };

            EmailNotificationRequestWrapper emailNotificationRequestWrapper = new()
            {
                EmailNotificationRequest = new()
                {
                    email_address = "sample@test.com",
                    email_reply_to_id = "reply@test.com",
                    personalisation = new Dictionary<string, string>(),
                    template_id = Guid.NewGuid().ToString(),
                }
            };

            emailNotificationRequestGenerator.Setup(x => x.PrepairNotificationRequestData(It.IsAny<bng_Notify>()))
                                             .ReturnsAsync(emailNotificationRequestWrapper);

            mappingManager.Setup(x => x.GetReplyEmailMapping())
                          .Returns(replyDictionary);

            dataverseService.Setup(x => x.RetrieveAsync<bng_Notify>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                .ReturnsAsync(notifyEntity);

            environmentVariableReader.Setup(x => x.Read(EnvironmentConstants.NotificationEmailService))
            .Returns(notificationEmailService);

            EmailNotificationResponse emailNotificationResponse = new()
            {
                Id = Guid.NewGuid().ToString(),
                Reference = Guid.NewGuid().ToString(),
                Uri = "test",
                Template = new Template()
                {
                    id = Guid.NewGuid().ToString(),
                    uri = "test",
                    version = 1
                }
            };

            mailService.Setup(x => x.SendMailAsync(It.IsAny<EmailNotificationRequest>(),
                                                 It.IsAny<string>()))
                       .ReturnsAsync(emailNotificationResponse);

            var actual = await systemUnderTest.EmailNotification(entityId);


            actual.Should().BeTrue();

            environmentVariableReader.VerifyAll();
            dataverseService.VerifyAll();
            dataverseService.Verify(x => x.UpdateAsync(It.IsAny<Entity>()), Times.Once());
            emailNotificationRequestGenerator.VerifyAll();
        }

        [Fact]
        public async Task EmailNotification_ActionType_Accepted()
        {
            var notificationEmailService = "notificationEmailService";

            Guid entityId = Guid.NewGuid();
            var notifyEntity = new bng_Notify()
            {
                Id = entityId,
                bng_ActionType = bng_notifytype.Accepted,
                bng_ApplicantID = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()),
                RegardingObjectId = Guid.NewGuid().GetEntityReference<SalesOrder>(),
                bng_RetryCount = 2,
                Description = "Description"
            };
            //var notifyEntity = new Entity(NotifyConstants.LogicalName, entityId);
            //notifyEntity.Attributes.Add(NotifyConstants.ActionType, new OptionSetValue(759150001));
            //notifyEntity.Attributes.Add(NotifyConstants.ApplicantId, new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()));
            //notifyEntity.Attributes.Add(NotifyConstants.RegardingObjectId, new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()));
            //notifyEntity.Attributes.Add(NotifyConstants.RetryCount, 2);
            //notifyEntity.Attributes.Add(NotifyConstants.Description, "Description");

            var replyDictionary = new Dictionary<int, string>
            {
                { 1, "test@email.com" },
                { 2, "test@email.com" }
            };

            EmailNotificationRequestWrapper emailNotificationRequestWrapper = new()
            {
                EmailNotificationRequest = new EmailNotificationRequest(),
            };

            emailNotificationRequestGenerator.Setup(x => x.PrepairNotificationRequestData(It.IsAny<bng_Notify>()))
                                             .ReturnsAsync(emailNotificationRequestWrapper);

            mappingManager.Setup(x => x.GetReplyEmailMapping())
                          .Returns(replyDictionary);

            dataverseService.Setup(x => x.RetrieveAsync<bng_Notify>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                .ReturnsAsync(notifyEntity);

            environmentVariableReader.Setup(x => x.Read(EnvironmentConstants.NotificationEmailService))
                .Returns(notificationEmailService);

            var actual = await systemUnderTest.EmailNotification(entityId);

            environmentVariableReader.VerifyAll();
            dataverseService.VerifyAll();
            dataverseService.Verify(x => x.UpdateAsync(It.IsAny<bng_Notify>()), Times.Once());
        }

        [Fact]
        public async Task UpdateNotifyEntity()
        {
            Entity notifyEntity = new();
            EmailNotificationResponse emailNotificationResponse = new();
            string errorMessage = "";
            bool isSuccess = true;
            int actionType = 1;

            await systemUnderTest.Invoking(x => x.UpdateNotifyEntity(notifyEntity, emailNotificationResponse, errorMessage, isSuccess, actionType))
                .Should()
                .NotThrowAsync();

            dataverseService.Verify(x => x.UpdateAsync(It.IsAny<Entity>()), Times.Once);
        }

        [Fact]
        public async Task UpdateNotifyEntity_UpdateAsync_ThrowsException()
        {
            Entity notifyEntity = new();
            EmailNotificationResponse emailNotificationResponse = new();
            string errorMessage = "";
            bool isSuccess = true;
            int actionType = 1;

            dataverseService.Setup(x => x.UpdateAsync(It.IsAny<Entity>()))
                            .Throws<Exception>();

            await systemUnderTest.Invoking(x => x.UpdateNotifyEntity(notifyEntity, emailNotificationResponse, errorMessage, isSuccess, actionType))
                .Should()
                .NotThrowAsync();

            dataverseService.Verify(x => x.UpdateAsync(It.IsAny<Entity>()), Times.Once);
        }

        [Fact]
        public async Task UpdateNotifyEntity_EmailNotificationResponse_RetryCount()
        {
            Entity notifyEntity = new();
            EmailNotificationResponse emailNotificationResponse = null;
            string errorMessage = "";
            bool isSuccess = true;
            int actionType = 1;

            var retryCountEntity = new bng_Notify
            {
                Id = Guid.NewGuid()
            };
            retryCountEntity.Attributes.Add(
                DataverseExtensions.AttributeLogicalName<bng_Notify>(nameof(bng_Notify.bng_RetryCount)), 2);

            dataverseService.Setup(x => x.RetrieveAsync<bng_Notify>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(retryCountEntity);


            await systemUnderTest.Invoking(x => x.UpdateNotifyEntity(notifyEntity, emailNotificationResponse, errorMessage, isSuccess, actionType))
                .Should()
                .NotThrowAsync();

            dataverseService.Verify(x => x.UpdateAsync(It.IsAny<Entity>()), Times.Once);
        }

        [Fact]
        public async Task UpdateNotifyEntity_EmailNotificationResponse_Null()
        {
            Entity notifyEntity = new();
            EmailNotificationResponse emailNotificationResponse = null;
            string errorMessage = "";
            bool isSuccess = true;
            int actionType = 1;

            bng_Notify retryCountEntity = new() { Id = Guid.NewGuid() };

            dataverseService.Setup(x => x.RetrieveAsync<bng_Notify>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(retryCountEntity);


            await systemUnderTest.Invoking(x => x.UpdateNotifyEntity(notifyEntity, emailNotificationResponse, errorMessage, isSuccess, actionType))
                .Should()
                .NotThrowAsync();

            dataverseService.Verify(x => x.UpdateAsync(It.IsAny<Entity>()), Times.Once);
        }

        [Fact]
        public async Task GetFailedNotifications()
        {
            await systemUnderTest.Invoking(x => x.GetFailedNotifications())
                    .Should()
                    .NotThrowAsync();
        }
    }
}