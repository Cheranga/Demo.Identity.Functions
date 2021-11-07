param sgName string
param storageSkuName string
param storageSkuTier string

var storageKind = 'StorageV2'

resource stg 'Microsoft.Storage/storageAccounts@2021-02-01' = {
  name: sgName
  location: resourceGroup().location
  kind: storageKind
  sku: {
    name: storageSkuName
    tier: storageSkuTier
  }
}


output storageAccountConnectionString string = 'DefaultEndpointsProtocol=https;AccountName=${stg.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(stg.id, stg.apiVersion).keys[0].value}'
