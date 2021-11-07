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
  name: appInsName
  params: {
    name: appInsName
  }
}
