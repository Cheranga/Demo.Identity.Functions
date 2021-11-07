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
param storageSkuName string

@description('SKU tier for storage account')
param storageSkuTier string

module storageAccountModule './StorageAccount/template.bicep' = {
  name: 'storageAccount-${buildNumber}'
  params: {
    sgName: sgName
    storageSkuName: storageSkuName
    storageSkuTier: storageSkuTier
  }
}
