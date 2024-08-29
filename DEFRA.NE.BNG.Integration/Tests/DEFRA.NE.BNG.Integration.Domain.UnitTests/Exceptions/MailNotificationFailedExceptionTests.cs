using DEFRA.NE.BNG.Integration.Domain.Exceptions;
using DEFRA.NE.BNG.Integration.Domain.Response;
using Newtonsoft.Json;

namespace DEFRA.NE.BNG.Integration.BusinessLogic.Tests.Exceptions
{
    public class MailNotificationFailedExceptionTests
    {
        [Fact]
        public void CanInstatiate_MailNotificationFailedException_WithNoParameters()
        {
            var actual = new MailNotificationFailedException();

            actual.Should().NotBeNull();
        }

        [Fact]
        public void CanInstatiate_MailNotificationFailedException_Message_ErrorList()
        {
            var errorList = new ErrorList
            {
                StatusCode = "2034"
            };
            errorList.Errors.Add(new Error { Message = "Error1Message", error = "Error1" });
            errorList.Errors.Add(new Error { Message = "Error2Message", error = "Error2" });
            string message = JsonConvert.SerializeObject(errorList);

            var actual = new MailNotificationFailedException(message);

            actual.CustomErrorMessasge.Should().ContainAll("Error1Message", "Error1", "Error2Message", "Error2");
        }

        [Fact]
        public void CanInstatiate_MailNotificationFailedException_MessageIsNotvalidJSON()
        {
            string message = "MailNotificationFaile";

            FluentActions.Invoking(() => new MailNotificationFailedException(message))
                         .Should()
                         .Throw<JsonReaderException>()
                         .WithMessage("Unexpected character encountered while parsing value*");
        }

        [Fact]
        public void CanInstatiate_MailNotificationFailedException_MessageAndException()
        {
            var errorList = new ErrorList
            {
                StatusCode = "2034"
            };
            errorList.Errors.Add(new Error { Message = "Error1Message", error = "Error1" });
            errorList.Errors.Add(new Error { Message = "Error2Message", error = "Error2" });
            string message = JsonConvert.SerializeObject(errorList);

            var innerExceptionMessage = "SampleInner";
            var innerException = new Exception(innerExceptionMessage);

            var actual = new MailNotificationFailedException(message, innerException);

            actual.CustomErrorMessasge.Should().ContainAll("Error1Message", "Error1", "Error2Message", "Error2");
            actual.InnerException.Message.Should().Be(innerExceptionMessage);
        }
    }
}