@description('Build number')
param buildNumber string

@description('Storage account name')
@minLength(3)
@maxLength(24)
param sgName string

@description('SKU name for storage account')
@allowed([
  'Standard_LRS'
  'Standard_GRS'
])
param storageSku string

@description('SKU tier for storage account')
param storageSkuTier string

@description('Function app name')
param functionAppName string

@description('Environment name')
param environmentName string

@description('Application service plan name')
param aspName string

@description('Application service plan SKU')
param planSku string

@description('Application service plan tier')
param planTier string

@description('ServiceBus namespace name')
param serviceBusNamespaceName string

@description('Topic name')
param topicName string

@description('Subscription name')
param subscriptionName string

var appInsName = 'ins-${functionAppName}-${environmentName}'

// Storage account
module storageAccountModule './StorageAccount/template.bicep' = {
  name: '${buildNumber}-storageaccount'
  params: {
    sgName: sgName
    sku: storageSku
    tier: storageSkuTier
  }
}

// Application insights
module appInsightsModule 'AppInsights/template.bicep' = {
  name: '${buildNumber}-applicationinsights'
  params: {
    name: appInsName
  }
}

// Application service plan
module aspModule 'AppServicePlan/template.bicep' = {
  name: '${buildNumber}-applicationserviceplan'
  params: {
    name: aspName
    sku: planSku
    tier: planTier
  }
}

// Function app without settings
module functionAppModule 'FunctionApp/template.bicep' = {
  name: '${buildNumber}-functionapp'
  params: {
    name: 'fn-${functionAppName}-${environmentName}'
    planName: aspModule.outputs.planId
  }
}

module keyVaultModule 'KeyVault/template.bicep' = {
  name: '${buildNumber}-keyvault'
  params: {
    appInsightsKey: appInsightsModule.outputs.appInsightsKey
    name: 'kv-${functionAppName}-${environmentName}'
    productionSlotPrincipalId: functionAppModule.outputs.productionPrincipalId
    stagingSlotPrincipalId: functionAppModule.outputs.stagingPrincipalId
    storageAccountConnectionString: storageAccountModule.outputs.storageAccountConnectionString
  }
}

module functionAppSettingsModule 'FunctionAppSettings/template.bicep' = {
  name: '${buildNumber}-functionappsettings'
  params: {
    functionAppName: 'fn-${functionAppName}-${environmentName}'
    keyVaultName: 'kv-${functionAppName}-${environmentName}'
    sgName: sgName
    serviceBusNamespaceName: serviceBusNamespaceName
    subscriptionName: subscriptionName
    topicName: topicName
  }
  dependsOn: [
    storageAccountModule
    appInsightsModule
    aspModule
    functionAppModule
    keyVaultModule
  ]
}
