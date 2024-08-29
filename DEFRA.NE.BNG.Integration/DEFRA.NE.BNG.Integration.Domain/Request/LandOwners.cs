using DEFRA.NE.BNG.Integration.Domain.Entities;

namespace DEFRA.NE.BNG.Integration.Domain.Request
{
    public class LandOwners
    {
        public List<Organisation> Organisation { get; set; }
        public List<Individual> Individual { get; set; }
    }
}
