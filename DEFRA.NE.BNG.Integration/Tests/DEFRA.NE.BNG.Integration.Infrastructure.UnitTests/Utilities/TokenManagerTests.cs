namespace DEFRA.NE.BNG.Integration.Infrastructure.UnitTests.Utilities
{
    public class TokenManagerTests : TestBase<TokenManager>
    {
        public TokenManagerTests() : base()
        {
            Environment.SetEnvironmentVariable(EnvironmentConstants.NotificationServiceId, "TestValue");
            Environment.SetEnvironmentVariable(EnvironmentConstants.NotificationSecret, "TestValue");
        }

        [Fact]
        public void GenerateAccessToken()
        {
            var actual = TokenManager.GenerateAccessToken();
            actual.Should().NotBeNull();
        }
    }
}