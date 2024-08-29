using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Infrastructure.EntityProcessors;

namespace DEFRA.NE.BNG.Integration.Infrastructure.UnitTests.EntityProcessors
{
    public class ProductProcessorTest : TestBase<ProductProcessor>
    {
        private readonly ProductProcessor systemUnderTest;

        public ProductProcessorTest() : base()
        {
            systemUnderTest = new ProductProcessor();
        }

        [Fact]
        public async Task CreateWhenProductDoesNotExistInDataverse()
        {
            var entityToCreate = new Domain.Entities.Product();
            Domain.Models.Product entity = null;
            var entityId = Guid.NewGuid();

            dataverseService.Setup(x => x.RetrieveFirstRecord<Domain.Models.Product>(It.IsAny<QueryExpression>()))
                               .ReturnsAsync(entity);

            var actualId = await systemUnderTest.Create(entityToCreate, dataverseService.Object, logger.Object);

            actualId.Should().BeEmpty();
            dataverseService.VerifyAll();
        }

        [Fact]
        public async Task CreateWhenProductExistInDataverse()
        {
            var entityToCreate = new Domain.Entities.Product();
            Domain.Models.Product entity = new() { Id = Guid.NewGuid() };
            entity.Attributes.Add(DataverseExtensions.AttributeLogicalName<Domain.Models.Product>(nameof(Domain.Models.Product.Price)), new Money(12.09M));
            var createdId = Guid.NewGuid();

            dataverseService.Setup(x => x.RetrieveFirstRecord<Domain.Models.Product>(It.IsAny<QueryExpression>()))
                               .ReturnsAsync(entity);

            dataverseService.Setup(x => x.RetrieveFirstRecord<Domain.Models.UoM>(It.IsAny<QueryExpression>()))
                               .ReturnsAsync(new UoM() { Id = Guid.NewGuid() });

            dataverseService.Setup(x => x.CreateAsync(It.IsAny<Entity>()))
                               .ReturnsAsync(createdId);

            var actualId = await systemUnderTest.Create(entityToCreate, dataverseService.Object, logger.Object);

            actualId.Should().Be(createdId);
            dataverseService.VerifyAll();
            dataverseService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Once);
            dataverseService.Verify(x => x.UpdateAsync(It.IsAny<Entity>()), Times.Once);
        }

        [Fact]
        public async Task CreateProducts()
        {
            var entityToCreate = new Domain.Entities.Product();
            Domain.Models.Product entity = new() { Id = Guid.NewGuid() };
            entity.Attributes.Add(DataverseExtensions.AttributeLogicalName<Domain.Models.Product>(nameof(Domain.Models.Product.Price)), new Money(12.09M));
            var createdId = Guid.NewGuid();

            var listOfEntities = new List<Domain.Entities.Product>
            {
                entityToCreate
            };

            dataverseService.Setup(x => x.RetrieveFirstRecord<Domain.Models.Product>(It.IsAny<QueryExpression>()))
                               .ReturnsAsync(entity);

            dataverseService.Setup(x => x.RetrieveFirstRecord<Domain.Models.UoM>(It.IsAny<QueryExpression>()))
                               .ReturnsAsync(new UoM() { Id = Guid.NewGuid() });

            dataverseService.Setup(x => x.CreateAsync(It.IsAny<Entity>()))
                               .ReturnsAsync(createdId);


            await systemUnderTest.CreateList(listOfEntities, dataverseService.Object, logger.Object);

            dataverseService.VerifyAll();
            dataverseService.VerifyAll();
            dataverseService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.AtLeastOnce);
            dataverseService.Verify(x => x.UpdateAsync(It.IsAny<Entity>()), Times.AtLeastOnce);
        }

        [Fact]
        public void CalculateTax()
        {
            decimal pricePerUnit = 42000;
            decimal quantity = 5;
            decimal percentage = 20;

            var actual = ProductProcessor.CalculateTax(pricePerUnit, quantity, percentage);

            actual.Value.Should().Be(42000M);
        }
    }
}