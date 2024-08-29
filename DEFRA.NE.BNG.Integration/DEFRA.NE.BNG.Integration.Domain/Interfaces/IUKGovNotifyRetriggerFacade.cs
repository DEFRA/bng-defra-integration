namespace DEFRA.NE.BNG.Integration.Domain.Interfaces
{
    public interface IUKGovNotifyRetriggerFacade
    {
        public Task RetrySendingMail();
    }
}
