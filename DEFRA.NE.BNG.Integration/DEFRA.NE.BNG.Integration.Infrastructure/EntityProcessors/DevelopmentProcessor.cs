using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Domain.Request;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace DEFRA.NE.BNG.Integration.Infrastructure.EntityProcessors
{
    /// <summary>
    /// Implements IEntityProcessor for an OrganisationProcessor
    /// </summary>
    public class DevelopmentProcessor
    {
        public async Task<(Guid Id, bool AlreadyLinkedToGainSite)> Create(DevelopmentDetails entityToCreate, string referenceNumber, EntityReference applicant, EntityReference localPlanningAuthority, IDataverseService dataverseService, ILogger logger)
        {
            logger.LogDebug("Executing {method}", nameof(Create));

            Guid id = Guid.Empty;
            var linkedToGainSite = false;

            if (entityToCreate != null)
            {
                var entityCollection = await RetrieveDeveloperRegistration(entityToCreate, localPlanningAuthority, dataverseService);

                if (entityCollection != null && entityCollection.Entities.Count > 0)
                {
                    id = entityCollection.Entities[0].Id;
                    linkedToGainSite = IsAlreadyLinkedToGainsite(referenceNumber, linkedToGainSite, entityCollection);
                }
                else
                {
                    bng_DeveloperRegistration developerEntity = InstantiateDeveloperRegistration(entityToCreate, applicant, localPlanningAuthority);

                    id = await dataverseService.CreateAsync(developerEntity);
                }
            }

            return (id, linkedToGainSite);
        }

        public static async Task<EntityCollection> RetrieveDeveloperRegistration(DevelopmentDetails development, EntityReference localPlanningAuthority, IDataverseService dataverseService)
        {
            EntityCollection result;
            var query = DataverseExtensions.GetQuery<bng_DeveloperRegistration>();

            if (string.IsNullOrWhiteSpace(development.PlanningReference))
            {
                result = null;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(development.PlanningReference))
                {
                    query.AddEqualOperatorCondition<bng_DeveloperRegistration>(nameof(bng_DeveloperRegistration.bng_PlanningReference), development.PlanningReference);
                }

                if (localPlanningAuthority != null && localPlanningAuthority.Id != Guid.Empty)
                {
                    query.AddEqualOperatorCondition<bng_DeveloperRegistration>(nameof(bng_DeveloperRegistration.bng_localplanningauthority), localPlanningAuthority.Id);
                }

                LinkEntity linkEntity1 = new(bng_DeveloperRegistration.EntityLogicalName,
                                               "bng_developerregistration_bng_gainsitereg",
                       DataverseExtensions.AttributeLogicalName<bng_DeveloperRegistration>(nameof(bng_DeveloperRegistration.bng_DeveloperRegistrationId)),
                       DataverseExtensions.AttributeLogicalName<bng_DeveloperRegistration>(nameof(bng_DeveloperRegistration.bng_DeveloperRegistrationId))
                       , JoinOperator.LeftOuter);

                LinkEntity linkEntity2 = new(
                   DataverseExtensions.GetRelationshipSchemaName<bng_DeveloperRegistration>(nameof(bng_DeveloperRegistration.bng_DeveloperRegistration_GainSiteReg)).ToLower()
                    , bng_GainSiteRegistration.EntityLogicalName,
                    DataverseExtensions.AttributeLogicalName<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.bng_GainSiteRegistrationId)),
                    DataverseExtensions.AttributeLogicalName<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.bng_GainSiteRegistrationId)), JoinOperator.Inner);
                linkEntity2.EntityAlias = "gs";
                linkEntity2.Columns.AddColumns(
                    DataverseExtensions.AttributeLogicalName<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.bng_GainSiteReference))
                    );

                linkEntity1.LinkEntities.Add(linkEntity2);
                query.LinkEntities.Add(linkEntity1);


                result = await dataverseService.RetrieveMultipleAsync(query);
            }
            return result;
        }

        private static bool IsAlreadyLinkedToGainsite(string referenceNumber, bool alreadyLinkedToGainSite, EntityCollection entityCollection)
        {
            foreach (var entity in entityCollection.Entities)
            {
                var aliasedValue = entity.GetAttributeValue<AliasedValue>($"gs.{DataverseExtensions.AttributeLogicalName<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.bng_GainSiteReference))}");

                if (referenceNumber == aliasedValue?.Value?.ToString())
                {
                    alreadyLinkedToGainSite = true;
                    break;
                }
            }

            return alreadyLinkedToGainSite;
        }

        private static bng_DeveloperRegistration InstantiateDeveloperRegistration(DevelopmentDetails entityToCreate, EntityReference applicant, EntityReference localPlanningAuthority)
        {
            var developerEntity = new bng_DeveloperRegistration
            {
                bng_PlanningReference = entityToCreate.PlanningReference,
                bng_ProjectName = entityToCreate.Name,
                bng_source = bng_source.Online
            };

            if (localPlanningAuthority != null && localPlanningAuthority.Id != Guid.Empty)
            {
                developerEntity.bng_localplanningauthority = localPlanningAuthority;
            }

            if (applicant != null)
            {
                developerEntity.bng_ApplicantID = applicant;
            }

            return developerEntity;
        }

    }
}