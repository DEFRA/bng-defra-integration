using DEFRA.NE.BNG.Integration.Domain.JsonRequestRoot;

namespace DEFRA.NE.BNG.Integration.Domain.Interfaces
{
    public interface IDefraIdSyncFacade
    {
        public Task UpdateUserAndAccountFromDefraId(DefraIdRequestPayload requestData);
    }
}
