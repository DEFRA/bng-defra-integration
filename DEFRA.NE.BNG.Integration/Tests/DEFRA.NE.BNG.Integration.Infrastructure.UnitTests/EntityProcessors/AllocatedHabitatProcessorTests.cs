using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Infrastructure.UnitTests;

namespace DEFRA.NE.BNG.Integration.Infrastructure.EntityProcessors.Tests
{
    public class AllocatedHabitatProcessorTests : TestBase<AllocatedHabitatProcessor>
    {
        private readonly AllocatedHabitatProcessor systemUnderTest;

        public AllocatedHabitatProcessorTests() : base()
        {
            systemUnderTest = new AllocatedHabitatProcessor();
        }

        [Fact]
        public async Task Create_ExistingRecord()
        {
            AllocatedHabitat entityToCreate = new();
            bng_HabitatType entity = new();

            dataverseService.Setup(x => x.RetrieveFirstRecord<bng_HabitatType>(It.IsAny<QueryExpression>()))
                                 .ReturnsAsync(entity);

            var actaul = await systemUnderTest.Create(entityToCreate, dataverseService.Object, logger.Object);

            dataverseService.VerifyAll();
            logger.VerifyAll();
            dataverseService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Once());
        }
        [Fact]
        public async Task Create_NewRecord()
        {
            AllocatedHabitat entityToCreate = new()
            {
                GainSiteId = Guid.NewGuid()
            };
            bng_HabitatType entity = new();

            dataverseService.Setup(x => x.RetrieveFirstRecord<bng_HabitatType>(It.IsAny<QueryExpression>()))
                                 .ReturnsAsync(entity);

            var actaul = await systemUnderTest.Create(entityToCreate, dataverseService.Object, logger.Object);

            dataverseService.VerifyAll();
            logger.VerifyAll();
            dataverseService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Once());
        }


        [Fact]
        public void Create_NewRecord_Exception()
        {
            AllocatedHabitat entityToCreate = new()
            {
                GainSiteId = Guid.NewGuid()
            };

            dataverseService.Setup(x => x.RetrieveFirstRecord<bng_HabitatType>(It.IsAny<QueryExpression>()))
                                 .Throws<Exception>();

            FluentActions.Invoking(() => systemUnderTest.Create(entityToCreate, dataverseService.Object, logger.Object))
                         .Should()
                         .ThrowAsync<Exception>();
        }

        [Fact]
        public async Task CreateList_ExistingRecords()
        {
            List<AllocatedHabitat> entityListToCreate = [];
            entityListToCreate.Add(new AllocatedHabitat());
            entityListToCreate.Add(new AllocatedHabitat());

            bng_HabitatType entity = new();

            dataverseService.Setup(x => x.RetrieveFirstRecord<bng_HabitatType>(It.IsAny<QueryExpression>()))
                                 .ReturnsAsync(entity);

            var actaul = await systemUnderTest.CreateList(entityListToCreate, dataverseService.Object, logger.Object);

            dataverseService.VerifyAll();
            logger.VerifyAll();
            dataverseService.Verify(x => x.RetrieveFirstRecord<bng_HabitatType>(It.IsAny<QueryExpression>()), Times.Exactly(2));
            dataverseService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Exactly(2));
        }

        [Fact]
        public async Task CreateList_NewRecords()
        {
            List<AllocatedHabitat> entityListToCreate = [];
            entityListToCreate.Add(new AllocatedHabitat());
            entityListToCreate.Add(new AllocatedHabitat());

            bng_HabitatType entity = new();

            dataverseService.Setup(x => x.RetrieveFirstRecord<bng_HabitatType>(It.IsAny<QueryExpression>()))
                                 .ReturnsAsync(entity);

            var actaul = await systemUnderTest.CreateList(entityListToCreate, dataverseService.Object, logger.Object);

            dataverseService.VerifyAll();
            logger.VerifyAll();
            dataverseService.Verify(x => x.RetrieveFirstRecord<bng_HabitatType>(It.IsAny<QueryExpression>()), Times.Exactly(2));
            dataverseService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Exactly(2));
        }
    }
}