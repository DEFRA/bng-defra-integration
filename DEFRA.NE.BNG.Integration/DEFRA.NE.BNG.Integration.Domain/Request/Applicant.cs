namespace DEFRA.NE.BNG.Integration.Domain.Request
{
    public class Applicant
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string Emailaddress { get; set; }
        public string DateOfBirth { get; set; }
        public List<string> Nationality { get; set; }
        public List<Guid> NationalityIdList { get; set; }
    }
}
