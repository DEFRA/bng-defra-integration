using DEFRA.NE.BNG.Integration.Domain.Constants;
using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;

namespace DEFRA.NE.BNG.Integration.Infrastructure.Services
{
    public class BlobClientAccess : IBlobClientAccess
    {
        private readonly ILogger logger;
        private readonly IConfigurationReader variableReader;

        public BlobClientAccess(ILogger logger, IConfigurationReader variableReader)
        {
            this.logger = logger;
            this.variableReader = variableReader;
        }

        public async Task<string> ReadDataFromBlob(string fileName)
        {
            logger.LogDebug("Executing {method}", nameof(ReadDataFromBlob));

            string containername = variableReader.Read(EnvironmentConstants.BlobContainer);
            var bytesData = await GetBlob(containername, fileName, variableReader);
            var returnString = Convert.ToBase64String(bytesData);

            return returnString;
        }

        public async Task<byte[]> GetBlob(string containerName, string fileName, IConfigurationReader variableReader)
        {
            logger.LogDebug("Executing {method}", nameof(GetBlob));

            byte[] bytes = null;

            var storageAccount = CloudStorageAccount.Parse(variableReader.Read(EnvironmentConstants.BlobConnectionString));

            var serviceClient = storageAccount.CreateCloudBlobClient();
            var container = serviceClient.GetContainerReference($"{containerName}");
            var blob = container.GetBlockBlobReference($"{fileName}");

            using (Stream inputStream = await blob.OpenReadAsync())
            {
                bytes = new byte[inputStream.Length];
                int numBytesToRead = (int)inputStream.Length;
                int numBytesRead = 0;
                while (numBytesToRead > 0)
                {
                    // Read may return anything from 0 to numBytesToRead.
                    int n = inputStream.Read(bytes, numBytesRead, numBytesToRead);

                    // Break when the end of the file is reached.
                    if (n == 0)
                        break;

                    numBytesRead += n;
                    numBytesToRead -= n;
                }
                numBytesToRead = bytes.Length;
            }
            return bytes;
        }
    }
}
