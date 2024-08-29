using DEFRA.NE.BNG.Integration.Domain.Request;

namespace DEFRA.NE.BNG.Integration.Domain.Entities
{
    public class EmailNotificationRequestWrapper
    {
        public int ActionType { get; set; }
        public EmailNotificationRequest EmailNotificationRequest { get; set; }
    }
}
