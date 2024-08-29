using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Infrastructure.EntityProcessors;

namespace DEFRA.NE.BNG.Integration.Infrastructure.UnitTests.EntityProcessors
{
    public class OrderProcessorTest : TestBase<OrderProcessor>
    {
        private readonly OrderProcessor systemUnderTest;

        public OrderProcessorTest() : base()
        {
            systemUnderTest = new OrderProcessor();
        }

        [Fact]
        public async Task CreateOrder()
        {
            var entityToCreate = new Order
            {
                OwningTeam = new EntityReference("contact", Guid.NewGuid())
            };
            var entityId = Guid.NewGuid();
            Entity entity = new("salesorder", entityId);

            dataverseService.Setup(x => x.CreateAsync(It.IsAny<Entity>()))
            .ReturnsAsync(entityId);

            var actualId = await systemUnderTest.Create(entityToCreate, dataverseService.Object, logger.Object);

            actualId.Should().Be(entityId);
            dataverseService.VerifyAll();
            dataverseService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.AtLeastOnce());
        }

        [Fact]
        public async Task CreateOrders()
        {
            var entityToCreate = new Order();
            var entityCollection = new EntityCollection();
            var entityId = Guid.NewGuid();

            var listOfEntities = new List<Order>
            {
                entityToCreate
            };

            dataverseService.Setup(x => x.CreateAsync(It.IsAny<Entity>()))
                               .ReturnsAsync(entityId);

            await systemUnderTest.CreateList(listOfEntities, dataverseService.Object, logger.Object);

            dataverseService.VerifyAll();
        }

        [Fact]
        public void CreateCDD_NoOrderFound()
        {
            Guid orderId = Guid.NewGuid();
            decimal orderTotalAmountThreshold = 0.93M;
            EntityReference customer = new(Contact.EntityLogicalName, Guid.NewGuid());

            SalesOrder orderEntity = null;

            dataverseService.Setup(x => x.RetrieveFirstRecord<SalesOrder>(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(orderEntity);

            FluentActions.Invoking(() => systemUnderTest.CreateCdd(orderId, orderTotalAmountThreshold, customer, dataverseService.Object, logger.Object))
                         .Should()
                         .NotThrowAsync();

            dataverseService.VerifyAll();

        }

        [Fact]
        public void CreateCDD_OrderSearchThrowsException()
        {
            Guid orderId = Guid.NewGuid();
            decimal orderTotalAmountThreshold = 0.93M;
            EntityReference customer = new(Contact.EntityLogicalName, Guid.NewGuid());

            dataverseService.Setup(x => x.RetrieveFirstRecord<SalesOrder>(It.IsAny<QueryExpression>()))
                            .Throws<Exception>();

            FluentActions.Invoking(() => systemUnderTest.CreateCdd(orderId, orderTotalAmountThreshold, customer, dataverseService.Object, logger.Object))
                         .Should()
                         .NotThrowAsync();

            dataverseService.VerifyAll();

        }

        [Fact]
        public void CreateCDD()
        {
            Guid orderId = Guid.NewGuid();
            decimal orderTotalAmountThreshold = 0.93M;
            EntityReference customer = new(Contact.EntityLogicalName, Guid.NewGuid());

            SalesOrder orderEntity = new()
            {
                Id = Guid.NewGuid(),
                TotalAmount = new Money(60000.90M)
            };

            dataverseService.Setup(x => x.RetrieveFirstRecord<SalesOrder>(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(orderEntity);

            FluentActions.Invoking(() => systemUnderTest.CreateCdd(orderId, orderTotalAmountThreshold, customer, dataverseService.Object, logger.Object))
                         .Should()
                         .NotThrowAsync();

            dataverseService.VerifyAll();
            dataverseService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Once());
        }
    }
}