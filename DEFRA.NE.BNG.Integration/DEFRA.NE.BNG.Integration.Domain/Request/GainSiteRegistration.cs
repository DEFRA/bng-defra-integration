using DEFRA.NE.BNG.Integration.Model.Request;

namespace DEFRA.NE.BNG.Integration.Domain.Request
{
    public class GainSiteRegistration
    {
        public string GainSiteReference { get; set; }
        public string LegalAgreementType { get; set; }
        public string SubmittedOn { get; set; }
        public string EnhancementWorkStartDate { get; set; }
        public string LandBoundaryGridReference { get; set; }
        public decimal LandBoundaryHectares { get; set; }
        public string LegalAgreementStartDate { get; set; }
        public string LegalAgreementEndDate { get; set; }
        public string ManagementMonitoringStartDate { get; set; }
        public string HabitatPlanIncludedLegalAgreementYesNo { get; set; }
        public Payment Payment { get; set; }
        public Applicant Applicant { get; set; }
        public Agent Agent { get; set; }
        public Habitats Habitats { get; set; }
        public List<FileDetails> Files { get; set; }
        public List<LegalAgreementParty> LegalAgreementParties { get; set; }
        public List<PlanningObligationLpa> PlanningObligationLPAs { get; set; }
        public List<ResponsibleBody> ConservationCovernantResponsibleBodies { get; set; }
        public LandOwners LandOwners { get; set; }
        public ClientAddress LandownerAddress { get; set; }
        public OrganisationSummary Organisation { get; set; }
    }
}
