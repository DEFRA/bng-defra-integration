using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Infrastructure.EntityProcessors;

namespace DEFRA.NE.BNG.Integration.Infrastructure.UnitTests.EntityProcessors
{
    public class ApplicantProcessorTest : TestBase<ApplicantProcessor>
    {
        private ApplicantProcessor systemUnderTest;

        public ApplicantProcessorTest() : base()
        {
        }

        [Fact]
        public void CreateAplicantWhenApplicantDoesNotExistInDataverse()
        {
            systemUnderTest = new ApplicantProcessor();
            var createdEntityId = Guid.NewGuid();
            var entityToCreate = new Applicant() { Id = createdEntityId.ToString() };
            Contact result = null;

            dataverseService.Setup(x =>
                            x.RetrieveFirstRecord<Contact>(It.IsAny<QueryExpression>())
                          )
                   .ReturnsAsync(result);

            FluentActions.Invoking(() => systemUnderTest.Create(entityToCreate, dataverseService.Object, logger.Object))
                         .Should()
                         .ThrowAsync<InvalidDataException>();


            dataverseService.VerifyAll();
        }

        [Fact]
        public async Task CreateAplicantWhenApplicantExistInDataverse()
        {
            systemUnderTest = new ApplicantProcessor();
            var entityToCreate = new Applicant() { Id = Guid.NewGuid().ToString() };
            var entity = GenerateSampleContacts(1).First();

            dataverseService.Setup(x =>
                                        x.RetrieveFirstRecord<Contact>(It.IsAny<QueryExpression>())
                                      )
                               .ReturnsAsync(entity);

            var actualId = await systemUnderTest.Create(entityToCreate, dataverseService.Object, logger.Object);

            actualId.Should().Be(entity.Id);
            dataverseService.VerifyAll();
            dataverseService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Never);
            dataverseService.Verify(x => x.UpdateAsync(It.IsAny<Entity>()), Times.Never);
        }

        [Fact]
        public async Task CreateAplicantWhenApplicantExistInDataverse_NationalityUpdated()
        {
            systemUnderTest = new ApplicantProcessor();
            var entityToCreate = new Applicant
            {
                Id = Guid.NewGuid().ToString(),
                NationalityIdList = [Guid.NewGuid()]
            };
            var entity = GenerateSampleContacts(1).First();

            dataverseService.Setup(x =>
                                        x.RetrieveFirstRecord<Contact>(It.IsAny<QueryExpression>())
                                      )
                               .ReturnsAsync(entity);

            var actualId = await systemUnderTest.Create(entityToCreate, dataverseService.Object, logger.Object);

            actualId.Should().Be(entity.Id);
            dataverseService.VerifyAll();
            dataverseService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Never);
            dataverseService.Verify(x => x.UpdateAsync(It.IsAny<Entity>()), Times.Once);
        }

        [Fact]
        public async Task CreateAplicantWhenApplicantExistInDataverse_MidleNameUpdated()
        {
            systemUnderTest = new ApplicantProcessor();
            var entityToCreate = new Applicant
            {
                Id = Guid.NewGuid().ToString(),
                MiddleName = "Smith"
            };
            var entity = GenerateSampleContacts(1).First();

            dataverseService.Setup(x =>
                                        x.RetrieveFirstRecord<Contact>(It.IsAny<QueryExpression>())
                                      )
                               .ReturnsAsync(entity);

            var actualId = await systemUnderTest.Create(entityToCreate, dataverseService.Object, logger.Object);

            actualId.Should().Be(entity.Id);
            dataverseService.VerifyAll();
            dataverseService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Never);
            dataverseService.Verify(x => x.UpdateAsync(It.IsAny<Entity>()), Times.Never);
        }

        [Fact]
        public async Task CreateAplicantWhenApplicantExistInDataverse_DateOfBirthUpdated()
        {
            systemUnderTest = new ApplicantProcessor();
            var entityToCreate = new Applicant
            {
                Id = Guid.NewGuid().ToString(),
                DateOfBirth = DateTime.UtcNow.AddYears(-20).ToShortDateString()
            };
            var entity = GenerateSampleContacts(1).First();


            dataverseService.Setup(x =>
                                        x.RetrieveFirstRecord<Contact>(It.IsAny<QueryExpression>())
                                      )
                               .ReturnsAsync(entity);

            var actualId = await systemUnderTest.Create(entityToCreate, dataverseService.Object, logger.Object);

            actualId.Should().Be(entity.Id);
            dataverseService.VerifyAll();
            dataverseService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Never);
            dataverseService.Verify(x => x.UpdateAsync(It.IsAny<Entity>()), Times.Once);
        }

        [Fact]
        public void CreateAplicants_EntityDoesNotExist()
        {
            systemUnderTest = new ApplicantProcessor();
            var createdEntityId = Guid.NewGuid();
            var entityToCreate = new Applicant()
            {
                Id = createdEntityId.ToString(),
                NationalityIdList =
                [
                    Guid.NewGuid(),
                    Guid.NewGuid()
                ]
            };
            Contact retrievedEntity = null;

            var listOfEntities = new List<Applicant>
            {
                entityToCreate
            };

            dataverseService.Setup(x =>
                                        x.RetrieveFirstRecord<Contact>(It.IsAny<QueryExpression>())
                                      )
                               .ReturnsAsync(retrievedEntity);

            FluentActions.Invoking(() => systemUnderTest.CreateList(listOfEntities, dataverseService.Object, logger.Object))
                          .Should()
                          .ThrowAsync<InvalidDataException>();

            dataverseService.VerifyAll();
        }

        [Fact]
        public async Task CreateAplicants_EntityExist()
        {
            systemUnderTest = new ApplicantProcessor();
            var createdEntityId = Guid.NewGuid();
            var entityToCreate = new Applicant()
            {
                Id = createdEntityId.ToString(),
                NationalityIdList =
                [
                    Guid.NewGuid(),
                    Guid.NewGuid()
                ]
            };
            Contact retrievedEntity = new() { Id = Guid.NewGuid() };

            var listOfEntities = new List<Applicant>
            {
                entityToCreate
            };

            dataverseService.Setup(x =>
                                        x.RetrieveFirstRecord<Contact>(It.IsAny<QueryExpression>())
                                      )
                               .ReturnsAsync(retrievedEntity);

            dataverseService.Setup(x => x.UpdateAsync(It.IsAny<Entity>()));

            await systemUnderTest.CreateList(listOfEntities, dataverseService.Object, logger.Object);

            dataverseService.VerifyAll();
        }
    }
}