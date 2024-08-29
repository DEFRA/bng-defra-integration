using DEFRA.NE.BNG.Integration.Function.Functions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Response = DEFRA.NE.BNG.Integration.Domain.Response;

namespace DEFRA.NE.BNG.Integration.Function.UnitTests.Functions
{
    public class BNGOperatorGainsiteValidatorTests : TestBase
    {
        private readonly BNGOperatorGainsiteValidator systemUnderTest;
        private readonly Mock<ILogger<BNGOperatorGainsiteValidator>> logger;
        private readonly Mock<IGainsiteValidatorFacade> facade;
        private string gainsiteNumber;
        private Response.GainsiteRegistrationIdValidation gainsiteValidation;

        public BNGOperatorGainsiteValidatorTests() : base()
        {
            gainsiteValidation = new Response.GainsiteRegistrationIdValidation();
            logger = new Mock<ILogger<BNGOperatorGainsiteValidator>>();
            facade = new Mock<IGainsiteValidatorFacade>();
            systemUnderTest = new BNGOperatorGainsiteValidator(logger.Object, facade.Object);
            gainsiteNumber = Guid.NewGuid().ToString();
            gainsiteValidation.gainsiteNumber = gainsiteNumber;
        }

        [Fact]
        public async Task Run_OkResult()
        {
            var req = new Mock<HttpRequest>();
            facade.Setup(x => x.ValidateGainsiteFromId(gainsiteNumber));
            var actual = await systemUnderTest.Run(req.Object, gainsiteNumber);
            actual.Should().BeEquivalentTo(new OkObjectResult(null));
            facade.Verify(x => x.ValidateGainsiteFromId(gainsiteNumber), Times.Once());
        }

        [Fact]
        public async Task Run_InternalServerError()
        {
            var req = new Mock<HttpRequest>();
            var actual = await systemUnderTest.Run(req.Object, "");
            actual.Should().BeEquivalentTo(new StatusCodeResult(StatusCodes.Status404NotFound));
        }
    }
}
