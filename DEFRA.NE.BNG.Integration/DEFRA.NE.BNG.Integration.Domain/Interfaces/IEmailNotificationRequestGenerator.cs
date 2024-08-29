using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Domain.Request;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace DEFRA.NE.BNG.Integration.Domain.Interfaces
{
    public interface IEmailNotificationRequestGenerator
    {
        Task<EmailNotificationRequestWrapper> PrepairNotificationRequestData(bng_Notify notifyEntity);
        Task ProcessWithdrawalReason(bng_Case caseEntity, EmailNotificationRequest emailNotification);
        bng_casetype ConfigureEmailTemplateForCase(EmailNotificationRequest emailNotification, bng_Case caseEntity, int actionType);
        Task ProcessRejectionReason(bng_Case caseEntity, EmailNotificationRequest emailNotification);
        Task ProcessLandOwnerCaseTypeOnAccepted(Guid gainSiteRefrenceId, EmailNotificationRequest emailNotification);
        Task PersonaliseEmail(bng_Notify notifyEntity, EmailNotificationRequest emailNotification);
        Task SetPersonalisationDetails<T>(Guid organisationId, EmailNotificationRequest emailNotification, ColumnSet columns, string emailColumnName, string displayNameColumn) where T : Entity;
    }
}
