using Azure.Identity;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Revision.Orders.Functions;
using Revision.Orders.Functions.Configs;
using Revision.Orders.Functions.Infrastructure;
using Revision.Orders.Functions.Services;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Revision.Orders.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = GetConfiguration(builder);
            var services = builder.Services;

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
    }
}