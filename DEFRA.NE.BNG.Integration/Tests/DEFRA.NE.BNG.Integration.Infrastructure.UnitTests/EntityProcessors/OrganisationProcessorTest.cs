using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Infrastructure.EntityProcessors;

namespace DEFRA.NE.BNG.Integration.Infrastructure.UnitTests.EntityProcessors
{
    public class OrganisationProcessorTest : TestBase<OrganisationProcessor>
    {
        private readonly OrganisationProcessor systemUnderTest;

        public OrganisationProcessorTest() : base()
        {
            systemUnderTest = new OrganisationProcessor();
        }

        [Fact]
        public void CreateAplicantWhenOrganisationDoesNotExistInDataverse()
        {
            var createdEntityId = Guid.NewGuid();
            var entityToCreate = new Organisation() { Id = createdEntityId.ToString() };
            Account entity = null;

            dataverseService.Setup(x =>
                             x.RetrieveFirstRecord<Account>(It.IsAny<QueryBase>())
                           )
                    .ReturnsAsync(entity);

            FluentActions.Invoking(() => systemUnderTest.Create(entityToCreate, dataverseService.Object, logger.Object))
                  .Should()
                  .ThrowAsync<InvalidDataException>();
        }

        [Fact]
        public async Task CreateAplicantWhenOrganisationExistInDataverse()
        {
            var createdEntityId = Guid.NewGuid();
            var entityToCreate = new Organisation() { Id = createdEntityId.ToString() };
            Account entity = new() { Id = createdEntityId };

            dataverseService.Setup(x =>
                             x.RetrieveFirstRecord<Account>(It.IsAny<QueryBase>())
                           )
                    .ReturnsAsync(entity);

            var actualId = await systemUnderTest.Create(entityToCreate, dataverseService.Object, logger.Object);

            actualId.Should().Be(entityToCreate.Id);
            dataverseService.VerifyAll();
            dataverseService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Never);
        }

        [Fact]
        public void CreateAplicants()
        {
            var createdEntityId = Guid.NewGuid();
            var entityToCreate = new Organisation() { Id = createdEntityId.ToString() };
            Account entity = null;

            var listOfEntities = new List<Organisation>
            {
                entityToCreate
            };

            dataverseService.Setup(x =>
                            x.RetrieveFirstRecord<Account>(It.IsAny<QueryBase>())
                          )
                   .ReturnsAsync(entity);


            FluentActions.Invoking(() => systemUnderTest.CreateList(listOfEntities, dataverseService.Object, logger.Object))
                 .Should()
                 .ThrowAsync<InvalidDataException>();

            dataverseService.VerifyAll();
        }
    }
}