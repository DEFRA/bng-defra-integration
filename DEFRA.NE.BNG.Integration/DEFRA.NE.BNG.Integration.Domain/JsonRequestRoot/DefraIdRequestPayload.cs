using DEFRA.NE.BNG.Integration.Domain.Request;

namespace DEFRA.NE.BNG.Integration.Domain.JsonRequestRoot
{
    public class DefraIdRequestPayload
    {
        public DefraIdMetadata Metadata { get; set; }
        public DefraIdRecorddata Recorddata { get; set; }
    }
}
