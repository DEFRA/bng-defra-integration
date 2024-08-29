using DEFRA.NE.BNG.Integration.Domain.Response;
using Newtonsoft.Json;

namespace DEFRA.NE.BNG.Integration.Domain.Exceptions
{
    public class InvalidDataException : Exception
    {
        public string CustomErrorMessasge { get; private set; }

        public InvalidDataException() : base()
        {
        }

        public InvalidDataException(string message) : base(message)
        {
            CustomErrorMessasge = CreateCustomMessage(message);
        }

        public InvalidDataException(string message, Exception inner) : base(message, inner)
        {
            CustomErrorMessasge = CreateCustomMessage(message);
        }

        private static string CreateCustomMessage(string message)
        {
            string buildError = string.Empty;
            var errors = JsonConvert.DeserializeObject<ErrorList>(message);
            if (errors?.Errors != null)
            {
                foreach (var error in errors.Errors)
                {
                    buildError = $"{buildError}{error.error}{Environment.NewLine}{error.Message}{Environment.NewLine}";
                }
            }
            return buildError;
        }
    }
}