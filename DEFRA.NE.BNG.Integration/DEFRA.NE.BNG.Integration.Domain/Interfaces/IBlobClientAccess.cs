namespace DEFRA.NE.BNG.Integration.Domain.Interfaces
{
    public interface IBlobClientAccess
    {
        public Task<string> ReadDataFromBlob(string fileName);
    }
}
