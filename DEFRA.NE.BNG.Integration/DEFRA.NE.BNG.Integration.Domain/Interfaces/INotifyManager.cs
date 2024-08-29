using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Domain.Request;
using Microsoft.Extensions.Logging;

namespace DEFRA.NE.BNG.Integration.Domain.Interfaces
{
    public interface INotifyManager
    {
        Task ProcessEmailContent(bng_casetype caseType, EmailNotificationRequest emailNotification);
        Task ProcessBankDetails(EmailNotificationRequest emailNotification);
        /// <summary>
        ///  Fees Process
        /// </summary>
        /// <param name="_emailNotification"></param>
        /// <returns></returns>
        Task ProcessFees(EmailNotificationRequest emailNotification, bng_casetype caseType);
        Task ProcessPaymentDetails(Guid caseId, EmailNotificationRequest emailNotification);
        Task<Guid> CreateNotify(Guid caseId, Guid applicantId, IDataverseService dataverseService, ILogger logger);
    }
}
