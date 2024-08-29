using DEFRA.NE.BNG.Integration.Domain.Response;

namespace DEFRA.NE.BNG.Integration.Infrastructure.UnitTests.Utilities
{
    public class MailServiceAgentTests : TestBase<MailServiceAgent>
    {
        private MailServiceAgent systemUnderTest;

        public MailServiceAgentTests() : base()
        {
            systemUnderTest = new MailServiceAgent(HttpClient.Object, logger.Object);
        }

        [Fact]
        public async Task SendMailAsync()
        {
            EmailNotificationRequest mailNotificationRequest = new();
            EmailNotificationResponse emailNotificationResponse = new();

            HttpClient.Setup(x => x.PostAsync<EmailNotificationResponse, EmailNotificationRequest>(It.IsAny<string>(),
                                                                                It.IsAny<EmailNotificationRequest>()))
                      .ReturnsAsync(emailNotificationResponse);

            var actual = await systemUnderTest.SendMailAsync(mailNotificationRequest, "requestUri");

            HttpClient.Verify(x => x.PostAsync<EmailNotificationResponse, EmailNotificationRequest>(It.IsAny<string>(), It.IsAny<EmailNotificationRequest>()), Times.Once);
        }
    }
}