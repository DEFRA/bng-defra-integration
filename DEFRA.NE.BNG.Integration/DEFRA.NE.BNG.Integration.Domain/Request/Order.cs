using DEFRA.NE.BNG.Integration.Domain.Request;
using DEFRA.NE.BNG.Integration.Model.Request;
using Microsoft.Xrm.Sdk;

namespace DEFRA.NE.BNG.Integration.Domain.Entities
{
    public class Order
    {
        public Applicant Applicant { get; set; }
        public Organisation Organisation { get; set; }
        public DevelopmentDetails Development { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public List<Product> Products { get; set; }
        public List<FileDetails> Files { get; set; }
        public Guid DevelopmentGuid { get; set; }
        public EntityReference Customer { get; set; }
        public EntityReference CustomerContact { get; set; }
        public EntityReference OwningTeam { get; set; }
        public int OrderTotalAmountThreshold { get; set; }
        public string CreditReference { get; set; }
        public string SubmittedOn { get; set; }
    }
}
