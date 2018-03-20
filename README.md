# QuamotionCloud.net
| build | NuGet |
|-------|-------|
| [![Build status](https://ci.appveyor.com/api/projects/status/8lb0bai9avs29983?svg=true)](https://ci.appveyor.com/project/bartsaintgermain/quamotioncloud-net)|

The QuamotionCloud.net provides C# bindings for the Quamotion Cloud.

## Request your API key
You can create an API key from the Quamotion Cloud portal. 

## Login into quamotion cloud.
```
var cloudClient = new CloudClient();
await cloudClient.Login(token).ConfigureAwait(false);
```
## Get your projects
```
var tenants = await cloudClient.GetTenants().ConfigureAwait(false);
```
## Publish a new test package
```
var testPackage = await cloudClient.PublishTestPackage(tenant, "acquaint.ps1").ConfigureAwait(false); 
```
## Publish a new application
```
var application = await cloudClient.PublishApplication(tenant, "acquaint.apk").ConfigureAwait(false);
```
## Get all applications
```
var applications = await cloudClient.GetApplications(tenant).ConfigureAwait(false);
```
## get all test packages
```
var testPackages = await cloudClient.GetTestPackages(tenant).ConfigureAwait(false);
```
## get all device groups
```
var deviceGroups = await cloudClient.GetDeviceGroups(tenant).ConfigureAwait(false);
```
## get a device group
```
var deviceGroup = await cloudClient.GetDeviceGroup(tenant, new Guid("6185bb7c-1f96-40bb-9f6c-798bd7547d36")).ConfigureAwait(false);
```
## get an application
```
var application = await cloudClient.GetApplication(tenat, "demo.quamotion.Acquaint", "1.51", "iOS").ConfigureAwait(false);
```
## get a test package
```
var testPackage = await cloudClient.GetTestPackage(tenant, "genymotiontest (8).ps1", "0.3").ConfigureAwait(false);
```
## schedule a test run
```
var testRun = await cloudClient.ScheduleTestRun(tenant, testPackage, application, deviceGroup).ConfigureAwait(false);
```
