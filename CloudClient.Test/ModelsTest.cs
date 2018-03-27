using Quamotion.Cloud.Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Quamotion.Cloud.Client.Test
{
    public class ModelsTest
    {
        [Fact]
        public void SchedulesTest()
        {
            string json = File.ReadAllText("schedules.json");
            var schedules = JsonConvert.DeserializeObject<List<TestRunSchedule>>(json);
            Assert.Single(schedules);
            var schedule = schedules[0];

            Assert.True(schedule.Active);
            Assert.Equal(new DateTime(636577700036360000), schedule.CreatedAt);
            Assert.Equal("0 */4 * * *", schedule.Cron);
            Assert.Equal("UTC", schedule.CronTimezone);
            Assert.Equal("Pipeline for test run 8195e174-3a5e-4152-99a6-120b5537073f", schedule.Description);
            Assert.Equal(6031, schedule.Id);
            Assert.Equal(new DateTime(636577776000000000), schedule.NextRunAt);
            Assert.Equal("schedules/8195e1743a5e415299a6120b5537073f", schedule.Ref);
            Assert.Equal(new DateTime(636577700036360000), schedule.UpdatedAt);
        }

        [Fact]
        public void ApplicationsTest()
        {
            string json = File.ReadAllText("applications.json");
            var applications = JsonConvert.DeserializeObject<ApplicationResponse>(json);
            Assert.Equal(2, applications.Count);
            var application = applications[0];

            Assert.Equal("demo.quamotion.Acquaint", application.AppId);
            Assert.Equal("ARMv5AndAbove, x86AndAbove", application.CpuType);
            Assert.Equal("Acquaint N", application.DisplayName);
            Assert.Equal("Android", application.OperatingSystem);
            Assert.Equal("4.2", application.OperatingSystemVersion);
            Assert.Equal("151", application.Version);
            Assert.Equal("1.51", application.VersionDisplayName);
        }

        [Fact]
        public void DeviceGroupTest()
        {
            string json = File.ReadAllText("device_groups.json");
            var deviceGroups = JsonConvert.DeserializeObject<DeviceGroupResponse>(json);
            Assert.Equal(37, deviceGroups.Count);
            var deviceGroup = deviceGroups[0];

            Assert.Equal("9415b293-9f73-4dd2-865e-4b7e02d11bd4", deviceGroup.DeviceGroupId.ToString());
            Assert.Equal(26, deviceGroup.Devices.Count);
            Assert.Equal("74ad0f8b-90f5-47c5-bc7a-9c05b04de4ca", deviceGroup.Devices[0].DeviceSelectionId);
            Assert.Single(deviceGroup.Devices[0].Variables);
            Assert.Equal("GENYMOTION_TEMPLATE", deviceGroup.Devices[0].Variables.First().Key);
            Assert.Equal("74ad0f8b-90f5-47c5-bc7a-9c05b04de4ca", deviceGroup.Devices[0].Variables.First().Value);
            Assert.Equal("Google Pixel - Android 8.0 (Genymotion)", deviceGroup.Devices[0].DisplayName);
            Assert.Equal(new List<string> { "genymotion" }, deviceGroup.Devices[0].Tags);

            Assert.Equal("Genymotion top 25 devices", deviceGroup.DisplayName);
            Assert.Equal("genymotion-top-25-devices", deviceGroup.Name);
            Assert.Equal(new Dictionary<string, string>(), deviceGroup.Variables);
        }

        [Fact]
        public void TenantsTest()
        {
            string json = File.ReadAllText("tenants.json");
            var tenants = JsonConvert.DeserializeObject<ProjectResponse>(json);

            Assert.Equal(7, tenants.Count);
            var tenant = tenants[5];

            Assert.Equal(0, tenant.Flags);
            Assert.Equal(5336851, tenant.GitLabId);
            Assert.Equal("genymotion", tenant.Name);
            Assert.Equal("/project/genymotion/", tenant.RelativeUrl);
            Assert.Equal(30, tenant.TenantId);

            tenant = tenants[0];
            Assert.Null(tenant.GitLabId);
        }

        [Fact]
        public void TestJobsTest()
        {
            string json = File.ReadAllText("test_jobs.json");
            var testJobs = JsonConvert.DeserializeObject<TestJobResponse>(json);
            Assert.Equal(20, testJobs.Count);
            var testJob = testJobs[5];

            Assert.Equal("0b4661ee5fe19329a41bdca62fe190a9780b62f2", testJob.CommitId);
            Assert.Equal(224.844612, testJob.Duration);
            Assert.False(testJob.HasArtifacts);
            Assert.Equal(57828277, testJob.Id);
            Assert.Equal("4a7cf261-fa75-4a18-a564-718ebefba390", testJob.Name);
            Assert.Equal(new DateTime(2018, 3, 16, 14, 2, 32, 132), testJob.Started);
            Assert.Equal("canceled", testJob.Status);
        }

        [Fact]
        public void TestPackagesTest()
        {
            string json = File.ReadAllText("test_packages.json");
            var testPackages = JsonConvert.DeserializeObject<TestPackageResponse>(json);
            Assert.Equal(32, testPackages.Count);
            var testPackage = testPackages[5];

            Assert.Equal("genymotiontest.ps1", testPackage.Name);
            Assert.Equal("PowerShellScript", testPackage.TestPackageType);
            Assert.Equal("0.1.0", testPackage.Version);
        }


        [Fact]
        public void TestRunsTest()
        {
            string json = File.ReadAllText("test_runs.json");
            var testRuns = JsonConvert.DeserializeObject<TestRunResponse>(json);
            Assert.Equal(122, testRuns.Count);
            var testRun = testRuns[5];

            Assert.Equal("demo.quamotion.Acquaint", testRun.ApplicationId);
            Assert.Equal("151", testRun.ApplicationVersion);

            Assert.Null(testRun.Branch);
            Assert.Null(testRun.CronSchedule);
            Assert.Equal("2f71872c-8bbc-4260-a284-7c6f50ede169", testRun.DeviceGroupId.ToString());
            Assert.Empty(testRun.Jobs);
            Assert.Equal(1, testRun.OperatingSystem);
            Assert.Equal(new DateTime(2018, 3, 16, 11, 35, 50), testRun.Scheduled);
            Assert.Null(testRun.ScheduleId);
            Assert.Equal("genymotiontest.ps1", testRun.TestPackageName);
            Assert.Equal("0.1.0", testRun.TestPackageVersion);
            Assert.Equal("f6e017fa-7e06-4938-82de-35071793d985", testRun.TestRunId.ToString());
        }
    }
}