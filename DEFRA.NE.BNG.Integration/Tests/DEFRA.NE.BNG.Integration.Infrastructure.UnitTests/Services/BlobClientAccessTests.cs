using Microsoft.WindowsAzure.Storage;

namespace DEFRA.NE.BNG.Integration.Infrastructure.UnitTests.Utilities
{
    public class BlobClientAccessTets : TestBase<BlobClientAccess>
    {
        private BlobClientAccess systemUnderTest;

        public BlobClientAccessTets() : base()
        {
            systemUnderTest = new BlobClientAccess(logger.Object, environmentVariableReader.Object);
        }

        [Fact]
        public void CanInitializeBlobClientAccess()
        {
            systemUnderTest.Should().NotBeNull();
        }

        [Fact]
        public async Task ReadDataFromBlobNoEnvironmentVariables()
        {
            var fileDetails = new Model.Request.FileDetails
            {
                FileLocation = "TestData/DefraId.json"
            };

            Exception exception = null;

            environmentVariableReader.Setup(x => x.Read("BlobConnectionString"))
                                     .Returns("UseDevelopmentStorage=true");
            environmentVariableReader.Setup(x => x.Read("BlobContainer"))
                                     .Returns("test");

            try
            {
                await systemUnderTest.ReadDataFromBlob(fileDetails.FileLocation);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            exception.Should().NotBeNull();
        }

        [Fact]
        public async Task GetBlob()
        {
            StorageException exception = null;

            environmentVariableReader.Setup(x => x.Read("BlobConnectionString"))
                                     .Returns("UseDevelopmentStorage=true");

            try
            {
                await systemUnderTest.GetBlob("testcontainer", "testfilename", environmentVariableReader.Object);
            }
            catch (StorageException ex)
            {
                exception = ex;
            }

            exception.Should().NotBeNull();
            //exception.Message.Should().Be("The specified container does not exist.");
        }
    }
}