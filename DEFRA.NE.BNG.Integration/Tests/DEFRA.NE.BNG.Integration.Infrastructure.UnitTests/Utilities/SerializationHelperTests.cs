namespace DEFRA.NE.BNG.Integration.Infrastructure.UnitTests.Utilities
{
    public class SerializationHelperTests : TestBase<SerializationHelperTests>
    {
        [Fact]
        public async Task DeserializeRemoteContextTypeString()
        {
            var json = await File.ReadAllTextAsync("TestData/notification/Notice of intent.json");

            var actual = SerializationHelper.DeserializeRemoteContextTypeString<RemoteExecutionContext>(json);

            actual.BusinessUnitId.Should().Be("64026cfe-999b-ee11-be37-000d3a875b5f");
            actual.CorrelationId.Should().Be("18a0dee1-85d0-4fbd-9e3e-4b1b7c83eca4");
            actual.MessageName.Should().Be("Update");
            actual.InputParameters.Should().NotBeNull();
        }

        [Fact]
        public void GetExtensionFromFileName()
        {
            var fileName = "Sample filename";
            var extension = ".txt";

            var actual = SerializationHelper.GetExtensionFromFileName($"{fileName}{extension}");

            actual.Should().Be(extension);
        }


        [Fact]
        public void GetExtensionFromFileName_WithPeriodInFilename()
        {
            var fileName = "Sample.filename";
            var extension = ".txt";

            var actual = SerializationHelper.GetExtensionFromFileName($"{fileName}{extension}");

            actual.Should().Be(extension);
        }

        [Fact]
        public void GroupbyAndIndexFileType()
        {
            var files = new List<Model.Request.FileDetails>()
            {
                new()
                {
                    FileName = "File1.txt",
                    FileType = "Type1"
                },
                new ()
                {
                    FileName = "File2.txt",
                    FileType = "Type2"
                },
                new ()
                {
                    FileName = "File3.txt",
                    FileType = "Type1"
                },
                new ()
                {
                    FileName = "File4.txt",
                    FileType = "Type3"
                }
            };

            FluentActions.Invoking(() => SerializationHelper.GroupbyAndIndexFileType(files))
                         .Should()
                         .NotThrow();

            files[0].FileType.Should().Contain("-");
            files[1].FileType.Should().NotContain("-");
            files[2].FileType.Should().Contain("-");
            files[3].FileType.Should().NotContain("-");
        }
    }
}