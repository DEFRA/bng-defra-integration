
using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.JsonRequestRoot;
using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Model.Request;
using Microsoft.Xrm.Sdk.Messages;

namespace DEFRA.NE.BNG.Integration.Infrastructure.UnitTests.Services
{
    public class DataverseServiceTests : TestBase<DataverseService>
    {
        private DataverseService systemUnderTest;
        private Mock<IOrganizationServiceAsync2> organizationService;

        public DataverseServiceTests() : base()
        {
            organizationService = new Mock<IOrganizationServiceAsync2>();

            systemUnderTest = new DataverseService(logger.Object, blobClientAccess.Object, organizationService.Object);
        }

        [Fact]
        public void InstantiateDataVerseService()
        {
            FluentActions.Invoking(() => systemUnderTest = new DataverseService(logger.Object,
                                                              blobClientAccess.Object,
                                                              organizationService.Object))
                         .Should()
                         .NotThrow();

            systemUnderTest.BlobClientAccess.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateAsync()
        {
            var expected = Guid.NewGuid();

            var entity = new Entity();

            organizationService.Setup(x => x.CreateAsync(It.IsAny<Entity>()))
                               .ReturnsAsync(expected);

            var actual = await systemUnderTest.CreateAsync(entity);

            actual.Should().Be(expected);
        }

        [Fact]
        public async Task CreateListAsync_OneEntity()
        {
            var entityList = new List<Entity>()
            {
                new("contact",Guid.NewGuid())
            };

            organizationService.Setup(x => x.CreateAsync(It.IsAny<Entity>()))
                               .ReturnsAsync(Guid.NewGuid);

            var actual = await systemUnderTest.CreateListAsync(entityList);

            actual.Count.Should().Be(1);

            organizationService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Once);
            organizationService.VerifyAll();
        }


        [Fact]
        public async Task CreateListAsync_ThreeEntities()
        {
            var entityList = new List<Entity>()
            {
                new("contact",Guid.NewGuid()),
                new("contact",Guid.NewGuid()),
                new("contact",Guid.NewGuid()),
            };

            organizationService.Setup(x => x.CreateAsync(It.IsAny<Entity>()))
                               .ReturnsAsync(Guid.NewGuid);

            var actual = await systemUnderTest.CreateListAsync(entityList);

            actual.Count.Should().Be(3);

            organizationService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Exactly(3));
            organizationService.VerifyAll();
        }

        [Fact]
        public void CreateListAsync_ThrowsException()
        {
            var entityList = new List<Entity>()
            {
                new("contact",Guid.NewGuid()),
                new("contact",Guid.NewGuid()),
                new("contact",Guid.NewGuid()),
            };

            organizationService.Setup(x => x.CreateAsync(It.IsAny<Entity>()))
                               .Throws<Exception>();

            FluentActions.Invoking(() => systemUnderTest.CreateListAsync(entityList))
                         .Should()
                         .ThrowAsync<Exception>();
        }

        [Fact]
        public async Task RetrieveAsync()
        {
            var entityName = "contact";
            var id = Guid.NewGuid();
            var columnSet = new ColumnSet();

            Contact expected = new() { Id = id };

            organizationService.Setup(x => x.RetrieveAsync(entityName, It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                                .ReturnsAsync(expected);

            var actual = await systemUnderTest.RetrieveAsync<Contact>(id, columnSet);

            actual.Should().NotBeNull();
            actual.Id.Should().Be(expected.Id);
            actual.LogicalName.Should().Be(expected.LogicalName);
            organizationService.VerifyAll();
        }

        [Fact]
        public async Task RetrieveFirstRecordForEntity_NoResult()
        {
            var entityName = "contact";
            var id = Guid.NewGuid();
            var columnSet = new ColumnSet();
            var query = new QueryExpression();

            var expected = new Entity(entityName, id);
            var entityCollection = new EntityCollection();

            organizationService.Setup(x => x.RetrieveMultipleAsync(It.IsAny<QueryBase>()))
                               .ReturnsAsync(entityCollection);

            var actual = await systemUnderTest.RetrieveFirstRecordForEntity(query);

            actual.Should().Be(Guid.Empty);
            organizationService.VerifyAll();
        }

        [Fact]
        public async Task RetrieveFirstRecordForEntity_3Result()
        {
            var entityName = "contact";
            var columnSet = new ColumnSet();
            var query = new QueryExpression();

            var entityCollection = new EntityCollection();
            entityCollection.Entities.Add(new Entity(entityName, Guid.NewGuid()));
            entityCollection.Entities.Add(new Entity(entityName, Guid.NewGuid()));
            entityCollection.Entities.Add(new Entity(entityName, Guid.NewGuid()));

            organizationService.Setup(x => x.RetrieveMultipleAsync(It.IsAny<QueryBase>()))
                               .ReturnsAsync(entityCollection);

            var actual = await systemUnderTest.RetrieveFirstRecordForEntity(query);

            actual.Should().Be(entityCollection.Entities[0].Id);
            organizationService.VerifyAll();
        }


        [Fact]
        public async Task RetrieveFirstRecord_NoResult()
        {
            var entityName = "contact";
            var id = Guid.NewGuid();
            var columnSet = new ColumnSet();
            var query = new QueryExpression();

            var expected = new Entity(entityName, id);
            var entityCollection = new EntityCollection();


            organizationService.Setup(x => x.RetrieveMultipleAsync(It.IsAny<QueryBase>()))
                               .ReturnsAsync(entityCollection);

            var actual = await systemUnderTest.RetrieveFirstRecord<Contact>(query);

            actual.Should().BeNull();
            organizationService.VerifyAll();
        }

        [Fact]
        public async Task RetrieveFirstRecord_3Result()
        {
            var entityCollection = new EntityCollection();

            foreach (var item in GenerateSampleContacts(10))
            {
                entityCollection.Entities.Add(item);
            }

            var expectedEntity = entityCollection.Entities[0];

            organizationService.Setup(x => x.RetrieveMultipleAsync(It.IsAny<QueryBase>()))
                               .ReturnsAsync(entityCollection);

            var actual = await systemUnderTest.RetrieveFirstRecord<Contact>(new QueryExpression());

            actual.Id.Should().Be(expectedEntity.Id);
            actual.LogicalName.Should().Be(entityCollection.Entities[0].LogicalName);
            actual.AssistantName.Should().Be((string)expectedEntity[DataverseExtensions.AttributeLogicalName<Contact>(nameof(Contact.AssistantName))]);
            actual.EMailAddress1.Should().Be((string)expectedEntity[DataverseExtensions.AttributeLogicalName<Contact>(nameof(Contact.EMailAddress1))]);
            actual.AccountId.Id.Should().Be(((EntityReference)expectedEntity[DataverseExtensions.AttributeLogicalName<Contact>(nameof(Contact.AccountId))]).Id);
            actual.LastName.Should().Be((string)expectedEntity[DataverseExtensions.AttributeLogicalName<Contact>(nameof(Contact.LastName))]);
            actual.FirstName.Should().Be((string)expectedEntity[DataverseExtensions.AttributeLogicalName<Contact>(nameof(Contact.FirstName))]);

            organizationService.VerifyAll();
        }

        [Fact]
        public async Task RetrieveMultipleAsync_NoResult()
        {
            var entityName = "contact";
            var id = Guid.NewGuid();
            var columnSet = new ColumnSet();
            var query = new QueryExpression();

            var expected = new Entity(entityName, id);
            var entityCollection = new EntityCollection();

            organizationService.Setup(x => x.RetrieveMultipleAsync(It.IsAny<QueryBase>()))
                               .ReturnsAsync(entityCollection);

            var actual = await systemUnderTest.RetrieveMultipleAsync(query);

            actual.Should().NotBeNull();
            actual.Entities.Count.Should().Be(0);
            organizationService.VerifyAll();
        }

        [Fact]
        public async Task RetrieveMultipleAsync_3Result()
        {
            var entityName = "contact";
            var columnSet = new ColumnSet();
            var query = new QueryExpression();

            var entityCollection = new EntityCollection();
            entityCollection.Entities.Add(new Entity(entityName, Guid.NewGuid()));
            entityCollection.Entities.Add(new Entity(entityName, Guid.NewGuid()));
            entityCollection.Entities.Add(new Entity(entityName, Guid.NewGuid()));

            organizationService.Setup(x => x.RetrieveMultipleAsync(It.IsAny<QueryBase>()))
                               .ReturnsAsync(entityCollection);

            var actual = await systemUnderTest.RetrieveMultipleAsync(query);

            actual.Should().NotBeNull();
            actual.Entities.Count.Should().Be(entityCollection.Entities.Count);
            organizationService.VerifyAll();
        }

        [Fact]
        public async Task RetrieveMultipleAsync_Generic_3Result()
        {
            var query = new QueryExpression();

            var entityCollection = new EntityCollection();
            foreach (var item in GenerateSampleContacts(10))
            {
                entityCollection.Entities.Add(item);
            }

            organizationService.Setup(x => x.RetrieveMultipleAsync(It.IsAny<QueryBase>()))
                               .ReturnsAsync(entityCollection);

            var actual = await systemUnderTest.RetrieveMultipleAsync<Contact>(query);

            actual.Should().NotBeNull();
            actual.Count.Should().Be(entityCollection.Entities.Count);
            organizationService.VerifyAll();
        }


        [Fact]
        public void UpdateAsync()
        {
            var entity = new Entity();

            organizationService.Setup(x => x.UpdateAsync(It.IsAny<Entity>()));

            FluentActions.Invoking(async () => await systemUnderTest.UpdateAsync(entity))
                         .Should()
                         .NotThrowAsync();

            organizationService.VerifyAll();
        }

        [Fact]
        public void AssosiateTwoEntitiesRecords()
        {
            var parentEntityReference = new EntityReference("contact", Guid.NewGuid());
            var relationship = new Relationship("sample_relationship");
            var relatedEntities = new List<EntityReference> {
                    new( "entity2", Guid.NewGuid()) };

            organizationService.Setup(x => x.AssociateAsync(It.IsAny<string>(),
                                                          It.IsAny<Guid>(),
                                                          It.IsAny<Relationship>(),
                                                          It.IsAny<EntityReferenceCollection>()));

            FluentActions.Invoking(() => systemUnderTest.AssosiateTwoEntitiesRecords(parentEntityReference, "sample_relationship", relatedEntities))
            .Should()
            .NotThrowAsync();

            organizationService.VerifyAll();
        }

        [Fact]
        public void AssosiateTwoEntitiesRecords_ThrowsException()
        {
            var parentEntityReference = new EntityReference("contact", Guid.NewGuid());
            var relationship = new Relationship("sample_relationship");
            var relatedEntities = new List<EntityReference> {
                    new( "entity2", Guid.NewGuid()) };

            organizationService.Setup(x => x.AssociateAsync(It.IsAny<string>(),
                                                          It.IsAny<Guid>(),
                                                          It.IsAny<Relationship>(),
                                                          It.IsAny<EntityReferenceCollection>()))
                                .Throws<Exception>();

            FluentActions.Invoking(() => systemUnderTest.AssosiateTwoEntitiesRecords(parentEntityReference, "sample_relationship", relatedEntities))
                         .Should()
                         .NotThrowAsync();

            organizationService.VerifyAll();
        }

        [Fact]
        public void AssociateAsync()
        {
            var entityName = "contact";
            var entityId = Guid.NewGuid();
            var relationship = new Relationship("sample_relationship");
            var relatedEntities = new EntityReferenceCollection(
                [
                    new( entityName, entityId) ]
                );

            FluentActions.Invoking(() => systemUnderTest.AssociateAsync(entityName, entityId, relationship, relatedEntities))
                         .Should()
                         .NotThrowAsync();
        }

        [Fact]
        public void ExecuteAsync()
        {
            var request = new OrganizationRequest();

            var response = new OrganizationResponse();

            organizationService.Setup(x => x.ExecuteAsync(It.IsAny<OrganizationRequest>()))
                               .ReturnsAsync(response);

            FluentActions.Invoking(() => systemUnderTest.ExecuteAsync(request))
                         .Should()
                         .NotThrowAsync();

            organizationService.VerifyAll();
        }

        [Fact]
        public void UpsertEntity_Exception()
        {
            var entity = new Entity();

            var response = new OrganizationResponse();

            organizationService.Setup(x => x.ExecuteAsync(It.IsAny<OrganizationRequest>()))
                               .ReturnsAsync(response);

            FluentActions.Invoking(() => systemUnderTest.UpsertEntity(entity))
                         .Should()
                         .NotThrowAsync();

            organizationService.Verify(x => x.ExecuteAsync(It.IsAny<OrganizationRequest>()), Times.Once);
        }

        [Fact]
        public void UpsertEntity_Insert_RecordCreated_False()
        {
            var entity = new Entity();

            var response = new UpsertResponse();
            response.Results.Add("RecordCreated", false);

            organizationService.Setup(x => x.ExecuteAsync(It.IsAny<OrganizationRequest>()))
                               .ReturnsAsync(response);

            FluentActions.Invoking(() => systemUnderTest.UpsertEntity(entity))
                         .Should()
                         .NotThrowAsync();

            organizationService.Verify(x => x.ExecuteAsync(It.IsAny<OrganizationRequest>()), Times.Once);
        }


        [Fact]
        public void UpsertEntity_Insert_RecordCreated_True()
        {
            var entity = new Entity();

            var response = new UpsertResponse();
            response.Results.Add("RecordCreated", true);

            organizationService.Setup(x => x.ExecuteAsync(It.IsAny<OrganizationRequest>()))
                               .ReturnsAsync(response);

            FluentActions.Invoking(() => systemUnderTest.UpsertEntity(entity))
                         .Should()
                         .NotThrowAsync();

            organizationService.Verify(x => x.ExecuteAsync(It.IsAny<OrganizationRequest>()), Times.Once);
        }

        [Fact]
        public void CreateAttachments()
        {
            var files = new List<FileDetails>
            {
                new()
                {
                    FileName = "FileName",
                    FileLocation= "FileLocation"
                }
            };
            var ownerEntity = new EntityReference("contact", Guid.NewGuid());

            dataverseService.SetupGet(x => x.BlobClientAccess).Returns(blobClientAccess.Object);

            blobClientAccess.Setup(x => x.ReadDataFromBlob(It.IsAny<string>()))
                               .ReturnsAsync("Test data");

            organizationService.Setup(x => x.CreateAsync(It.IsAny<Entity>()))
                               .ReturnsAsync(Guid.NewGuid());

            FluentActions.Invoking(() => systemUnderTest.CreateAttachments(files, ownerEntity))
                         .Should()
                         .NotThrowAsync();

            organizationService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Once);
        }

        [Fact]
        public void CreateAttachments_Exception()
        {
            var files = new List<FileDetails>
            {
                new()
                {
                    FileName = "FileName",
                    FileLocation= "FileLocation"
                }
            };
            var ownerEntity = new EntityReference("contact", Guid.NewGuid());

            dataverseService.SetupGet(x => x.BlobClientAccess).Returns(blobClientAccess.Object);

            blobClientAccess.Setup(x => x.ReadDataFromBlob(It.IsAny<string>()))
                               .ReturnsAsync("Test data");

            organizationService.Setup(x => x.CreateAsync(It.IsAny<Entity>()))
                               .Throws<Exception>();

            FluentActions.Invoking(() => systemUnderTest.CreateAttachments(files, ownerEntity))
                         .Should()
                         .NotThrowAsync();

            organizationService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Once);
        }

        [Fact]
        public void CreateAttachment()
        {
            var entityToCreate = new FileDetails
            {
                ContentMediaType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                FileType = "metric",
                FileName = "metric.xlsx",
                FileLocation = "b38d60d1-8550-4cd8-aab6-01683a1a4cdf/metric/metric.xlsx",
                FileSize = "0.04"

            };
            var files = new List<FileDetails> { entityToCreate };

            var entityId = Guid.NewGuid();
            var ownerEntity = Guid.NewGuid().GetEntityReference<SalesOrder>();

            organizationService.Setup(x => x.CreateAsync(It.IsAny<Entity>()))
                             .ReturnsAsync(entityId);

            blobClientAccess.Setup(x => x.ReadDataFromBlob(entityToCreate.FileLocation))
                .ReturnsAsync("sdfdsfds sdsfds");

            FluentActions.Invoking(() => systemUnderTest.CreateAttachment(entityToCreate, ownerEntity))
                       .Should()
                       .NotThrowAsync();

            organizationService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Once);
        }


        [Fact]
        public void CreateAttachment_Exception()
        {
            var entityToCreate = new FileDetails
            {
                ContentMediaType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                FileType = "metric",
                FileName = "metric.xlsx",
                FileLocation = "b38d60d1-8550-4cd8-aab6-01683a1a4cdf/metric/metric.xlsx",
                FileSize = "0.04"

            };
            var files = new List<FileDetails> { entityToCreate };

            var entityId = Guid.NewGuid();
            var ownerEntity = Guid.NewGuid().GetEntityReference<SalesOrder>();

            blobClientAccess.Setup(x => x.ReadDataFromBlob(entityToCreate.FileLocation))
                            .Throws<Exception>();

            FluentActions.Invoking(() => systemUnderTest.CreateAttachment(entityToCreate, ownerEntity))
                       .Should()
                       .NotThrowAsync();

            organizationService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Never);
            organizationService.VerifyAll();
        }

        [Fact]
        public void CreateFileDetails()
        {
            var entityToCreate = new FileDetails
            {
                ContentMediaType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                FileType = "metric",
                FileName = "metric.xlsx",
                FileLocation = "b38d60d1-8550-4cd8-aab6-01683a1a4cdf/metric/metric.xlsx",
                FileSize = "0.04"
            };
            var entityCollection = new EntityCollection();

            var listOfEntities = new List<FileDetails>
            {
                entityToCreate
            };

            var ownerEntity = Guid.NewGuid().GetEntityReference<SalesOrder>();

            dataverseService.SetupGet(x => x.BlobClientAccess).Returns(blobClientAccess.Object);

            organizationService.Setup(x => x.CreateAsync(It.IsAny<Entity>()));

            FluentActions.Invoking(() => systemUnderTest.CreateAttachments(listOfEntities, ownerEntity))
                         .Should()
                         .NotThrowAsync();

            organizationService.VerifyAll();
            organizationService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Once);

        }

        [Fact]
        public async Task RetrieveConfigurations()
        {
            var entities = new EntityCollection();

            organizationService.Setup(x => x.RetrieveMultipleAsync(It.IsAny<QueryBase>()))
                            .ReturnsAsync(entities);

            var actual = await systemUnderTest.RetrieveConfigurations("test");

            actual.Count.Should().Be(entities.Entities.Count);

            organizationService.Verify(x => x.RetrieveMultipleAsync(It.IsAny<QueryBase>()), Times.Once);
        }



        [Fact]
        public async Task UpsertClient_Agent_Organisation()
        {
            var payload = await HydrateFromTestDataFile<LandOwnerRequestPayload>("TestData/registration/agent-applyingfor-organisation.json");
            Guid applicantId = Guid.NewGuid();
            var agent = payload.LandownerGainSiteRegistration.Agent;

            dataverseService.Setup(x => x.UpsertClient(It.IsAny<Agent>()))
                           .ReturnsAsync(new EntityReference("contact", applicantId));

            dataverseService.Setup(x => x.RetrieveFirstRecordForEntity(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(applicantId);

            dataverseService.Setup(x => x.CreateAsync(It.IsAny<Entity>())).ReturnsAsync(Guid.NewGuid());

            var actual = await systemUnderTest.UpsertClient(agent);

            actual.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public async Task OrchestrationBNG_Agent_Individuals()
        {
            var payload = await HydrateFromTestDataFile<LandOwnerRequestPayload>("TestData/registration/agent-applyingfor-individual.json");
            Guid applicantId = Guid.NewGuid();
            var agent = payload.LandownerGainSiteRegistration.Agent;

            dataverseService.Setup(x => x.UpsertClient(It.IsAny<Agent>()))
                           .ReturnsAsync(new EntityReference("contact", applicantId));

            dataverseService.Setup(x => x.RetrieveFirstRecordForEntity(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(applicantId);

            dataverseService.Setup(x => x.CreateAsync(It.IsAny<Entity>())).ReturnsAsync(Guid.NewGuid());

            var actual = await systemUnderTest.UpsertClient(agent);

            actual.Should().NotBe(Guid.Empty);
        }

    }
}