using DEFRA.NE.BNG.Integration.Domain.JsonRequestRoot;
using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Infrastructure.Facades;
using DEFRA.NE.BNG.Integration.Model.Request;

namespace DEFRA.NE.BNG.Integration.Infrastructure.UnitTests.Facades
{
    public class DeveloperRegistrationFacadeTests : TestBase<DeveloperRegistrationFacade>
    {
        private readonly DeveloperRequestPayload data;

        private readonly DeveloperRegistrationFacade systemUnderTest;

        public DeveloperRegistrationFacadeTests() : base()
        {
            data = CreateSamplePayload();

            systemUnderTest = new DeveloperRegistrationFacade(logger.Object, environmentVariableReader.Object, mailService.Object, dataverseService.Object);
        }

        [Fact]
        public void CanCreateDeveloperRegistrationFacade()
        {
            FluentActions.Invoking(() => new DeveloperRegistrationFacade(logger.Object, environmentVariableReader.Object, mailService.Object, dataverseService.Object))
                            .Should()
                            .NotThrow();
        }

        [Fact]
        public void OrchestrationBNG_Null_DeveloperRegistration()
        {
            Contact application = new();

            dataverseService.Setup(x => x.RetrieveFirstRecord<Contact>(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(application);

            dataverseService.Setup(x => x.CreateAsync(It.IsAny<Entity>()))
                            .ReturnsAsync(Guid.NewGuid());

            DeveloperRegistration developerRegistration = null;

            FluentActions.Invoking(() => systemUnderTest.OrchestrationBNG(developerRegistration))
                         .Should()
                         .ThrowAsync<InvalidDataException>()
                         .WithMessage("Applicant is required!");
        }


        [Fact]
        public async Task OrchestrationBNG_Individual()
        {
            Contact application = new() { Id = Guid.NewGuid() };

            var payload = await HydrateFromTestDataFile<DeveloperRequestPayload>("TestData/allocation/allocation.individual.json");

            dataverseService.Setup(x => x.RetrieveFirstRecord<Contact>(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(application);

            EntityCollection caseCol = new();
            caseCol.Entities.Add(new Entity("development", Guid.NewGuid()));

            dataverseService.Setup(x => x.RetrieveMultipleAsync(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(caseCol);

            var actual = await systemUnderTest.OrchestrationBNG(payload.DeveloperRegistration);

            actual.Should().NotBe(Guid.Empty);

            dataverseService.VerifyAll();
            dataverseService.Verify(x => x.UpdateAsync(It.IsAny<Entity>()), Times.Once);
        }

        [Fact]
        public async Task OrchestrationBNG_Organisation()
        {
            Contact application = new() { Id = Guid.NewGuid() };

            var payload = await HydrateFromTestDataFile<DeveloperRequestPayload>("TestData/allocation/allocation.organisation.json");

            dataverseService.Setup(x => x.RetrieveFirstRecord<Contact>(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(application);

            EntityCollection caseCol = new();
            caseCol.Entities.Add(new Entity("development", Guid.NewGuid()));

            dataverseService.Setup(x => x.RetrieveMultipleAsync(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(caseCol);

            var actual = await systemUnderTest.OrchestrationBNG(payload.DeveloperRegistration);

            actual.Should().NotBe(Guid.Empty);

            dataverseService.VerifyAll();
            dataverseService.Verify(x => x.UpdateAsync(It.IsAny<Entity>()), Times.Once);
        }

        [Fact]
        public async Task OrchestrationBNG_Agent()
        {
            Contact application = new() { Id = Guid.NewGuid() };

            var payload = await HydrateFromTestDataFile<DeveloperRequestPayload>("TestData/allocation/allocation.agent.individual.json");

            dataverseService.Setup(x => x.RetrieveFirstRecord<Contact>(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(application);

            EntityCollection caseCol = new();
            caseCol.Entities.Add(new Entity("development", Guid.NewGuid()));

            dataverseService.Setup(x => x.RetrieveMultipleAsync(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(caseCol);

            var actual = await systemUnderTest.OrchestrationBNG(payload.DeveloperRegistration);

            actual.Should().NotBe(Guid.Empty);

            dataverseService.VerifyAll();
            dataverseService.Verify(x => x.UpdateAsync(It.IsAny<Entity>()), Times.Once);
        }

        [Fact]
        public async Task RetrieveDevReistrationIdForAllocations()
        {
            DevelopmentDetails developmentDetails = new()
            {
                PlanningReference = "Test"
            };

            Entity entity = new Entity("tests", Guid.NewGuid());

            EntityCollection entityCollection = new();
            entityCollection.Entities.Add(
                entity
                );

            dataverseService.Setup(x => x.RetrieveMultipleAsync(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(entityCollection);

            var actual = await systemUnderTest.RetrieveDevReistrationIdForAllocations(developmentDetails);

            dataverseService.VerifyAll();
        }

        [Fact]
        public async Task RetrieveGainSiteWithDeveloper()
        {
            GainSite gainSite = new()
            {
                Reference = "Test reference"
            };

            dataverseService.Setup(x => x.RetrieveFirstRecordForEntity(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(Guid.NewGuid());

            var actual = await systemUnderTest.RetrieveGainSiteWithDeveloper(gainSite);

            dataverseService.VerifyAll();
        }

        [Fact]
        public void GenerateCaseCreationInstructionsJson_Agent_IndividualLandOwner()
        {
            DeveloperRegistration developerRegistration = new()
            {
                IsLandownerLeaseholder = "Yes",
                GainSite = new GainSite
                {
                    OffSiteUnitChange = new OffSiteUnitChange()
                    {
                        Habitat = 29.8M,
                        Hedge = 3435M,
                        Watercourse = 80986M
                    }
                },
                Applicant = new()
                {
                    Role = "agent"
                },
                Agent = new()
                {
                    ClientType = "individual"
                }
            };

            EntityReference clientReference = new(Contact.EntityLogicalName, Guid.NewGuid());
            Guid gainSiteId = Guid.NewGuid();
            List<Guid> allocatedHabitatIds = [Guid.NewGuid(), Guid.NewGuid()];

            var actual = DeveloperRegistrationFacade.GenerateCaseCreationInstructionsJson(developerRegistration, clientReference, gainSiteId, allocatedHabitatIds, bng_casetype.Allocation);

            actual.Should().NotBeNull();
            actual.Should().Contain($"\"applicantRole\":759150001");
        }

        [Fact]
        public void GenerateCaseCreationInstructionsJson_Agent_Individual()
        {
            DeveloperRegistration developerRegistration = new()
            {
                IsLandownerLeaseholder = "No",
                GainSite = new GainSite
                {
                    OffSiteUnitChange = new OffSiteUnitChange()
                    {
                        Habitat = 29.8M,
                        Hedge = 3435M,
                        Watercourse = 80986M
                    }
                },
                Applicant = new()
                {
                    Role = "agent"
                },
                Agent = new()
                {
                    ClientType = "individual"
                }
            };

            EntityReference clientReference = new(Contact.EntityLogicalName, Guid.NewGuid());
            Guid gainSiteId = Guid.NewGuid();
            List<Guid> allocatedHabitatIds = [Guid.NewGuid(), Guid.NewGuid()];

            var actual = DeveloperRegistrationFacade.GenerateCaseCreationInstructionsJson(developerRegistration, clientReference, gainSiteId, allocatedHabitatIds, bng_casetype.Allocation);

            actual.Should().NotBeNull();
            actual.Should().Contain($"\"applicantRole\":759150004");
        }


        [Fact]
        public void GenerateCaseCreationInstructionsJson_Agent_Organisation()
        {
            DeveloperRegistration developerRegistration = new()
            {
                IsLandownerLeaseholder = "No",
                GainSite = new GainSite
                {
                    OffSiteUnitChange = new OffSiteUnitChange()
                    {
                        Habitat = 29.8M,
                        Hedge = 3435M,
                        Watercourse = 80986M
                    }
                },
                Applicant = new()
                {
                    Role = "agent"
                },
                Agent = new()
                {
                    ClientType = "organisation"
                }
            };

            EntityReference clientReference = new(Contact.EntityLogicalName, Guid.NewGuid());
            Guid gainSiteId = Guid.NewGuid();
            List<Guid> allocatedHabitatIds = [Guid.NewGuid(), Guid.NewGuid()];

            var actual = DeveloperRegistrationFacade.GenerateCaseCreationInstructionsJson(developerRegistration, clientReference, gainSiteId, allocatedHabitatIds, bng_casetype.Allocation);

            actual.Should().NotBeNull();
            actual.Should().Contain($"\"applicantRole\":759150001");
            actual.Should().NotContain($"\"organisationId\":");
        }


        [Fact]
        public void GenerateCaseCreationInstructionsJson_Individual_Landowner_OrganisationClientType()
        {
            DeveloperRegistration developerRegistration = new()
            {
                IsLandownerLeaseholder = "Yes",
                GainSite = new GainSite
                {
                    OffSiteUnitChange = new OffSiteUnitChange()
                    {
                        Habitat = 29.8M,
                        Hedge = 3435M,
                        Watercourse = 80986M
                    }
                },
                Applicant = new()
                {
                    Role = "individual"
                },
                Agent = new()
                {
                    ClientType = "organisation"
                },
                Organisation = new() { Id = Guid.NewGuid().ToString() },
            };

            EntityReference clientReference = new(Contact.EntityLogicalName, Guid.NewGuid());
            Guid gainSiteId = Guid.NewGuid();
            List<Guid> allocatedHabitatIds = [Guid.NewGuid(), Guid.NewGuid()];

            var actual = DeveloperRegistrationFacade.GenerateCaseCreationInstructionsJson(developerRegistration, clientReference, gainSiteId, allocatedHabitatIds, bng_casetype.Allocation);

            actual.Should().NotBeNull();
            actual.Should().Contain($"\"applicantRole\":759150002");
            actual.Should().NotContain($"\"organisationId\":");
        }


        [Fact]
        public void GenerateCaseCreationInstructionsJson_Individual()
        {
            DeveloperRegistration developerRegistration = new()
            {
                IsLandownerLeaseholder = "No",
                GainSite = new GainSite
                {
                    OffSiteUnitChange = new OffSiteUnitChange()
                    {
                        Habitat = 29.8M,
                        Hedge = 3435M,
                        Watercourse = 80986M
                    }
                },
                Applicant = new()
                {
                    Role = "individual"
                }
            };

            EntityReference clientReference = new(Contact.EntityLogicalName, Guid.NewGuid());
            Guid gainSiteId = Guid.NewGuid();
            List<Guid> allocatedHabitatIds = [Guid.NewGuid(), Guid.NewGuid()];

            var actual = DeveloperRegistrationFacade.GenerateCaseCreationInstructionsJson(developerRegistration, clientReference, gainSiteId, allocatedHabitatIds, bng_casetype.Allocation);

            actual.Should().NotBeNull();
            actual.Should().Contain($"\"applicantRole\":759150003");
        }


        [Fact]
        public void GenerateCaseCreationInstructionsJson_Organisation_LandOwner()
        {
            DeveloperRegistration developerRegistration = new()
            {
                IsLandownerLeaseholder = "Yes",
                GainSite = new GainSite
                {
                    OffSiteUnitChange = new OffSiteUnitChange()
                    {
                        Habitat = 29.8M,
                        Hedge = 3435M,
                        Watercourse = 80986M
                    }
                },
                Applicant = new()
                {
                    Role = "organisation"
                },
                Organisation = new() { Id = Guid.NewGuid().ToString() }
            };

            EntityReference clientReference = new(Contact.EntityLogicalName, Guid.NewGuid());
            Guid gainSiteId = Guid.NewGuid();
            List<Guid> allocatedHabitatIds = [Guid.NewGuid(), Guid.NewGuid()];

            var actual = DeveloperRegistrationFacade.GenerateCaseCreationInstructionsJson(developerRegistration, clientReference, gainSiteId, allocatedHabitatIds, bng_casetype.Allocation);

            actual.Should().NotBeNull();
            actual.Should().Contain($"\"applicantRole\":759150000");
            actual.Should().NotContain($"\"organisationId\":");
        }

        [Fact]
        public void GenerateCaseCreationInstructionsJson_Individual_LandOwner()
        {
            DeveloperRegistration developerRegistration = new()
            {
                IsLandownerLeaseholder = "Yes",
                GainSite = new GainSite
                {
                    OffSiteUnitChange = new OffSiteUnitChange()
                    {
                        Habitat = 29.8M,
                        Hedge = 3435M,
                        Watercourse = 80986M
                    }
                },
                Applicant = new()
                {
                    Role = "individual"
                }
            };

            EntityReference clientReference = new(Contact.EntityLogicalName, Guid.NewGuid());
            Guid gainSiteId = Guid.NewGuid();
            List<Guid> allocatedHabitatIds = [Guid.NewGuid(), Guid.NewGuid()];

            var actual = DeveloperRegistrationFacade.GenerateCaseCreationInstructionsJson(developerRegistration, clientReference, gainSiteId, allocatedHabitatIds, bng_casetype.Allocation);

            actual.Should().NotBeNull();
            actual.Should().Contain($"\"applicantRole\":759150000");
        }

        [Fact]
        public void GenerateCaseCreationInstructionsJson_Organisation_LandOwner_No()
        {
            DeveloperRegistration developerRegistration = new()
            {
                IsLandownerLeaseholder = "no",
                GainSite = new GainSite
                {
                    OffSiteUnitChange = new OffSiteUnitChange()
                    {
                        Habitat = 29.8M,
                        Hedge = 3435M,
                        Watercourse = 80986M
                    }
                },
                Applicant = new()
                {
                    Role = "organisation"
                },
                Organisation = new() { Id = Guid.NewGuid().ToString() }
            };

            EntityReference clientReference = new(Contact.EntityLogicalName, Guid.NewGuid());
            Guid gainSiteId = Guid.NewGuid();
            List<Guid> allocatedHabitatIds = [Guid.NewGuid(), Guid.NewGuid()];

            var actual = DeveloperRegistrationFacade.GenerateCaseCreationInstructionsJson(developerRegistration, clientReference, gainSiteId, allocatedHabitatIds, bng_casetype.Allocation);

            actual.Should().NotBeNull();
            actual.Should().Contain($"\"applicantRole\":759150002");
            actual.Should().Contain($"\"organisationId\":\"{developerRegistration.Organisation.Id}\"");
        }



        private static DeveloperRequestPayload CreateSamplePayload()
        {
            return new DeveloperRequestPayload
            {
                DeveloperRegistration = new DeveloperRegistration
                {
                    Applicant = new Applicant
                    {
                        LastName = "Application Test",
                        Role = "TestUser",
                        Emailaddress = "test@testing.com"
                    },
                    Files =
                    [
                        new FileDetails
                        {
                            FileName = "PDFFile.pdf",
                            ContentMediaType = "pdf",
                            FileLocation = "PDFFile.pdf"
                        }
                    ]
                }
            };
        }
    }
}