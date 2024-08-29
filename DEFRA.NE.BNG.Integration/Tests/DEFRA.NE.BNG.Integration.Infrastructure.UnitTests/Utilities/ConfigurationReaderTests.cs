namespace DEFRA.NE.BNG.Integration.Infrastructure.UnitTests.Utilities
{
    public class ConfigurationReaderTests : TestBase<ConfigurationReader>
    {
        private ConfigurationReader systemUnderTest;

        public ConfigurationReaderTests() : base()
        {
            Environment.SetEnvironmentVariable(EnvironmentConstants.NotificationServiceId, "Test1234Value");
            Environment.SetEnvironmentVariable(EnvironmentConstants.NotificationSecret, "TestValue");

            Environment.SetEnvironmentVariable(EnvironmentConstants.environment, "TestEnvironment");
            Environment.SetEnvironmentVariable(EnvironmentConstants.clientId, "TestClientId");
            Environment.SetEnvironmentVariable(EnvironmentConstants.clientSecret, "SomeSecret");

            systemUnderTest = new ConfigurationReader();
        }

        [Fact]
        public void Read_ExistingEnvironmentVariable()
        {
            Environment.SetEnvironmentVariable(EnvironmentConstants.NotificationServiceId, "Test1234Value");

            var actual = systemUnderTest.Read("NotificationServiceId");

            actual.Should().Be("Test1234Value");
        }

        [Fact]
        public void Read_NonExistingEnvironmentVariable()
        {
            var actual = systemUnderTest.Read("808Notification15253344ServiceId");

            actual.Should().BeNull();
        }

        [Fact]
        public void BuildDataVerseConnectionString()
        {
            Environment.SetEnvironmentVariable(EnvironmentConstants.environment, "TestEnvironment");
            Environment.SetEnvironmentVariable(EnvironmentConstants.clientId, "TestClientId");
            Environment.SetEnvironmentVariable(EnvironmentConstants.clientSecret, "SomeSecret");

            var actual = systemUnderTest.BuildDataVerseConnectionString();

            actual.Should().ContainAll("Url=https://", ".dynamics.com;AuthType=ClientSecret;ClientId=", ";ClientSecret=", ";RequireNewInstance=true", "TestEnvironment", "TestClientId", "SomeSecret");
        }

    }
}