namespace DEFRA.NE.BNG.Integration.Domain.Response
{
    public class ErrorList
    {
        public List<Error> Errors { get; set; } = [];
        public string StatusCode { get; set; }
    }
}
