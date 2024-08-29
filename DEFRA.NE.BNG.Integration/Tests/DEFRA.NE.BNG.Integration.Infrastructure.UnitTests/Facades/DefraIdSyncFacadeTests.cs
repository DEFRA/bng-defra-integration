using DEFRA.NE.BNG.Integration.Domain.JsonRequestRoot;
using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Infrastructure.Facades;
using Microsoft.Xrm.Sdk.Messages;

namespace DEFRA.NE.BNG.Integration.Infrastructure.UnitTests.Facades
{
    public class DefraIdSyncFacadeTests : TestBase<DefraIdSyncFacade>
    {
        private readonly DefraIdSyncFacade systemUnderTest;
        private DefraIdRequestPayload requestData;

        public DefraIdSyncFacadeTests() : base()
        {
            requestData = new DefraIdRequestPayload
            {
                Recorddata = new DefraIdRecorddata()
            };

            systemUnderTest = new DefraIdSyncFacade(logger.Object, dataverseService.Object);
        }

        [Fact]
        public async Task UpdateUserAndAccountFromDefraId_RequestPayloadIsNull()
        {
            requestData = null;

            await systemUnderTest.Invoking(
                async x => await x.UpdateUserAndAccountFromDefraId(requestData)
                )
                .Should()
                .NotThrowAsync();
            ;
        }

        [Fact]
        public async Task UpdateUserAndAccountFromDefraIdDefraId_MetaData_Null()
        {
            requestData.Metadata = null;

            await systemUnderTest.Invoking(
                async x => await x.UpdateUserAndAccountFromDefraId(requestData)
                )
                .Should()
                .NotThrowAsync();
            ;
        }

        [Fact]
        public async Task UpdateUserAndAccountFromDefraId_Recorddata_Null()
        {
            requestData.Recorddata = null;

            await systemUnderTest.Invoking(
                async x => await x.UpdateUserAndAccountFromDefraId(requestData)
                )
                .Should()
                .NotThrowAsync();
            ;
        }

        [Fact]
        public async Task UpdateUserAndAccountFromDefraId_defra_lobserviceuserlink_CreateAcount_()
        {
            var metaData = new DefraIdMetadata
            {
                Entity = "defra_lobserviceuserlink",
                Operationtype = "create",
                Origin = "origin",
                Recordid = Guid.NewGuid().ToString(),
                TrackingId = Guid.NewGuid().ToString()
            };

            requestData.Metadata = metaData;
            requestData.Recorddata.Account = CreateTestDefraIdAccount();

            EntityCollection entityCollection = null;
            OrganizationResponse upsertResponse = new UpsertResponse();
            dataverseService.Setup(x => x.RetrieveMultipleAsync(It.IsAny<QueryBase>()))
                             .ReturnsAsync(entityCollection);

            dataverseService.Setup(x => x.UpsertEntity(It.IsAny<Entity>()));

            await systemUnderTest.UpdateUserAndAccountFromDefraId(requestData);


            dataverseService.Verify(x => x.RetrieveMultipleAsync(It.IsAny<QueryBase>()), Times.Once());
            dataverseService.Verify(x => x.UpsertEntity(It.IsAny<Entity>()), Times.Once());
            //dataverseService.VerifyAll();
            logger.VerifyAll();
        }

        [Fact]
        public async Task UpdateUserAndAccountFromDefraId_defra_lobserviceuserlink_CreateContact()
        {
            var organizationResponse = new UpsertResponse();

            var metaData = new DefraIdMetadata
            {
                Entity = "defra_lobserviceuserlink",
                Operationtype = "create"
            };

            requestData.Metadata = metaData;
            requestData.Recorddata.Contact = CreateTestDefraIdContact();

            dataverseService.Setup(x => x.UpsertEntity(It.IsAny<Entity>()));

            await systemUnderTest.Invoking(
                async x => await x.UpdateUserAndAccountFromDefraId(requestData)
                )
                .Should()
                .NotThrowAsync();

            dataverseService.VerifyAll();
            logger.VerifyAll();
        }

        [Fact]
        public async Task UpdateUserAndAccountFromDefraId_defra_lobserviceuserlink_UpdateAccount()
        {
            var metaData = new DefraIdMetadata
            {
                Entity = "defra_lobserviceuserlink",
                Operationtype = "update"
            };

            requestData.Metadata = metaData;
            requestData.Recorddata.Account = CreateTestDefraIdAccount();

            await systemUnderTest.Invoking(
                async x => await x.UpdateUserAndAccountFromDefraId(requestData)
                )
                .Should()
                .NotThrowAsync();

            dataverseService.VerifyAll();
            logger.VerifyAll();
        }

        [Fact]
        public async Task UpdateUserAndAccountFromDefraId_defra_lobserviceuserlink_UpdateContact()
        {
            var metaData = new DefraIdMetadata
            {
                Entity = "defra_lobserviceuserlink",
                Operationtype = "update"
            };

            requestData.Metadata = metaData;
            requestData.Recorddata.Contact = CreateTestDefraIdContact();

            await systemUnderTest.Invoking(
                async x => await x.UpdateUserAndAccountFromDefraId(requestData)
                )
                .Should()
                .NotThrowAsync();

            dataverseService.VerifyAll();
            logger.VerifyAll();
        }

        [Fact]
        public async Task UpdateUserAndAccountFromDefraId_Individual_Create()
        {
            var metaData = new DefraIdMetadata
            {
                Entity = "contact",
                Operationtype = "create"
            };

            requestData.Metadata = metaData;
            requestData.Recorddata.Contact = CreateTestDefraIdContact();

            await systemUnderTest.Invoking(
                async x => await x.UpdateUserAndAccountFromDefraId(requestData)
                )
                .Should()
                 .NotThrowAsync();

            dataverseService.VerifyAll();
            logger.VerifyAll();
        }

        [Fact]
        public async Task UpdateUserAndAccountFromDefraId_Individual_Update()
        {
            var metaData = new DefraIdMetadata
            {
                Entity = "contact",
                Operationtype = "update"
            };

            requestData.Metadata = metaData;
            requestData.Recorddata.Contact = CreateTestDefraIdContact();

            Contact entity = new() { Id = Guid.NewGuid() };
            var upsertResponse = new UpsertResponse();

            dataverseService.Setup(x => x.RetrieveAsync<Contact>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
            .ReturnsAsync(entity);

            dataverseService.Setup(x => x.UpdateAsync(It.IsAny<Entity>()));

            await systemUnderTest.Invoking(
                async x => await x.UpdateUserAndAccountFromDefraId(requestData)
                )
                .Should()
                .NotThrowAsync();

            dataverseService.VerifyAll();
            logger.VerifyAll();
        }

        [Fact]
        public async Task UpdateUserAndAccountFromDefraId_Account_Update()
        {
            var metaData = new DefraIdMetadata
            {
                Entity = "account",
                Operationtype = "update"
            };
            requestData.Metadata = metaData;
            requestData.Recorddata.Account = CreateTestDefraIdAccount();

            Account entity = new() { Id = Guid.NewGuid() };
            var upsertResponse = new UpsertResponse();

            dataverseService.Setup(x => x.RetrieveAsync<Account>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
            .ReturnsAsync(entity);

            dataverseService.Setup(x => x.UpdateAsync(It.IsAny<Entity>()));

            await systemUnderTest.Invoking(
                async x => await x.UpdateUserAndAccountFromDefraId(requestData)
                )
                .Should()
                 .NotThrowAsync();

            dataverseService.VerifyAll();
            logger.VerifyAll();
        }

        [Fact]
        public async Task UpdateUserAndAccountFromDefraId_Account_NotFound_Update()
        {
            var metaData = new DefraIdMetadata
            {
                Entity = "account",
                Operationtype = "update"
            };
            requestData.Metadata = metaData;
            requestData.Recorddata.Account = CreateTestDefraIdAccount();

            Account entity = null;
            var upsertResponse = new UpsertResponse();

            dataverseService.Setup(x => x.RetrieveAsync<Account>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
            .ReturnsAsync(entity);

            await systemUnderTest.Invoking(
                async x => await x.UpdateUserAndAccountFromDefraId(requestData)
                )
                .Should()
                 .NotThrowAsync();

            dataverseService.VerifyAll();
            logger.VerifyAll();
            dataverseService.Verify(x => x.UpdateAsync(It.IsAny<Entity>()), Times.Never);
        }

        [Fact]
        public async Task UpdateUserAndAccountFromDefraId_Account_Create()
        {
            var metaData = new DefraIdMetadata
            {
                Entity = "account",
                Operationtype = "create"
            };

            requestData.Metadata = metaData;
            requestData.Recorddata.Account = CreateTestDefraIdAccount();

            await systemUnderTest.Invoking(
                async x => await x.UpdateUserAndAccountFromDefraId(requestData)
                )
                .Should()
                 .NotThrowAsync();

            dataverseService.VerifyAll();
            logger.VerifyAll();
        }

        [Fact]
        public async Task GetOrganisationType_Throwsexception()
        {
            dataverseService.Setup(x => x.RetrieveMultipleAsync(It.IsAny<QueryExpression>()))
                            .Throws<Exception>();

            var actual = await systemUnderTest.GetOrganisationType("test");

            actual.Should().Be(Guid.Empty);
            dataverseService.VerifyAll();
        }


        [Fact]
        public async Task GetOrganisationType()
        {
            var entity = new Entity("account", Guid.NewGuid());

            var entityCollection = new EntityCollection();
            entityCollection.Entities.Add(entity);


            dataverseService.Setup(x => x.RetrieveMultipleAsync(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(entityCollection);

            var actual = await systemUnderTest.GetOrganisationType("test");

            actual.Should().Be(entity.Id);
            dataverseService.VerifyAll();
        }

        [Fact]
        public async Task GetPlatformDataById()
        {
            var entityId = Guid.NewGuid();
            var columns = new string[] { "column1" };

            Account entity = new() { Id = entityId };


            dataverseService.Setup(x => x.RetrieveAsync<Account>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .ReturnsAsync(entity);

            var actual = await systemUnderTest.GetPlatformDataById<Account>(entityId, columns);

            actual.Should().Be(entityId);
            dataverseService.VerifyAll();
        }

        [Fact]
        public async Task GetPlatformDataById_ThrowsException()
        {
            var entityId = Guid.NewGuid();
            var columns = new string[] { "column1" };

            Account entity = new() { Id = entityId };


            dataverseService.Setup(x => x.RetrieveAsync<Account>(It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                            .Throws<Exception>();

            var actual = await systemUnderTest.GetPlatformDataById<Account>(entityId, columns);

            actual.Should().Be(Guid.Empty);
            dataverseService.VerifyAll();
        }
    }
}