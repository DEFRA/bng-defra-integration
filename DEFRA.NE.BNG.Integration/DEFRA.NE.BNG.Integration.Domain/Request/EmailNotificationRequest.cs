namespace DEFRA.NE.BNG.Integration.Domain.Request
{
    public class EmailNotificationRequest
    {
        public IDictionary<string, string> personalisation { get; set; }
        public string email_address { get; set; }
        public string template_id { get; set; }
        public string email_reply_to_id { get; set; }
    }
}
