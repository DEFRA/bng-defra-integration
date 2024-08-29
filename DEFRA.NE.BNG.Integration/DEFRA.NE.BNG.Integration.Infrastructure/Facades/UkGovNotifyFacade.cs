using DEFRA.NE.BNG.Integration.Domain.Constants;
using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Exceptions;
using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Domain.Response;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace DEFRA.NE.BNG.Integration.Infrastructure.Facades
{
    public class UkGovNotifyFacade : FacadeBase, IUkGovNotifyFacade
    {
        private readonly ILogger<UkGovNotifyFacade> logger;
        protected readonly IMappingManager mappingManager;
        protected readonly INotifyManager notifyManager;
        protected readonly IEmailNotificationRequestGenerator emailNotificationRequestGenerator;

        public UkGovNotifyFacade(ILogger<UkGovNotifyFacade> logger,
                                 IMailServiceAgent mailService,
                                 IConfigurationReader variableReader,
                                 IDataverseService dataverseService,
                                 IMappingManager mappingManager,
                                 INotifyManager notifyManager,
                                 IEmailNotificationRequestGenerator emailNotificationRequestGenerator)
                                : base(
                                                                      variableReader,
                                                                      mailService,
                                                                      dataverseService)
        {
            this.logger = logger;
            this.mappingManager = mappingManager;
            this.notifyManager = notifyManager;
            this.emailNotificationRequestGenerator = emailNotificationRequestGenerator;
        }

        /// <summary>
        /// Sends email from GovNotify
        /// </summary>
        /// <param name="mailNotificationContext"></param>
        /// <returns></returns>
        public async Task<bool> EmailNotification(Guid notifyEntityId)
        {
            EmailNotificationRequestWrapper mailNotificationRequestWrapper = new();

            var notifyEntity = await dataverseService.RetrieveAsync<bng_Notify>(notifyEntityId,
                                                         new ColumnSet(
                                                            DataverseExtensions.AttributeLogicalName<bng_Notify>(nameof(bng_Notify.bng_ActionType)),
                                                            DataverseExtensions.AttributeLogicalName<bng_Notify>(nameof(bng_Notify.bng_ApplicantID)),
                                                            DataverseExtensions.AttributeLogicalName<bng_Notify>(nameof(bng_Notify.bng_Account)),
                                                            DataverseExtensions.AttributeLogicalName<bng_Notify>(nameof(bng_Notify.RegardingObjectId)),
                                                            DataverseExtensions.AttributeLogicalName<bng_Notify>(nameof(bng_Notify.bng_RetryCount)),
                                                            DataverseExtensions.AttributeLogicalName<bng_Notify>(nameof(bng_Notify.Description))
                                                         ));

            try
            {
                mailNotificationRequestWrapper = await emailNotificationRequestGenerator.PrepairNotificationRequestData(notifyEntity);

                var mailNotificationRequest = mailNotificationRequestWrapper.EmailNotificationRequest;

                if (mailNotificationRequest != null)
                {
                    var emailNotificationResponse = await mailService.SendMailAsync(mailNotificationRequest, environmentVariableReader.Read(EnvironmentConstants.NotificationEmailService));
                    await UpdateNotifyEntity(notifyEntity, emailNotificationResponse, string.Empty, true, mailNotificationRequestWrapper.ActionType);
                }

                return true;
            }
            catch (MailNotificationFailedException notificationError)
            {
                logger.LogError(notificationError, "{message}", notificationError.Message);
                await UpdateNotifyEntity(notifyEntity, null, notificationError.CustomErrorMessasge, false, mailNotificationRequestWrapper.ActionType);
                return false;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{message}", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Updates dynamics Notify entity with response
        /// </summary>
        /// <param name="notifyEntity"></param>
        /// <param name="emailNotificationResponse"></param>
        /// <param name="errorMessage"></param>
        /// <param name="isSuccess"></param>
        /// <returns></returns>
        public async Task UpdateNotifyEntity(Entity notifyEntity, EmailNotificationResponse emailNotificationResponse, string errorMessage, bool isSuccess, int actionType)
        {
            try
            {
                var notifyEntityUpdate = new bng_Notify()
                {
                    Id = notifyEntity.Id
                };

                if (isSuccess && emailNotificationResponse != null)
                {
                    notifyEntityUpdate.StatusCode = bng_notify_statuscode.Sent;
                    notifyEntityUpdate.StateCode = bng_notify_statecode.Completed;
                    notifyEntityUpdate.Description = emailNotificationResponse.Content?.Body;
                    notifyEntityUpdate.bng_ExternalNotifyID = emailNotificationResponse.Id;
                    notifyEntityUpdate.bng_RetryCount = 1;
                }
                else
                {
                    notifyEntityUpdate.StatusCode = bng_notify_statuscode.PendingSend;
                    notifyEntityUpdate.StateCode = bng_notify_statecode.Open;

                    if (actionType != (int)bng_notifytype.FurtherInformation)
                    {
                        notifyEntityUpdate.Description = errorMessage;
                    }

                    var retryCountColumnName = DataverseExtensions.AttributeLogicalName<bng_Notify>(nameof(bng_Notify.bng_RetryCount));

                    var retryCountEntity = await dataverseService.RetrieveAsync<bng_Notify>(notifyEntity.Id, new ColumnSet(retryCountColumnName));

                    if (retryCountEntity != null && retryCountEntity.Contains(retryCountColumnName))
                    {
                        var retryCount = (int)retryCountEntity[retryCountColumnName] + 1;

                        notifyEntityUpdate.bng_RetryCount = retryCount;
                        if (retryCount == 3)
                        {
                            notifyEntityUpdate.StatusCode = bng_notify_statuscode.Failed;
                            notifyEntityUpdate.StateCode = bng_notify_statecode.Completed;
                            notifyEntityUpdate.Description = errorMessage;
                        }
                    }
                    else
                    {
                        notifyEntityUpdate.bng_RetryCount = 1;

                        if (actionType != (int)bng_notifytype.FurtherInformation)
                        {
                            notifyEntityUpdate.Description = errorMessage;
                        }
                    }
                }
                await dataverseService.UpdateAsync(notifyEntityUpdate);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{message}", ex.Message);
            }
        }

        /// <summary>
        /// Gets list of entity with pending email status
        /// </summary>
        /// <returns></returns>
        public async Task<List<bng_Notify>> GetFailedNotifications()
        {
            var query = DataverseExtensions.GetQuery<bng_Notify>();
            query.Criteria.AddCondition(DataverseExtensions.AttributeLogicalName<bng_Notify>(nameof(bng_Notify.bng_ExternalNotifyID)), ConditionOperator.Null);
            query.Criteria.AddCondition(DataverseExtensions.AttributeLogicalName<bng_Notify>(nameof(bng_Notify.bng_RetryCount)), ConditionOperator.GreaterThan, 0);
            query.Criteria.AddCondition(DataverseExtensions.AttributeLogicalName<bng_Notify>(nameof(bng_Notify.bng_RetryCount)), ConditionOperator.LessThan, 3);

            return await dataverseService.RetrieveMultipleAsync<bng_Notify>(query);
        }
    }
}