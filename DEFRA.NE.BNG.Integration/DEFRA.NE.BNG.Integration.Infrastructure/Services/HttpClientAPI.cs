using System.Net;
using System.Text;
using DEFRA.NE.BNG.Integration.Domain.Constants;
using DEFRA.NE.BNG.Integration.Domain.Exceptions;
using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using DEFRA.NE.BNG.Integration.Infrastructure.Utilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DEFRA.NE.BNG.Integration.Infrastructure.Services
{
    public class HttpClientApi : IHttpClient
    {
        private readonly ILogger logger;

        private HttpClient httpClient;

        public HttpClientApi(string basePath, ILogger logger)
        {
            httpClient = CreateHttpClient(basePath);
            this.logger = logger;
        }

        private HttpClient CreateHttpClient(string serviceBaseAddress)
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(serviceBaseAddress)
            };
            return httpClient;
        }

        public async Task<T> PostAsync<T, S>(string requestUri, S payloadData)
        {
            logger.LogDebug("Executing {postAsync}", nameof(PostAsync));

            T returnType;
            var json = JsonConvert.SerializeObject(payloadData);
            var httpContent = new StringContent(json, Encoding.UTF8, GovNotificationConstants.GovNot_API_ContentType);
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add(GovNotificationConstants.GovNot_API_Authorization, GovNotificationConstants.GovNot_API_Bearer + TokenManager.GenerateAccessToken());
            var mailResponse = await httpClient.PostAsync(requestUri, httpContent);
            var responseContent = await mailResponse.Content.ReadAsStringAsync();
            if (mailResponse.StatusCode == HttpStatusCode.Created)
            {
                returnType = JsonConvert.DeserializeObject<T>(responseContent);
            }
            else
            {
                throw new MailNotificationFailedException(responseContent);
            }

            return returnType;
        }
    }
}