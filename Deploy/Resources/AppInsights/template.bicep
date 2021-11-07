@description('Application insights name')
param name string

resource appIns 'Microsoft.Insights/components@2020-02-02' = {
  name: name
  location: resourceGroup().location
  kind: 'web'
  properties:{
    Application_Type: 'web'
    Request_Source:'rest'
    Flow_Type:'Bluefield'
  }
}

output appInsightsKey string = reference(appIns.id, appIns.apiVersion).InstrumentationKey
