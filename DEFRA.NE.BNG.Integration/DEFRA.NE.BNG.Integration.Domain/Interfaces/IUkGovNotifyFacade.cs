using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Domain.Response;
using Microsoft.Xrm.Sdk;

namespace DEFRA.NE.BNG.Integration.Domain.Interfaces
{
    public interface IUkGovNotifyFacade
    {
        public Task<bool> EmailNotification(Guid notifyEntityId);
        public Task UpdateNotifyEntity(Entity notifyEntity, EmailNotificationResponse emailNotificationResponse, string errorMessage, bool isSuccess, int actionType);
        public Task<List<bng_Notify>> GetFailedNotifications();
    }
}