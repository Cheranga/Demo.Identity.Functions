@description('Key vault name')
param name string

@description('Production slot principal id')
param productionSlotPrincipalId string

@description('Staging slot principal id')
param stagingSlotPrincipalId string

@description('Storage account connection string')
@secure()
param storageAccountConnectionString string

@description('Application insights key')
@secure()
param appInsightsKey string

resource keyVault 'Microsoft.KeyVault/vaults@2021-06-01-preview' = {
  name: name
  location: resourceGroup().location
  properties: {
    enabledForDeployment: true
    enabledForTemplateDeployment: true
    enabledForDiskEncryption: true
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: subscription().tenantId
    accessPolicies: [
      {
        tenantId: subscription().tenantId
        objectId: productionSlotPrincipalId
        permissions: {
          secrets: [
            'get'
            'list'
          ]
        }
      }
      {
        tenantId: subscription().tenantId
        objectId: stagingSlotPrincipalId
        permissions: {
          secrets: [
            'get'
            'list'
          ]
        }
      }
    ]
  }
}


resource storageAccountConnectionStringSecret 'Microsoft.KeyVault/vaults/secrets@2021-06-01-preview'={
  name: '${name}/storageAccountConnectionString'
  properties: {
    value:storageAccountConnectionString
  }
  dependsOn:[
    keyVault
  ]
}

resource appInsightsKeySecret 'Microsoft.KeyVault/vaults/secrets@2021-06-01-preview'={
  name: '${name}/appInsightsKey'
  properties: {
    value:appInsightsKey
  }
  dependsOn:[
    keyVault
  ]
}
