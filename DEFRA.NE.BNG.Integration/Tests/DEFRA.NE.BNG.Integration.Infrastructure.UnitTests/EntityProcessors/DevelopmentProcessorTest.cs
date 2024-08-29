using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Infrastructure.EntityProcessors;

namespace DEFRA.NE.BNG.Integration.Infrastructure.UnitTests.EntityProcessors
{
    public class DevelopmentProcessorTest : TestBase<DevelopmentProcessor>
    {
        private readonly DevelopmentProcessor systemUnderTest;
        private readonly string referenceNumber;
        private readonly EntityReference applicant;
        private readonly EntityReference localPlanningAuthority;

        public DevelopmentProcessorTest() : base()
        {
            referenceNumber = Guid.NewGuid().ToString();
            applicant = new("contact", Guid.NewGuid());
            localPlanningAuthority = new("localPlanningAuthority", Guid.NewGuid());

            systemUnderTest = new DevelopmentProcessor();
        }

        [Fact]
        public async Task CreateExitingDevelopment()
        {
            var entityToCreate = new DevelopmentDetails
            {
                LocalPlanningAuthority = new LocalPlanningAuthority(),
                PlanningReference = localPlanningAuthority.ToString()
            };

            Entity entity = new("sample", Guid.NewGuid());

            var aliasedValue = new AliasedValue(bng_GainSiteRegistration.EntityLogicalName,
                DataverseExtensions.AttributeLogicalName<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.bng_GainSiteReference)),
                Guid.NewGuid());
            entity.Attributes.Add($"gs.{DataverseExtensions.AttributeLogicalName<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.bng_GainSiteReference))}", aliasedValue);


            var entityCollection = new EntityCollection() { };
            entityCollection.Entities.Add(entity);


            dataverseService.Setup(x => x.RetrieveMultipleAsync(It.IsAny<QueryExpression>()))
                               .ReturnsAsync(entityCollection);


            var actualId = await systemUnderTest.Create(entityToCreate, referenceNumber, applicant, localPlanningAuthority, dataverseService.Object, logger.Object);

            actualId.Item1.Should().Be(entity.Id);
            dataverseService.VerifyAll();
        }

        [Fact]
        public async Task Create_NewDevelopment()
        {
            var entityId = Guid.NewGuid();
            var entityToCreate = new DeveloperRegistration
            {
                Development = new DevelopmentDetails
                {
                    LocalPlanningAuthority = new LocalPlanningAuthority(),
                    PlanningReference = Guid.NewGuid().ToString()
                },
                Applicant = new Applicant
                {
                    Id = Guid.NewGuid().ToString()
                }
            };

            var entityCollection = new EntityCollection() { };

            dataverseService.Setup(x => x.RetrieveMultipleAsync(It.IsAny<QueryExpression>()))
                               .ReturnsAsync(entityCollection);

            dataverseService.Setup(x => x.CreateAsync(It.IsAny<Entity>()))
                            .ReturnsAsync(entityId);

            var actualId = await systemUnderTest.Create(entityToCreate.Development, referenceNumber, applicant, localPlanningAuthority, dataverseService.Object, logger.Object);

            dataverseService.VerifyAll();
        }

        [Fact]
        public void Create_NewDevelopment_ThrowsException()
        {
            var entityId = Guid.NewGuid();
            var entityToCreate = new DeveloperRegistration
            {
                Development = new DevelopmentDetails
                {
                    LocalPlanningAuthority = new LocalPlanningAuthority()
                },
                Applicant = new Applicant
                {
                    Id = Guid.NewGuid().ToString()
                }
            };

            bng_DeveloperRegistration entity = null;

            dataverseService.Setup(x => x.RetrieveFirstRecord<bng_DeveloperRegistration>(It.IsAny<QueryExpression>()))
                               .ReturnsAsync(entity);

            dataverseService.Setup(x => x.CreateAsync(It.IsAny<Entity>()))
                            .Throws<Exception>();

            FluentActions.Invoking(() => systemUnderTest.Create(entityToCreate.Development, referenceNumber, applicant, localPlanningAuthority, dataverseService.Object, logger.Object))
                         .Should()
                         .ThrowAsync<Exception>();
        }


        [Fact]
        public async Task Create_ExistingDevelopment()
        {
            var entityId = Guid.NewGuid();
            var entityToCreate = new DeveloperRegistration
            {
                Development = new DevelopmentDetails
                {
                    LocalPlanningAuthority = new LocalPlanningAuthority(),
                    PlanningReference = Guid.NewGuid().ToString()
                }
            };

            var entityCollection = new EntityCollection() { };

            dataverseService.Setup(x => x.RetrieveMultipleAsync(It.IsAny<QueryExpression>()))
                               .ReturnsAsync(entityCollection);

            var actualId = await systemUnderTest.Create(entityToCreate.Development, referenceNumber, applicant, localPlanningAuthority, dataverseService.Object, logger.Object);

            dataverseService.VerifyAll();
        }

        [Fact]
        public async Task RetrieveDeveloperRegistration_NoProjectName_NoPlanningReference()
        {
            DevelopmentDetails development = new();
            EntityReference localPlanningAuthority = new();

            EntityCollection entityCollection = new() { };

            dataverseService.Setup(x => x.RetrieveMultipleAsync(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(entityCollection);

            var actual = await DevelopmentProcessor.RetrieveDeveloperRegistration(development, localPlanningAuthority, dataverseService.Object);

            actual.Should().BeNull();
            dataverseService.Verify(x => x.RetrieveMultipleAsync(It.IsAny<QueryExpression>()), Times.Never);
        }

        [Fact]
        public async Task RetrieveDeveloperRegistration_NoProjectName_ValidPlanningReference()
        {
            DevelopmentDetails development = new()
            {
                PlanningReference = "test"
            };
            EntityReference localPlanningAuthority = new();

            EntityCollection entityCollection = new() { };

            dataverseService.Setup(x => x.RetrieveMultipleAsync(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(entityCollection);

            var actual = await DevelopmentProcessor.RetrieveDeveloperRegistration(development, localPlanningAuthority, dataverseService.Object);

            actual.Should().NotBeNull();
            dataverseService.Verify(x => x.RetrieveMultipleAsync(It.IsAny<QueryExpression>()), Times.Once);
        }

        [Fact]
        public async Task RetrieveDeveloperRegistration_ValidProjectName_NoPlanningReference()
        {
            DevelopmentDetails development = new()
            {
                Name = "test"
            };
            EntityReference localPlanningAuthority = new();

            EntityCollection entityCollection = new() { };

            dataverseService.Setup(x => x.RetrieveMultipleAsync(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(entityCollection);

            var actual = await DevelopmentProcessor.RetrieveDeveloperRegistration(development, localPlanningAuthority, dataverseService.Object);

            actual.Should().BeNull();
            dataverseService.Verify(x => x.RetrieveMultipleAsync(It.IsAny<QueryExpression>()), Times.Never);
        }


        [Fact]
        public async Task RetrieveDeveloperRegistration_ValidProjectNameAndPlanningReference_NoPlanningAuthority()
        {
            DevelopmentDetails development = new()
            {
                Name = "test",
                PlanningReference = "Test"
            };
            EntityReference localPlanningAuthority = null;

            EntityCollection entityCollection = new() { };

            dataverseService.Setup(x => x.RetrieveMultipleAsync(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(entityCollection);

            var actual = await DevelopmentProcessor.RetrieveDeveloperRegistration(development, localPlanningAuthority, dataverseService.Object);

            actual.Should().NotBeNull();
            dataverseService.Verify(x => x.RetrieveMultipleAsync(It.IsAny<QueryExpression>()), Times.Once);
        }
    }
}