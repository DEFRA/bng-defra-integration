using DEFRA.NE.BNG.Integration.Domain.Exceptions;
using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Infrastructure.Facades;

namespace DEFRA.NE.BNG.Integration.Infrastructure.UnitTests.Facades
{
    public class GainsiteValidatorFacadeTests : TestBase<GainsiteValidatorFacade>
    {
        private GainsiteValidatorFacade systemUnderTest;

        public GainsiteValidatorFacadeTests()
        {
            systemUnderTest = new GainsiteValidatorFacade(logger.Object, environmentVariableReader.Object, mailService.Object, dataverseService.Object);
        }

        [Fact]
        public async Task ValidateGainsiteFromId_NullGainSiteId()
        {
            Exception exception = null;

            try
            {
                await systemUnderTest.ValidateGainsiteFromId(null);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            exception.Should().NotBeNull();
            exception.Message.Should().Be("Gainsite is npt provided!");
        }

        [Fact]
        public async Task ValidateGainsiteFromId_EmptyGainSiteId()
        {
            Exception exception = null;

            try
            {
                await systemUnderTest.ValidateGainsiteFromId("");
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            exception.Should().NotBeNull();
            exception.Message.Should().Be("Gainsite is npt provided!");
        }

        [Fact]
        public async Task ValidateGainsiteFromId_NoGainSiteFound()
        {
            Exception exception = null;
            bng_GainSiteRegistration entity = null;
            var gainSiteId = Guid.NewGuid().ToString();

            dataverseService.Setup(x => x.RetrieveFirstRecord<bng_GainSiteRegistration>(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(entity);

            try
            {
                await systemUnderTest.ValidateGainsiteFromId(gainSiteId);
            }
            catch (DataNotFoundException ex)
            {
                exception = ex;
            }

            exception.Should().NotBeNull();
            exception.Message.Should().Be("Gainsite not found");
        }

        [Fact]
        public async Task ValidateGainsiteFromId_RetrieveFirstRecordThrowsException()
        {
            Exception exception = null;
            bng_GainSiteRegistration entity = new();
            var gainSiteId = Guid.NewGuid().ToString();

            dataverseService.Setup(x => x.RetrieveFirstRecord<bng_GainSiteRegistration>(It.IsAny<QueryExpression>()))
                            .Throws<Exception>();

            try
            {
                await systemUnderTest.ValidateGainsiteFromId(gainSiteId);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            exception.Should().NotBeNull();
        }

        [Fact]
        public async Task ValidateGainsiteFromId_PartialValueForHabitat()
        {
            var gainSiteId = "BGS-070524010";

            bng_GainSiteRegistration entity = new()
            {
                bng_GainSiteReference = gainSiteId,
                statuscode = bng_gainsiteregistration_statuscode.Registered,
                bng_GainSiteRegistrationId = Guid.NewGuid()
            };

            bng_HabitatType habitat = new()
            {
                bng_HabitatName = "HAB-00001221-BP4H8"
            };
            List<bng_HabitatType> entityCollection = [habitat];

            dataverseService.Setup(x => x.RetrieveMultipleAsync<bng_HabitatType>(It.IsAny<QueryBase>()))
                            .ReturnsAsync(entityCollection);
            dataverseService.Setup(x => x.RetrieveFirstRecord<bng_GainSiteRegistration>(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(entity);

            var actual = await systemUnderTest.ValidateGainsiteFromId(gainSiteId);

            actual.gainsiteNumber.Should().Be(entity.bng_GainSiteReference);
            actual.gainsiteStatus.Should().Be(entity.statuscode.ToString());
            actual.habitats.Count.Should().Be(1);
            actual.habitats[0].HabitatId.Should().Be(habitat.bng_HabitatName);
        }

        [Fact]
        public async Task ValidateGainsiteFromId_HabitatWithValuesPopulated()
        {
            var gainSiteId = "BGS-070524010";

            bng_GainSiteRegistration entity = new()
            {
                bng_GainSiteReference = gainSiteId,
                statuscode = bng_gainsiteregistration_statuscode.Registered,
                bng_GainSiteRegistrationId = Guid.NewGuid()
            };

            bng_HabitatType habitat = new()
            {
                bng_HabitatName = "HAB-00001221-BP4H8",
                bng_HabitatType1 = bng_habitattypechoices.Hedgerow,
                bng_ProposedHabitatSubTypeLookup = new EntityReference(),
                bng_Size = 12M,
                bng_Allocated = 10M,
                bng_Condition = bng_habitatconditionchoices.Moderate
            };
            List<bng_HabitatType> entityCollection = [habitat];

            dataverseService.Setup(x => x.RetrieveMultipleAsync<bng_HabitatType>(It.IsAny<QueryBase>()))
                            .ReturnsAsync(entityCollection);
            dataverseService.Setup(x => x.RetrieveFirstRecord<bng_GainSiteRegistration>(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(entity);

            var actual = await systemUnderTest.ValidateGainsiteFromId(gainSiteId);

            actual.gainsiteNumber.Should().Be(entity.bng_GainSiteReference);
            actual.gainsiteStatus.Should().Be(entity.statuscode.ToString());
            actual.habitats.Count.Should().Be(1);
            actual.habitats[0].HabitatId.Should().Be(habitat.bng_HabitatName);
            actual.habitats[0].HabitatModule.Should().Be(habitat.bng_HabitatType1.Value.ToString());
            actual.habitats[0].ProposedHabitatType.Should().Be(habitat.bng_ProposedHabitatSubTypeLookup?.Name);
            actual.habitats[0].AreaLength.Should().Be(habitat.bng_Size?.ToString());
            actual.habitats[0].TotalAllocated.Should().Be(habitat.bng_Allocated?.ToString());
            actual.habitats[0].Remaining.Should().Be(habitat.bng_Remaining?.ToString());
            actual.habitats[0].Condition.Should().Be(habitat.bng_Condition.Value.ToString());
        }
    }
}