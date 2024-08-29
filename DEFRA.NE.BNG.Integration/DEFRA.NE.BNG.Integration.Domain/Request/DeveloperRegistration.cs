using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Model.Request;

namespace DEFRA.NE.BNG.Integration.Domain.Request
{
    public class DeveloperRegistration
    {
        public Applicant Applicant { get; set; }
        public Organisation Organisation { get; set; }
        public string IsLandownerLeaseholder { get; set; }
        public string SubmittedOn { get; set; }
        public Agent Agent { get; set; }
        public Habitats Habitats { get; set; }
        public List<FileDetails> Files { get; set; }
        public string AllocationReference { get; set; }
        public GainSite GainSite { get; set; }
        public DevelopmentDetails Development { get; set; }
        public Payment Payment { get; set; }
    }
}