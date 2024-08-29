namespace DEFRA.NE.BNG.Integration.Domain.Response
{
    public class EmailNotificationResponse
    {
        public string Id { get; set; }
        public string Reference { get; set; }
        public string Uri { get; set; }
        public Template Template { get; set; }
        public EmailResponseContent Content { get; set; }
    }
}
