namespace DEFRA.NE.BNG.Integration.Domain.Request
{
    public class ClientAddress
    {
        public string Type { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string County { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
    }

}
