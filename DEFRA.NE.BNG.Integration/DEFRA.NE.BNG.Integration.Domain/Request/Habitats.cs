namespace DEFRA.NE.BNG.Integration.Domain.Request
{
    public class Habitats
    {
        public List<BaselineHabitat> Baseline { get; set; }
        public List<Habitat> Proposed { get; set; }
        public List<DEFRA.NE.BNG.Integration.Domain.Entities.AllocatedHabitat> Allocated { get; set; }
    }


}

