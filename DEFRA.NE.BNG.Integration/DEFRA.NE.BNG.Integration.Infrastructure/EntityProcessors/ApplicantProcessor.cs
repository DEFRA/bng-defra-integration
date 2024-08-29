using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Domain.Request;
using Microsoft.Extensions.Logging;

namespace DEFRA.NE.BNG.Integration.Infrastructure.EntityProcessors
{
    /// <summary>
    /// Implements IEntityProcessor for an ApplicantProcessor
    /// </summary>
    public class ApplicantProcessor
    {
        /// <summary>
        /// Creates an applicant in Dataverse
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <returns></returns>
        public async Task<Guid> Create(Applicant entityToCreate, IDataverseService dataverseService, ILogger logger)
        {
            logger.LogDebug("Executing {Method}", nameof(Create));

            Guid creationId;

            var query = DataverseExtensions.GetQuery<Contact>();

            query.AddEqualOperatorCondition<Contact>(nameof(Contact.bng_DefraID), entityToCreate.Id);

            var entity = await dataverseService.RetrieveFirstRecord<Contact>(query);

            if (entity != null)
            {
                creationId = entity.Id;
                bool updatesAvailable = false;

                var entityToUpdate = new Contact() { Id = creationId };

                if (!string.IsNullOrWhiteSpace(entityToCreate.MiddleName))
                {
                    entityToUpdate.MiddleName = entityToCreate.MiddleName;
                    updatesAvailable = true;
                }

                updatesAvailable = SetNationalities(entityToCreate, entityToUpdate);

                if (!string.IsNullOrWhiteSpace(entityToCreate.DateOfBirth) && DateTime.TryParse(entityToCreate.DateOfBirth, out DateTime dob))
                {
                    entityToUpdate.BirthDate = dob;
                    updatesAvailable = true;
                }

                if (updatesAvailable)
                {
                    await dataverseService.UpdateAsync(entityToUpdate);
                }
            }
            else
            {
                throw new InvalidDataException("Applicant does not exist!");
            }
            return creationId;
        }

        public async Task<List<Guid>> CreateList(List<Applicant> entityListToCreate, IDataverseService dataverseService, ILogger logger)
        {
            var tasks = entityListToCreate.Select(async x =>
              {
                  var result = await Create(x, dataverseService, logger);
                  return result;
              });

            var tasksArray = await Task.WhenAll(tasks);

            var combinedResults = tasksArray.ToList();

            return combinedResults;
        }


        private static bool SetNationalities(Applicant entityToCreate, Contact entity)
        {
            bool updated = false;
            if (entityToCreate?.NationalityIdList?.Count > 0)
            {
                var nationalityList = entityToCreate.NationalityIdList.Where(x => x != Guid.Empty).ToList();

                updated = nationalityList.Count > 0;

                if (nationalityList.Count > 0)
                {
                    entity.bng_NationalityId1 = nationalityList[0].GetEntityReference<bng_Nationality>();
                }

                if (nationalityList.Count > 1)
                {
                    entity.bng_NationalityId2 = nationalityList[1].GetEntityReference<bng_Nationality>();
                }

                if (nationalityList.Count > 2)
                {
                    entity.bng_NationalityId3 = nationalityList[2].GetEntityReference<bng_Nationality>();
                }

                if (nationalityList.Count > 3)
                {
                    entity.bng_NationalityId4 = nationalityList[3].GetEntityReference<bng_Nationality>();
                }
            }
            return updated;
        }

    }
}
