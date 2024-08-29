using DEFRA.NE.BNG.Integration.Domain.Request;
using System.Text;

namespace DEFRA.NE.BNG.Integration.Infrastructure.Utilities
{
    public class StringHelper
    {
        public static string ConcatenateAddressLines(ClientAddress clientAddress)
        {
            var addressBuilder = new StringBuilder();
            if (clientAddress != null)
            {
                if (clientAddress.Line1 != null)
                {
                    addressBuilder.Append(clientAddress.Line1);
                }

                if (clientAddress.Line2 != null)
                {
                    addressBuilder.Append($", {clientAddress.Line2}");
                }

                if (clientAddress.Line3 != null)
                {
                    addressBuilder.Append($", {clientAddress.Line3}");
                }
            }
            return addressBuilder.ToString();
        }

        public static string GenerateJsonAttribute(string attributeName, string attributeValue)
        {
            var result = new StringBuilder();

            if (!string.IsNullOrEmpty(attributeValue))
            {
                result.Append($"\"{attributeName}\":\"{attributeValue}\"");
            }
            else
            {
                result.Append($"\"{attributeName}\":null");
            }

            return result.ToString();
        }
    }
}