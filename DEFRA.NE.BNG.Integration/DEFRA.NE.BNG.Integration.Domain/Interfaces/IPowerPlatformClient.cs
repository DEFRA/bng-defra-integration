using Microsoft.PowerPlatform.Dataverse.Client;

namespace DEFRA.NE.BNG.Integration.Domain.Interfaces
{
    public interface IPowerPlatformClient
    {
        public IOrganizationServiceAsync2 GetServiceClient(string connectionString);
    }
}
