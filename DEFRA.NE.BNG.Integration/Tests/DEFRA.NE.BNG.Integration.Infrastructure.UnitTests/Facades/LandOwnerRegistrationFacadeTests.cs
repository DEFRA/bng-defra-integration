using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.JsonRequestRoot;
using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Infrastructure.Facades;
using DEFRA.NE.BNG.Integration.Model.Request;
using Newtonsoft.Json;

namespace DEFRA.NE.BNG.Integration.Infrastructure.UnitTests.Facades
{
    public class LandOwnerRegistrationFacadeTests : TestBase<LandOwnerRegistrationFacade>
    {
        private LandOwnerRegistrationFacade systemUnderTest;

        public LandOwnerRegistrationFacadeTests()
        {
            systemUnderTest = new LandOwnerRegistrationFacade(logger.Object, environmentVariableReader.Object, mailService.Object, dataverseService.Object, mappingManager.Object);
        }

        [Fact]
        public async Task CreateUpdateGainSiteRegistration()
        {
            var data = await CreateSamplePayload();

            var entityCollection = new EntityCollection();

            dataverseService.Setup(x => x.RetrieveMultipleAsync(It.IsAny<QueryExpression>()))
                    .ReturnsAsync(entityCollection);

            var actual = await systemUnderTest.CreateGainSiteRegistration(data.LandownerGainSiteRegistration, new EntityReference(), new EntityReference(), new Entity(), bng_casetype.Registration);

            actual.Should().BeEmpty();
        }

        [Fact]
        public async Task CreateLegalAgreementParty()
        {
            var data = await CreateSamplePayload();

            data.LandownerGainSiteRegistration.LegalAgreementParties =
            [
                 new() {Name="LegalAgreementParty1",Role ="Agent"},
                 new() {Name="LegalAgreementParty2",Role ="Agent"}
            ];

            dataverseService.Setup(x => x.CreateAsync(It.IsAny<Entity>()))
                            .ReturnsAsync(Guid.NewGuid());

            var actual = await systemUnderTest.CreateLegalAgreementParties(data.LandownerGainSiteRegistration.LegalAgreementParties, Guid.NewGuid());

            actual.Should().NotBeEmpty();
            dataverseService.VerifyAll();
        }

        [Fact]
        public async Task CreateLegalAgreementParty_ThrowsException()
        {
            var data = await CreateSamplePayload();

            data.LandownerGainSiteRegistration.LegalAgreementParties =
            [
                 new() {Name="LegalAgreementParty1",Role ="Agent"},
                 new() {Name="LegalAgreementParty2",Role ="Agent"}
            ];

            dataverseService.Setup(x => x.CreateAsync(It.IsAny<Entity>()))
                            .Throws<Exception>();

            var actual = await systemUnderTest.CreateLegalAgreementParties(data.LandownerGainSiteRegistration.LegalAgreementParties, Guid.NewGuid());

            actual.Should().BeEmpty();

            dataverseService.VerifyAll();
        }

        [Fact]
        public void GenerateJsonForCaseCreation_ApplicantIsNull()
        {
            var gainSiteRegistration = new GainSiteRegistration
            {
            };
            var clientReference = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid());

            var actual = LandOwnerRegistrationFacade.GenerateJsonForCaseCreation(gainSiteRegistration, clientReference, new Entity());

            actual.Should().BeEmpty();
        }

        [Fact]
        public void GenerateJsonForCaseCreation_ApplicantRoleIsNull()
        {
            var gainSiteRegistration = new GainSiteRegistration
            {
                Applicant = new Applicant { }
            };
            var clientReference = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid());

            var actual = LandOwnerRegistrationFacade.GenerateJsonForCaseCreation(gainSiteRegistration, clientReference, new Entity());

            actual.Should().BeEmpty();
        }

        [Fact]
        public void GenerateJsonForCaseCreation_ApplicantRoleIsNotAgent()
        {
            var gainSiteRegistration = new GainSiteRegistration
            {
                Applicant = new Applicant { Role = "" }
            };
            var clientReference = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid());

            var actual = LandOwnerRegistrationFacade.GenerateJsonForCaseCreation(gainSiteRegistration, clientReference, new Entity());

            actual.Should().Be("{}");
        }

        [Fact]
        public void GenerateJsonForCaseCreation_ApplicantRoleIsAgent()
        {
            var gainSiteRegistration = new GainSiteRegistration
            {
                Applicant = new Applicant { Role = "agent" }
            };
            var clientReference = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid());

            var actual = LandOwnerRegistrationFacade.GenerateJsonForCaseCreation(gainSiteRegistration, clientReference, new Entity());

            actual.Should().Be("{}");
        }

        [Fact]
        public void GenerateJsonForCaseCreation_ApplicantRoleIsAgent_AgentIsNull()
        {
            var gainSiteRegistration = new GainSiteRegistration
            {
                Applicant = new Applicant { Role = "agent" }
            };
            var clientReference = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid());

            var actual = LandOwnerRegistrationFacade.GenerateJsonForCaseCreation(gainSiteRegistration, clientReference, new Entity());

            actual.Should().Be("{}");
        }

        [Fact]
        public void GenerateJsonForCaseCreation_ApplicantRoleIsAgent_AgentIsNotNull()
        {
            var gainSiteRegistration = new GainSiteRegistration
            {
                Applicant = new Applicant { Role = "agent" },
                Agent = new Agent
                {
                    ClientType = "individual",
                    ClientEmail = "testemail",
                    ClientNameIndividual = new ClientNameIndividual
                    {
                        FirstName = "John",
                        LastName = "Smith"
                    },
                    ClientPhoneNumber = "932572390573290",
                    ClientAddress = new ClientAddress
                    {
                        Line1 = "Line1",
                        Line2 = "Line 2",
                        Line3 = "Line 3",
                        Country = "Uk",
                        County = "London",
                        Postcode = "SW1 2EA",
                        Town = "London Town",
                        Type = "international"
                    }
                }
            };
            var clientReference = new EntityReference(Contact.EntityLogicalName, Guid.NewGuid());

            var actual = LandOwnerRegistrationFacade.GenerateJsonForCaseCreation(gainSiteRegistration, clientReference, new Entity());

            actual.Should().NotBeEmpty();
        }

        [Fact]
        public void OrchestrationBNG_ApplicantNotFoundInPayload()
        {
            GainSiteRegistration gainSiteRegistration = new();

            dataverseService.Setup(x => x.RetrieveFirstRecordForEntity(It.IsAny<QueryExpression>()));

            FluentActions.Invoking(() => systemUnderTest.OrchestrationBNG(gainSiteRegistration, bng_casetype.Registration))
                         .Should()
                         .ThrowAsync<InvalidDataException>()
                         .WithMessage("No valid applicant was supplied in payload!");

            dataverseService.Verify(x => x.RetrieveFirstRecordForEntity(It.IsAny<QueryExpression>()), Times.Never);
        }

        [Fact]
        public void OrchestrationBNG_ApplicantNotFound()
        {
            GainSiteRegistration gainSiteRegistration = new()
            {
                Applicant = new Applicant { Id = Guid.NewGuid().ToString() }
            };
            Guid applicantId = Guid.Empty;

            dataverseService.Setup(x => x.RetrieveFirstRecordForEntity(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(applicantId);

            FluentActions.Invoking(() => systemUnderTest.OrchestrationBNG(gainSiteRegistration, bng_casetype.Registration))
                         .Should()
                         .ThrowAsync<InvalidDataException>()
                         .WithMessage("Applicant does not exist!");

            dataverseService.Verify(x => x.RetrieveFirstRecordForEntity(It.IsAny<QueryExpression>()), Times.Once());
        }

        [Fact]
        public async Task OrchestrationBNG_Agent_Individuals()
        {
            var payload = await HydrateFromTestDataFile<LandOwnerRequestPayload>("TestData/registration/agent-applyingfor-individual.json");
            Guid applicantId = Guid.NewGuid();

            dataverseService.Setup(x => x.UpsertClient(It.IsAny<Agent>()))
                            .ReturnsAsync(new EntityReference("contact", applicantId));

            dataverseService.Setup(x => x.RetrieveFirstRecordForEntity(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(applicantId);

            dataverseService.Setup(x => x.CreateAsync(It.IsAny<Entity>())).ReturnsAsync(Guid.NewGuid());

            var actual = await systemUnderTest.OrchestrationBNG(payload.LandownerGainSiteRegistration, bng_casetype.Registration);

            actual.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public async Task OrchestrationBNG_Agent_Organisation()
        {
            var payload = await HydrateFromTestDataFile<LandOwnerRequestPayload>("TestData/registration/agent-applyingfor-organisation.json");
            Guid applicantId = Guid.NewGuid();

            dataverseService.Setup(x => x.UpsertClient(It.IsAny<Agent>()))
                            .ReturnsAsync(new EntityReference("contact", applicantId));

            dataverseService.Setup(x => x.RetrieveFirstRecordForEntity(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(applicantId);

            dataverseService.Setup(x => x.CreateAsync(It.IsAny<Entity>())).ReturnsAsync(Guid.NewGuid());

            var actual = await systemUnderTest.OrchestrationBNG(payload.LandownerGainSiteRegistration, bng_casetype.Registration);

            actual.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public async Task OrchestrationBNG_Landowner()
        {
            var payload = await HydrateFromTestDataFile<LandOwnerRequestPayload>("TestData/registration/registration.example-landowner.json");
            Guid applicantId = Guid.NewGuid();

            dataverseService.Setup(x => x.RetrieveFirstRecordForEntity(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(applicantId);

            dataverseService.Setup(x => x.CreateAsync(It.IsAny<Entity>())).ReturnsAsync(Guid.NewGuid());

            var actual = await systemUnderTest.OrchestrationBNG(payload.LandownerGainSiteRegistration, bng_casetype.Registration);

            actual.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public async Task OrchestrationBNG_Organisation()
        {
            var payload = await HydrateFromTestDataFile<LandOwnerRequestPayload>("TestData/registration/registration.example-organisation.json");
            Guid applicantId = Guid.NewGuid();
            Account organisationEntity = new();

            dataverseService.Setup(x => x.RetrieveFirstRecordForEntity(It.IsAny<QueryExpression>()))
            .ReturnsAsync(applicantId);

            dataverseService.Setup(x => x.RetrieveFirstRecord<Account>(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(organisationEntity);

            dataverseService.Setup(x => x.CreateAsync(It.IsAny<Entity>())).ReturnsAsync(Guid.NewGuid());

            var actual = await systemUnderTest.OrchestrationBNG(payload.LandownerGainSiteRegistration, bng_casetype.Registration);

            actual.Should().NotBe(Guid.Empty);

            dataverseService.VerifyAll();
        }

        [Fact]
        public async Task OrchestrationBNG_ConservationCovernantResponsibleBodies_DoesNotExist()
        {
            var payload = await HydrateFromTestDataFile<LandOwnerRequestPayload>("TestData/registration/conservationCovernantResponsibleBodies.json");
            Guid applicantId = Guid.NewGuid();
            Account organisationEntity = new();

            dataverseService.Setup(x => x.RetrieveFirstRecordForEntity(It.IsAny<QueryExpression>()))
            .ReturnsAsync(applicantId);

            dataverseService.Setup(x => x.RetrieveFirstRecord<Account>(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(organisationEntity);

            EntityCollection responsibleBodyCollection = new();

            dataverseService.Setup(x => x.CreateAsync(It.IsAny<Entity>())).ReturnsAsync(Guid.NewGuid());
            dataverseService.Setup(x => x.RetrieveMultipleAsync(It.Is<QueryExpression>(a => a.EntityName == bng_ResponsibleBody.EntityLogicalName)))
                            .ReturnsAsync(responsibleBodyCollection);

            var actual = await systemUnderTest.OrchestrationBNG(payload.LandownerGainSiteRegistration, bng_casetype.Registration);

            actual.Should().NotBe(Guid.Empty);

            dataverseService.Verify(x => x.CreateAttachments(It.IsAny<List<FileDetails>>(), It.IsAny<EntityReference>()), Times.AtLeastOnce);
            dataverseService.Verify(x => x.CreateAsync(It.Is<Entity>(a => a.LogicalName == bng_GainSiteRegistration.EntityLogicalName)), Times.Once);

            dataverseService.Verify(x => x.RetrieveMultipleAsync(It.Is<QueryExpression>(a => a.EntityName == bng_ResponsibleBody.EntityLogicalName)), Times.Exactly(2));
            dataverseService.Verify(x => x.CreateAsync(It.Is<Entity>(a => a.LogicalName == bng_ResponsibleBody.EntityLogicalName)), Times.Exactly(2));
            dataverseService.Verify(x => x.AssosiateTwoEntitiesRecords(
                                    It.Is<EntityReference>(a => a.LogicalName == bng_GainSiteRegistration.EntityLogicalName), It.Is<string>(a => a == "bng_ResponsibleBody_bng_GainSiteRegistrat"),
                                                    It.IsAny<List<EntityReference>>()), Times.Once);

            dataverseService.VerifyAll();
        }

        [Fact]
        public async Task OrchestrationBNG_ConservationCovernantResponsibleBodies_AlreadyExists()
        {
            var payload = await HydrateFromTestDataFile<LandOwnerRequestPayload>("TestData/registration/conservationCovernantResponsibleBodies.json");
            Guid applicantId = Guid.NewGuid();
            Account organisationEntity = new();

            dataverseService.Setup(x => x.RetrieveFirstRecordForEntity(It.IsAny<QueryExpression>()))
            .ReturnsAsync(applicantId);

            dataverseService.Setup(x => x.RetrieveFirstRecord<Account>(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(organisationEntity);

            EntityCollection responsibleBodyCollection = new();
            for (int i = 0; i < 2; i++)
            {
                responsibleBodyCollection.Entities.Add(new bng_ResponsibleBody() { Id = Guid.NewGuid() });
            }

            dataverseService.Setup(x => x.CreateAsync(It.IsAny<Entity>())).ReturnsAsync(Guid.NewGuid());
            dataverseService.Setup(x => x.RetrieveMultipleAsync(It.Is<QueryExpression>(a => a.EntityName == bng_ResponsibleBody.EntityLogicalName)))
                            .ReturnsAsync(responsibleBodyCollection);

            var actual = await systemUnderTest.OrchestrationBNG(payload.LandownerGainSiteRegistration, bng_casetype.Registration);

            actual.Should().NotBe(Guid.Empty);

            dataverseService.Verify(x => x.CreateAttachments(It.IsAny<List<FileDetails>>(), It.IsAny<EntityReference>()), Times.AtLeastOnce);
            dataverseService.Verify(x => x.CreateAsync(It.Is<Entity>(a => a.LogicalName == bng_GainSiteRegistration.EntityLogicalName)), Times.Once);

            dataverseService.Verify(x => x.RetrieveMultipleAsync(It.Is<QueryExpression>(a => a.EntityName == bng_ResponsibleBody.EntityLogicalName)), Times.Exactly(2));
            dataverseService.Verify(x => x.CreateAsync(It.Is<Entity>(a => a.LogicalName == bng_ResponsibleBody.EntityLogicalName)), Times.Never);
            dataverseService.Verify(x => x.AssosiateTwoEntitiesRecords(
                                    It.Is<EntityReference>(a => a.LogicalName == bng_GainSiteRegistration.EntityLogicalName), It.Is<string>(a => a == "bng_ResponsibleBody_bng_GainSiteRegistrat"),
                                                    It.IsAny<List<EntityReference>>()), Times.Once);

            dataverseService.VerifyAll();
        }

        [Fact]
        public async Task OrchestrationBNG_PlanningObligationLPAs_DoesNotExist()
        {
            var payload = await HydrateFromTestDataFile<LandOwnerRequestPayload>("TestData/registration/conservationCovernantResponsibleBodies.json");
            payload.LandownerGainSiteRegistration.ConservationCovernantResponsibleBodies = null;
            payload.LandownerGainSiteRegistration.LegalAgreementType = ((int)bng_legalagreementtype.PlanningObligation).ToString();
            payload.LandownerGainSiteRegistration.PlanningObligationLPAs =

                                                        [
                                                            new PlanningObligationLpa()
                                                            {
                                                                LpaId = "Test",
                                                                LpaName = "Test"
                                                            }
                                                        ];
            Guid applicantId = Guid.NewGuid();
            Account organisationEntity = new();


            dataverseService.Setup(x => x.RetrieveFirstRecordForEntity(It.IsAny<QueryExpression>()))
            .ReturnsAsync(applicantId);

            dataverseService.Setup(x => x.RetrieveFirstRecord<Account>(It.IsAny<QueryExpression>()))
                            .ReturnsAsync(organisationEntity);

            EntityCollection lpaCollection = new();

            dataverseService.Setup(x => x.CreateAsync(It.IsAny<Entity>())).ReturnsAsync(Guid.NewGuid());
            dataverseService.Setup(x => x.RetrieveMultipleAsync(It.Is<QueryExpression>(a => a.EntityName == bng_LocalPlanningAuthority.EntityLogicalName)))
                            .ReturnsAsync(lpaCollection);

            var actual = await systemUnderTest.OrchestrationBNG(payload.LandownerGainSiteRegistration, bng_casetype.Registration);

            actual.Should().NotBe(Guid.Empty);

            dataverseService.Verify(x => x.CreateAttachments(It.IsAny<List<FileDetails>>(), It.IsAny<EntityReference>()), Times.AtLeastOnce);
            dataverseService.Verify(x => x.CreateAsync(It.Is<Entity>(a => a.LogicalName == bng_GainSiteRegistration.EntityLogicalName)), Times.Once);

            dataverseService.Verify(x => x.RetrieveMultipleAsync(It.Is<QueryExpression>(a => a.EntityName == bng_ResponsibleBody.EntityLogicalName)), Times.Never);
            dataverseService.Verify(x => x.CreateAsync(It.Is<Entity>(a => a.LogicalName == bng_ResponsibleBody.EntityLogicalName)), Times.Never);
            dataverseService.Verify(x => x.AssosiateTwoEntitiesRecords(
                                    It.Is<EntityReference>(a => a.LogicalName == bng_GainSiteRegistration.EntityLogicalName), It.Is<string>(a => a == DataverseExtensions.GetRelationshipSchemaName<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.bng_ResponsibleBody_bng_GainSiteRegistrat))),
                                                    It.IsAny<List<EntityReference>>()), Times.Never);

            dataverseService.Verify(x => x.RetrieveMultipleAsync(It.Is<QueryExpression>(a => a.EntityName == bng_LocalPlanningAuthority.EntityLogicalName)), Times.Exactly(payload.LandownerGainSiteRegistration.PlanningObligationLPAs.Count));

            dataverseService.Verify(x => x.AssosiateTwoEntitiesRecords(
                                    It.Is<EntityReference>(a => a.LogicalName == bng_GainSiteRegistration.EntityLogicalName), It.Is<string>(a => a == DataverseExtensions.GetRelationshipSchemaName<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.bng_GainSiteRegistration_bng_LocalPlanning))),
                                                    It.IsAny<List<EntityReference>>()), Times.Once);
            dataverseService.VerifyAll();
        }

        [Fact]
        public async Task CreateOtherLandOwners_NoOtherLandOwner()
        {
            List<OtherLandOwner> otherLandOwners = [];
            Guid gainSiteRegistrationId = Guid.NewGuid();

            var actual = await systemUnderTest.CreateOtherLandOwners(otherLandOwners, gainSiteRegistrationId);

            dataverseService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Never);
        }


        [Fact]
        public async Task CreateOtherLandOwners_WithOtherLandOwners()
        {
            Guid gainSiteRegistrationId = Guid.NewGuid();
            List<OtherLandOwner> otherLandOwners = [
                                                    new OtherLandOwner() { Name = "Name1" },
                                                    new OtherLandOwner() { Name = "Name2" }
                                                   ];

            var actual = await systemUnderTest.CreateOtherLandOwners(otherLandOwners, gainSiteRegistrationId);

            dataverseService.Verify(x => x.CreateAsync(It.IsAny<Entity>()), Times.Exactly(otherLandOwners.Count));
            dataverseService.VerifyAll();
        }

        [Fact]
        public void CreateLandOwners_EmptyLandOwners()
        {
            LandOwners landOwners = new();
            Guid gainSiteRegistrationId = Guid.NewGuid();

            FluentActions.Invoking(() => systemUnderTest.CreateLandOwners(landOwners, gainSiteRegistrationId))
             .Should()
             .NotThrowAsync();

            dataverseService.Verify(x => x.AssosiateTwoEntitiesRecords(It.IsAny<EntityReference>(),
                                                                   It.IsAny<string>(),
                                                                   It.IsAny<List<EntityReference>>()), Times.Never);
            dataverseService.VerifyAll();
        }

        [Fact]
        public void CreateLandOwners_IndividualLandOwners()
        {
            LandOwners landOwners = new()
            {
                Individual = [
                                new Individual{ Firstname="Firstname1"},
                                new Individual{ Firstname="Firstname2"},
                             ]
            };
            Guid gainSiteRegistrationId = Guid.NewGuid();

            dataverseService.Setup(x => x.AssosiateTwoEntitiesRecords(It.IsAny<EntityReference>(),
                                                                    It.IsAny<string>(),
                                                                    It.IsAny<List<EntityReference>>()));

            FluentActions.Invoking(() => systemUnderTest.CreateLandOwners(landOwners, gainSiteRegistrationId))
                         .Should()
                         .NotThrowAsync();

            dataverseService.Verify(x => x.AssosiateTwoEntitiesRecords(It.IsAny<EntityReference>(),
                                                                   It.IsAny<string>(),
                                                                   It.IsAny<List<EntityReference>>()), Times.Once());
            dataverseService.VerifyAll();
        }


        [Fact]
        public void CreateLandOwners_OrganisationLandOwners()
        {
            LandOwners landOwners = new()
            {
                Organisation = [
                 new Organisation{ Id=Guid.NewGuid().ToString(), Name="Org1"  },
                 new Organisation{ Id=Guid.NewGuid().ToString(), Name="Org2"  }
                ]
            };
            Guid gainSiteRegistrationId = Guid.NewGuid();

            dataverseService.Setup(x => x.AssosiateTwoEntitiesRecords(It.IsAny<EntityReference>(),
                                                                    It.IsAny<string>(),
                                                                    It.IsAny<List<EntityReference>>()));

            FluentActions.Invoking(() => systemUnderTest.CreateLandOwners(landOwners, gainSiteRegistrationId))
                         .Should()
                         .NotThrowAsync();

            dataverseService.Verify(x => x.AssosiateTwoEntitiesRecords(It.IsAny<EntityReference>(),
                                                                   It.IsAny<string>(),
                                                                   It.IsAny<List<EntityReference>>()), Times.Once());
            dataverseService.VerifyAll();
        }


        private static async Task<LandOwnerRequestPayload> CreateSamplePayload()
        {
            var json = await File.ReadAllTextAsync("TestData/registration/agent-applyingfor-individual.json");
            var payload = JsonConvert.DeserializeObject<LandOwnerRequestPayload>(json);

            return payload;
        }

        private void ValidateOrganisation(Times times)
        {
            dataverseService.Verify(x => x.RetrieveFirstRecord<Account>(It.IsAny<QueryExpression>()), times);
            dataverseService.Verify(x => x.CreateAsync(It.Is<Entity>(a => a.LogicalName == Account.EntityLogicalName)), times);

            dataverseService.Verify(x => x.AssosiateTwoEntitiesRecords(
                It.Is<EntityReference>(a => a.LogicalName == bng_GainSiteRegistration.EntityLogicalName),
                It.Is<string>(a => a == DataverseExtensions.GetRelationshipSchemaName<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.bng_GainSiteRegistration_Account_Account))),
                It.IsAny<List<EntityReference>>()), Times.Once);
        }

        private void ValidateFiles(Times times)
        {
            dataverseService.Verify(x => x.CreateAttachments(It.IsAny<List<FileDetails>>(), It.Is<EntityReference>(a => a.LogicalName == bng_GainSiteRegistration.EntityLogicalName)), times);
        }

        private void ValidateAgent()
        {
            dataverseService.Verify(x => x.UpsertClient(It.IsAny<Agent>()), Times.Once);

            dataverseService.Verify(x => x.AssosiateTwoEntitiesRecords(
                                             It.Is<EntityReference>(a => a.LogicalName == bng_GainSiteRegistration.EntityLogicalName),
                                             It.Is<string>(a => a ==
                                                                      DataverseExtensions.GetRelationshipSchemaName<bng_GainSiteRegistration>(
                                                                          nameof(bng_GainSiteRegistration.bng_GainSiteRegistration_Contact_Contact))),
                                        It.IsAny<List<EntityReference>>()), Times.Once);

            dataverseService.Verify(x => x.AssosiateTwoEntitiesRecords(
                                    It.Is<EntityReference>(a => a.LogicalName == bng_GainSiteRegistration.EntityLogicalName),
                                    It.Is<string>(a => a == DataverseExtensions.GetRelationshipSchemaName<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.bng_GainSiteRegistration_Account_Account))),
                                  It.IsAny<List<EntityReference>>()), Times.Once);
        }

        private void ValidateLpa(Times times)
        {
            dataverseService.Verify(x => x.RetrieveMultipleAsync(It.Is<QueryExpression>(a => a.EntityName == bng_LocalPlanningAuthority.EntityLogicalName)), times);
            dataverseService.Verify(x => x.AssosiateTwoEntitiesRecords(It.Is<EntityReference>(a => a.LogicalName == bng_GainSiteRegistration.EntityLogicalName),
                                                                       It.Is<string>(a => a == DataverseExtensions.
                                                                                    GetRelationshipSchemaName<bng_GainSiteRegistration>(
                                                                            nameof(
                                                                            bng_GainSiteRegistration.bng_GainSiteRegistration_bng_LocalPlanning
                                                                            ))),
                                                                        It.IsAny<List<EntityReference>>()), times);
        }

        private void CovenantBodies()
        {
            dataverseService.Verify(x => x.RetrieveMultipleAsync(It.Is<QueryExpression>(a => a.EntityName == bng_ResponsibleBody.EntityLogicalName)), Times.AtLeastOnce);
            dataverseService.Verify(x => x.CreateAsync(It.Is<Entity>(a => a.LogicalName == Account.EntityLogicalName)), Times.AtLeastOnce);
            dataverseService.Verify(x => x.AssosiateTwoEntitiesRecords(
                                           It.Is<EntityReference>(a => a.LogicalName == bng_GainSiteRegistration.EntityLogicalName),
                                           It.Is<string>(a => a == DataverseExtensions.GetRelationshipSchemaName<bng_GainSiteRegistration>(nameof(
                                                       bng_GainSiteRegistration.bng_ResponsibleBody_bng_GainSiteRegistrat))),
                                           It.IsAny<List<EntityReference>>()), Times.Once);
        }

        private void ValidateDataverseService()
        {
            dataverseService.Verify(x => x.RetrieveFirstRecordForEntity(It.Is<QueryExpression>(a => a.EntityName == Contact.EntityLogicalName)), Times.Once);

            dataverseService.Verify(x => x.RetrieveFirstRecordForEntity(It.Is<QueryExpression>(a => a.EntityName == bng_Case.EntityLogicalName)), Times.Once);

            dataverseService.Verify(x => x.CreateAsync(It.Is<Entity>(a => a.LogicalName == bng_GainSiteRegistration.EntityLogicalName)), Times.Once);

            dataverseService.Verify(x => x.AssosiateTwoEntitiesRecords(
                                It.Is<EntityReference>(a => a.LogicalName == bng_GainSiteRegistration.EntityLogicalName),
                                It.Is<string>(a => a == DataverseExtensions.GetRelationshipSchemaName<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.bng_GainSiteRegistration_Contact_Contact))),
                                It.IsAny<List<EntityReference>>()), Times.Once);

            dataverseService.Verify(x => x.AssosiateTwoEntitiesRecords(
                    It.Is<EntityReference>(a => a.LogicalName == bng_GainSiteRegistration.EntityLogicalName),
                    It.Is<string>(a => a == DataverseExtensions.GetRelationshipSchemaName<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.bng_GainSiteRegistration_Contact_Contact))),
                    It.IsAny<List<EntityReference>>()), Times.Once);

            dataverseService.Verify(x => x.AssosiateTwoEntitiesRecords(
                      It.Is<EntityReference>(a => a.LogicalName == bng_GainSiteRegistration.EntityLogicalName),
                      It.Is<string>(a => a == DataverseExtensions.GetRelationshipSchemaName<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.bng_GainSiteRegistration_Account_Account))),
                      It.IsAny<List<EntityReference>>()), Times.Once);

            dataverseService.Verify(x => x.UpdateAsync(It.Is<Entity>(a => a.LogicalName == bng_GainSiteRegistration.EntityLogicalName)));
        }
    }
}