using DEFRA.NE.BNG.Integration.Domain.Constants;
using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Exceptions;
using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace DEFRA.NE.BNG.Integration.Infrastructure.Facades
{
    public class UKGovNotifyRetriggerFacade : FacadeBase, IUKGovNotifyRetriggerFacade
    {
        private readonly ILogger<UKGovNotifyRetriggerFacade> logger;
        private IUkGovNotifyFacade govNotify;
        protected readonly IEmailNotificationRequestGenerator emailNotificationRequestGenerator;

        public UKGovNotifyRetriggerFacade(ILogger<UKGovNotifyRetriggerFacade> logger,
                                          IUkGovNotifyFacade govNotify,
                                          IMailServiceAgent mailService,
                                          IConfigurationReader environmentVariableReader,
                                          IDataverseService dataverseService,
                                          IEmailNotificationRequestGenerator emailNotificationRequestGenerator)
                                        : base(
                                            environmentVariableReader,
                                            mailService,
                                            dataverseService)
        {
            this.logger = logger;
            this.govNotify = govNotify;
            this.emailNotificationRequestGenerator = emailNotificationRequestGenerator;
        }

        /// <summary>
        /// Retry sending email
        /// </summary>
        /// <returns></returns>
        public async Task RetrySendingMail()
        {
            var result = await govNotify.GetFailedNotifications();
            if (result?.Count > 0)
            {
                logger.LogInformation("Number of Record To be retry: {count}", result.Count);

                foreach (var entity in result)
                {
                    EmailNotificationRequestWrapper mailNotificationRequestWrapper = new();
                    try
                    {
                        mailNotificationRequestWrapper = await emailNotificationRequestGenerator.PrepairNotificationRequestData(entity);
                        var emailNotificationResponse = await mailService.SendMailAsync(mailNotificationRequestWrapper.EmailNotificationRequest, environmentVariableReader.Read(EnvironmentConstants.NotificationEmailService));

                        await govNotify.UpdateNotifyEntity(entity, emailNotificationResponse, string.Empty, true, mailNotificationRequestWrapper.ActionType);
                    }
                    catch (MailNotificationFailedException notificationError)
                    {
                        logger.LogError(notificationError.Message);
                        await govNotify.UpdateNotifyEntity(entity, null, notificationError.CustomErrorMessasge, false, mailNotificationRequestWrapper.ActionType);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex.Message);
                        await govNotify.UpdateNotifyEntity(entity, null, ex.Message, false, mailNotificationRequestWrapper.ActionType);
                    }
                }
            }
            else
            {
                logger.LogInformation("No Records To Retry.");
            }
        }
    }
}