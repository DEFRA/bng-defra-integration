namespace DEFRA.NE.BNG.Integration.Infrastructure.Utilities.Tests
{
    public class StringHelperTests
    {
        [Fact]
        public void ConcatenateAddressLines_Full()
        {
            var clientAddress = new ClientAddress
            {
                Line1 = "line1",
                Line2 = "line2",
                Line3 = "line3"
            };

            var expected = $"{clientAddress.Line1}, {clientAddress.Line2}, {clientAddress.Line3}";

            var actual = StringHelper.ConcatenateAddressLines(clientAddress);

            actual.Should().Be(expected);
        }

        [Fact]
        public void ConcatenateAddressLines_NoLine2AndLine3()
        {
            var clientAddress = new ClientAddress
            {
                Line1 = "line1",
                Line2 = null,
                Line3 = null
            };

            var expected = $"{clientAddress.Line1}";

            var actual = StringHelper.ConcatenateAddressLines(clientAddress);

            actual.Should().Be(expected);
        }

        [Fact]
        public void ConcatenateAddressLines_NoLine3()
        {
            var clientAddress = new ClientAddress
            {
                Line1 = "line1",
                Line2 = "line2",
                Line3 = null
            };

            var expected = $"{clientAddress.Line1}, {clientAddress.Line2}";

            var actual = StringHelper.ConcatenateAddressLines(clientAddress);

            actual.Should().Be(expected);
        }

        [Fact()]
        public void GenerateJsonAttribute()
        {
            var attributeName = "agentId";
            var attributeValue = "47385627836572";

            var expected = $"\"{attributeName}\":\"{attributeValue}\"";

            var actual = StringHelper.GenerateJsonAttribute(attributeName, attributeValue);

            actual.Should().Be(expected);
        }


        [Fact()]
        public void GenerateJsonAttribute_AttributeValueIsNull()
        {
            var attributeName = "agentId";
            string attributeValue = null;

            var expected = $"\"{attributeName}\":null";

            var actual = StringHelper.GenerateJsonAttribute(attributeName, attributeValue);

            actual.Should().Be(expected);
        }
    }
}