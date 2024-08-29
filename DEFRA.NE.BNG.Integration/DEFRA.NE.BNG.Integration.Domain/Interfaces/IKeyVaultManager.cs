namespace DEFRA.NE.BNG.Integration.Domain.Interfaces
{
    public interface IKeyVaultManager
    {
        public Task<string> GetSecret(string secretName);
    }
}
