using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using DEFRA.NE.BNG.Integration.Domain.JsonRequestRoot;
using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Infrastructure.Services;
using DEFRA.NE.BNG.Integration.Infrastructure.UnitTests;
using Newtonsoft.Json;

namespace DEFRA.NE.BNG.Integration.Infrastructure.Facades.Tests
{
    public class CombinedRegistrationFacadeTests : TestBase<CombinedRegistrationFacade>
    {
        private CombinedRegistrationFacade systemUnderTest;
        private Mock<ILandOwnerRegistrationFacade> landOwnerRegistrationFacade;
        private Mock<IDeveloperRegistrationFacade> developerRegistrationFacade;

        public CombinedRegistrationFacadeTests()
        {
            landOwnerRegistrationFacade = new Mock<ILandOwnerRegistrationFacade>();
            developerRegistrationFacade = new Mock<IDeveloperRegistrationFacade>();
            systemUnderTest = new CombinedRegistrationFacade(logger.Object, environmentVariableReader.Object, mailService.Object, dataverseService.Object, landOwnerRegistrationFacade.Object, developerRegistrationFacade.Object);
        }

        [Fact]
        public void CanInstantiate()
        {
            FluentActions.Invoking(() => new CombinedRegistrationFacade(logger.Object, environmentVariableReader.Object, mailService.Object, dataverseService.Object, landOwnerRegistrationFacade.Object, developerRegistrationFacade.Object))
                         .Should()
                         .NotThrow();
        }

        [Fact]
        public async Task OrchestrationBNG_LandRegistration_ThrowsApplicantNotFoundException()
        {
            var json = await File.ReadAllTextAsync("TestData/combined/combined-case-individual.json");
            var payload = JsonConvert.DeserializeObject<CombinedRequestPayload>(json);
            Exception thrownException = null;

            var createdGainSiteGuid = Guid.NewGuid();

            landOwnerRegistrationFacade.Setup(x => x.OrchestrationBNG(It.IsAny<GainSiteRegistration>(), It.IsAny<bng_casetype>()))
                                       .Throws(new InvalidDataException("No valid applicant was supplied in payload!"));

            try
            {
                await systemUnderTest.OrchestrationBNG(payload.combinedCase);
            }
            catch (Exception ex)
            {
                thrownException = ex;
            }

            thrownException.Should().NotBeNull();
            thrownException.Message.Should().Be("No valid applicant was supplied in payload!");
        }

        [Fact]
        public async Task OrchestrationBNG_LandRegistration()
        {
            var json = await File.ReadAllTextAsync("TestData/combined/combined-case-individual.json");
            var payload = JsonConvert.DeserializeObject<CombinedRequestPayload>(json);

            var createdGainSiteGuid = Guid.Empty;

            landOwnerRegistrationFacade.Setup(x => x.OrchestrationBNG(It.IsAny<GainSiteRegistration>(), It.IsAny<bng_casetype>()))
                                       .ReturnsAsync(createdGainSiteGuid);

            var actual = await systemUnderTest.OrchestrationBNG(payload.combinedCase);

            landOwnerRegistrationFacade.Verify(x => x.OrchestrationBNG(It.IsAny<GainSiteRegistration>(), It.IsAny<bng_casetype>()),
                                                    Times.Once);
        }

        [Fact]
        public async Task OrchestrationBNG_LandAndAllocationRegistration()
        {
            var json = await File.ReadAllTextAsync("TestData/combined/combined-case-individual.json");
            var payload = JsonConvert.DeserializeObject<CombinedRequestPayload>(json);
            var gainsite = new bng_GainSiteRegistration
            {
                bng_GainSiteReference = "BGS-Test"
            };
            var createdGainSiteGuid = Guid.NewGuid();

            landOwnerRegistrationFacade.Setup(x => x.OrchestrationBNG(It.IsAny<GainSiteRegistration>(), It.IsAny<bng_casetype>()))
                                       .ReturnsAsync(createdGainSiteGuid);

            dataverseService.Setup(x => x.RetrieveAsync<bng_GainSiteRegistration>(createdGainSiteGuid, It.IsAny<ColumnSet>()))
                .ReturnsAsync(gainsite);

            var actual = await systemUnderTest.OrchestrationBNG(payload.combinedCase);

            landOwnerRegistrationFacade.Verify(x => x.OrchestrationBNG(It.IsAny<GainSiteRegistration>(), It.IsAny<bng_casetype>()),
                                                    Times.Once);
        }
    }
}