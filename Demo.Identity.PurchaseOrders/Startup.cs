using Azure.Identity;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Demo.Identity.PurchaseOrders;
using Demo.Identity.PurchaseOrders.Configs;
using Demo.Identity.PurchaseOrders.Infrastructure;
using Demo.Identity.PurchaseOrders.Services;
using Microsoft.ApplicationInsights.Extensibility;
using Serilog;
using Serilog.Events;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Demo.Identity.PurchaseOrders
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = GetConfiguration(builder);
            var services = builder.Services;

            RegisterLogging(services);
            RegisterConfigurations(services, configuration);
            RegisterServices(services, configuration);
        }

        protected virtual IConfigurationRoot GetConfiguration(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;
            var executionContextOptions = services.BuildServiceProvider().GetService<IOptions<ExecutionContextOptions>>().Value;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(executionContextOptions.AppDirectory)
                .AddJsonFile("local.settings.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            return configuration;
        }

        private void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IMessagePublisher, MessagePublisher>();
            services.AddSingleton<IReceiveOrderRequestHandler, ReceiveOrderRequestHandler>();

            services.AddAzureClients(builder =>
            {
                builder.AddServiceBusClient(configuration.GetSection(nameof(ServiceBusConfig))).WithCredential(new DefaultAzureCredential(new DefaultAzureCredentialOptions
                {
                    ExcludeEnvironmentCredential = true,
                    ExcludeVisualStudioCredential = true,
                    ExcludeVisualStudioCodeCredential = true,
                    ExcludeAzurePowerShellCredential = true,
                    ExcludeInteractiveBrowserCredential = true
                }));
            });
        }

        private void RegisterConfigurations(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ServiceBusConfig>(configuration.GetSection(nameof(ServiceBusConfig)));
            services.AddSingleton(provider =>
            {
                var config = provider.GetRequiredService<IOptionsSnapshot<ServiceBusConfig>>().Value;
                return config;
            });
        }

        private void RegisterLogging(IServiceCollection services)
        {
            services.AddLogging(builder =>
            {
                var logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                    .WriteTo.ApplicationInsights(TelemetryConfiguration.CreateDefault(), TelemetryConverter.Traces, LogEventLevel.Debug)
                    .CreateLogger();

                builder.AddSerilog(logger);
            });
        }
    }
}