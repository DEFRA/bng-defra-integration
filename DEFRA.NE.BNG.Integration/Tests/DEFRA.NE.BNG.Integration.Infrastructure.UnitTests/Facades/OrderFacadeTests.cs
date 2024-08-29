using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.JsonRequestRoot;
using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Infrastructure.Facades;

namespace DEFRA.NE.BNG.Integration.Infrastructure.UnitTests.Facades
{
    public class OrderFacadeTests : TestBase<OrderFacade>
    {
        private OrderFacade systemUnderTest;


        public OrderFacadeTests() : base()
        {
            systemUnderTest = new OrderFacade(logger.Object, environmentVariableReader.Object, mailService.Object, dataverseService.Object);

        }

        [Fact]
        public async Task InvokeOrchestrationBNG_NoProducts_NoFiles()
        {
            var order = new Order()
            {
                Applicant = new Applicant(),
                Organisation = new Organisation(),
                Development = new DevelopmentDetails()
            };

            await systemUnderTest.Invoking(async x => await x.OrchestrationBNG(order, environmentVariableReader.Object))
                            .Should()
                            .ThrowAsync<InvalidDataException>();
        }

        [Fact]
        public async Task InvokeOrchestrationBNG_WithProducts_NoFiles()
        {
            var order = new Order()
            {
                Applicant = new Applicant(),
                Organisation = new Organisation(),
                Development = new DevelopmentDetails(),
                Products = [new DEFRA.NE.BNG.Integration.Domain.Entities.Product(), new DEFRA.NE.BNG.Integration.Domain.Entities.Product()]
            };

            await systemUnderTest.Invoking(async x => await x.OrchestrationBNG(order, environmentVariableReader.Object))
                            .Should()
                            .ThrowAsync<InvalidDataException>();
        }

        [Fact]
        public async Task InvokeOrchestrationBNG_WithProductsAndFiles()
        {
            var order = new Order
            {
                Applicant = new Applicant(),
                Organisation = new Organisation(),
                Development = new DevelopmentDetails(),
                Products = [new Domain.Entities.Product(), new Domain.Entities.Product()],
                Files = [new Model.Request.FileDetails(), new Model.Request.FileDetails()]
            };

            await systemUnderTest.Invoking(async x => await x.OrchestrationBNG(order, environmentVariableReader.Object))
                            .Should()
                            .ThrowAsync<InvalidDataException>();
        }

        [Fact]
        public async Task InvokeOrchestrationBNG_PropertiesNotInitialized()
        {
            var order = new Order() { };

            await systemUnderTest.Invoking(async x => await x.OrchestrationBNG(order, environmentVariableReader.Object))
                                .Should()
                                .NotThrowAsync();

            environmentVariableReader.VerifyAll();
            dataverseService.VerifyAll();
        }

        [Fact]
        public async Task InvokeOrchestrationBNG_CreditsPurchase_Individual()
        {
            Contact applicant = new() { Id = Guid.NewGuid() };

            dataverseService.Setup(x => x.RetrieveFirstRecord<Contact>(It.IsAny<QueryExpression>()))
            .ReturnsAsync(applicant);

            dataverseService.Setup(x => x.CreateAsync(It.IsAny<Entity>())).ReturnsAsync(Guid.NewGuid());

            var payload = await HydrateFromTestDataFile<OrderRequestPayload>("TestData/credits/credits-purchase-individual.example.json");

            var actual = await systemUnderTest.OrchestrationBNG(payload.CreditsPurchase, environmentVariableReader.Object);

            actual.Should().NotBe(Guid.Empty);
            environmentVariableReader.VerifyAll();
            dataverseService.VerifyAll();
        }

        [Fact]
        public async Task InvokeOrchestrationBNG_CreditsPurchase_Organisation()
        {
            dataverseService.Setup(x => x.RetrieveFirstRecord<Contact>(It.Is<QueryExpression>(x => x.EntityName == Contact.EntityLogicalName)))
            .ReturnsAsync(new Contact() { Id = Guid.NewGuid() });

            dataverseService.Setup(x => x.RetrieveFirstRecord<Account>(It.Is<QueryExpression>(x => x.EntityName == Account.EntityLogicalName)))
            .ReturnsAsync(new Account() { Id = Guid.NewGuid() });

            dataverseService.Setup(x => x.RetrieveFirstRecord<SalesOrder>(It.Is<QueryExpression>(x => x.EntityName == SalesOrder.EntityLogicalName)))
            .ReturnsAsync(new SalesOrder() { Id = Guid.NewGuid() });

            dataverseService.Setup(x => x.CreateAsync(It.IsAny<Entity>())).ReturnsAsync(Guid.NewGuid());
            var payload = await HydrateFromTestDataFile<OrderRequestPayload>("TestData/credits/credits-purchase-organisation.example.json");

            var actual = await systemUnderTest.OrchestrationBNG(payload.CreditsPurchase, environmentVariableReader.Object);

            actual.Should().NotBe(Guid.Empty);
            environmentVariableReader.VerifyAll();
            dataverseService.VerifyAll();
        }


        [Fact]
        public async Task InvokeOrchestrationBNG_CreditsPurchase_Individual_WithConfigurationSetup()
        {
            Contact applicant = new() { Id = Guid.NewGuid() };

            dataverseService.Setup(x => x.RetrieveFirstRecord<Contact>(It.IsAny<QueryExpression>())).ReturnsAsync(applicant);

            List<bng_BNGConfiguration> configurations =
            [
                new bng_BNGConfiguration()
                {
                    bng_Standard = (decimal?)10.0
                },
            ];

            environmentVariableReader.Setup(x => x.Read(It.Is<string>(y => y == "OrderOwningTeamGuid"))).Returns(Guid.NewGuid().ToString());
            environmentVariableReader.Setup(x => x.Read(It.Is<string>(y => y == "OrderTotalAmountThreshold"))).Returns("65236589");

            dataverseService.Setup(x => x.RetrieveConfigurations(It.Is<string>(y => y == EnvironmentConstants.BNGConfigurationDetails)))
                            .ReturnsAsync(configurations);

            dataverseService.Setup(x => x.CreateAsync(It.IsAny<Entity>())).ReturnsAsync(Guid.NewGuid());

            var payload = await HydrateFromTestDataFile<OrderRequestPayload>("TestData/credits/credits-purchase-individual.example.json");

            var actual = await systemUnderTest.OrchestrationBNG(payload.CreditsPurchase, environmentVariableReader.Object);

            actual.Should().NotBe(Guid.Empty);
            environmentVariableReader.VerifyAll();
            dataverseService.VerifyAll();
        }

        [Fact]
        public async Task CreateOrderDevelopmentRegistration_NoPlanningAuthority()
        {
            var payload = await HydrateFromTestDataFile<OrderRequestPayload>("TestData/credits/credits-with-development-and-planningauthority.json");

            Guid applicantId = Guid.NewGuid();

            bng_LocalPlanningAuthority localPlanningAuthority = null;

            dataverseService.Setup(x => x.RetrieveFirstRecord<bng_LocalPlanningAuthority>(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(localPlanningAuthority);

            await FluentActions.Invoking(() => systemUnderTest.CreateOrderDevelopmentRegistration(payload.CreditsPurchase, applicantId))
                 .Should()
                 .NotThrowAsync();

            dataverseService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Once);
        }

        [Fact]
        public async Task CreateOrderDevelopmentRegistration_PlanningAuthority()
        {
            var payload = await HydrateFromTestDataFile<OrderRequestPayload>("TestData/credits/credits-with-development-and-planningauthority.json");

            Guid applicantId = Guid.NewGuid();

            bng_LocalPlanningAuthority localPlanningAuthority = new() { Id = Guid.NewGuid() };

            dataverseService.Setup(x => x.RetrieveFirstRecord<bng_LocalPlanningAuthority>(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(localPlanningAuthority);

            await FluentActions.Invoking(() => systemUnderTest.CreateOrderDevelopmentRegistration(payload.CreditsPurchase, applicantId))
                 .Should()
                 .NotThrowAsync();

            dataverseService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Once);
        }

        [Fact]
        public async Task CreateOrderDevelopmentRegistration_PlanningAuthority_NoDevelopmentName()
        {
            var payload = await HydrateFromTestDataFile<OrderRequestPayload>("TestData/credits/credits-with-development-and-planningauthority.json");

            payload.CreditsPurchase.Development.Name = null;

            Guid applicantId = Guid.NewGuid();

            bng_LocalPlanningAuthority localPlanningAuthority = new() { Id = Guid.NewGuid() };

            dataverseService.Setup(x => x.RetrieveFirstRecord<bng_LocalPlanningAuthority>(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(localPlanningAuthority);

            await FluentActions.Invoking(() => systemUnderTest.CreateOrderDevelopmentRegistration(payload.CreditsPurchase, applicantId))
                 .Should()
                 .NotThrowAsync();

            dataverseService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Once);
        }

        [Fact]
        public async Task CreateOrderDevelopmentRegistration_PlanningAuthority_NoDevelopmentPlanningReference()
        {
            var payload = await HydrateFromTestDataFile<OrderRequestPayload>("TestData/credits/credits-with-development-and-planningauthority.json");

            payload.CreditsPurchase.Development.PlanningReference = null;

            Guid applicantId = Guid.NewGuid();

            bng_LocalPlanningAuthority localPlanningAuthority = new() { Id = Guid.NewGuid() };

            dataverseService.Setup(x => x.RetrieveFirstRecord<bng_LocalPlanningAuthority>(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(localPlanningAuthority);

            await FluentActions.Invoking(() => systemUnderTest.CreateOrderDevelopmentRegistration(payload.CreditsPurchase, applicantId))
                 .Should()
                 .NotThrowAsync();

            dataverseService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Once);
        }

        [Fact]
        public async Task CreateOrderDevelopmentRegistration_PlanningAuthority_ExistingDevelopment()
        {
            var payload = await HydrateFromTestDataFile<OrderRequestPayload>("TestData/credits/credits-with-development-and-planningauthority.json");

            Guid applicantId = Guid.NewGuid();
            EntityCollection entityCollection = new();
            entityCollection.Entities.Add(
                new bng_DeveloperRegistration()
                {
                    Id = Guid.NewGuid()
                }
                );

            bng_LocalPlanningAuthority localPlanningAuthority = new() { Id = Guid.NewGuid() };

            dataverseService.Setup(x => x.RetrieveFirstRecord<bng_LocalPlanningAuthority>(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(localPlanningAuthority);

            dataverseService.Setup(x => x.RetrieveMultipleAsync(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(entityCollection);

            await FluentActions.Invoking(() => systemUnderTest.CreateOrderDevelopmentRegistration(payload.CreditsPurchase, applicantId))
                 .Should()
                 .NotThrowAsync();

            dataverseService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Never);
        }

        [Fact]
        public async Task CreateOrderDevelopmentRegistration_PlanningAuthority_NoDevelopmentNameAndPlanningReference()
        {
            var payload = await HydrateFromTestDataFile<OrderRequestPayload>("TestData/credits/credits-with-development-and-planningauthority.json");

            payload.CreditsPurchase.Development.PlanningReference = null;
            payload.CreditsPurchase.Development.Name = null;

            Guid applicantId = Guid.NewGuid();
            EntityCollection entityCollection = null;

            bng_LocalPlanningAuthority localPlanningAuthority = new() { Id = Guid.NewGuid() };

            dataverseService.Setup(x => x.RetrieveFirstRecord<bng_LocalPlanningAuthority>(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(localPlanningAuthority);

            dataverseService.Setup(x => x.RetrieveMultipleAsync(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(entityCollection);

            await FluentActions.Invoking(() => systemUnderTest.CreateOrderDevelopmentRegistration(payload.CreditsPurchase, applicantId))
                 .Should()
                 .NotThrowAsync();

            dataverseService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Once);
        }
    }
}