using DEFRA.NE.BNG.Integration.Domain.Response;

namespace DEFRA.NE.BNG.Integration.Infrastructure.UnitTests.Utilities
{
    public class HttpClientAPITests : TestBase<HttpClientApi>
    {
        private HttpClientApi systemUnderTest;

        public HttpClientAPITests() : base()
        {
            string basePath = "https://devbnginfla1401.azurewebsites.net";
            systemUnderTest = new HttpClientApi(basePath, logger.Object);
        }

        [Fact]
        public void CanInstantiateHttpClientAPI()
        {
            systemUnderTest.Should().NotBeNull();
        }

        [Fact]
        public void PostAsync_ThrowsException()
        {
            Environment.SetEnvironmentVariable(EnvironmentConstants.NotificationServiceId, "TestValue");
            Environment.SetEnvironmentVariable(EnvironmentConstants.NotificationSecret, "TestValue");

            var request = new EmailNotificationRequest();
            var requestUri = "www.bbc.com";

            FluentActions.Invoking(() => systemUnderTest.PostAsync<EmailNotificationResponse, EmailNotificationRequest>(requestUri, request))
                        .Should()
                        .ThrowAsync<Exception>()
                        .WithMessage("Value cannot be null. (Parameter 's')");
        }
    }
}