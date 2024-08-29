using DEFRA.NE.BNG.Integration.Domain.Entities;

namespace DEFRA.NE.BNG.Integration.Domain.Request
{
    public class DevelopmentDetails
    {
        public string Name { get; set; }
        public string PlanningReference { get; set; }
        public LocalPlanningAuthority LocalPlanningAuthority { get; set; }
    }
}