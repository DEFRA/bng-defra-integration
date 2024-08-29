namespace DEFRA.NE.BNG.Integration.Infrastructure.UnitTests.TestModels
{
    public class OrderProcessorTestVarriables
    {
        public Guid ApplicantId { get; set; } = Guid.NewGuid();
        public Guid OrganisationId { get; set; } = Guid.NewGuid();
        public Guid DevelopmentId { get; set; } = Guid.NewGuid();
        public Guid OrderId { get; set; } = Guid.NewGuid();
        public List<Guid> ProductIdList { get; set; } = [];
        public List<Guid> FileIdList { get; set; } = [];
        public List<Guid> NationalityIdList { get; set; } = [];
    }
}