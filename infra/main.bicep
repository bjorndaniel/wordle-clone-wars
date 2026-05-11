@description('Azure region for all resources.')
param location string = resourceGroup().location

@description('Short prefix used for resource names.')
@maxLength(20)
param namePrefix string = 'wordlewars'

@description('Azure SQL admin username.')
param sqlAdminUsername string = 'wordleadmin'

@description('Azure SQL admin password.')
@secure()
param sqlAdminPassword string

@description('Application database name.')
param sqlDatabaseName string = 'wordleclonewars'

var uniqueSuffix = toLower(uniqueString(subscription().id, resourceGroup().id))
var appServicePlanName = take('${namePrefix}-plan-${uniqueSuffix}', 40)
var webAppName = take('${namePrefix}-web-${uniqueSuffix}', 60)
var sqlServerName = take('${namePrefix}-sql-${uniqueSuffix}', 63)

resource appServicePlan 'Microsoft.Web/serverfarms@2023-12-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: 'F1'
    tier: 'Free'
    size: 'F1'
    capacity: 1
  }
  kind: 'app'
  properties: {
    reserved: false
  }
}

resource webApp 'Microsoft.Web/sites@2023-12-01' = {
  name: webAppName
  location: location
  kind: 'app'
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      ftpsState: 'Disabled'
      minTlsVersion: '1.2'
      http20Enabled: true
      alwaysOn: false
      netFrameworkVersion: 'v10.0'
      metadata: [
        {
          name: 'CURRENT_STACK'
          value: 'dotnet'
        }
      ]
    }
  }
}

resource sqlServer 'Microsoft.Sql/servers@2023-08-01-preview' = {
  name: sqlServerName
  location: location
  properties: {
    administratorLogin: sqlAdminUsername
    administratorLoginPassword: sqlAdminPassword
    version: '12.0'
    minimalTlsVersion: '1.2'
    publicNetworkAccess: 'Enabled'
  }
}

// Azure SQL free offer: GP serverless with useFreeLimit. Limited to one per subscription.
resource sqlDatabase 'Microsoft.Sql/servers/databases@2023-08-01-preview' = {
  parent: sqlServer
  name: sqlDatabaseName
  location: location
  sku: {
    name: 'GP_S_Gen5_2'
    tier: 'GeneralPurpose'
    family: 'Gen5'
    capacity: 2
  }
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
    maxSizeBytes: 34359738368
    useFreeLimit: true
    freeLimitExhaustionBehavior: 'AutoPause'
    autoPauseDelay: 60
    minCapacity: json('0.5')
    zoneRedundant: false
  }
}

// Allows access from Azure services such as App Service.
resource allowAzureServices 'Microsoft.Sql/servers/firewallRules@2023-08-01-preview' = {
  parent: sqlServer
  name: 'AllowAllAzureServicesAndResourcesWithinAzureIps'
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}

output webAppName string = webApp.name
output appServicePlanName string = appServicePlan.name
output sqlServerName string = sqlServer.name
output sqlServerFqdn string = sqlServer.properties.fullyQualifiedDomainName
output sqlDatabaseName string = sqlDatabase.name
output sqlAdminUsername string = sqlAdminUsername
