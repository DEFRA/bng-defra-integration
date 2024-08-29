using DEFRA.NE.BNG.Integration.Domain.Entities;

namespace DEFRA.NE.BNG.Integration.Domain.Interfaces
{
    public interface IOrderFacade
    {
        public Task<Guid> OrchestrationBNG(Order order, IConfigurationReader environmentVariableReader);
    }
}
