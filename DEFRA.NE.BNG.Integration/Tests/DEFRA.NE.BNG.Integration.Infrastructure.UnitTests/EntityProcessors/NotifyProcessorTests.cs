using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Infrastructure.EntityProcessors;

namespace DEFRA.NE.BNG.Integration.Infrastructure.UnitTests.EntityProcessors
{
    public class NotifyProcessorTests : TestBase<NotifyProcessor>
    {
        private readonly NotifyProcessor systemUnderTest;

        public NotifyProcessorTests() : base()
        {
            systemUnderTest = new NotifyProcessor(mappingManager.Object, dataverseService.Object);
        }

        [Fact]
        public void ProcessEmailContent()
        {
            EmailNotificationRequest emailNotification = new()
            {
                personalisation = new Dictionary<string, string>()
            };
            Guid caseId = Guid.NewGuid();
            bng_casetype caseType = bng_casetype.Combined;

            bng_Emailcontent entity = new()
            {
                Id = Guid.NewGuid(),
                bng_Name = "Test content",
                bng_DocumentList = "Document list"
            };

            EntityCollection entityCollection = new();
            entityCollection.Entities.Add(entity);

            dataverseService.Setup(x => x.RetrieveMultipleAsync(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(entityCollection);

            FluentActions.Invoking(() => systemUnderTest.ProcessEmailContent(caseType, emailNotification))
                         .Should()
                         .NotThrowAsync();

            logger.VerifyAll();
            dataverseService.VerifyAll();
            mappingManager.Verify(X => X.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                                 It.IsAny<string>(),
                                                                 It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public void ProcessBankDetails()
        {
            bng_BankDetails entity = new()
            {
                Id = Guid.NewGuid(),
                bng_SortCode = "34-432-234",
                bng_AccountName = "Test name",
                bng_AccountNumber = 463863634
            };

            dataverseService.Setup(x => x.RetrieveFirstRecord<bng_BankDetails>(It.Is<QueryExpression>(x => x.EntityName == bng_BankDetails.EntityLogicalName)))
                            .ReturnsAsync(entity);

            EmailNotificationRequest emailNotification = new()
            {
                personalisation = new Dictionary<string, string>()
            };

            FluentActions.Invoking(() => systemUnderTest.ProcessBankDetails(emailNotification))
                         .Should()
                         .NotThrowAsync();

            logger.VerifyAll();
            dataverseService.VerifyAll();
            mappingManager.Verify(X => X.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                                 It.IsAny<string>(),
                                                                 It.IsAny<string>()), Times.Exactly(3));
        }

        [Fact]
        public void ProcessFees()
        {
            EmailNotificationRequest emailNotification = new EmailNotificationRequest();
            bng_casetype caseType = bng_casetype.Combined;

            bng_fees entity = new()
            {
                Id = Guid.NewGuid(),
                bng_fee = new Money(23.09M)
            };

            EntityCollection entityCollection = new();
            entityCollection.Entities.Add(entity);

            dataverseService.Setup(x => x.RetrieveMultipleAsync(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(entityCollection);

            FluentActions.Invoking(() => systemUnderTest.ProcessFees(emailNotification, caseType))
                         .Should()
                         .NotThrowAsync();

            logger.VerifyAll();
            dataverseService.VerifyAll();
            mappingManager.Verify(X => X.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                                 It.IsAny<string>(),
                                                                 It.IsAny<string>()), Times.Exactly(1));
        }

        [Fact]
        public void ProcessPaymentDetails()
        {
            var entity = new bng_PaymentDetails(); entity.Id = Guid.NewGuid();

            entity.bng_PaymentTotal = new Money(23.09M);
            entity.bng_PaymentReference = "Test Ref";
            entity.bng_PaymentDeadline = DateTime.UtcNow;

            EntityCollection entityCollection = new();
            entityCollection.Entities.Add(entity);

            dataverseService.Setup(x => x.RetrieveMultipleAsync(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(entityCollection);

            EmailNotificationRequest emailNotification = new();
            Guid caseId = Guid.NewGuid();

            FluentActions.Invoking(() => systemUnderTest.ProcessPaymentDetails(caseId, emailNotification))
                        .Should()
                        .NotThrowAsync();

            logger.VerifyAll();
            dataverseService.VerifyAll();
            mappingManager.Verify(X => X.CreateNewOrUpdateExisting(It.IsAny<IDictionary<string, string>>(),
                                                                 It.IsAny<string>(),
                                                                 It.IsAny<string>()), Times.Exactly(3));
        }

        [Fact]
        public async Task CreateNotify()
        {
            EmailNotificationRequest emailNotification = new();
            Guid caseId = Guid.NewGuid();
            Guid applicantId = Guid.NewGuid();

            Guid createdEnityId = Guid.NewGuid();

            dataverseService.Setup(x => x.CreateAsync(It.IsAny<Entity>()))
                            .ReturnsAsync(createdEnityId);

            var actual = await systemUnderTest.CreateNotify(caseId, applicantId, dataverseService.Object, logger.Object);

            actual.Should().Be(createdEnityId);

            logger.VerifyAll();
            dataverseService.VerifyAll();
        }

        [Fact]
        public async Task CreateNotify_HandlesException()
        {
            EmailNotificationRequest emailNotification = new();
            Guid caseId = Guid.NewGuid();
            Guid applicantId = Guid.NewGuid();

            Guid createdEnityId = Guid.NewGuid();

            dataverseService.Setup(x => x.CreateAsync(It.IsAny<Entity>()))
                            .Throws<Exception>();

            var actual = await systemUnderTest.CreateNotify(caseId, applicantId, dataverseService.Object, logger.Object);

            actual.Should().Be(Guid.Empty);

            logger.VerifyAll();
            dataverseService.VerifyAll();
        }
    }
}