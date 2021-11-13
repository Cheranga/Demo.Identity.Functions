@description('Function app name')
param name string

@description('Application service plan name')
param planName string

resource functionAppProductionSlot 'Microsoft.Web/sites@2021-02-01' = {
  name: name
  location: resourceGroup().location
  kind:'functionapp'
  identity:{
    type:'SystemAssigned'
  }
  properties:{
    serverFarmId:planName
  }
}

resource functionAppStagingSlot 'Microsoft.Web/sites/slots@2021-02-01' = {
  name: '${functionAppProductionSlot.name}/Staging'
  location: resourceGroup().location
  kind:'functionapp'
  identity:{
    type:'SystemAssigned'
  }
  properties:{
    serverFarmId:planName
  }
  dependsOn:[
    functionAppProductionSlot
  ]
}

output productionPrincipalId string = functionAppProductionSlot.identity.principalId
output stagingPrincipalId string = functionAppStagingSlot.identity.principalId
