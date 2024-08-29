using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Domain.Request;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace DEFRA.NE.BNG.Integration.Infrastructure.Facades
{
    public class CombinedRegistrationFacade : FacadeBase, ICombinedRegistrationFacade
    {
        private readonly ILogger<CombinedRegistrationFacade> logger;
        private readonly ILandOwnerRegistrationFacade landOwnerRegistrationFacade;
        private readonly IDeveloperRegistrationFacade developerRegistrationFacade;

        public CombinedRegistrationFacade(ILogger<CombinedRegistrationFacade> logger,
                                           IConfigurationReader environmentVariableReader,
                                           IMailServiceAgent mailService,
                                           IDataverseService dataverseService,
                                           ILandOwnerRegistrationFacade landOwnerRegistrationFacade,
                                           IDeveloperRegistrationFacade developerRegistrationFacade
                                           ) : base(
                                                    environmentVariableReader,
                                                    mailService,
                                                    dataverseService)
        {
            this.logger = logger;
            this.landOwnerRegistrationFacade = landOwnerRegistrationFacade;
            this.developerRegistrationFacade = developerRegistrationFacade;
        }

        public async Task<Guid> OrchestrationBNG(CombinedRegistration combinedRegistration)
        {
            logger.LogInformation("Starting {className} orchestration...", nameof(CombinedRegistrationFacade));

            combinedRegistration.RegistrationDetails.SubmittedOn = combinedRegistration.SubmittedOn;
            combinedRegistration.RegistrationDetails.Payment = combinedRegistration.Payment;
            combinedRegistration.RegistrationDetails.Applicant = combinedRegistration.Applicant;
            combinedRegistration.RegistrationDetails.Agent = combinedRegistration.Agent;
            combinedRegistration.RegistrationDetails.LandownerAddress = combinedRegistration.LandownerAddress;
            combinedRegistration.RegistrationDetails.Files = combinedRegistration.Files;
            combinedRegistration.RegistrationDetails.Organisation = combinedRegistration.Organisation;

            combinedRegistration.RegistrationDetails.GainSiteReference = combinedRegistration.ApplicationReference;

            var gainSiteId = await landOwnerRegistrationFacade.OrchestrationBNG(combinedRegistration.RegistrationDetails, Domain.Models.bng_casetype.Combined);
            if(gainSiteId != Guid.Empty)
            {
                var gainSiteRegistration = await dataverseService.RetrieveAsync<bng_GainSiteRegistration>(gainSiteId, new ColumnSet(DataverseExtensions.AttributeLogicalName<bng_GainSiteRegistration>
                    (nameof(bng_GainSiteRegistration.bng_GainSiteReference))));
                combinedRegistration.AllocationDetails.GainSite.Reference = gainSiteRegistration.bng_GainSiteReference;
                combinedRegistration.AllocationDetails.Habitats = combinedRegistration.RegistrationDetails.Habitats;
                await developerRegistrationFacade.OrchestrationBNG(combinedRegistration.AllocationDetails, Domain.Models.bng_casetype.Combined);
            }
            logger.LogInformation("Completed {className} orchestration...", nameof(CombinedRegistrationFacade));
            return gainSiteId;
        }
    }
}
