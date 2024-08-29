using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEFRA.NE.BNG.Integration.Domain.Response
{
    public class GainsiteRegistrationIdValidation
    {
        public string gainsiteNumber { get; set; }
        public string gainsiteStatus { get; set; }
        public List<Habitat> habitats {  get; set; }
    }
}