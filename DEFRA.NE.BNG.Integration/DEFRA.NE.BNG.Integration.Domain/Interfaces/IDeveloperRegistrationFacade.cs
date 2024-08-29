using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Domain.Request;

namespace DEFRA.NE.BNG.Integration.Domain.Interfaces
{
    public interface IDeveloperRegistrationFacade
    {
        public Task<Guid> OrchestrationBNG(DeveloperRegistration developerRegistration, bng_casetype caseType = bng_casetype.Allocation);
    }
}
