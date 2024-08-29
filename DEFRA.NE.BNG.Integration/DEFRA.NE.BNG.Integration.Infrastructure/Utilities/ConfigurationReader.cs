using DEFRA.NE.BNG.Integration.Domain.Constants;
using DEFRA.NE.BNG.Integration.Domain.Interfaces;

namespace DEFRA.NE.BNG.Integration.Infrastructure.Utilities
{
    public class ConfigurationReader : IConfigurationReader
    {
        public string Read(string variableName)
        {
            var value = Environment.GetEnvironmentVariable(variableName);

            return value;
        }


        public string BuildDataVerseConnectionString()
        {
            var connectionString = @$"Url=https://{Read(EnvironmentConstants.environment)}.dynamics.com;AuthType=ClientSecret;ClientId={Read(EnvironmentConstants.clientId)};ClientSecret={Read(EnvironmentConstants.clientSecret)};RequireNewInstance=true";

            return connectionString;
        }
    }
}
