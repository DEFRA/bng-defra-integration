using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Infrastructure.EntityProcessors;

namespace DEFRA.NE.BNG.Integration.Infrastructure.UnitTests.EntityProcessors
{
    public class LocalPlanningAuthorityProcessorTest : TestBase<LocalPlanningAuthorityProcessor>
    {
        private readonly LocalPlanningAuthorityProcessor systemUnderTest;

        public LocalPlanningAuthorityProcessorTest() : base()
        {
            systemUnderTest = new LocalPlanningAuthorityProcessor();
        }

        [Fact]
        public async Task FindFirstWhenLocalPlanningAuthorityNameIsNotSupplied()
        {
            var entityToCreate = new LocalPlanningAuthority()
            {
            };

            var actualId = await systemUnderTest.FindFirst(entityToCreate, dataverseService.Object, logger.Object);

            actualId.Should().Be(Guid.Empty);
            dataverseService.VerifyAll();
            dataverseService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Never);
            dataverseService.Verify(x => x.RetrieveFirstRecord<bng_LocalPlanningAuthority>(It.IsAny<QueryExpression>()), Times.Never);
        }


        [Fact]
        public async Task FindFirstWhenLocalPlanningAuthorityDoesNotExistInDataverse()
        {
            var entityToCreate = new LocalPlanningAuthority()
            {
                Name = "Test"
            };
            bng_LocalPlanningAuthority entity = null;

            dataverseService.Setup(x => x.RetrieveFirstRecord<bng_LocalPlanningAuthority>(It.IsAny<QueryExpression>()))
                              .ReturnsAsync(entity);

            var actualId = await systemUnderTest.FindFirst(entityToCreate, dataverseService.Object, logger.Object);

            actualId.Should().Be(Guid.Empty);
            dataverseService.VerifyAll();
            dataverseService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Never);
        }

        [Fact]
        public async Task FindFirstWhenLocalPlanningAuthorityExistInDataverse()
        {
            var entityToCreate = new LocalPlanningAuthority()
            {
                Name = "Test"
            };
            bng_LocalPlanningAuthority entity = new() { Id = Guid.NewGuid() };

            dataverseService.Setup(x => x.RetrieveFirstRecord<bng_LocalPlanningAuthority>(It.IsAny<QueryExpression>()))
                               .ReturnsAsync(entity);

            var actualId = await systemUnderTest.FindFirst(entityToCreate, dataverseService.Object, logger.Object);

            actualId.Should().Be(entity.Id);
            dataverseService.VerifyAll();
            dataverseService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Never);
        }

    }
}