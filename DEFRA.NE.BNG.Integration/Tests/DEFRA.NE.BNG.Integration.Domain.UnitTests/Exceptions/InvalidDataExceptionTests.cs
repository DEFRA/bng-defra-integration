using DEFRA.NE.BNG.Integration.Domain.Response;
using Newtonsoft.Json;

namespace DEFRA.NE.BNG.Integration.Domain.Exceptions.Tests
{
    public class InvalidDataExceptionTests
    {
        [Fact]
        public void CanInstatiate_InvalidDataException_WithNoParameters()
        {
            var actual = new InvalidDataException();

            actual.Should().NotBeNull();
        }

        [Fact]
        public void CanInstatiate_InvalidDataException_Message_ErrorList()
        {
            var errorList = new ErrorList
            {
                StatusCode = "2034"
            };
            errorList.Errors.Add(new Error { Message = "Error1Message", error = "Error1" });
            errorList.Errors.Add(new Error { Message = "Error2Message", error = "Error2" });
            string message = JsonConvert.SerializeObject(errorList);


            var actual = new InvalidDataException(message);

            actual.CustomErrorMessasge.Should().ContainAll("Error1Message", "Error1", "Error2Message", "Error2");
        }

        [Fact]
        public void CanInstatiate_InvalidDataException_MessageIsNotvalidJSON()
        {
            string message = "MailNotificationFaile";

            FluentActions.Invoking(() => new InvalidDataException(message))
                         .Should()
                         .Throw<JsonReaderException>()
                         .WithMessage("Unexpected character encountered while parsing value*");
        }

        [Fact]
        public void CanInstatiate_InvalidDataException_MessageAndException()
        {
            var errorList = new ErrorList
            {
                StatusCode = "2034"
            };
            errorList.Errors.Add(new Error { Message = "Error1Message", error = "Error1" });
            errorList.Errors.Add(new Error { Message = "Error2Message", error = "Error2" });
            string message = JsonConvert.SerializeObject(errorList);

            var expected = $"Error1{Environment.NewLine}Error1Message{Environment.NewLine}Error2{Environment.NewLine}Error2Message{Environment.NewLine}";

            var innerExceptionMessage = "SampleInner";
            var innerException = new Exception(innerExceptionMessage);

            var actual = new InvalidDataException(message, innerException);


            actual.CustomErrorMessasge.Should().ContainAll("Error1Message", "Error1", "Error2Message", "Error2");
            actual.InnerException.Message.Should().Be(innerExceptionMessage);
        }

    }
}