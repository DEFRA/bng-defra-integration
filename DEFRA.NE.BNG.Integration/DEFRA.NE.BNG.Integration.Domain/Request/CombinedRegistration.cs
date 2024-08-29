using DEFRA.NE.BNG.Integration.Model.Request;

namespace DEFRA.NE.BNG.Integration.Domain.Request
{
    public class CombinedRegistration
    {
        public Applicant Applicant { get; set; }
        public Agent Agent { get; set; }
        public string ApplicationReference { get; set; }
        public GainSiteRegistration RegistrationDetails { get; set; }
        //Used Gainsite and development
        public DeveloperRegistration AllocationDetails { get; set; }
        public string SubmittedOn { get; set; }
        public Payment Payment { get; set; }
        public List<FileDetails> Files { get; set; }
        public List<LegalAgreementParty> LegalAgreementParties { get; set; }
        public ClientAddress LandownerAddress { get; set; }
        public OrganisationSummary Organisation { get; set; }
    }
}
