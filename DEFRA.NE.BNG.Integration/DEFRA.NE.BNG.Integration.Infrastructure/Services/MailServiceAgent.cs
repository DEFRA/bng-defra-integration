using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using DEFRA.NE.BNG.Integration.Domain.Request;
using DEFRA.NE.BNG.Integration.Domain.Response;
using Microsoft.Extensions.Logging;

namespace DEFRA.NE.BNG.Integration.Infrastructure.Services
{
    public class MailServiceAgent : IMailServiceAgent
    {
        private readonly IHttpClient httpClient;
        private readonly ILogger logger;

        public MailServiceAgent(IHttpClient httpClient, ILogger logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public async Task<EmailNotificationResponse> SendMailAsync(EmailNotificationRequest mailNotificationRequest, string requestUri)
        {
            logger.LogInformation("{emailAddress}", mailNotificationRequest.email_address);
            return await httpClient.PostAsync<EmailNotificationResponse, EmailNotificationRequest>(requestUri, mailNotificationRequest);
        }
    }
}
