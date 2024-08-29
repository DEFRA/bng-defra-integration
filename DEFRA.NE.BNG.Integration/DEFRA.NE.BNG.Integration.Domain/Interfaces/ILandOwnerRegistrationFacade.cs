using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Domain.Request;
using Microsoft.Xrm.Sdk;

namespace DEFRA.NE.BNG.Integration.Domain.Interfaces
{
    public interface ILandOwnerRegistrationFacade
    {
        public Task<Guid> OrchestrationBNG(GainSiteRegistration gainSiteRegistration, bng_casetype caseType);

        public Task<Guid> CreateGainSiteRegistration(GainSiteRegistration gainSiteRegistration, EntityReference applicantId, EntityReference clientReference, Entity organisationEntity, bng_casetype caseType);

        public Task<Guid> CreateLegalAgreementParties(List<LegalAgreementParty> legalAgreementParties, Guid gainSiteRegistrationId);

        public Task<Guid> CreateOtherLandOwners(List<OtherLandOwner> otherLandOwners, Guid gainSiteRegistrationId);

        public Task<IDictionary<string, Guid>> CreateBaselineHabitats(Habitats habitats, Guid gainSiteRegistrationId);

        public Task CreateProposedHabitats(Habitats habitats, Guid gainSiteRegistrationId, IDictionary<string, Guid> baselineHabitatCollection);
    }
}