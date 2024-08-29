namespace DEFRA.NE.BNG.Integration.Domain.Interfaces
{
    public interface IHttpClient
    {
        public Task<T> PostAsync<T, S>(string requestUri, S payloadData);
    }
}
