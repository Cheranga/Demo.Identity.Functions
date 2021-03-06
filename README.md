### Using identity based connection in Azure functions

* `AzureFunctionsVersion` is case sensitive! It needs to be set to "v3".
* Apps using the version 5 or higher in service bus extension use the `ServiceBusReceivedMessage`. This version drops support for the legact `Message` type.
* Be specific about how the `DefaultAzureCredential` will be used. In here the code will only allow access through `Azure CLI` or through `Managed Identity`.

```c#
services.AddAzureClients(builder =>
            {
                builder.AddServiceBusClient(configuration.GetSection(nameof(ServiceBusConfig))).WithCredential(new DefaultAzureCredential(new DefaultAzureCredentialOptions
                {
                    ExcludeEnvironmentCredential = true,
                    ExcludeVisualStudioCredential = true,
                    ExcludeVisualStudioCodeCredential = true,
                    ExcludeAzurePowerShellCredential = true,
                    ExcludeInteractiveBrowserCredential = true,
                    ExcludeSharedTokenCacheCredential = true
                }));
            });
```
* Wrapping the `ServiceBusClient` in your own service class

When you are doing this obviously you'll have some configurations to set. Make those configuration settings as shown below.
```
{
    ...
    ...
    "ServiceBusConfig:Topic": "customer-orders",
    "ServiceBusConfig:Subscription": "all",
    "ServiceBusConfig:FullyQualifiedNamespace": "sb-cc-platform.servicebus.windows.net"
}
```
Then register the `ServiceBusClient` as shown below.
```c#
services.AddAzureClients(builder =>
{
    builder.AddServiceBusClient(configuration.GetSection(nameof(ServiceBusConfig))).WithCredential(new DefaultAzureCredential(new DefaultAzureCredentialOptions
    {
        ExcludeEnvironmentCredential = true,
        ExcludeVisualStudioCredential = true,
        ExcludeVisualStudioCodeCredential = true,
        ExcludeAzurePowerShellCredential = true,
        ExcludeInteractiveBrowserCredential = true,
        ExcludeSharedTokenCacheCredential = true
    }));
});
```
* Setting up the local environment

Use `Azure CLI` inside your preferred IDE. First use the command `az login --allow-no-subscriptions` to login to Azure.
Then add yourself to the roles `Azure Service Bus Data Sender` and `Azure Service Bus Data Receiver` by using the commands below. 

```
az role assignment create --assignee [YOUR OBJECT ID] --role "Azure Service Bus Data Receiver" --scope /subscriptions/[SUBSCRIPTION ID]/resourceGroups/[RESOURCE GROUP OF SERVICE BUS]/providers/Microsoft.ServiceBus/namespaces/sb-cc-platform
az role assignment create --assignee [YOUR OBJECT ID] --role "Azure Service Bus Data Sender" --scope /subscriptions/[SUBSCRIPTION ID]/resourceGroups/[RESOURCE GROUP OF SERVICE BUS]/providers/Microsoft.ServiceBus/namespaces/sb-cc-platform
``` 
  * `ServiceBusConnection__fullyQualifiedNamespace` does not work when set inside the `local.settings.json` file. Just create a config as below and use the connection string inside there.
```
"ServiceBusConnection": "[CONNECTION STRING FOR THE TOPIC]",
```
IMPORTANT: This configuration will NOT be used in the deployed application. Otherwise it will defeat the whole purpose of this approach! :laugh:

* Deployed Application Configurations

In the deployed application the setting `ServiceBusConnection_fullyQualifiedNamespace` must be set with the service bus namespace.
```json
{
    "name": "ServiceBusConnection__fullyQualifiedNamespace",
    "value": "sb-cc-platform.servicebus.windows.net"
}
```