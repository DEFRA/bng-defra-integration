namespace DEFRA.NE.BNG.Integration.Domain.Request
{
    public class Habitat
    {
        public string HabitatId { get; set; }
        public string HabitatType { get; set; }
        public string BaselineReference { get; set; }
        public string Module { get; set; }
        public string State { get; set; }
        public string Condition { get; set; }
        public string StrategicSignificance { get; set; }
        public string AdvanceCreation { get; set; }
        public string DelayedCreation { get; set; }
        public decimal Area { get; set; }
        public string MeasurementUnits { get; set; }
        public string Id { get; set; }
        public float DeliveredUnits { get; set; }
        public string EncroachmentExtent { get; set; }
        public string EncroachmentExtentBothBanks { get; set; }
        public Guid DeveloperRegistrationId { get; set; }
        public Guid GainSiteId { get; set; }
    }
}