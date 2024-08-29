using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Infrastructure.UnitTests;

namespace DEFRA.NE.BNG.Integration.Infrastructure.Utilities.Tests
{
    public class EmailNotificationRequestGeneratorTests : TestBase<EmailNotificationRequestGenerator>
    {
        private EmailNotificationRequestGenerator systemUnderTest;
        private Dictionary<bng_casetype, IDictionary<int, string>> actionTemplateDictionary;

        public EmailNotificationRequestGeneratorTests() : base()
        {
            systemUnderTest = new EmailNotificationRequestGenerator(mappingManager.Object,
                                                                    notifyManager.Object,
                                                                    logger.Object,
                                                                    dataverseService.Object);

            SetupEnvironmentVariables();
            actionTemplateDictionary = GetActionTemplateMappingForCase();
        }

        [Fact]
        public void EmailNotificationRequestGenerator()
        {
            FluentActions.Invoking(() => new EmailNotificationRequestGenerator(mappingManager.Object,
                                                                               notifyManager.Object,
                                                                               logger.Object,
                                                                               dataverseService.Object))
                         .Should()
                         .NotThrow();
        }

        [Fact]
        public async Task PrepairNotificationRequestData_Created()
        {
            Guid entityId = Guid.NewGuid();
            var notifyEntity = new bng_Notify()
            {
                Id = entityId,
                bng_ActionType = bng_notifytype.Created,
                bng_ApplicantID = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()),
                RegardingObjectId = Guid.NewGuid().GetEntityReference<bng_Case>(),
                bng_RetryCount = 2,
                Description = "Description"
            };


            var replyDictionary = new Dictionary<int, string>
            {
                { 1, "test@email.com" },
                { 2, "test@email.com" }
            };

            mappingManager.Setup(x => x.GetReplyEmailMapping())
                          .Returns(replyDictionary);

            bng_Case caseEntity = new() { Id = Guid.NewGuid() };

            dataverseService.Setup(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(caseEntity);

            var actual = await systemUnderTest.PrepairNotificationRequestData(notifyEntity);


            actual.EmailNotificationRequest.email_reply_to_id.Should().NotBeNullOrEmpty();

            mappingManager.Verify(x => x.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                                 It.IsAny<string>(),
                                                                 It.IsAny<string>()), Times.AtLeastOnce());

            dataverseService.Verify(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()), Times.AtLeastOnce());
        }


        [Fact]
        public async Task PrepairNotificationRequestData_Closed()
        {
            Guid entityId = Guid.NewGuid();
            var notifyEntity = new bng_Notify()
            {
                Id = entityId,
                bng_ActionType = bng_notifytype.Closed,
                bng_ApplicantID = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()),
                RegardingObjectId = Guid.NewGuid().GetEntityReference<bng_Case>(),
                bng_RetryCount = 2,
                Description = "Description"
            };

            var contact = GenerateSampleContacts(1).First();

            var replyDictionary = new Dictionary<int, string>
            {
                { 1, "test@email.com" },
                { 2, "test@email.com" }
            };


            mappingManager.Setup(x => x.GetReplyEmailMapping())
                          .Returns(replyDictionary);

            mappingManager.Setup(x => x.GetActionTemplateMappingForCase())
                          .Returns(actionTemplateDictionary);

            bng_Case caseEntity = new()
            {
                Id = Guid.NewGuid(),
                bng_Case_Type = bng_casetype.Combined,
                bng_Deadline = DateTime.Now,
                bng_GainSiteRegistrationID = new EntityReference(bng_GainSiteRegistration.EntityLogicalName, Guid.NewGuid()),
                bng_NoticeofIntent = DateTime.Now,
                bng_InternalJustification = "No Justification Required"
            };

            dataverseService.Setup(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(caseEntity);

            dataverseService.Setup(x => x.RetrieveAsync<Contact>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(contact);

            var actual = await systemUnderTest.PrepairNotificationRequestData(notifyEntity);


            actual.EmailNotificationRequest.email_reply_to_id.Should().NotBeNullOrEmpty();

            mappingManager.Verify(x => x.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                                 It.IsAny<string>(),
                                                                 It.IsAny<string>()), Times.AtLeastOnce());

            dataverseService.Verify(x => x.RetrieveAsync<Contact>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()), Times.AtLeastOnce());
            dataverseService.Verify(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()), Times.AtLeastOnce());
        }

        [Fact]
        public async Task PrepairNotificationRequestData_Accepted_LandOwner()
        {
            Guid entityId = Guid.NewGuid();
            var notifyEntity = new bng_Notify()
            {
                Id = entityId,
                bng_ActionType = bng_notifytype.Accepted,
                bng_ApplicantID = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()),
                RegardingObjectId = Guid.NewGuid().GetEntityReference<bng_Case>(),
                bng_RetryCount = 2,
                Description = "Description"
            };

            var replyDictionary = new Dictionary<int, string>
            {
                { 1, "test@email.com" },
                { 2, "test@email.com" }
            };

            mappingManager.Setup(x => x.GetReplyEmailMapping())
                          .Returns(replyDictionary);

            bng_Case caseEntity = new() { Id = Guid.NewGuid() };
            caseEntity.bng_Case_Type = bng_casetype.Registration;
            caseEntity.bng_GainSiteRegistrationID = new EntityReference(bng_GainSiteRegistration.EntityLogicalName, Guid.NewGuid());


            dataverseService.Setup(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(caseEntity);



            mappingManager.Setup(x => x.GetActionTemplateMappingForCase())
                          .Returns(actionTemplateDictionary);

            var actual = await systemUnderTest.PrepairNotificationRequestData(notifyEntity);


            actual.EmailNotificationRequest.email_reply_to_id.Should().NotBeNullOrEmpty();

            mappingManager.Verify(x => x.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                                 It.IsAny<string>(),
                                                                 It.IsAny<string>()), Times.AtLeastOnce());

            dataverseService.Verify(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()), Times.AtLeastOnce());
        }

        [Fact]
        public async Task PrepairNotificationRequestData_Accepted_Combined()
        {
            Guid entityId = Guid.NewGuid();
            var notifyEntity = new bng_Notify()
            {
                Id = entityId,
                bng_ActionType = bng_notifytype.Accepted,
                bng_ApplicantID = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()),
                RegardingObjectId = Guid.NewGuid().GetEntityReference<bng_Case>(),
                bng_RetryCount = 2,
                Description = "Description"
            };

            var replyDictionary = new Dictionary<int, string>
            {
                { 1, "test@email.com" },
                { 2, "test@email.com" }
            };



            mappingManager.Setup(x => x.GetActionTemplateMappingForCase())
                          .Returns(actionTemplateDictionary);

            mappingManager.Setup(x => x.GetReplyEmailMapping())
                          .Returns(replyDictionary);

            bng_Case caseEntity = new() { Id = Guid.NewGuid() };
            caseEntity.bng_Case_Type = bng_casetype.Combined;
            caseEntity.bng_GainSiteRegistrationID = new EntityReference(bng_GainSiteRegistration.EntityLogicalName, Guid.NewGuid());

            dataverseService.Setup(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(caseEntity);

            var actual = await systemUnderTest.PrepairNotificationRequestData(notifyEntity);


            actual.EmailNotificationRequest.email_reply_to_id.Should().NotBeNullOrEmpty();

            mappingManager.Verify(x => x.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                                 It.IsAny<string>(),
                                                                 It.IsAny<string>()), Times.AtLeastOnce());

            dataverseService.Verify(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()), Times.AtLeastOnce());
        }

        [Fact]
        public async Task PrepairNotificationRequestData_Accepted()
        {
            Guid entityId = Guid.NewGuid();
            var notifyEntity = new bng_Notify()
            {
                Id = entityId,
                bng_ActionType = bng_notifytype.Accepted,
                bng_ApplicantID = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()),
                RegardingObjectId = Guid.NewGuid().GetEntityReference<bng_Case>(),
                bng_RetryCount = 2,
                Description = "Description"
            };

            var replyDictionary = new Dictionary<int, string>
            {
                { 1, "test@email.com" },
                { 2, "test@email.com" }
            };



            mappingManager.Setup(x => x.GetActionTemplateMappingForCase())
                          .Returns(actionTemplateDictionary);

            mappingManager.Setup(x => x.GetReplyEmailMapping())
                          .Returns(replyDictionary);

            bng_Case caseEntity = new() { Id = Guid.NewGuid() };

            dataverseService.Setup(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(caseEntity);

            var actual = await systemUnderTest.PrepairNotificationRequestData(notifyEntity);


            actual.EmailNotificationRequest.email_reply_to_id.Should().NotBeNullOrEmpty();

            mappingManager.Verify(x => x.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                                 It.IsAny<string>(),
                                                                 It.IsAny<string>()), Times.AtLeastOnce());

            dataverseService.Verify(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()), Times.AtLeastOnce());
        }


        [Fact]
        public async Task PrepairNotificationRequestData_Rejected_LandOwner()
        {
            Guid entityId = Guid.NewGuid();
            var notifyEntity = new bng_Notify()
            {
                Id = entityId,
                bng_ActionType = bng_notifytype.Rejected,
                bng_ApplicantID = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()),
                RegardingObjectId = Guid.NewGuid().GetEntityReference<bng_Case>(),
                bng_RetryCount = 2,
                Description = "Description"
            };

            var replyDictionary = new Dictionary<int, string>
            {
                { 1, "test@email.com" },
                { 2, "test@email.com" }
            };

            mappingManager.Setup(x => x.GetActionTemplateMappingForCase())
                          .Returns(actionTemplateDictionary);

            mappingManager.Setup(x => x.GetReplyEmailMapping())
                          .Returns(replyDictionary);

            bng_Case caseEntity = new() { Id = Guid.NewGuid() };
            caseEntity.bng_Case_Type = bng_casetype.Combined;
            caseEntity.bng_GainSiteRegistrationID = new EntityReference(bng_GainSiteRegistration.EntityLogicalName, Guid.NewGuid());

            dataverseService.Setup(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(caseEntity);

            var actual = await systemUnderTest.PrepairNotificationRequestData(notifyEntity);


            actual.EmailNotificationRequest.email_reply_to_id.Should().NotBeNullOrEmpty();

            mappingManager.Verify(x => x.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                                 It.IsAny<string>(),
                                                                 It.IsAny<string>()), Times.AtLeastOnce());

            dataverseService.Verify(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()), Times.AtLeastOnce());
        }

        [Fact]
        public async Task PrepairNotificationRequestData_Rejected_AmendGainSite()
        {
            Guid entityId = Guid.NewGuid();
            var notifyEntity = new bng_Notify()
            {
                Id = entityId,
                bng_ActionType = bng_notifytype.Rejected,
                bng_ApplicantID = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()),
                RegardingObjectId = Guid.NewGuid().GetEntityReference<bng_Case>(),
                bng_RetryCount = 2,
                Description = "Description"
            };

            var replyDictionary = new Dictionary<int, string>
            {
                { 1, "test@email.com" },
                { 2, "test@email.com" }
            };



            mappingManager.Setup(x => x.GetActionTemplateMappingForCase())
                          .Returns(actionTemplateDictionary);

            mappingManager.Setup(x => x.GetReplyEmailMapping())
                          .Returns(replyDictionary);

            bng_Case caseEntity = new() { Id = Guid.NewGuid() };
            caseEntity.bng_Case_Type = bng_casetype.AmendGainSite;
            caseEntity.bng_GainSiteRegistrationID = new EntityReference(bng_GainSiteRegistration.EntityLogicalName, Guid.NewGuid());

            dataverseService.Setup(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(caseEntity);

            var actual = await systemUnderTest.PrepairNotificationRequestData(notifyEntity);


            actual.EmailNotificationRequest.email_reply_to_id.Should().NotBeNullOrEmpty();

            mappingManager.Verify(x => x.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                                 It.IsAny<string>(),
                                                                 It.IsAny<string>()), Times.AtLeastOnce());

            dataverseService.Verify(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()), Times.AtLeastOnce());
        }

        [Fact]
        public async Task PrepairNotificationRequestData_FurtherInformation()
        {
            Guid entityId = Guid.NewGuid();
            var notifyEntity = new bng_Notify()
            {
                Id = entityId,
                bng_ActionType = bng_notifytype.FurtherInformation,
                bng_ApplicantID = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()),
                RegardingObjectId = Guid.NewGuid().GetEntityReference<bng_Case>(),
                bng_RetryCount = 2,
                Description = "Description"
            };

            var replyDictionary = new Dictionary<int, string>
            {
                { 1, "test@email.com" },
                { 2, "test@email.com" }
            };

            mappingManager.Setup(x => x.GetReplyEmailMapping())
                          .Returns(replyDictionary);

            bng_Case caseEntity = new() { Id = Guid.NewGuid() };

            dataverseService.Setup(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(caseEntity);

            var actual = await systemUnderTest.PrepairNotificationRequestData(notifyEntity);


            actual.EmailNotificationRequest.email_reply_to_id.Should().NotBeNullOrEmpty();

            mappingManager.Verify(x => x.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                                 It.IsAny<string>(),
                                                                 It.IsAny<string>()), Times.AtLeastOnce());

            dataverseService.Verify(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()), Times.AtLeastOnce());
        }

        [Fact]
        public async Task PrepairNotificationRequestData_TriageComplete()
        {
            Guid entityId = Guid.NewGuid();
            var notifyEntity = new bng_Notify()
            {
                Id = entityId,
                bng_ActionType = bng_notifytype.TriageComplete,
                bng_ApplicantID = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()),
                RegardingObjectId = Guid.NewGuid().GetEntityReference<bng_Case>(),
                bng_RetryCount = 2,
                Description = "Description"
            };

            var replyDictionary = new Dictionary<int, string>
            {
                { 1, "test@email.com" },
                { 2, "test@email.com" }
            };

            mappingManager.Setup(x => x.GetReplyEmailMapping())
                          .Returns(replyDictionary);

            mappingManager.Setup(x => x.GetActionTemplateMappingForCase())
                           .Returns(actionTemplateDictionary);

            bng_Case caseEntity = new()
            {
                Id = Guid.NewGuid(),
                bng_Case_Type = bng_casetype.Combined,
                bng_Deadline = DateTime.Now,
                bng_GainSiteRegistrationID = new EntityReference(bng_GainSiteRegistration.EntityLogicalName, Guid.NewGuid())
            };

            dataverseService.Setup(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(caseEntity);

            var actual = await systemUnderTest.PrepairNotificationRequestData(notifyEntity);


            actual.EmailNotificationRequest.email_reply_to_id.Should().NotBeNullOrEmpty();

            mappingManager.Verify(x => x.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                                 It.IsAny<string>(),
                                                                 It.IsAny<string>()), Times.AtLeastOnce());

            dataverseService.Verify(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()), Times.AtLeastOnce());
        }


        [Fact]
        public async Task PrepairNotificationRequestData_RequestDocumentsPayment()
        {
            Guid entityId = Guid.NewGuid();
            var notifyEntity = new bng_Notify()
            {
                Id = entityId,
                bng_ActionType = bng_notifytype.RequestDocumentsPayment,
                bng_ApplicantID = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()),
                RegardingObjectId = Guid.NewGuid().GetEntityReference<bng_Case>(),
                bng_RetryCount = 2,
                Description = "Description"
            };

            var replyDictionary = new Dictionary<int, string>
            {
                { 1, "test@email.com" },
                { 2, "test@email.com" }
            };

            mappingManager.Setup(x => x.GetReplyEmailMapping())
                          .Returns(replyDictionary);

            bng_Case caseEntity = new() { Id = Guid.NewGuid() };

            dataverseService.Setup(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(caseEntity);

            var actual = await systemUnderTest.PrepairNotificationRequestData(notifyEntity);


            actual.EmailNotificationRequest.email_reply_to_id.Should().NotBeNullOrEmpty();

            mappingManager.Verify(x => x.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                                 It.IsAny<string>(),
                                                                 It.IsAny<string>()), Times.AtLeastOnce());

            dataverseService.Verify(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()), Times.AtLeastOnce());
        }


        [Fact]
        public async Task PrepairNotificationRequestData_PaymentRemainder()
        {
            Guid entityId = Guid.NewGuid();
            var notifyEntity = new bng_Notify()
            {
                Id = entityId,
                bng_ActionType = bng_notifytype.PaymentRemainder,
                bng_ApplicantID = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()),
                RegardingObjectId = Guid.NewGuid().GetEntityReference<bng_Case>(),
                bng_RetryCount = 2,
                Description = "Description"
            };

            var replyDictionary = new Dictionary<int, string>
            {
                { 1, "test@email.com" },
                { 2, "test@email.com" }
            };

            mappingManager.Setup(x => x.GetReplyEmailMapping())
                          .Returns(replyDictionary);

            bng_Case caseEntity = new() { Id = Guid.NewGuid() };

            dataverseService.Setup(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(caseEntity);

            var actual = await systemUnderTest.PrepairNotificationRequestData(notifyEntity);


            actual.EmailNotificationRequest.email_reply_to_id.Should().NotBeNullOrEmpty();

            mappingManager.Verify(x => x.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                                 It.IsAny<string>(),
                                                                 It.IsAny<string>()), Times.AtLeastOnce());

            dataverseService.Verify(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()), Times.AtLeastOnce());
        }


        [Fact]
        public async Task PrepairNotificationRequestData_Withdraw()
        {
            Guid entityId = Guid.NewGuid();
            var notifyEntity = new bng_Notify()
            {
                Id = entityId,
                bng_ActionType = bng_notifytype.Withdraw,
                bng_ApplicantID = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()),
                RegardingObjectId = Guid.NewGuid().GetEntityReference<bng_Case>(),
                bng_RetryCount = 2,
                Description = "Description"
            };

            var replyDictionary = new Dictionary<int, string>
            {
                { 1, "test@email.com" },
                { 2, "test@email.com" }
            };

            mappingManager.Setup(x => x.GetReplyEmailMapping())
                          .Returns(replyDictionary);

            bng_Case caseEntity = new() { Id = Guid.NewGuid() };

            dataverseService.Setup(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(caseEntity);

            var actual = await systemUnderTest.PrepairNotificationRequestData(notifyEntity);


            actual.EmailNotificationRequest.email_reply_to_id.Should().NotBeNullOrEmpty();

            mappingManager.Verify(x => x.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                                 It.IsAny<string>(),
                                                                 It.IsAny<string>()), Times.AtLeastOnce());

            dataverseService.Verify(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()), Times.AtLeastOnce());
        }


        [Fact]
        public async Task PrepairNotificationRequestData_ConfirmDocumentsReceivedRequestPayment()
        {
            Guid entityId = Guid.NewGuid();
            var notifyEntity = new bng_Notify()
            {
                Id = entityId,
                bng_ActionType = bng_notifytype.ConfirmdocumentsreceivedRequestpayment,
                bng_ApplicantID = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()),
                RegardingObjectId = Guid.NewGuid().GetEntityReference<bng_Case>(),
                bng_RetryCount = 2,
                Description = "Description"
            };

            var replyDictionary = new Dictionary<int, string>
            {
                { 1, "test@email.com" },
                { 2, "test@email.com" }
            };

            mappingManager.Setup(x => x.GetReplyEmailMapping())
                          .Returns(replyDictionary);

            mappingManager.Setup(x => x.GetActionTemplateMappingForCase())
               .Returns(actionTemplateDictionary);

            bng_Case caseEntity = new()
            {
                Id = Guid.NewGuid(),
                bng_Case_Type = bng_casetype.Combined,
                bng_Deadline = DateTime.Now,
                bng_GainSiteRegistrationID = new EntityReference(bng_GainSiteRegistration.EntityLogicalName, Guid.NewGuid())
            };

            dataverseService.Setup(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(caseEntity);

            var actual = await systemUnderTest.PrepairNotificationRequestData(notifyEntity);


            actual.EmailNotificationRequest.email_reply_to_id.Should().NotBeNullOrEmpty();

            mappingManager.Verify(x => x.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                                 It.IsAny<string>(),
                                                                 It.IsAny<string>()), Times.AtLeastOnce());

            dataverseService.Verify(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()), Times.AtLeastOnce());
        }


        [Fact]
        public async Task PrepairNotificationRequestData_NoticeOfIntent()
        {
            Guid entityId = Guid.NewGuid();
            var notifyEntity = new bng_Notify()
            {
                Id = entityId,
                bng_ActionType = bng_notifytype.NoticeofIntent,
                bng_ApplicantID = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()),
                RegardingObjectId = Guid.NewGuid().GetEntityReference<bng_Case>(),
                bng_RetryCount = 2,
                Description = "Description"
            };

            var replyDictionary = new Dictionary<int, string>
            {
                { 1, "test@email.com" },
                { 2, "test@email.com" }
            };

            mappingManager.Setup(x => x.GetReplyEmailMapping())
                          .Returns(replyDictionary);

            mappingManager.Setup(x => x.GetActionTemplateMappingForCase())
               .Returns(actionTemplateDictionary);

            bng_Case caseEntity = new()
            {
                Id = Guid.NewGuid(),
                bng_Case_Type = bng_casetype.Combined,
                bng_Deadline = DateTime.Now,
                bng_GainSiteRegistrationID = new EntityReference(bng_GainSiteRegistration.EntityLogicalName, Guid.NewGuid())
            };

            dataverseService.Setup(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(caseEntity);

            var actual = await systemUnderTest.PrepairNotificationRequestData(notifyEntity);


            actual.EmailNotificationRequest.email_reply_to_id.Should().NotBeNullOrEmpty();

            mappingManager.Verify(x => x.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                                 It.IsAny<string>(),
                                                                 It.IsAny<string>()), Times.AtLeastOnce());

            dataverseService.Verify(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()), Times.AtLeastOnce());
        }

        [Fact]
        public async Task PrepairNotificationRequestData_ProceedwithAmendment()
        {
            Guid entityId = Guid.NewGuid();
            var notifyEntity = new bng_Notify
            {
                Id = entityId,
                bng_ActionType = bng_notifytype.ProceedwithAmendment,
                bng_ApplicantID = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()),
                RegardingObjectId = Guid.NewGuid().GetEntityReference<bng_Case>(),
                bng_RetryCount = 2,
                Description = "Description"
            };

            var replyDictionary = new Dictionary<int, string>
            {
                { 1, "test@email.com" },
                { 2, "test@email.com" }
            };

            mappingManager.Setup(x => x.GetReplyEmailMapping())
                          .Returns(replyDictionary);

            mappingManager.Setup(x => x.GetActionTemplateMappingForCase())
               .Returns(actionTemplateDictionary);

            bng_Case caseEntity = new()
            {
                Id = Guid.NewGuid(),
                bng_Case_Type = bng_casetype.Combined,
                bng_Deadline = DateTime.Now,
                bng_GainSiteRegistrationID = new EntityReference(bng_GainSiteRegistration.EntityLogicalName, Guid.NewGuid()),
                bng_NoticeofIntent = DateTime.Now,
                bng_InternalJustification = "No Justification Required"
            };

            dataverseService.Setup(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(caseEntity);

            var actual = await systemUnderTest.PrepairNotificationRequestData(notifyEntity);


            actual.EmailNotificationRequest.email_reply_to_id.Should().NotBeNullOrEmpty();

            mappingManager.Verify(x => x.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                                 It.IsAny<string>(),
                                                                 It.IsAny<string>()), Times.AtLeastOnce());

            dataverseService.Verify(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()), Times.AtLeastOnce());
        }

        [Fact]
        public async Task PrepairNotificationRequestData_FullAcceptance()
        {
            Guid entityId = Guid.NewGuid();
            var notifyEntity = new bng_Notify
            {
                Id = entityId,
                bng_ActionType = bng_notifytype.FullAcceptance,
                bng_ApplicantID = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()),
                RegardingObjectId = Guid.NewGuid().GetEntityReference<bng_Case>(),
                bng_RetryCount = 2,
                Description = "Description"
            };

            var replyDictionary = new Dictionary<int, string>
            {
                { 1, "test@email.com" },
                { 2, "test@email.com" }
            };

            mappingManager.Setup(x => x.GetReplyEmailMapping())
                          .Returns(replyDictionary);

            mappingManager.Setup(x => x.GetActionTemplateMappingForCase())
               .Returns(actionTemplateDictionary);

            bng_Case caseEntity = new()
            {
                Id = Guid.NewGuid(),
                bng_Case_Type = bng_casetype.AmendGainSite,
                bng_Deadline = DateTime.Now,
                bng_GainSiteRegistrationID = new EntityReference(bng_GainSiteRegistration.EntityLogicalName, Guid.NewGuid()),
                bng_NoticeofIntent = DateTime.Now,
                bng_InternalJustification = "No Justification Required"
            };

            dataverseService.Setup(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(caseEntity);

            var actual = await systemUnderTest.PrepairNotificationRequestData(notifyEntity);


            actual.EmailNotificationRequest.email_reply_to_id.Should().NotBeNullOrEmpty();

            mappingManager.Verify(x => x.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                                 It.IsAny<string>(),
                                                                 It.IsAny<string>()), Times.AtLeastOnce());

            dataverseService.Verify(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()), Times.AtLeastOnce());
        }

        [Fact]
        public async Task PrepairNotificationRequestData_PartialAcceptance()
        {
            Guid entityId = Guid.NewGuid();
            var notifyEntity = new bng_Notify
            {
                Id = entityId,
                bng_ActionType = bng_notifytype.PartialAcceptance,
                bng_ApplicantID = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()),
                RegardingObjectId = Guid.NewGuid().GetEntityReference<bng_Case>(),
                bng_RetryCount = 2,
                Description = "Description"
            };

            var replyDictionary = new Dictionary<int, string>
            {
                { 1, "test@email.com" },
                { 2, "test@email.com" }
            };

            mappingManager.Setup(x => x.GetReplyEmailMapping())
                          .Returns(replyDictionary);

            mappingManager.Setup(x => x.GetActionTemplateMappingForCase())
               .Returns(actionTemplateDictionary);

            bng_Case caseEntity = new()
            {
                Id = Guid.NewGuid(),
                bng_Case_Type = bng_casetype.AmendGainSite,
                bng_Deadline = DateTime.Now,
                bng_GainSiteRegistrationID = new EntityReference(bng_GainSiteRegistration.EntityLogicalName, Guid.NewGuid()),
                bng_NoticeofIntent = DateTime.Now,
                bng_InternalJustification = "No Justification Required",
                bng_Informationtobeamended = "This is the Information we are amending",
                bng_InformationnotAmended = "Hello! we are not amending this information"
            };

            dataverseService.Setup(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(caseEntity);

            var actual = await systemUnderTest.PrepairNotificationRequestData(notifyEntity);


            actual.EmailNotificationRequest.email_reply_to_id.Should().NotBeNullOrEmpty();

            mappingManager.Verify(x => x.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                                 It.IsAny<string>(),
                                                                 It.IsAny<string>()), Times.AtLeastOnce());

            dataverseService.Verify(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()), Times.AtLeastOnce());
        }

        [Fact]
        public async Task PrepairNotificationRequestData_RemovalCaseAccepted()
        {
            Guid entityId = Guid.NewGuid();
            var notifyEntity = new bng_Notify
            {
                Id = entityId,
                bng_ActionType = bng_notifytype.RemovalCaseAccepted,
                bng_ApplicantID = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()),
                RegardingObjectId = Guid.NewGuid().GetEntityReference<bng_Case>(),
                bng_RetryCount = 2,
                Description = "Description"
            };

            var replyDictionary = new Dictionary<int, string>
            {
                { 1, "test@email.com" },
                { 2, "test@email.com" }
            };

            mappingManager.Setup(x => x.GetReplyEmailMapping())
                          .Returns(replyDictionary);

            mappingManager.Setup(x => x.GetActionTemplateMappingForCase())
               .Returns(actionTemplateDictionary);

            bng_Case caseEntity = new()
            {
                Id = Guid.NewGuid(),
                bng_Case_Type = bng_casetype.AmendGainSite,
                bng_Deadline = DateTime.Now,
                bng_GainSiteRegistrationID = new EntityReference(bng_GainSiteRegistration.EntityLogicalName, Guid.NewGuid()),
                bng_NoticeofIntent = DateTime.Now,
                bng_InternalJustification = "No Justification Required",
                bng_Informationtobeamended = "This is the Information we are amending",
                bng_InformationnotAmended = "Hello! we are not amending this information"
            };

            dataverseService.Setup(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(caseEntity);

            var actual = await systemUnderTest.PrepairNotificationRequestData(notifyEntity);


            actual.EmailNotificationRequest.email_reply_to_id.Should().NotBeNullOrEmpty();

            mappingManager.Verify(x => x.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                                 It.IsAny<string>(),
                                                                 It.IsAny<string>()), Times.AtLeastOnce());

            dataverseService.Verify(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()), Times.AtLeastOnce());
        }

        [Fact]
        public async Task PrepairNotificationRequestData_AmendmentAllocationCaseAccepted()
        {
            Guid entityId = Guid.NewGuid();
            var notifyEntity = new bng_Notify
            {
                Id = entityId,
                bng_ActionType = bng_notifytype.AmendmentAllocationCaseAccepted,
                bng_ApplicantID = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()),
                RegardingObjectId = Guid.NewGuid().GetEntityReference<bng_Case>(),
                bng_RetryCount = 2,
                Description = "Description"
            };

            var replyDictionary = new Dictionary<int, string>
            {
                { 1, "test@email.com" },
                { 2, "test@email.com" }
            };

            mappingManager.Setup(x => x.GetReplyEmailMapping())
                          .Returns(replyDictionary);

            mappingManager.Setup(x => x.GetActionTemplateMappingForCase())
               .Returns(actionTemplateDictionary);

            bng_Case caseEntity = new()
            {
                Id = Guid.NewGuid(),
                bng_Case_Type = bng_casetype.AmendGainSite,
                bng_Deadline = DateTime.Now,
                bng_GainSiteRegistrationID = new EntityReference(bng_GainSiteRegistration.EntityLogicalName, Guid.NewGuid()),
                bng_NoticeofIntent = DateTime.Now,
                bng_InternalJustification = "No Justification Required",
                bng_Informationtobeamended = "This is the Information we are amending",
                bng_InformationnotAmended = "Hello! we are not amending this information"
            };

            dataverseService.Setup(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(caseEntity);

            var actual = await systemUnderTest.PrepairNotificationRequestData(notifyEntity);


            actual.EmailNotificationRequest.email_reply_to_id.Should().NotBeNullOrEmpty();

            mappingManager.Verify(x => x.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                                 It.IsAny<string>(),
                                                                 It.IsAny<string>()), Times.AtLeastOnce());

            dataverseService.Verify(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()), Times.AtLeastOnce());
        }

        [Fact]
        public async Task PrepairNotificationRequestData_InternalAmendmentConfirmation()
        {
            Guid entityId = Guid.NewGuid();
            var notifyEntity = new bng_Notify
            {
                Id = entityId,
                bng_ActionType = bng_notifytype.InternalAmendmentConfirmation,
                bng_ApplicantID = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()),
                RegardingObjectId = Guid.NewGuid().GetEntityReference<bng_Case>(),
                bng_RetryCount = 2,
                Description = "Description"
            };

            var replyDictionary = new Dictionary<int, string>
            {
                { 1, "test@email.com" },
                { 2, "test@email.com" }
            };

            mappingManager.Setup(x => x.GetReplyEmailMapping())
                          .Returns(replyDictionary);

            mappingManager.Setup(x => x.GetActionTemplateMappingForCase())
               .Returns(actionTemplateDictionary);

            bng_Case caseEntity = new()
            {
                Id = Guid.NewGuid(),
                bng_Case_Type = bng_casetype.AmendGainSite,
                bng_Deadline = DateTime.Now,
                bng_GainSiteRegistrationID = new EntityReference(bng_GainSiteRegistration.EntityLogicalName, Guid.NewGuid()),
                bng_NoticeofIntent = DateTime.Now,
                bng_InternalJustification = "No Justification Required",
                bng_Informationtobeamended = "This is the Information we are amending",
                bng_InformationnotAmended = "Hello! we are not amending this information"
            };

            dataverseService.Setup(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(caseEntity);

            var actual = await systemUnderTest.PrepairNotificationRequestData(notifyEntity);


            actual.EmailNotificationRequest.email_reply_to_id.Should().NotBeNullOrEmpty();

            mappingManager.Verify(x => x.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                                 It.IsAny<string>(),
                                                                 It.IsAny<string>()), Times.AtLeastOnce());

            dataverseService.Verify(x => x.RetrieveAsync<bng_Case>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()), Times.AtLeastOnce());
        }

        [Fact]
        public async Task PrepairNotificationRequestData_OrderCreated_NotemplateFound()
        {
            Guid entityId = Guid.NewGuid();
            var notifyEntity = new bng_Notify
            {
                Id = entityId,
                bng_ActionType = bng_notifytype.OrderCreated,
                bng_ApplicantID = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()),
                RegardingObjectId = Guid.NewGuid().GetEntityReference<SalesOrder>(),
                bng_RetryCount = 2,
                Description = "Description"
            };

            var contact = GenerateSampleContacts(1).First();

            var replyDictionary = new Dictionary<int, string>
            {
                { 1, "test@email.com" },
                { 2, "test@email.com" }
            };

            mappingManager.Setup(x => x.GetReplyEmailMapping())
                          .Returns(replyDictionary);

            SalesOrder order = new() { Id = Guid.NewGuid() };

            dataverseService.Setup(x => x.RetrieveAsync<SalesOrder>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(order);

            dataverseService.Setup(x => x.RetrieveAsync<Contact>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(contact);

            var orderTemplatMapping = new Dictionary<int, string>();

            mappingManager.Setup(x => x.GetOrderTemplateMapping()).Returns(orderTemplatMapping);

            var actual = await systemUnderTest.PrepairNotificationRequestData(notifyEntity);


            actual.EmailNotificationRequest.email_reply_to_id.Should().NotBeNullOrEmpty();
            actual.EmailNotificationRequest.template_id.Should().BeNullOrEmpty();

            mappingManager.Verify(x => x.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                                 It.Is<string>(a => a == GovNotificationConstants.GovNot_API_Personalisation_REFNumber),
                                                                 It.IsAny<string>()), Times.Once());

            dataverseService.Verify(x => x.RetrieveAsync<Contact>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()), Times.Once());
        }

        [Fact]
        public async Task PrepairNotificationRequestData_OrderCreated()
        {
            Guid entityId = Guid.NewGuid();
            var regardingObject = new EntityReference(SalesOrder.EntityLogicalName, Guid.NewGuid())
            {
                Name = Guid.NewGuid().ToString()
            };


            var notifyEntity = new bng_Notify
            {
                Id = entityId,
                bng_ActionType = bng_notifytype.OrderCreated,
                bng_ApplicantID = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid()),
                RegardingObjectId = regardingObject,
                bng_RetryCount = 2,
                Description = "Description"
            };

            var contact = GenerateSampleContacts(1).First();

            var replyDictionary = new Dictionary<int, string>
            {
                { 1, "test@email.com" },
                { 2, "test@email.com" }
            };

            mappingManager.Setup(x => x.GetReplyEmailMapping())
                          .Returns(replyDictionary);

            SalesOrder order = new() { Id = Guid.NewGuid() };

            dataverseService.Setup(x => x.RetrieveAsync<SalesOrder>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(order);

            dataverseService.Setup(x => x.RetrieveAsync<Contact>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(contact);

            var orderTemplatMapping = new Dictionary<int, string>()
                {
                    {(int)bng_notifytype.OrderCreated,Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdOrderCreated)},
                };

            mappingManager.Setup(x => x.GetOrderTemplateMapping()).Returns(orderTemplatMapping);

            var actual = await systemUnderTest.PrepairNotificationRequestData(notifyEntity);


            actual.EmailNotificationRequest.email_reply_to_id.Should().NotBeNullOrEmpty();
            actual.EmailNotificationRequest.template_id.Should().NotBeNullOrEmpty();
            mappingManager.Verify(x => x.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                                 It.Is<string>(a => a == GovNotificationConstants.GovNot_API_Personalisation_REFNumber),
                                                                 It.IsAny<string>()), Times.Once());

            mappingManager.Verify(x => x.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                                 It.Is<string>(a => a == GovNotificationConstants.GovNot_API_Personalisation_OrderId),
                                                                 It.IsAny<string>()), Times.Once());

            dataverseService.Verify(x => x.RetrieveAsync<Contact>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()), Times.AtLeastOnce());
        }

        [Fact]
        public void ConfigureEmailTemplateForCase_NoCaseTypeAttribute()
        {
            EmailNotificationRequest emailNotification = new();
            bng_Case entity = new();
            var actionType = (int)bng_notifytype.Accepted;

            var actual = systemUnderTest.ConfigureEmailTemplateForCase(emailNotification, entity, actionType);

            actual.Should().Be(0);
            emailNotification.template_id.Should().BeNull();
        }

        [Fact]
        public void ConfigureEmailTemplateForCase_CaseTypeAttribute_Developer()
        {
            EmailNotificationRequest emailNotification = new();
            bng_Case entity = new()
            {
                bng_Case_Type = bng_casetype.Allocation
            };
            var actionType = (int)bng_notifytype.Accepted;

            mappingManager.Setup(x => x.GetActionTemplateMappingForCase())
                          .Returns(actionTemplateDictionary);


            var actual = systemUnderTest.ConfigureEmailTemplateForCase(emailNotification, entity, actionType);

            actual.Should().Be(entity.bng_Case_Type);
            emailNotification.template_id.Should().NotBeNull();
        }

        [Fact]
        public void PersonaliseEmail_Applicant()
        {
            var expectedEmail = "test@test.com";
            EmailNotificationRequest emailNotification = new();
            Guid applicantId = Guid.NewGuid();
            Contact entity = new() { Id = applicantId };
            entity.Attributes.Add(new KeyValuePair<string, object>("emailaddress1", expectedEmail));

            EntityReference contact = new(Contact.EntityLogicalName, Guid.NewGuid());
            bng_Notify notifyEntity = new();
            notifyEntity.Attributes.Add(
            DataverseExtensions.AttributeLogicalName<bng_Notify>(nameof(bng_Notify.bng_ApplicantID)), contact);


            dataverseService.Setup(x => x.RetrieveAsync<Contact>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                .ReturnsAsync(entity);


            var actual = systemUnderTest.PersonaliseEmail(notifyEntity, emailNotification);

            dataverseService.VerifyAll();
            emailNotification.email_address.Should().Be(expectedEmail);
            dataverseService.Verify(x => x.RetrieveAsync<Contact>(It.IsAny<Guid>(),
                                                                  It.Is<ColumnSet>(a => a.Columns.Count == 2)),
                                         Times.Once);
            dataverseService.Verify(x => x.RetrieveAsync<Account>(It.IsAny<Guid>(),
                                                                  It.Is<ColumnSet>(a => a.Columns.Count == 2)),
                                         Times.Never);
        }

        [Fact]
        public void PersonaliseEmail_Organisation()
        {
            var expectedEmail = "test@test.com";
            EmailNotificationRequest emailNotification = new();
            Guid applicantId = Guid.NewGuid();
            Account entity = new() { Id = applicantId };
            entity.Attributes.Add(new KeyValuePair<string, object>("emailaddress1", expectedEmail));

            EntityReference organisation = new(Account.EntityLogicalName, Guid.NewGuid());
            bng_Notify notifyEntity = new();
            notifyEntity.Attributes.Add(
            DataverseExtensions.AttributeLogicalName<bng_Notify>(nameof(bng_Notify.bng_Account)), organisation);

            dataverseService.Setup(x => x.RetrieveAsync<Account>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(entity);

            var actual = systemUnderTest.PersonaliseEmail(notifyEntity, emailNotification);

            dataverseService.VerifyAll();
            emailNotification.email_address.Should().Be(expectedEmail);
            dataverseService.Verify(x => x.RetrieveAsync<Account>(It.IsAny<Guid>(),
                                                                  It.Is<ColumnSet>(a => a.Columns.Count == 2)),
                                         Times.Once);
            dataverseService.Verify(x => x.RetrieveAsync<Contact>(It.IsAny<Guid>(),
                                                                  It.Is<ColumnSet>(a => a.Columns.Count == 2)),
                                         Times.Never);
        }

        [Fact]
        public void SetPersonalisationDetails_Contact()
        {
            string emailColumnName = DataverseExtensions.AttributeLogicalName<Account>(nameof(Account.EMailAddress1));
            string displayNameColumn = DataverseExtensions.AttributeLogicalName<Account>(nameof(Account.Name));
            var expectedEmail = "test@test.com";
            var recordName = "John Smith";
            EmailNotificationRequest emailNotification = new();
            Contact entity = new() { Id = Guid.NewGuid() };
            entity.Attributes.Add(new KeyValuePair<string, object>(emailColumnName, expectedEmail));
            entity.Attributes.Add(new KeyValuePair<string, object>(displayNameColumn, recordName));
            ColumnSet columnSet = new(emailColumnName, displayNameColumn);

            dataverseService.Setup(x => x.RetrieveAsync<Contact>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                .ReturnsAsync(entity);

            var actual = systemUnderTest.SetPersonalisationDetails<Contact>(entity.Id,
            emailNotification,
                                                                   columnSet,
                                                                   emailColumnName,
                                                                  displayNameColumn);

            emailNotification.email_address.Should().Be(expectedEmail);
            dataverseService.Verify(x => x.RetrieveAsync<Contact>(It.IsAny<Guid>(),
                                                                  It.Is<ColumnSet>(a => a.Columns.Count == 2)),
                                         Times.Once);
            dataverseService.Verify(x => x.RetrieveAsync<Account>(It.IsAny<Guid>(),
                                                                  It.Is<ColumnSet>(a => a.Columns.Count == 2)),
                                         Times.Never);
            mappingManager.Verify(x => x.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                          It.IsAny<string>(),
                                                          It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public void SetPersonalisationDetails_Account()
        {
            string emailColumnName = DataverseExtensions.AttributeLogicalName<Account>(nameof(Account.EMailAddress1));
            string displayNameColumn = DataverseExtensions.AttributeLogicalName<Account>(nameof(Account.Name));
            var expectedEmail = "test@test.com";
            var recordName = "John Smith";
            EmailNotificationRequest emailNotification = new();
            Account entity = new() { Id = Guid.NewGuid() };
            entity.Attributes.Add(new KeyValuePair<string, object>(emailColumnName, expectedEmail));
            entity.Attributes.Add(new KeyValuePair<string, object>(displayNameColumn, recordName));
            ColumnSet columnSet = new(emailColumnName, displayNameColumn);

            dataverseService.Setup(x => x.RetrieveAsync<Account>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                .ReturnsAsync(entity);

            var actual = systemUnderTest.SetPersonalisationDetails<Account>(entity.Id,
                                                                   emailNotification,
                                                                   columnSet,
                                                                   emailColumnName,
                                                                  displayNameColumn);

            emailNotification.email_address.Should().Be(expectedEmail);
            dataverseService.Verify(x => x.RetrieveAsync<Account>(It.IsAny<Guid>(),
                                                                  It.Is<ColumnSet>(a => a.Columns.Count == 2)),
                                         Times.Once);
            dataverseService.Verify(x => x.RetrieveAsync<Contact>(It.IsAny<Guid>(),
                                                                  It.Is<ColumnSet>(a => a.Columns.Count == 2)),
                                         Times.Never);
            mappingManager.Verify(x => x.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                          It.IsAny<string>(),
                                                          It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public void ProcessLandOwnerCaseTypeOnAccepted()
        {
            Guid gainSiteRefrenceId = Guid.NewGuid();
            EmailNotificationRequest emailNotification = new();

            bng_GainSiteRegistration entity = new()
            {
                bng_GainSiteReference = Guid.NewGuid().ToString()
            };

            dataverseService.Setup(x => x.RetrieveAsync<bng_GainSiteRegistration>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(entity);

            mappingManager.Setup(x => x.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                                                It.IsAny<string>(),
                                                                                It.IsAny<string>()));

            FluentActions.Invoking(() => systemUnderTest.ProcessLandOwnerCaseTypeOnAccepted(gainSiteRefrenceId, emailNotification))
                       .Should()
                       .NotThrowAsync();

            dataverseService.VerifyAll();
            mappingManager.VerifyAll();
        }

        [Fact]
        public void ProcessEmail_InputCaseDoesNotContainWithdrawalReason()
        {
            bng_Case caseEntity = new();
            EmailNotificationRequest emailNotification = new();
            bng_WithdrawalReason withdrawalReasonEntity = new();

            FluentActions.Invoking(() => systemUnderTest.ProcessWithdrawalReason(caseEntity, emailNotification))
                         .Should()
                         .NotThrowAsync();

            dataverseService.Verify(x => x.RetrieveAsync<bng_WithdrawalReason>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()), Times.Never());
        }
        [Fact]
        public void ProcessEmail()
        {
            bng_Case caseEntity = new();
            caseEntity.Attributes.Add(
                DataverseExtensions.AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_withdrawalreason))
                , new EntityReference("test", Guid.NewGuid()));
            EmailNotificationRequest emailNotification = new();
            bng_WithdrawalReason withdrawalReasonEntity = new();
            withdrawalReasonEntity.Attributes.Add(EnvironmentConstants.WithdrawalReasonEntityContent, "Test value");

            dataverseService.Setup(x => x.RetrieveAsync<bng_WithdrawalReason>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(withdrawalReasonEntity);


            FluentActions.Invoking(() => systemUnderTest.ProcessWithdrawalReason(caseEntity, emailNotification))
                         .Should()
                         .NotThrowAsync();

            dataverseService.Verify(x => x.RetrieveAsync<bng_WithdrawalReason>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()), Times.Once());
        }

        [Fact]
        public void ProcessEmail_WithdrawalReasonEntity_NotFound()
        {
            bng_Case caseEntity = new()
            {
                bng_withdrawalreason = new EntityReference(bng_WithdrawalReason.EntityLogicalName, Guid.NewGuid())
            };
            EmailNotificationRequest emailNotification = new();
            bng_WithdrawalReason withdrawalReasonEntity = null;

            dataverseService.Setup(x => x.RetrieveAsync<bng_WithdrawalReason>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(withdrawalReasonEntity);


            FluentActions.Invoking(() => systemUnderTest.ProcessWithdrawalReason(caseEntity, emailNotification))
                         .Should()
                         .NotThrowAsync();

            dataverseService.Verify(x => x.RetrieveAsync<bng_WithdrawalReason>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()), Times.Once());
        }

        [Fact]
        public void ProcessRejectionReason_RejectReason_NotSupplied()
        {
            bng_Case caseEntity = new();

            EmailNotificationRequest emailNotification = new();
            bng_RejectionReason rejectionReason = new()
            {
                Id = Guid.NewGuid()
            };

            dataverseService.Setup(x => x.RetrieveAsync<bng_RejectionReason>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(rejectionReason);


            FluentActions.Invoking(() => systemUnderTest.ProcessRejectionReason(caseEntity, emailNotification))
                         .Should()
                         .NotThrowAsync();

            dataverseService.Verify(x => x.RetrieveAsync<bng_RejectionReason>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()), Times.Never());
        }

        [Fact]
        public void ProcessRejectionReason()
        {
            bng_Case caseEntity = new()
            {
                bng_RejectionReasonID = new EntityReference(bng_RejectionReason.EntityLogicalName, Guid.NewGuid())
            };
            EmailNotificationRequest emailNotification = new();
            bng_RejectionReason rejectionReason = new()
            {
                bng_RejectionReasonName = "Test value"
            };

            dataverseService.Setup(x => x.RetrieveAsync<bng_RejectionReason>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(rejectionReason);


            FluentActions.Invoking(() => systemUnderTest.ProcessRejectionReason(caseEntity, emailNotification))
                         .Should()
                         .NotThrowAsync();

            dataverseService.Verify(x => x.RetrieveAsync<bng_RejectionReason>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()), Times.Once());
            mappingManager.Verify(x => x.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                                 It.IsAny<string>(),
                                                                 It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void TranslateAmendmentReasonToUserFriendlyText_aChangestolegalagreement()
        {
            var actual = systemUnderTest.TranslateAmendmentReasonToUserFriendlyText(bng_amendmentreason.aChangestolegalagreement);

            actual.Should().Be("changes to legal agreement");
        }

        [Fact]
        public void TranslateAmendmentReasonToUserFriendlyText_bReducinganallocation()
        {
            var actual = systemUnderTest.TranslateAmendmentReasonToUserFriendlyText(bng_amendmentreason.bReducinganallocation);

            actual.Should().Be("reducing an allocation");
        }


        [Fact]
        public void TranslateAmendmentReasonToUserFriendlyText_cRemovinganallocation()
        {
            var actual = systemUnderTest.TranslateAmendmentReasonToUserFriendlyText(bng_amendmentreason.cRemovinganallocation);

            actual.Should().Be("removing an allocation");
        }

        [Fact]
        public void TranslateAmendmentReasonToUserFriendlyText_dAnyotherreason()
        {
            var actual = systemUnderTest.TranslateAmendmentReasonToUserFriendlyText(bng_amendmentreason.dAnyotherreason);

            actual.Should().Be("any other reason");
        }

        [Fact]
        public void TranslateAmendmentReasonToUserFriendlyText_eIncompleteorinaccurateinformation()
        {
            var actual = systemUnderTest.TranslateAmendmentReasonToUserFriendlyText(bng_amendmentreason.eIncompleteorinaccurateinformation);

            actual.Should().Be("incomplete or inaccurate information");
        }
        [Fact]
        public void TranslateAmendmentReasonToUserFriendlyText_fThelegalagreementhavingceasedtohaveaneffect()
        {
            var actual = systemUnderTest.TranslateAmendmentReasonToUserFriendlyText(bng_amendmentreason.fThelegalagreementhavingceasedtohaveaneffect);

            actual.Should().Be("the legal agreement having ceased to have an effect");
        }


        [Fact]
        public void TranslateAmendmentReasonToUserFriendlyText_gTheBNGeligibilityforthissiteisnolongermet()
        {
            var actual = systemUnderTest.TranslateAmendmentReasonToUserFriendlyText(bng_amendmentreason.gTheBNGeligibilityforthissiteisnolongerbeingmet);

            actual.Should().Be("the BNG eligibility for this site is no longer being met");
        }

        private static Dictionary<bng_casetype, IDictionary<int, string>> GetActionTemplateMappingForCase()
        {
            var result = new Dictionary<bng_casetype, IDictionary<int, string>>
                {
                    {bng_casetype.Registration,  GetActionTemplateMapping()},
                    {bng_casetype.Allocation,   GetActionTemplateMapping()},
                    {bng_casetype.Combined,   GetActionTemplateMapping()},
                    {bng_casetype.AmendGainSite,   GetActionTemplateMapping()},
                    {bng_casetype.AmendAllocation,   GetActionTemplateMapping()}
                };
            return result;
        }

        private static IDictionary<int, string> GetActionTemplateMapping()
        {
            var result = new Dictionary<int, string>
                {
                    {(int)bng_notifytype.Created,Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdCreated)},
                    {(int)bng_notifytype.Accepted,Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdSuccess)},
                    {(int)bng_notifytype.Rejected, Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdFail)},
                    {(int)bng_notifytype.FurtherInformation, Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdOnHold)},
                    {(int)bng_notifytype.RequestDocumentsPayment, Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdDocumentAndPaymentRequest)},
                    {(int)bng_notifytype.TriageComplete, Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdDocumentAndPaymentConfirmation)},
                    {(int)bng_notifytype.PaymentRemainder, Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdDocumentAndPaymentRemainder)},
                    {(int)bng_notifytype.Withdraw, Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdWithdraw)},
                    {(int)bng_notifytype.ConfirmdocumentsreceivedRequestpayment, Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdConfirmationOfDocsAndRequestPayment)},
                    {(int)bng_notifytype.NoticeofIntent, Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdNoticeOfIntent)},
                    {(int)bng_notifytype.Closed, Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdNoticeOfIntent)}
                };
            return result;
        }
    }
}