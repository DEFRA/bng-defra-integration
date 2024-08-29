using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Infrastructure.EntityProcessors;
namespace DEFRA.NE.BNG.Integration.Infrastructure.UnitTests.EntityProcessors
{
    public class HabitatProcessorTests : TestBase<HabitatProcessor>
    {
        private readonly HabitatProcessor systemUnderTest;

        public HabitatProcessorTests() : base()
        {
            systemUnderTest = new HabitatProcessor();
        }

        [Fact]
        public async Task CreateExitingHabitat()
        {
            var entityId = Guid.NewGuid();
            var entityToCreate = new Habitat
            {
            };

            bng_HabitatType entity = new() { Id = entityId };

            dataverseService.Setup(x => x.RetrieveFirstRecord<bng_HabitatType>(It.IsAny<QueryExpression>()))
                              .ReturnsAsync(entity);

            var actualId = await systemUnderTest.Create(entityToCreate, dataverseService.Object, logger.Object);

            actualId.Should().Be(entityId);
            dataverseService.VerifyAll();
        }

        [Fact]
        public async Task CreateNewHabitat()
        {
            var entityId = Guid.NewGuid();
            var entityToCreate = new Habitat();

            bng_HabitatType entity = new() { Id = entityId };

            dataverseService.Setup(x => x.RetrieveFirstRecord<bng_HabitatType>(It.IsAny<QueryExpression>()))
                              .ReturnsAsync(entity);

            var actualId = await systemUnderTest.Create(entityToCreate, dataverseService.Object, logger.Object);

            actualId.Should().Be(entityId);
            dataverseService.VerifyAll();
        }

        [Fact]
        public async Task CreateHabitats()
        {
            var entityToCreate = new Habitat()
            {
            };
            bng_HabitatType entity = null;
            var entityId = Guid.NewGuid();

            var listOfEntities = new List<Habitat>
            {
                entityToCreate
            };


            dataverseService.Setup(x => x.RetrieveFirstRecord<bng_HabitatType>(It.IsAny<QueryExpression>()))
                              .ReturnsAsync(entity);

            await systemUnderTest.CreateList(listOfEntities, dataverseService.Object, logger.Object);

            dataverseService.VerifyAll();
        }
    }
}