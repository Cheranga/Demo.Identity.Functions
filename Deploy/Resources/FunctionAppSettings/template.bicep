
@description('Function app name')
param functionAppName string

@description('Key vault name')
param keyVaultName string

@description('Storage account name')
param sgName string

@description('ServiceBus namespace name')
param serviceBusNamespaceName string

@description('Topic name')
param topicName string

@description('Subscription name')
param subscriptionName string

var storageAccountConnectionStringSecret = '@Microsoft.KeyVault(SecretUri=https://${keyVaultName}.vault.azure.net/secrets/storageAccountConnectionString/)'
var appInsightsKeySecret = '@Microsoft.KeyVault(SecretUri=https://${keyVaultName}.vault.azure.net/secrets/appInsightsKey/)'
var timeZone = 'AUS Eastern Standard Time'

resource productionSlotAppSettings 'Microsoft.Web/sites/config@2021-02-01' = {
  name: '${functionAppName}/appsettings'
  properties:{
    CustomerApiKey: 'This is the production setting'      
    AzureWebJobsStorage__accountName: sgName
    WEBSITE_CONTENTAZUREFILECONNECTIONSTRING: storageAccountConnectionStringSecret
    WEBSITE_CONTENTSHARE: toLower(functionAppName)
    FUNCTIONS_EXTENSION_VERSION: '~3'
    APPINSIGHTS_INSTRUMENTATIONKEY: appInsightsKeySecret
    FUNCTIONS_WORKER_RUNTIME: 'dotnet'
    WEBSITE_TIME_ZONE: timeZone
    WEBSITE_ADD_SITENAME_BINDINGS_IN_APPHOST_CONFIG: 1  
  }
}

resource stagingSlotAppSettings 'Microsoft.Web/sites/slots/config@2021-02-01'= {
  name: '${functionAppName}/Staging/appsettings'
  properties:{
    CustomerApiKey: 'This is the staging setting'  
    ServiceBusConfig__Topic: topicName
    ServiceBusConfig__Subscription: subscriptionName
    ServiceBusConfig__FullyQualifiedNamespace: '${serviceBusNamespaceName}.servicebus.windows.net'
    ServiceBusConnection__fullyQualifiedNamespace: '${serviceBusNamespaceName}.servicebus.windows.net'
    AzureWebJobsStorage__accountName: sgName
    WEBSITE_CONTENTAZUREFILECONNECTIONSTRING: storageAccountConnectionStringSecret
    WEBSITE_CONTENTSHARE: toLower(functionAppName)
    FUNCTIONS_EXTENSION_VERSION: '~3'
    APPINSIGHTS_INSTRUMENTATIONKEY: appInsightsKeySecret
    FUNCTIONS_WORKER_RUNTIME: 'dotnet'
    WEBSITE_TIME_ZONE: timeZone
    WEBSITE_ADD_SITENAME_BINDINGS_IN_APPHOST_CONFIG: 1
  }
}
