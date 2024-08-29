namespace DEFRA.NE.BNG.Integration.Domain.Entities
{
    public class AllocatedHabitat
    {
        public Guid DeveloperRegistrationId { get; set; }
        public Guid GainSiteId { get; set; }
        public Guid CaseId { get; set; }
        public string HabitatId { get; set; }
        public decimal Area { get; set; }
    }
}