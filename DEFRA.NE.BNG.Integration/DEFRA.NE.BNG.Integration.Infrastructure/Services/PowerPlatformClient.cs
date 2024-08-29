using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using Microsoft.PowerPlatform.Dataverse.Client;
using System.Net;

namespace DEFRA.NE.BNG.Integration.Infrastructure.Services
{
    public class PowerPlatformClient : IPowerPlatformClient
    {
        private IOrganizationServiceAsync2 serviceClient;
        private static PowerPlatformClient powerPlatformClient;

        public static PowerPlatformClient PowerPlatformClientInstance
        {
            get
            {
                if (powerPlatformClient == null)
                {
                    return powerPlatformClient = new PowerPlatformClient();
                }
                else
                {
                    return powerPlatformClient;
                }
            }
        }

        private PowerPlatformClient()
        {

        }

        /// <summary>
        /// Provide service client for Dataverse
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public IOrganizationServiceAsync2 GetServiceClient(string connectionString)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            if (serviceClient != null)
            {
                return serviceClient;
            }

            serviceClient = new ServiceClient(connectionString);

            return serviceClient;
        }
    }
}
