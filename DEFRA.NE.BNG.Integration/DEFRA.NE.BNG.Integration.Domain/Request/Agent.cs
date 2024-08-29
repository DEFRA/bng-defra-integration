namespace DEFRA.NE.BNG.Integration.Domain.Request
{
    public class Agent
    {
        public string ClientType { get; set; }
        public ClientNameIndividual ClientNameIndividual { get; set; }
        public string ClientNameOrganisation { get; set; }
        public string ClientPhoneNumber { get; set; }
        public string ClientEmail { get; set; }
        public ClientAddress ClientAddress { get; set; }
    }

}
