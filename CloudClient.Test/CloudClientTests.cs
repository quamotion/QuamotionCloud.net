using Moq;
using Quamotion.Cloud.Client.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Quamotion.Cloud.Client.Test
{
    public class CloudClientTests
    {
        [Fact]
        public async Task LoginTest()
        {
            // arrange
            var receivedToken = string.Empty;
            var cloudConnectionMock = new Mock<ICloudConnection>();
            cloudConnectionMock.Setup(cc => cc.Login(It.IsAny<string>(), CancellationToken.None)).Returns(Task.CompletedTask).Callback<string, CancellationToken>((t, cancellationToken) => receivedToken = t);
            var cloudClient = new CloudClient(cloudConnectionMock.Object);

            // act
            var token = new Guid().ToString();
            await cloudClient.Login(token, CancellationToken.None).ConfigureAwait(false);

            // assert
            Assert.Equal(token, receivedToken);
        }

        [Fact]
        public async Task GetTenantsTest()
        {
            // arrange
            string requestUrl = string.Empty;
            var cloudConnectionMock = new Mock<ICloudConnection>();
            string json = File.ReadAllText("tenants.json");
            cloudConnectionMock.Setup(cc => cc.GetRequest(It.IsAny<string>(), CancellationToken.None)).Returns(Task.FromResult(json)).Callback<string, CancellationToken>((url, cancellationToken) => requestUrl = url);
            var cloudClient = new CloudClient(cloudConnectionMock.Object);

            // act
            var tenants = await cloudClient.GetTenants(CancellationToken.None).ConfigureAwait(false);

            // assert
            Assert.Equal("/api/project", requestUrl);
            Assert.Equal(7, tenants.Count);
        }

        [Fact]
        public async Task PublishTestPackageTest()
        {
            // arrange
            string requestUrl = string.Empty;
            string requestFile = null;
            var cloudConnectionMock = new Mock<ICloudConnection>();
            string json = File.ReadAllText("publish-testpackage-response.json");
            cloudConnectionMock.Setup(cc => cc.SendFile(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).Returns(Task.FromResult(json)).Callback<string, string, CancellationToken>((url, file, cancellationToken) =>
            {
                requestUrl = url;
                requestFile = file;
            });

            var cloudClient = new CloudClient(cloudConnectionMock.Object);

            // act
            var testPackage = await cloudClient.PublishTestPackage(new Tenant() { Name = "localbsg" }, "genymotiontest.ps1", CancellationToken.None).ConfigureAwait(false);

            // assert
            Assert.Equal("/project/localbsg/api/testPackage", requestUrl);
            Assert.Equal("genymotiontest.ps1", requestFile);
            Assert.NotNull(testPackage);
            Assert.Equal("quamotion-acquaint", testPackage.Name);
        }

        [Fact]
        public async Task PublishApplicationTest()
        {
            // arrange
            string requestUrl = string.Empty;
            string requestFile = null;
            var cloudConnectionMock = new Mock<ICloudConnection>();
            string json = File.ReadAllText("publish-application-response.json");
            cloudConnectionMock.Setup(cc => cc.SendFile(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None)).Returns(Task.FromResult(json)).Callback<string, string, CancellationToken>((url, file, cancellationToken) =>
            {
                requestUrl = url;
                requestFile = file;
            });

            var cloudClient = new CloudClient(cloudConnectionMock.Object);

            // act
            var application = await cloudClient.PublishApplication(new Tenant() { Name = "localbsg" }, "acquaint.apk", CancellationToken.None).ConfigureAwait(false);

            // assert
            Assert.Equal("/project/localbsg/api/app", requestUrl);
            Assert.Equal("acquaint.apk", requestFile);
            Assert.NotNull(application);
            Assert.Equal("demo.quamotion.Acquaint", application.AppId);
        }

        [Fact]
        public async Task GetApplicationsTest()
        {
            // arrange
            string requestUrl = string.Empty;
            var cloudConnectionMock = new Mock<ICloudConnection>();
            string json = File.ReadAllText("applications.json");
            cloudConnectionMock.Setup(cc => cc.GetRequest(It.IsAny<string>(), CancellationToken.None)).Returns(Task.FromResult(json)).Callback<string,CancellationToken>((url, cancellationToken) => requestUrl = url);
            var cloudClient = new CloudClient(cloudConnectionMock.Object);

            // act
            var applications = await cloudClient.GetApplications(new Tenant() { Name = "localbsg" }, CancellationToken.None).ConfigureAwait(false);

            // assert
            Assert.Equal("/project/localbsg/api/app", requestUrl);
            Assert.Equal(2, applications.Count);
        }

        [Fact]
        public async Task GetTestPackagesTest()
        {
            // arrange
            string requestUrl = string.Empty;
            var cloudConnectionMock = new Mock<ICloudConnection>();
            string json = File.ReadAllText("test_packages.json");
            cloudConnectionMock.Setup(cc => cc.GetRequest(It.IsAny<string>(), CancellationToken.None)).Returns(Task.FromResult(json)).Callback<string, CancellationToken>((url, cancellationToken) => requestUrl = url);
            var cloudClient = new CloudClient(cloudConnectionMock.Object);

            // act
            var testPackages = await cloudClient.GetTestPackages(new Tenant() { Name = "localbsg" }, CancellationToken.None).ConfigureAwait(false);

            // assert
            Assert.Equal("/project/localbsg/api/testPackage", requestUrl);
            Assert.Equal(32, testPackages.Count);
        }

        [Fact]
        public async Task GetDeviceGroupsTest()
        {
            // arrange
            string requestUrl = string.Empty;
            var cloudConnectionMock = new Mock<ICloudConnection>();
            string json = File.ReadAllText("device_groups.json");
            cloudConnectionMock.Setup(cc => cc.GetRequest(It.IsAny<string>(), CancellationToken.None)).Returns(Task.FromResult(json)).Callback<string, CancellationToken>((url, cancellationToken) => requestUrl = url);
            var cloudClient = new CloudClient(cloudConnectionMock.Object);

            // act
            var deviceGroups = await cloudClient.GetDeviceGroups(new Tenant() { Name = "localbsg" }, CancellationToken.None).ConfigureAwait(false);

            // assert
            Assert.Equal("/project/localbsg/api/deviceGroup", requestUrl);
            Assert.Equal(37, deviceGroups.Count);
        }


        [Fact]
        public async Task GetDeviceGroupTest()
        {
            // arrange
            string requestUrl = string.Empty;
            var cloudConnectionMock = new Mock<ICloudConnection>();
            string json = File.ReadAllText("device_groups.json");
            cloudConnectionMock.Setup(cc => cc.GetRequest(It.IsAny<string>(), CancellationToken.None)).Returns(Task.FromResult(json)).Callback<string, CancellationToken>((url, cancellationToken) => requestUrl = url);
            var cloudClient = new CloudClient(cloudConnectionMock.Object);

            // act
            var deviceGroup = await cloudClient.GetDeviceGroup(new Tenant() { Name = "localbsg" }, new Guid("6185bb7c-1f96-40bb-9f6c-798bd7547d36"), CancellationToken.None).ConfigureAwait(false);

            // assert
            Assert.Equal("/project/localbsg/api/deviceGroup", requestUrl);
            Assert.Equal("Samsung Galaxy S7 - Android 7.0 (Genymotion)", deviceGroup.DisplayName);
        }

        [Fact]
        public async Task GetApplicationTest()
        {
            // arrange
            string requestUrl = string.Empty;
            var cloudConnectionMock = new Mock<ICloudConnection>();
            string json = File.ReadAllText("applications.json");
            cloudConnectionMock.Setup(cc => cc.GetRequest(It.IsAny<string>(), CancellationToken.None)).Returns(Task.FromResult(json)).Callback<string, CancellationToken>((url, cancellationToken) => requestUrl = url);
            var cloudClient = new CloudClient(cloudConnectionMock.Object);

            // act
            var application = await cloudClient.GetApplication(new Tenant() { Name = "localbsg" }, "demo.quamotion.Acquaint", "1.51", "iOS", CancellationToken.None).ConfigureAwait(false);

            // assert
            Assert.Equal("/project/localbsg/api/app", requestUrl);
            Assert.Equal("1.51", application.Version);
        }

        [Fact]
        public async Task GetTestPackageTest()
        {
            // arrange
            string requestUrl = string.Empty;
            var cloudConnectionMock = new Mock<ICloudConnection>();
            string json = File.ReadAllText("test_packages.json");
            cloudConnectionMock.Setup(cc => cc.GetRequest(It.IsAny<string>(), CancellationToken.None)).Returns(Task.FromResult(json)).Callback<string, CancellationToken>((url, cancellationToken) => requestUrl = url);
            var cloudClient = new CloudClient(cloudConnectionMock.Object);

            // act
            var testPackage = await cloudClient.GetTestPackage(new Tenant() { Name = "localbsg" }, "genymotiontest (8).ps1", "0.3", CancellationToken.None).ConfigureAwait(false);

            // assert
            Assert.Equal("/project/localbsg/api/testPackage", requestUrl);
            Assert.Equal("genymotiontest (8).ps1", testPackage.Name);
            Assert.Equal("PowerShellScript", testPackage.TestPackageType);
        }

        [Fact]
        public async Task ScheduleTestRunTest()
        {
            // arrange
            object requestContent = null;
            string requestUrl = string.Empty;

            var cloudConnectionMock = new Mock<ICloudConnection>();

            string testPackagesJson = File.ReadAllText("test_packages.json");
            cloudConnectionMock.Setup(cc => cc.GetRequest("/project/localbsg/api/testPackage", CancellationToken.None)).Returns(Task.FromResult(testPackagesJson));

            string applicationsJson = File.ReadAllText("applications.json");
            cloudConnectionMock.Setup(cc => cc.GetRequest("/project/localbsg/api/app", CancellationToken.None)).Returns(Task.FromResult(applicationsJson));

            string deviceGroupsJson = File.ReadAllText("device_groups.json");
            cloudConnectionMock.Setup(cc => cc.GetRequest("/project/localbsg/api/deviceGroup", CancellationToken.None)).Returns(Task.FromResult(deviceGroupsJson));

            string scheduleResponse = File.ReadAllText("schedule-testrun-response.json");
            cloudConnectionMock.Setup(cc => cc.PostJsonRequest(It.IsAny<string>(), It.IsAny<object>(), CancellationToken.None)).Returns(Task.FromResult(scheduleResponse)).Callback<string, object, CancellationToken>((url, content, cancellationToken) =>
             {
                 requestUrl = url;
                 requestContent = content;
             });

            var cloudClient = new CloudClient(cloudConnectionMock.Object);

            // act
            var tenant = new Tenant() { Name = "localbsg" };
            var testPackage = await cloudClient.GetTestPackage(tenant, "quamotion-test-acquaint", "0.0.1.50584433", CancellationToken.None).ConfigureAwait(false);
            var application = await cloudClient.GetApplication(tenant, "demo.quamotion.Acquaint", "151", "Android", CancellationToken.None).ConfigureAwait(false);
            var deviceGroup = await cloudClient.GetDeviceGroup(tenant, new Guid("4a7cf261-fa75-4a18-a564-718ebefba390"), CancellationToken.None).ConfigureAwait(false);
            var testRun = await cloudClient.ScheduleTestRun(tenant, testPackage, application, deviceGroup, CancellationToken.None).ConfigureAwait(false);

            // assert
            Assert.Equal("/project/localbsg/api/testRun", requestUrl);
            Assert.IsType<CreateTestRunRequest>(requestContent);
            var testRunRequest = requestContent as CreateTestRunRequest;
            Assert.Equal(deviceGroup.DeviceGroupId, testRunRequest.DeviceGroup.DeviceGroupId);
            Assert.Equal(application.AppId, testRunRequest.App.AppId);
            Assert.Equal(application.CpuType, testRunRequest.App.CpuType);
            Assert.Equal(application.DisplayName, testRunRequest.App.DisplayName);
            Assert.Equal(application.VersionDisplayName, testRunRequest.App.VersionDisplayName);
            Assert.Equal(application.OperatingSystem, testRunRequest.App.OperatingSystem);
            Assert.Equal(application.OperatingSystemVersion, testRunRequest.App.OperatingSystemVersion);
            Assert.Equal(application.Version, testRunRequest.App.Version);
            Assert.Equal(application.VersionDisplayName, testRunRequest.App.VersionDisplayName);
            Assert.Equal(string.Empty, testRunRequest.Schedule);
            Assert.Equal(testPackage.Name, testRunRequest.TestPackage.Name);
            Assert.Equal(testPackage.TestPackageType, testRunRequest.TestPackage.TestPackageType);
            Assert.Equal(testPackage.Version, testRunRequest.TestPackage.Version);
            Assert.Empty(testRunRequest.TestScriptEnvironmentVariables);
            Assert.Equal(string.Empty, testRunRequest.TestScriptParameters);

            Assert.NotNull(testRun);
            Assert.Empty(testRun.Jobs);
            Assert.Equal(deviceGroup.DeviceGroupId, testRun.DeviceGroupId);
            Assert.Equal(application.AppId, testRun.ApplicationId);
            Assert.Equal(application.Version, testRun.ApplicationVersion);
            Assert.Null(testRun.Branch);
            Assert.Null(testRun.CronSchedule);
            Assert.Equal(1, testRun.OperatingSystem);
            Assert.Equal(new DateTime(2018, 3, 20, 7, 11, 38), testRun.Scheduled);
            Assert.Null(testRun.ScheduleId);
            Assert.Equal(testPackage.Name, testRun.TestPackageName);
            Assert.Equal(testPackage.Version, testRun.TestPackageVersion);
            Assert.Equal(new Guid("a4188fd1-c4d3-4c5d-a952-c43ca816d3bf"), testRun.TestRunId);
        }

        [Fact]
        public async Task GetTestRunsTest()
        {
            // arrange
            string requestUrl = string.Empty;
            var cloudConnectionMock = new Mock<ICloudConnection>();
            string json = File.ReadAllText("test_runs.json");
            cloudConnectionMock.Setup(cc => cc.GetRequest(It.IsAny<string>(), CancellationToken.None)).Returns(Task.FromResult(json)).Callback<string, CancellationToken>((url, cancellationToken) => requestUrl = url);
            var cloudClient = new CloudClient(cloudConnectionMock.Object);

            // act
            var tenant = new Tenant() { Name = "localbsg" };
            var testRun = await cloudClient.GetTestRun(tenant, new Guid("6774bf25-c57a-4ba3-bc45-67545cd79976"), CancellationToken.None).ConfigureAwait(false);

            // assert
            Assert.Equal("/project/localbsg/api/testRun", requestUrl);
            Assert.Empty(testRun.Jobs);
            Assert.Equal(new Guid("1c3e3668-304b-4f3d-8cd5-1130d42b6d3e"), testRun.DeviceGroupId);
        }
    }
}
