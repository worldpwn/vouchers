param location string = 'eastus'
param scaleOutLimits int = 1
param stage string
param baseName string
param hash string
param repository string
@allowed([
  'F1'
  'S1'
  'B1'
])
param sku string = 'F1'

var tags = {
  CommitHash: hash
  Description: 'Voucher System'
  Repository: repository
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-08-01' = {
  name: '${replace(baseName, '-', '')}${stage}'
  location: location
  kind: 'StorageV2'
  tags: tags
  sku: {
    name: 'Standard_LRS'
  }
}

resource logWorkspace 'Microsoft.OperationalInsights/workspaces@2021-12-01-preview' = {
  name: '${baseName}-${stage}'
  location: location
}

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: '${baseName}-${stage}'
  location: location
  kind: 'webapi'
  tags: tags
  properties: {
    Application_Type: 'web'
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
    WorkspaceResourceId: logWorkspace.id
  }
}

resource hostingPlan 'Microsoft.Web/serverfarms@2020-12-01' = {
  name: '${baseName}-${stage}'
  location: location
  kind: 'linux'
  tags: tags
  properties: {
    reserved: true
  }
  sku: {
    name: sku
    capacity: scaleOutLimits
  }
}

resource appService 'Microsoft.Web/sites@2021-03-01' = {
  name: '${baseName}-${stage}'
  location: location
  tags: tags
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: hostingPlan.id
    httpsOnly: true
    siteConfig: {
      ftpsState: 'Disabled'
      http20Enabled: true
      netFrameworkVersion: 'v7.0'
      linuxFxVersion: 'DOTNETCORE|7.0'
      appCommandLine: 'dotnet VoucherSystem.dll'
      publicNetworkAccess: 'Enabled'
    }
  }
}

resource appSettings 'Microsoft.Web/sites/config@2022-03-01' = {
  name: 'appsettings'
  kind: 'string'
  parent: appService
  properties: {
    // Base Config
    APPLICATIONINSIGHTS_CONNECTION_STRING: appInsights.properties.ConnectionString
  }
}

var storageTableDataContributor = '0a9a7e1f-b9d0-4cc4-a60d-0319b160aaa3'
resource rbacFAppToKv 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(storageAccount.id, appService.id, storageTableDataContributor)
  scope: storageAccount
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', storageTableDataContributor)
    principalId: appService.identity.principalId
    principalType: 'ServicePrincipal'
  }
}
