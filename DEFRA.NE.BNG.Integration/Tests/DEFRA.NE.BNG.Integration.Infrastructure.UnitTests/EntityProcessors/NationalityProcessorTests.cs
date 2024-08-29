using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Infrastructure.UnitTests;

namespace DEFRA.NE.BNG.Integration.Infrastructure.EntityProcessors.Tests
{
    public class NationalityProcessorTests : TestBase<NationalityProcessor>
    {
        private readonly NationalityProcessor systemUnderTest;

        public NationalityProcessorTests() : base()
        {
            systemUnderTest = new NationalityProcessor();
        }

        [Fact]
        public async Task Create_ExistingRecord()
        {
            Nationality entityToCreate = new();
            bng_Nationality entity = new();

            dataverseService.Setup(x => x.RetrieveFirstRecord<bng_Nationality>(It.IsAny<QueryExpression>()))
                                 .ReturnsAsync(entity);

            var actaul = await systemUnderTest.Create(entityToCreate, dataverseService.Object, logger.Object);

            dataverseService.VerifyAll();
            logger.VerifyAll();
            dataverseService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Never());
        }
        [Fact]
        public async Task Create_NewRecord()
        {
            Nationality entityToCreate = new();
            bng_Nationality entity = null;

            dataverseService.Setup(x => x.RetrieveFirstRecord<bng_Nationality>(It.IsAny<QueryExpression>()))
                                 .ReturnsAsync(entity);

            var actaul = await systemUnderTest.Create(entityToCreate, dataverseService.Object, logger.Object);

            dataverseService.VerifyAll();
            logger.VerifyAll();
            dataverseService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Once());
        }

        [Fact]
        public async Task CreateList_ExistingRecords()
        {
            List<Nationality> entityListToCreate = [];
            entityListToCreate.Add(new Nationality());
            entityListToCreate.Add(new Nationality());

            bng_Nationality entity = new();

            dataverseService.Setup(x => x.RetrieveFirstRecord<bng_Nationality>(It.IsAny<QueryExpression>()))
                                 .ReturnsAsync(entity);

            var actaul = await systemUnderTest.CreateList(entityListToCreate, dataverseService.Object, logger.Object);

            dataverseService.VerifyAll();
            logger.VerifyAll();
            dataverseService.Verify(x => x.RetrieveFirstRecord<bng_Nationality>(It.IsAny<QueryExpression>()), Times.Exactly(2));
            dataverseService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Never());
        }


        [Fact]
        public async Task CreateList_NewRecords()
        {
            List<Nationality> entityListToCreate = [];
            entityListToCreate.Add(new Nationality());
            entityListToCreate.Add(new Nationality());

            bng_Nationality entity = null;

            dataverseService.Setup(x => x.RetrieveFirstRecord<bng_Nationality>(It.IsAny<QueryExpression>()))
                                 .ReturnsAsync(entity);

            var actaul = await systemUnderTest.CreateList(entityListToCreate, dataverseService.Object, logger.Object);

            dataverseService.VerifyAll();
            logger.VerifyAll();
            dataverseService.Verify(x => x.RetrieveFirstRecord<bng_Nationality>(It.IsAny<QueryExpression>()), Times.Exactly(2));
            dataverseService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Exactly(2));
        }
    }
}