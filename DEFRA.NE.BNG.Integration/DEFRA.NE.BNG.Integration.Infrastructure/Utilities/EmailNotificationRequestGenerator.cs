using DEFRA.NE.BNG.Integration.Domain.Constants;
using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Domain.Request;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace DEFRA.NE.BNG.Integration.Infrastructure.Utilities
{
    public class EmailNotificationRequestGenerator : IEmailNotificationRequestGenerator
    {
        protected readonly IMappingManager mappingManager;
        protected readonly INotifyManager notifyManager;
        protected ILogger<EmailNotificationRequestGenerator> logger;
        protected readonly IDataverseService dataverseService;

        public EmailNotificationRequestGenerator(IMappingManager mappingManager,
                                                INotifyManager notifyManager,
                                                ILogger<EmailNotificationRequestGenerator> logger,
                                                IDataverseService dataverseService)
        {
            this.mappingManager = mappingManager;
            this.notifyManager = notifyManager;
            this.logger = logger;
            this.dataverseService = dataverseService;
        }

        public async Task<EmailNotificationRequestWrapper> PrepairNotificationRequestData(bng_Notify notifyEntity)
        {
            EmailNotificationRequestWrapper notificationapper = new();

            var emailNotification = new EmailNotificationRequest { personalisation = new Dictionary<string, string>() };

            UpdateReplyToEmailAddress(emailNotification, 2);

            notificationapper.ActionType = notifyEntity.GetAttributeValue<OptionSetValue>(
                DataverseExtensions.AttributeLogicalName<bng_Notify>(nameof(bng_Notify.bng_ActionType))).Value;

            await PersonaliseEmail(notifyEntity, emailNotification);

            if (notifyEntity.TryGetAttributeValue(DataverseExtensions.AttributeLogicalName<bng_Notify>(nameof(bng_Notify.RegardingObjectId)), out EntityReference regardingObjectReference))
            {
                mappingManager.CreateNewOrUpdateExisting(emailNotification.personalisation,
                                                          GovNotificationConstants.GovNot_API_Personalisation_REFNumber,
                                                          regardingObjectReference.Name);

                if (regardingObjectReference.LogicalName == bng_Case.EntityLogicalName)
                {
                    var columnSet = new ColumnSet()
                                        .AddTableColumns<bng_Case>(
                                                            [
                                                                nameof(bng_Case.bng_DeveloperRegistrationId),
                                                                nameof(bng_Case.bng_GainSiteRegistrationID),
                                                                nameof(bng_Case.bng_RejectionReasonID),
                                                                nameof(bng_Case.bng_Case_Type),
                                                                nameof(bng_Case.bng_Deadline),
                                                                nameof(bng_Case.bng_InternalJustification),
                                                                nameof(bng_Case.bng_NoticeofIntent),
                                                                nameof(bng_Case.bng_withdrawalreason),
                                                                nameof(bng_Case.bng_Informationtobeamended),
                                                                nameof(bng_Case.bng_InformationnotAmended),
                                                                nameof(bng_Case.bng_AmendmentReason)
                                                            ]
                                                        );

                    var caseEntity = await dataverseService.RetrieveAsync<bng_Case>(regardingObjectReference.Id, columnSet);

                    if (caseEntity != null)
                    {
                        var caseType = ConfigureEmailTemplateForCase(emailNotification, caseEntity, notificationapper.ActionType);
                        switch (notificationapper.ActionType)
                        {
                            case (int)bng_notifytype.Created:
                                await notifyManager.ProcessEmailContent(caseType, emailNotification);
                                await notifyManager.ProcessBankDetails(emailNotification);
                                await notifyManager.ProcessFees(emailNotification, caseType);
                                break;

                            case (int)bng_notifytype.Accepted:
                                {
                                    var gainsiteIdColumn = DataverseExtensions.AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_GainSiteRegistrationID));
                                    await ProcessNotifytypeAccepted(emailNotification, caseEntity, caseType, gainsiteIdColumn);
                                }
                                break;

                            case (int)bng_notifytype.Rejected:
                                await notifyManager.ProcessEmailContent(caseType, emailNotification);
                                await ProcessRejectionReason(caseEntity, emailNotification);
                                break;

                            case (int)bng_notifytype.FurtherInformation:
                                await ProcessNotifytypeFurtherInformation(notifyEntity, emailNotification, caseType);
                                break;

                            case (int)bng_notifytype.TriageComplete:
                                await ProcessNotifytypeTriageComplete(emailNotification, caseEntity, caseType);
                                break;

                            case (int)bng_notifytype.RequestDocumentsPayment:
                            case (int)bng_notifytype.PaymentRemainder:
                                {
                                    UpdateReplyToEmailAddress(emailNotification, 1);
                                    await notifyManager.ProcessEmailContent(caseType, emailNotification);
                                    await notifyManager.ProcessBankDetails(emailNotification);
                                    await notifyManager.ProcessPaymentDetails(caseEntity.Id, emailNotification);
                                }
                                break;
                            case (int)bng_notifytype.Withdraw:
                                {
                                    await notifyManager.ProcessEmailContent(caseType, emailNotification);
                                    await ProcessWithdrawalReason(caseEntity, emailNotification);
                                }
                                break;

                            case (int)bng_notifytype.ConfirmdocumentsreceivedRequestpayment:
                                {
                                    await ProcessNotifytypeConfirmdocumentsreceivedRequestpayment(emailNotification, caseEntity, caseType);
                                }
                                break;
                            case (int)bng_notifytype.NoticeofIntent:
                                await GenerateNoticeOfIntentEmail(emailNotification, caseEntity, caseType);

                                break;

                            case (int)bng_notifytype.ProceedwithAmendment:
                                await ProcessNotifytypeProceedwithAmendment(emailNotification, caseEntity, caseType);
                                break;

                            case (int)bng_notifytype.Closed:
                                await ProcessNotifytypeClosed(emailNotification, caseEntity, caseType);
                                break;

                            case (int)bng_notifytype.FullAcceptance:
                                {
                                    await ProcessLandOwnerCaseTypeOnAccepted(caseEntity.bng_GainSiteRegistrationID.Id, emailNotification);
                                }
                                break;

                            case (int)bng_notifytype.PartialAcceptance:
                                await ProcessNotifytypePartialAcceptance(emailNotification, caseEntity);
                                break;
                            case (int)bng_notifytype.InternalAmendmentConfirmation:
                            case (int)bng_notifytype.RemovalCaseAccepted:
                            case (int)bng_notifytype.AmendmentAllocationCaseAccepted:
                                {
                                    await notifyManager.ProcessEmailContent(caseType, emailNotification);
                                    await ProcessLandOwnerCaseTypeOnAccepted(caseEntity.bng_GainSiteRegistrationID.Id, emailNotification);
                                }
                                break;
                        }
                    }
                }
                else if (regardingObjectReference.LogicalName == SalesOrder.EntityLogicalName)
                {
                    ProcessNotifytypeOrderCreated(emailNotification, regardingObjectReference);
                }
            }

            notificationapper.EmailNotificationRequest = emailNotification;
            return notificationapper;
        }

        public async Task PersonaliseEmail(bng_Notify notifyEntity, EmailNotificationRequest emailNotification)
        {
            if (notifyEntity.TryGetAttributeValue(
                            DataverseExtensions.AttributeLogicalName<bng_Notify>(nameof(bng_Notify.bng_ApplicantID)), out EntityReference applicant))
            {
                var emailColumnName = DataverseExtensions.AttributeLogicalName<Contact>(nameof(Contact.EMailAddress1));
                var displayNameColumn = DataverseExtensions.AttributeLogicalName<Contact>(nameof(Contact.FullName));

                var columns = new ColumnSet(emailColumnName, displayNameColumn);
                await SetPersonalisationDetails<Contact>(applicant.Id, emailNotification, columns, emailColumnName, displayNameColumn);
            }
            else if (notifyEntity.TryGetAttributeValue(
                DataverseExtensions.AttributeLogicalName<bng_Notify>(nameof(bng_Notify.bng_Account)), out EntityReference organisation))
            {
                var emailColumnName = DataverseExtensions.AttributeLogicalName<Account>(nameof(Account.EMailAddress1));
                var displayNameColumn = DataverseExtensions.AttributeLogicalName<Account>(nameof(Account.Name));

                var columns = new ColumnSet(emailColumnName, displayNameColumn);

                await SetPersonalisationDetails<Account>(organisation.Id, emailNotification, columns, emailColumnName, displayNameColumn);
            }
        }

        public async Task SetPersonalisationDetails<T>(Guid organisationId, EmailNotificationRequest emailNotification, ColumnSet columns, string emailColumnName, string displayNameColumn) where T : Entity
        {
            var entity = await dataverseService.RetrieveAsync<T>(organisationId, columns);

            if (entity != null)
            {
                if (entity.TryGetAttributeValue(emailColumnName, out string email))
                {
                    emailNotification.email_address = email;
                }

                mappingManager.CreateNewOrUpdateExisting(emailNotification.personalisation,
                                                          GovNotificationConstants.GovNot_API_Personalisation_Name,
                                                          entity.GetAttributeValue<string>(displayNameColumn));
            }
        }

        /// <summary>
        /// Process for landowner case type for accepted
        /// </summary>
        /// <param name="gainSiteRefrenceId"></param>
        /// <param name="emailNotification"></param>
        /// <returns></returns>
        public async Task ProcessLandOwnerCaseTypeOnAccepted(Guid gainSiteRefrenceId, EmailNotificationRequest emailNotification)
        {
            var gainsiteColumn = DataverseExtensions.AttributeLogicalName<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.bng_GainSiteReference));

            var bioDiversityEntity = await dataverseService.RetrieveAsync<bng_GainSiteRegistration>(gainSiteRefrenceId, new ColumnSet(gainsiteColumn));

            if (bioDiversityEntity != null && !string.IsNullOrWhiteSpace(bioDiversityEntity.bng_GainSiteReference))
            {
                mappingManager.CreateNewOrUpdateExisting(emailNotification.personalisation,
                                                        GovNotificationConstants.GovNot_API_Personalisation_BioDiversityNo,
                                                        bioDiversityEntity.bng_GainSiteReference);
            }
        }

        /// <summary>
        /// Process Rejection reason
        /// </summary>
        /// <param name="caseEntity"></param>
        /// <param name="emailNotification"></param>
        /// <returns></returns>
        public async Task ProcessRejectionReason(bng_Case caseEntity, EmailNotificationRequest emailNotification)
        {
            var rejectionReasonColumn = DataverseExtensions.AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_RejectionReasonID));
            var rejectionReasonNameColumn = DataverseExtensions.AttributeLogicalName<bng_RejectionReason>(nameof(bng_RejectionReason.bng_RejectionReasonName));

            if (caseEntity.Contains(rejectionReasonColumn) && caseEntity[rejectionReasonColumn] is EntityReference)
            {

                var rejectionReasonEntity = await dataverseService.RetrieveAsync<bng_RejectionReason>(
                        ((EntityReference)caseEntity[rejectionReasonColumn]).Id,
                        new(rejectionReasonNameColumn));

                if (rejectionReasonEntity != null && rejectionReasonEntity.Contains(rejectionReasonNameColumn))
                {
                    mappingManager.CreateNewOrUpdateExisting(emailNotification.personalisation,
                                                            GovNotificationConstants.GovNot_API_Personalisation_RejectionReason,
                                                            rejectionReasonEntity[rejectionReasonNameColumn].ToString());
                }
            }
        }


        /// <summary>
        /// Process Withdrawal reason
        /// </summary>
        /// <param name="caseEntity"></param>
        /// <param name="emailNotification"></param>
        /// <returns></returns>
        public async Task ProcessWithdrawalReason(bng_Case caseEntity, EmailNotificationRequest emailNotification)
        {
            if (caseEntity.Contains(
                DataverseExtensions.AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_withdrawalreason))
                ) && caseEntity[
                    DataverseExtensions.AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_withdrawalreason))
                    ] is EntityReference)
            {
                var withdrawalReasonEntity = await dataverseService.RetrieveAsync<bng_WithdrawalReason>(((EntityReference)caseEntity[
                            DataverseExtensions.AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_withdrawalreason))
                        ]).Id,
                         new(EnvironmentConstants.WithdrawalReasonEntityContent));

                if (withdrawalReasonEntity != null && withdrawalReasonEntity.Contains(EnvironmentConstants.WithdrawalReasonEntityContent))
                {
                    mappingManager.CreateNewOrUpdateExisting(emailNotification.personalisation, GovNotificationConstants.GovNot_API_Personalisation_WithdrawalReason,
                        withdrawalReasonEntity[EnvironmentConstants.WithdrawalReasonEntityContent].ToString());
                }
            }
        }


        public bng_casetype ConfigureEmailTemplateForCase(EmailNotificationRequest emailNotification, bng_Case caseEntity, int actionType)
        {
            bng_casetype caseType = 0;

            if (caseEntity.bng_Case_Type.HasValue)
            {
                caseType = caseEntity.bng_Case_Type.Value;
                var caseTemplates = mappingManager.GetActionTemplateMappingForCase();

                if (caseTemplates.TryGetValue(caseType, out IDictionary<int, string> validTypetemplates))
                {
                    if (validTypetemplates.TryGetValue(actionType, out string templateId))
                    {
                        emailNotification.template_id = templateId;
                    }

                    logger.LogInformation("Template id {TemplateId}", templateId);
                }
            }

            return caseType;
        }

        public string TranslateAmendmentReasonToUserFriendlyText(bng_amendmentreason amendment)
        {
            var result = "";
            switch (amendment)
            {
                case bng_amendmentreason.aChangestolegalagreement:
                    result = "changes to legal agreement";
                    break;
                case bng_amendmentreason.bReducinganallocation:
                    result = "reducing an allocation";
                    break;
                case bng_amendmentreason.cRemovinganallocation:
                    result = "removing an allocation";
                    break;
                case bng_amendmentreason.dAnyotherreason:
                    result = "any other reason";
                    break;
                case bng_amendmentreason.eIncompleteorinaccurateinformation:
                    result = "incomplete or inaccurate information";
                    break;
                case bng_amendmentreason.fThelegalagreementhavingceasedtohaveaneffect:
                    result = "the legal agreement having ceased to have an effect";
                    break;
                case bng_amendmentreason.gTheBNGeligibilityforthissiteisnolongerbeingmet:
                    result = "the BNG eligibility for this site is no longer being met";
                    break;
            }

            return result;
        }

        private void UpdateReplyToEmailAddress(EmailNotificationRequest emailNotification, int replyType)
        {
            var replyTemplates = mappingManager.GetReplyEmailMapping();

            if (replyTemplates.TryGetValue(replyType, out string value))
            {
                emailNotification.email_reply_to_id = value;
            }
        }

        private async Task GenerateNoticeOfIntentEmail(EmailNotificationRequest emailNotification, bng_Case caseEntity, bng_casetype caseType)
        {
            await notifyManager.ProcessEmailContent(caseType, emailNotification);
            await ProcessLandOwnerCaseTypeOnAccepted(caseEntity.bng_GainSiteRegistrationID.Id, emailNotification);

            if (caseEntity.bng_AmendmentReason.HasValue)
            {
                mappingManager.CreateNewOrUpdateExisting(emailNotification.personalisation, GovNotificationConstants.GovNot_API_Personalisation_amendments_reason,
                          TranslateAmendmentReasonToUserFriendlyText(caseEntity.bng_AmendmentReason.Value));
            }
        }

        private async Task ProcessNotifytypePartialAcceptance(EmailNotificationRequest emailNotification, bng_Case caseEntity)
        {
            await ProcessLandOwnerCaseTypeOnAccepted(caseEntity.bng_GainSiteRegistrationID.Id, emailNotification);
            if (caseEntity.Contains(DataverseExtensions.AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_Informationtobeamended))))
            {
                mappingManager.CreateNewOrUpdateExisting(emailNotification.personalisation, GovNotificationConstants.GovNot_API_Personalisation_amendments_accepted,
                          caseEntity[DataverseExtensions.AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_Informationtobeamended))].ToString());
            }

            if (caseEntity.Contains(DataverseExtensions.AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_InformationnotAmended))))
            {
                mappingManager.CreateNewOrUpdateExisting(emailNotification.personalisation, GovNotificationConstants.GovNot_API_Personalisation_amendments_rejected,
                           caseEntity[DataverseExtensions.AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_InformationnotAmended))].ToString());
            }
        }

        private async Task ProcessNotifytypeTriageComplete(EmailNotificationRequest emailNotification, bng_Case caseEntity, bng_casetype caseType)
        {
            await notifyManager.ProcessEmailContent(caseType, emailNotification);
            if (caseEntity.Contains(DataverseExtensions.AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_Deadline))))
            {
                mappingManager.CreateNewOrUpdateExisting(emailNotification.personalisation, GovNotificationConstants.GovNot_API_Personalisation_CaseDueDate,
                           ((DateTime)caseEntity[DataverseExtensions.AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_Deadline))]).ToString(EnvironmentConstants.UkTimeFormat));
            }
        }

        private void ProcessNotifytypeOrderCreated(EmailNotificationRequest emailNotification, EntityReference regardingObjectReference)
        {
            var orderTemplatMapping = mappingManager.GetOrderTemplateMapping();
            var key = (int)bng_notifytype.OrderCreated;

            if (orderTemplatMapping != null && orderTemplatMapping.TryGetValue(key, out string value))
            {
                emailNotification.template_id = value;

                mappingManager.CreateNewOrUpdateExisting(emailNotification.personalisation,
                                                       GovNotificationConstants.GovNot_API_Personalisation_OrderId,
                                                            regardingObjectReference.Name
                                                            );
            }
        }

        private async Task ProcessNotifytypeClosed(EmailNotificationRequest emailNotification, bng_Case caseEntity, bng_casetype caseType)
        {
            await notifyManager.ProcessEmailContent(caseType, emailNotification);
            if (caseEntity.Contains(
                DataverseExtensions.AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_NoticeofIntent))
                ))
            {
                mappingManager.CreateNewOrUpdateExisting(emailNotification.personalisation,
                                    GovNotificationConstants.GovNot_API_Personalisation_NOI_end_date,
                                    ((DateTime)caseEntity[
                                    DataverseExtensions.AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_NoticeofIntent))
                                    ]).ToString(EnvironmentConstants.UkTimeFormat));
            }
        }

        private async Task ProcessNotifytypeProceedwithAmendment(EmailNotificationRequest emailNotification, bng_Case caseEntity, bng_casetype caseType)
        {
            await notifyManager.ProcessEmailContent(caseType, emailNotification);
            if (caseEntity.Contains(
                DataverseExtensions.AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_InternalJustification))
                ))
            {
                mappingManager.CreateNewOrUpdateExisting(emailNotification.personalisation,
                                    GovNotificationConstants.GovNot_API_Personalisation_internal_justification,
                                    caseEntity[
                                      DataverseExtensions.AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_InternalJustification))

                                    ].ToString());
            }
            if (caseEntity.Contains(DataverseExtensions.AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_NoticeofIntent))
                ))
            {
                mappingManager.CreateNewOrUpdateExisting(emailNotification.personalisation,
                                    GovNotificationConstants.GovNot_API_Personalisation_NOI_end_date,
                                    ((DateTime)caseEntity[
                                        DataverseExtensions.AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_NoticeofIntent))
                                    ]).ToString(EnvironmentConstants.UkTimeFormat));
            }
        }

        private async Task ProcessNotifytypeConfirmdocumentsreceivedRequestpayment(EmailNotificationRequest emailNotification, bng_Case caseEntity, bng_casetype caseType)
        {
            UpdateReplyToEmailAddress(emailNotification, 1);
            await notifyManager.ProcessEmailContent(caseType, emailNotification);
            await notifyManager.ProcessBankDetails(emailNotification);
            await notifyManager.ProcessPaymentDetails(caseEntity.Id, emailNotification);
            if (caseEntity.Contains(DataverseExtensions.AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_Deadline))))
            {
                mappingManager.CreateNewOrUpdateExisting(emailNotification.personalisation,
                                    GovNotificationConstants.GovNot_API_Personalisation_CaseDueDate,
                                    ((DateTime)caseEntity[DataverseExtensions.AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_Deadline))]).ToString(EnvironmentConstants.UkTimeFormat));
            }
        }

        private async Task ProcessNotifytypeFurtherInformation(bng_Notify notifyEntity, EmailNotificationRequest emailNotification, bng_casetype caseType)
        {
            UpdateReplyToEmailAddress(emailNotification, 1);
            await notifyManager.ProcessEmailContent(caseType, emailNotification);
            if (notifyEntity.Contains(DataverseExtensions.AttributeLogicalName<bng_Notify>(nameof(bng_Notify.Description))))
            {
                mappingManager.CreateNewOrUpdateExisting(emailNotification.personalisation, GovNotificationConstants.GovNot_API_Personalisation_FurtherInfo,
                             notifyEntity[DataverseExtensions.AttributeLogicalName<bng_Notify>(nameof(bng_Notify.Description))].ToString());
            }
        }

        private async Task ProcessNotifytypeAccepted(EmailNotificationRequest emailNotification, bng_Case caseEntity, bng_casetype caseType, string gainsiteIdColumn)
        {
            if (caseType == bng_casetype.Registration || caseType == bng_casetype.Combined)
            {
                await notifyManager.ProcessEmailContent(caseType, emailNotification);
                await ProcessLandOwnerCaseTypeOnAccepted(((EntityReference)caseEntity[gainsiteIdColumn]).Id, emailNotification);
            }
            else
            {
                await notifyManager.ProcessEmailContent(caseType, emailNotification);
            }
        }

    }
}
