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

var appInsName = 'ins-${functionAppName}-${environmentName}'

// Storage account
module storageAccountModule './StorageAccount/template.bicep' = {
  name: 'storageAccount-${buildNumber}'
  params: {
    sgName: sgName
    sku: storageSku
    tier: storageSkuTier
  }
}

// Application insights
module appInsightsModule 'AppInsights/template.bicep' = {
  name: 'app-insights-${buildNumber}'
  params: {
    name: appInsName
  }
}

// Application service plan
module aspModule 'AppServicePlan/template.bicep'= {
  name: 'asp-${buildNumber}'
  params: {
    name: aspName
    sku: planSku
    tier: planTier
  }
}

// Function app without settings
module functionAppModule 'FunctionApp/template.bicep' = {
  name: 'funcapp-${buildNumber}'
  params: {
    name: 'fn-${functionAppName}-${environmentName}'
    planName: aspModule.outputs.planId
  }  
}

module keyVaultModule 'KeyVault/template.bicep' = {
  name: 'keyvault-${buildNumber}'
  params: {
    appInsightsKey: appInsightsModule.outputs.appInsightsKey
    name: 'kv-${functionAppName}-${environmentName}'
    productionSlotPrincipalId: functionAppModule.outputs.productionPrincipalId
    stagingSlotPrincipalId: functionAppModule.outputs.stagingPrincipalId
    storageAccountConnectionString: storageAccountModule.outputs.storageAccountConnectionString
  }
}

module functionAppSettingsModule 'FunctionAppSettings/template.bicep' = {
  name: 'funcapp-settings-${buildNumber}'
  params: {
    functionAppName: 'fn-${functionAppName}-${environmentName}'
    keyVaultName: 'kv-${functionAppName}-${environmentName}'
    sgName: sgName
  }
  dependsOn:[
    storageAccountModule    
    appInsightsModule
    aspModule
    functionAppModule
    keyVaultModule
  ]
}
