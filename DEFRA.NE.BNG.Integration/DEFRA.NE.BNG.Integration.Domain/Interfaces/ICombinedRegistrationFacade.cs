using DEFRA.NE.BNG.Integration.Domain.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEFRA.NE.BNG.Integration.Domain.Interfaces
{
    public interface ICombinedRegistrationFacade
    {
        public Task<Guid> OrchestrationBNG(CombinedRegistration combinedRegistration);
        
    }
}
