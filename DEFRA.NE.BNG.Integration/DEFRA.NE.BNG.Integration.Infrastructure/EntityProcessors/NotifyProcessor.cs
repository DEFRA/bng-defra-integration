using DEFRA.NE.BNG.Integration.Domain.Constants;
using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Domain.Request;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace DEFRA.NE.BNG.Integration.Infrastructure.EntityProcessors
{
    public class NotifyProcessor : INotifyManager
    {
        private readonly IMappingManager mappingManager;
        private readonly IDataverseService dataverseService;

        public NotifyProcessor(IMappingManager mappingManager, IDataverseService dataverseService)
        {
            this.mappingManager = mappingManager;
            this.dataverseService = dataverseService;
        }

        public async Task ProcessEmailContent(bng_casetype caseType, EmailNotificationRequest emailNotification)
        {
            var query = new QueryExpression(bng_Emailcontent.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(DataverseExtensions.AttributeLogicalName<bng_Emailcontent>(nameof(bng_Emailcontent.bng_DocumentList)), DataverseExtensions.AttributeLogicalName<bng_Emailcontent>(nameof(bng_Emailcontent.bng_Name)), DataverseExtensions.AttributeLogicalName<bng_Emailcontent>(nameof(bng_Emailcontent.bng_EmailCaseType))),
                Criteria = new FilterExpression(LogicalOperator.And)
            };

            query.AddEqualOperatorCondition<bng_Emailcontent>(nameof(bng_Emailcontent.bng_EmailCaseType), (int)caseType);

            var emailContent = await dataverseService.RetrieveMultipleAsync(query);

            if (emailContent != null && emailContent.Entities?.Count > 0)
            {
                mappingManager.CreateNewOrUpdateExisting(emailNotification.personalisation, GovNotificationConstants.GovNot_API_Personalisation_EmailContent,
                        emailContent.Entities[0][
                            DataverseExtensions.AttributeLogicalName<bng_Emailcontent>(nameof(bng_Emailcontent.bng_Name))].ToString());
                mappingManager.CreateNewOrUpdateExisting(emailNotification.personalisation, GovNotificationConstants.GovNot_API_Personalisation_DocumentList,
                            emailContent.Entities[0][DataverseExtensions.AttributeLogicalName<bng_Emailcontent>(nameof(bng_Emailcontent.bng_DocumentList))].ToString());
            }
        }

        public async Task ProcessBankDetails(EmailNotificationRequest emailNotification)
        {
            var query = DataverseExtensions.GetQuery<bng_BankDetails>();
            var bankDetail = await dataverseService.RetrieveFirstRecord<bng_BankDetails>(query);

            if (bankDetail != null)
            {
                mappingManager.CreateNewOrUpdateExisting(emailNotification.personalisation, GovNotificationConstants.GovNot_API_Personalisation_SortCode,
                        bankDetail.bng_SortCode);

                if (bankDetail.bng_AccountNumber.HasValue)
                {
                    mappingManager.CreateNewOrUpdateExisting(emailNotification.personalisation, GovNotificationConstants.GovNot_API_Personalisation_AccountNumber,
                                bankDetail.bng_AccountNumber.Value.ToString());
                }

                mappingManager.CreateNewOrUpdateExisting(emailNotification.personalisation, GovNotificationConstants.GovNot_API_Personalisation_AccountName,
                            bankDetail.bng_AccountName);
            }
        }

        /// <summary>
        ///  Fees Process
        /// </summary>
        /// <param name="_emailNotification"></param>
        /// <returns></returns>
        public async Task ProcessFees(EmailNotificationRequest emailNotification, bng_casetype caseType)
        {
            QueryExpression query = new QueryExpression(bng_fees.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(
                    DataverseExtensions.AttributeLogicalName<bng_fees>(nameof(bng_fees.bng_fee))
                     , DataverseExtensions.AttributeLogicalName<bng_fees>(nameof(bng_fees.bng_casetype))),
                Criteria = new FilterExpression(LogicalOperator.And)
            };
            query.AddEqualOperatorCondition<bng_fees>(nameof(bng_fees.bng_casetype), (int)caseType);
            var fees = await dataverseService.RetrieveMultipleAsync(query);
            if (fees != null && fees.Entities?.Count > 0)
            {
                mappingManager.CreateNewOrUpdateExisting(emailNotification.personalisation, GovNotificationConstants.GovNot_API_Personalisation_PaymentAmount,
                        ((Money)fees.Entities[0][DataverseExtensions.AttributeLogicalName<bng_fees>(nameof(bng_fees.bng_fee))]).Value.ToString("F"));

            }
        }

        /// <summary>
        /// Process payment details
        /// </summary>
        /// <param name="emailNotification"></param>
        /// <returns></returns>
        public async Task ProcessPaymentDetails(Guid caseId, EmailNotificationRequest emailNotification)
        {
            var query = new QueryExpression(bng_PaymentDetails.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(
                    DataverseExtensions.AttributeLogicalName<bng_PaymentDetails>(nameof(bng_PaymentDetails.bng_PaymentTotal)),
                    DataverseExtensions.AttributeLogicalName<bng_PaymentDetails>(nameof(bng_PaymentDetails.bng_PaymentReference)),
                    DataverseExtensions.AttributeLogicalName<bng_PaymentDetails>(nameof(bng_PaymentDetails.bng_PaymentDeadline))),
                Criteria = new FilterExpression(LogicalOperator.And)
            };
            query.AddEqualOperatorCondition<bng_PaymentDetails>(nameof(bng_PaymentDetails.bng_CaseId), caseId);
            var paymentDetails = await dataverseService.RetrieveMultipleAsync(query);

            if (paymentDetails != null && paymentDetails.Entities?.Count > 0)
            {
                mappingManager.CreateNewOrUpdateExisting(emailNotification.personalisation, GovNotificationConstants.GovNot_API_Personalisation_PaymentAmount,
                        ((Money)paymentDetails.Entities[0][
                            DataverseExtensions.AttributeLogicalName<bng_PaymentDetails>(nameof(bng_PaymentDetails.bng_PaymentTotal))
                             ]).Value.ToString("F"));

                if (paymentDetails[0].Contains(
                    DataverseExtensions.AttributeLogicalName<bng_PaymentDetails>(nameof(bng_PaymentDetails.bng_PaymentReference))
                     ))
                {
                    mappingManager.CreateNewOrUpdateExisting(emailNotification.personalisation, GovNotificationConstants.GovNot_API_Personalisation_PaymentReference,
                        paymentDetails.Entities[0][
                            DataverseExtensions.AttributeLogicalName<bng_PaymentDetails>(nameof(bng_PaymentDetails.bng_PaymentReference))
                             ].ToString());
                }

                if (paymentDetails[0].Contains(
                    DataverseExtensions.AttributeLogicalName<bng_PaymentDetails>(nameof(bng_PaymentDetails.bng_PaymentDeadline))
                     ))
                {
                    mappingManager.CreateNewOrUpdateExisting(emailNotification.personalisation, GovNotificationConstants.GovNot_API_Personalisation_PaymentDueDate,
                            ((DateTime)paymentDetails.Entities[0][
                                 DataverseExtensions.AttributeLogicalName<bng_PaymentDetails>(nameof(bng_PaymentDetails.bng_PaymentDeadline))
                                 ]).ToString("dd/MM/yyyy"));
                }
            }
        }

        /// <summary>
        /// Create notify entity to hold email response
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="applicantId"></param>
        /// <returns></returns>
        public async Task<Guid> CreateNotify(Guid caseId, Guid applicantId, IDataverseService dataverseService, ILogger logger)
        {
            Guid notifyId = Guid.Empty;
            try
            {
                var notifyEntity = new bng_Notify
                {
                    Subject = EnvironmentConstants.SubjectConstant,
                    RegardingObjectId = caseId.GetEntityReference<bng_Case>(),
                    bng_ApplicantID = new EntityReference(Contact.EntityLogicalName, applicantId),
                    bng_ActionType = bng_notifytype.Created
                };
                notifyId = await dataverseService.CreateAsync(notifyEntity);
                logger.LogInformation("Related Notify Created: {id}", notifyId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{error}", ex.Message);
            }
            return notifyId;
        }
    }
}
