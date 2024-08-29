using DEFRA.NE.BNG.Integration.Function;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = new HostBuilder()
     .ConfigureFunctionsWebApplication()
        .ConfigureServices((hostBuilderContext, services) =>
        {
            services.AddApplicationInsightsTelemetryWorkerService();
            services.ConfigureFunctionsApplicationInsights();
            ServiceRegister.RegisterCustomServices(services);
        })
        .ConfigureLogging((context, logging) =>
        {
            logging.SetMinimumLevel(LogLevel.Debug);
        })
        .Build();

host.Run();
