using DEFRA.NE.BNG.Integration.Domain.Response;
using Newtonsoft.Json;

namespace DEFRA.NE.BNG.Integration.Domain.Exceptions.Tests
{
    public class DataNotFoundExceptionTests
    {
        [Fact]
        public void CanInstatiate_DataNotFoundException_WithNoParameters()
        {
            var actual = new DataNotFoundException();

            actual.Should().NotBeNull();
        }

        [Fact]
        public void CanInstatiate_DataNotFoundException_Message_ErrorList()
        {
            var errorList = new ErrorList
            {
                StatusCode = "2034"
            };
            errorList.Errors.Add(new Error { Message = "Error1Message", error = "Error1" });
            errorList.Errors.Add(new Error { Message = "Error2Message", error = "Error2" });
            string message = JsonConvert.SerializeObject(errorList);

            var actual = new DataNotFoundException(message);

            actual.CustomErrorMessasge.Should().ContainAll("Error1Message", "Error1", "Error2Message", "Error2");
        }


        [Fact]
        public void CanInstatiate_DataNotFoundException_MessageAndException()
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

            var actual = new DataNotFoundException(message, innerException);

            actual.CustomErrorMessasge.Should().ContainAll("Error1Message", "Error1", "Error2Message", "Error2");
            actual.InnerException.Message.Should().Be(innerExceptionMessage);
        }

    }
}