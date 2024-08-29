using DEFRA.NE.BNG.Integration.Domain.Request;
using DEFRA.NE.BNG.Integration.Domain.Response;

namespace DEFRA.NE.BNG.Integration.Domain.Interfaces
{
    public interface IMailServiceAgent
    {
        public Task<EmailNotificationResponse> SendMailAsync(EmailNotificationRequest mailNotificationRequest, string requestUri);
    }
}
