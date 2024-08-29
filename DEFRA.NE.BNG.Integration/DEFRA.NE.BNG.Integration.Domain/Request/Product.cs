namespace DEFRA.NE.BNG.Integration.Domain.Entities
{
    public class Product
    {
        public string Code { get; set; }
        public decimal Qty { get; set; }
        public Guid OrderGuid { get; set; }
        public decimal Percentage { get; set; }
    }
}
