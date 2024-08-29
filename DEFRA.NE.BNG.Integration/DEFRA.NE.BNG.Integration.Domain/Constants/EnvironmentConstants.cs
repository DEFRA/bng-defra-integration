using System.Globalization;

namespace DEFRA.NE.BNG.Integration.Domain.Constants
{
    public static class EnvironmentConstants
    {
        public const string OwnerIdEntity = "team";
        public const string OwnerIdGuid = "e14f0f28-1625-ed11-9db1-000d3a0cbed8";
        public const string ExecutionContextTarget = "Target";
        public const string WithdrawalReasonEntityContent = "bng_withdrawalcontentreason";
        public const string PostProcessingNotifyEntityId = "notifyEntityId";
        public const string PostProcessingApplicantEntityId = "applicantEntityId";
        public const string DefraIdMetaDataLOBEntity = "defra_lobserviceuserlink";
        public const string DefraIdMetaDataCreateOperation = "create";
        public const string DefraIdMetaDataUpdateOperation = "update";
        public const string environment = "environment";
        public const string clientId = "clientId";
        public const string clientSecret = "clientSecret";
        public const string BlobContainer = "BlobContainer";
        public const string ServiceBusConnection = "ServiceBusConnection";
        public const string NotificationSecret = "NotificationSecret";
        public const string NotificationServiceId = "NotificationServiceId";
        public const string NotificationServiceBaseUrl = "NotificationServiceBaseUrl";
        public const string NotificationEmailService = "NotificationEmailService";
        public const string NotificationTemplateIdCreated = "EmailTemplateIdCreated";
        public const string NotificationTemplateIdSuccess = "EmailTemplateIdSuccess";
        public const string NotificationTemplateIdFail = "EmailTemplateIdFail";
        public const string NotificationTemplateProceedwithAmendment = "EmailTemplateProceedwithAmendment";
        public const string NotificationTemplateCloseAmendment = "EmailTemplateCloseAmendment";
        public const string NotificationTemplateIdOnHold = "EmailTemplateOnHold";
        public const string NotificationTemplateIdDeveloperSuccess = "EmailTemplateIdSuccessDeveloper";
        public const string NotificationTemplateIdDeveloperLandOwnerAccepted = "EmailTemplateIdDeveloperLandOwnerAccepted";
        public const string NotificationTemplateIdDocumentAndPaymentRequest = "EmailTemplateIdDocumentAndPaymentRequest";
        public const string NotificationTemplateIdDocumentAndPaymentConfirmation = "EmailTemplateIdDocumentAndPaymentConfirmation";
        public const string NotificationTemplateIdConfirmationOfDocsAndRequestPayment = "EmailTemplateIdConfirmationOfDocsAndRequestPayment";
        public const string NotificationTemplateIdDocumentAndPaymentRemainder = "EmailTemplateIdDocumentAndPaymentRemainder";
        public const string NotificationTemplateIdCombinedSuccess = "EmailTemplateIdSuccessCombined";
        public const string NotificationTemplateIdWithdraw = "EmailTemplateIdWithdraw";
        public const string NotificationTemplateIdOrderCreated = "EmailTemplateIdOrderCreated";
        public const string NotificationReplyToEmailId = "ReplyToEmailAddress";
        public const string NotificationDoNotReplyToEmailId = "DoNotReply";
        public const string BlobConnectionString = "BlobConnectionString";
        public const string BNGConfigurationDetails = "BNG Configuration Details";
        public const string NotificationTemplateIdNoticeOfIntent = "EmailTemplateIdNoticeOfIntent";
        public const string NotificationTemplateIdInternalAmendmentConfirmation = "EmailTemplateIdInternalAmendmentConfirmation";
        public const string OrderOwningTeamGuid = "OrderOwningTeamGuid";
        public const string OrderTotalAmountThreshold = "OrderTotalAmountThreshold";
        public const string SubjectConstant = "Registered NetGainSite For Developer";
        public const string UkTimeFormat = "dd/MM/yyyy";
        public static readonly CultureInfo DefaultCultureInfo = new("en-GB");
        public const string NotificationTemplateIdAmendmentPartialSuccess = "EmailTemplateIdAmendmentPartialSuccess";
        public const string NotificationTemplateIdAmendmentFullSuccess = "EmailTemplateIdAmendmentFullSuccess";
        public const string NotificationTemplateIdRemovalCaseAccepted = "EmailTemplateIdRemovalCaseAccepted";
        public const string NotificationTemplateIdAmendmentAllocationCaseAccepted = "EmailTemplateIdAmendmentAllocationCaseAccepted";
    }
}
