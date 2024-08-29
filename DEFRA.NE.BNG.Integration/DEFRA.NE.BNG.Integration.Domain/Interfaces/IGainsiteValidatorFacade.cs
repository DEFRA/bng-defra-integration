using DEFRA.NE.BNG.Integration.Domain.Request;
using DEFRA.NE.BNG.Integration.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEFRA.NE.BNG.Integration.Domain.Interfaces
{
    public interface IGainsiteValidatorFacade
    {
        public Task<GainsiteRegistrationIdValidation> ValidateGainsiteFromId(string gainsiteId);

    }
}
