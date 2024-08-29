namespace DEFRA.NE.BNG.Integration.Domain.Request
{
    public class BaselineHabitat
    {
        public string HabitatType { get; set; }
        public string BaselineReference { get; set; }
        public string Module { get; set; }
        public string State { get; set; }
        public string Condition { get; set; }
        public Area Area { get; set; }
        public string MeasurementUnits { get; set; }
    }
}
