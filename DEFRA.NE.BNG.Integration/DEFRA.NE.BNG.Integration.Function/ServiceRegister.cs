using DEFRA.NE.BNG.Integration.Domain.Constants;
using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using DEFRA.NE.BNG.Integration.Infrastructure.EntityProcessors;
using DEFRA.NE.BNG.Integration.Infrastructure.Facades;
using DEFRA.NE.BNG.Integration.Infrastructure.Services;
using DEFRA.NE.BNG.Integration.Infrastructure.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DEFRA.NE.BNG.Integration.Function
{
    public static class ServiceRegister
    {
        public static void RegisterCustomServices(IServiceCollection services)
        {
            var connection = new ConfigurationReader().BuildDataVerseConnectionString();

            services.AddScoped<IConfigurationReader, ConfigurationReader>();

            services.AddScoped<IHttpClient, HttpClientApi>(x =>
                                              new HttpClientApi(
                                                    Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationServiceBaseUrl),
                                                    x.GetRequiredService<ILogger<HttpClientApi>>()));

            services.AddTransient<IMailServiceAgent, MailServiceAgent>(x =>
                                                new MailServiceAgent(
                                                                x.GetRequiredService<IHttpClient>(),
                                                                x.GetRequiredService<ILogger<MailServiceAgent>>()));

            services.AddTransient<IEmailNotificationRequestGenerator, EmailNotificationRequestGenerator>(x =>
                                                new EmailNotificationRequestGenerator(
                                                                x.GetRequiredService<IMappingManager>(),
                                                                x.GetRequiredService<INotifyManager>(),
                                                                x.GetRequiredService<ILogger<EmailNotificationRequestGenerator>>(),
                                                                x.GetRequiredService<IDataverseService>())
                                                );

            services.AddScoped<IUkGovNotifyFacade, UkGovNotifyFacade>(x =>
                                                new UkGovNotifyFacade(x.GetRequiredService<ILogger<UkGovNotifyFacade>>(),
                                                                x.GetRequiredService<IMailServiceAgent>(),
                                                                x.GetRequiredService<IConfigurationReader>(),
                                                                x.GetRequiredService<IDataverseService>(),
                                                                x.GetRequiredService<IMappingManager>(),
                                                                x.GetRequiredService<INotifyManager>(),
                                                                x.GetRequiredService<IEmailNotificationRequestGenerator>())
                                                );

            services.AddScoped<IPowerPlatformClient, PowerPlatformClient>(x => PowerPlatformClient.PowerPlatformClientInstance);

            services.AddTransient<IDefraIdSyncFacade, DefraIdSyncFacade>(x =>
                                                new DefraIdSyncFacade(
                                                                x.GetRequiredService<ILogger<DefraIdSyncFacade>>(),
                                                                x.GetRequiredService<IDataverseService>()
                                                                ));
            services.AddTransient<IBlobClientAccess, BlobClientAccess>(x =>
                                                new BlobClientAccess(
                                                                x.GetRequiredService<ILogger<BlobClientAccess>>(),
                                                                x.GetRequiredService<IConfigurationReader>()
                                                                ));
            services.AddTransient<IDeveloperRegistrationFacade, DeveloperRegistrationFacade>(x =>
                                                new DeveloperRegistrationFacade(
                                                                x.GetRequiredService<ILogger<DeveloperRegistrationFacade>>(),
                                                                x.GetRequiredService<IConfigurationReader>(),
                                                                x.GetRequiredService<IMailServiceAgent>(),
                                                                x.GetRequiredService<IDataverseService>()
                                                                ));
            services.AddTransient<IDataverseService, DataverseService>(x =>
                                                new DataverseService(
                                                                x.GetRequiredService<ILogger<DataverseService>>(),
                                                                x.GetRequiredService<IBlobClientAccess>(),
                                                                PowerPlatformClient.PowerPlatformClientInstance
                                                                .GetServiceClient(connection)
                                                                ));

            services.AddTransient<IUKGovNotifyRetriggerFacade, UKGovNotifyRetriggerFacade>(x =>
                                                new UKGovNotifyRetriggerFacade(
                                                                x.GetRequiredService<ILogger<UKGovNotifyRetriggerFacade>>(),
                                                                x.GetRequiredService<IUkGovNotifyFacade>(),
                                                                x.GetRequiredService<IMailServiceAgent>(),
                                                                x.GetRequiredService<IConfigurationReader>(),
                                                                x.GetRequiredService<IDataverseService>(),
                                                                x.GetRequiredService<IEmailNotificationRequestGenerator>()
                                                                ));

            services.AddTransient<ILandOwnerRegistrationFacade, LandOwnerRegistrationFacade>(x =>
                                                new LandOwnerRegistrationFacade(
                                                                x.GetRequiredService<ILogger<LandOwnerRegistrationFacade>>(),
                                                                x.GetRequiredService<IConfigurationReader>(),
                                                                x.GetRequiredService<IMailServiceAgent>(),
                                                                x.GetRequiredService<IDataverseService>(),
                                                                x.GetRequiredService<IMappingManager>()
                                                                ));

            services.AddTransient<IOrderFacade, OrderFacade>(x =>
                                                new OrderFacade(
                                                                x.GetRequiredService<ILogger<OrderFacade>>(),
                                                                x.GetRequiredService<IConfigurationReader>(),
                                                                x.GetRequiredService<IMailServiceAgent>(),
                                                                x.GetRequiredService<IDataverseService>()
                                                                ));

            services.AddTransient<INotifyManager, NotifyProcessor>(x =>
                                                new NotifyProcessor(
                                                                x.GetRequiredService<IMappingManager>(),
                                                                x.GetRequiredService<IDataverseService>()
                                                                ));

            services.AddTransient<IMappingManager, MappingManager>(x => new MappingManager());

            services.AddTransient<IGainsiteValidatorFacade, GainsiteValidatorFacade>(x =>
                                                new GainsiteValidatorFacade(
                                                                x.GetRequiredService<ILogger<GainsiteValidatorFacade>>(),
                                                                x.GetRequiredService<IConfigurationReader>(),
                                                                x.GetRequiredService<IMailServiceAgent>(),
                                                                x.GetRequiredService<IDataverseService>()
                                                                ));

            services.AddTransient<ICombinedRegistrationFacade, CombinedRegistrationFacade>(x =>
                                                new CombinedRegistrationFacade(
                                                                x.GetRequiredService<ILogger<CombinedRegistrationFacade>>(),
                                                                x.GetRequiredService<IConfigurationReader>(),
                                                                x.GetRequiredService<IMailServiceAgent>(),
                                                                x.GetRequiredService<IDataverseService>(),
                                                                x.GetRequiredService<ILandOwnerRegistrationFacade>(),
                                                                x.GetRequiredService<IDeveloperRegistrationFacade>()
                                                                ));
        }
    }
}
