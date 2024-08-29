namespace DEFRA.NE.BNG.Integration.Domain.Interfaces
{
    public interface IConfigurationReader
    {
        public string Read(string variableName);
        public string BuildDataVerseConnectionString();
    }
}
