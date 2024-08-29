using Microsoft.PowerPlatform.Dataverse.Client.Utils;

namespace DEFRA.NE.BNG.Integration.Infrastructure.UnitTests.Services
{
    public class PowerPlatformClientTests : TestBase<PowerPlatformClient>
    {
        public PowerPlatformClientTests() : base()
        {
            Environment.SetEnvironmentVariable(EnvironmentConstants.environment, "jaugiuawaiua");
            Environment.SetEnvironmentVariable(EnvironmentConstants.clientId, "jaugiuawaiua");
            Environment.SetEnvironmentVariable(EnvironmentConstants.clientSecret, "jaugiuawaiua");
        }

        [Fact]
        public void PowerPlatformClientInstance()
        {
            var actual = PowerPlatformClient.PowerPlatformClientInstance;

            actual.Should().NotBeNull();
        }

        [Fact]
        public void GetServiceClient()
        {
            var connectionString = @$"Url=https://{Environment.GetEnvironmentVariable(EnvironmentConstants.environment)}.dynamics.com;AuthType=ClientSecret;ClientId={Environment.GetEnvironmentVariable(EnvironmentConstants.clientId)};ClientSecret={Environment.GetEnvironmentVariable(EnvironmentConstants.clientSecret)};RequireNewInstance=true";

            PowerPlatformClient.PowerPlatformClientInstance
                .Invoking(x => x.GetServiceClient(connectionString))
                .Should()
                .Throw<DataverseConnectionException>()
                .WithMessage("Failed to connect to Dataverse")
                ;
        }
    }
}