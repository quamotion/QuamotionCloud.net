using Quamotion.Cloud.Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Quamotion.Cloud.Client
{
    public class CloudClient
    {
        private ICloudConnection CloudConnection { get; set; }

        public CloudClient(ICloudConnection cloudConnection)
        {
            this.CloudConnection = cloudConnection;
        }

        public CloudClient(string host)
            : this(new CloudConnection(host))
        {
        }

        public CloudClient()
            : this("https://cloud.quamotion.mobi")
        {
        }

        public async Task Login(string apiToken, CancellationToken cancellationToken)
        {
            await this.CloudConnection.Login(apiToken, cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<Tenant>> GetTenants(CancellationToken cancellationToken)
        {
            string response = await this.CloudConnection.GetRequest("/api/project", cancellationToken).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<ProjectResponse>(response);
        }


        public async Task<TestPackage> PublishTestPackage(Tenant tenant, string filePath, CancellationToken cancellationToken)
        {
            string response = await this.CloudConnection.SendFile("/project/" + tenant.Name + "/api/testPackage", filePath, cancellationToken).ConfigureAwait(false);
            TestPackageResponse testPackages = JsonConvert.DeserializeObject<TestPackageResponse>(response);
            if (testPackages.Count == 0)
            {
                throw new InvalidOperationException("No test package was added for: " + filePath);
            }

            if (testPackages.Count > 1)
            {
                throw new InvalidOperationException("More than one test package were added for: " + filePath);
            }

            return testPackages[0];
        }

        public async Task<Application> PublishApplication(Tenant tenant, string filePath, CancellationToken cancellationToken)
        {
            string response = await this.CloudConnection.SendFile("/project/" + tenant.Name + "/api/app", filePath, cancellationToken).ConfigureAwait(false);

            List<Application> applications = JsonConvert.DeserializeObject<ApplicationResponse>(response);

            if (applications.Count == 0)
            {
                throw new InvalidOperationException("No application was added for: " + filePath);
            }

            if (applications.Count > 1)
            {
                throw new InvalidOperationException("More than one application was added for: " + filePath);
            }

            return applications.LastOrDefault();
        }

        public async Task<List<Application>> GetApplications(Tenant tenant, CancellationToken cancellationToken)
        {
            string response = await this.CloudConnection.GetRequest("/project/" + tenant.Name + "/api/app", cancellationToken).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<ApplicationResponse>(response);
        }

        public async Task<List<TestPackage>> GetTestPackages(Tenant tenant, CancellationToken cancellationToken)
        {
            string response = await this.CloudConnection.GetRequest("/project/" + tenant.Name + "/api/testPackage", cancellationToken).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<TestPackageResponse>(response);
        }

        public async Task<List<DeviceGroup>> GetDeviceGroups(Tenant tenant, CancellationToken cancellationToken)
        {
            string response = await this.CloudConnection.GetRequest("/project/" + tenant.Name + "/api/deviceGroup", cancellationToken).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<DeviceGroupResponse>(response);
        }

        public async Task<DeviceGroup> GetDeviceGroup(Tenant tenant, Guid deviceGroupId, CancellationToken cancellationToken)
        {
            List<DeviceGroup> deviceGroups = await this.GetDeviceGroups(tenant, cancellationToken).ConfigureAwait(false);
            DeviceGroup result = null;
            foreach (DeviceGroup deviceGroup in deviceGroups)
            {
                if (deviceGroup.DeviceGroupId == deviceGroupId)
                {
                    result = deviceGroup;
                }
            }

            return result;
        }

        public async Task<Application> GetApplication(Tenant tenant, string applicationId, string applicationVersion, string operatingSystem, CancellationToken cancellationToken)
        {
            List<Application> applications = await this.GetApplications(tenant, cancellationToken).ConfigureAwait(false);
            Application result = null;
            foreach (Application application in applications)
            {
                if (application.AppId == applicationId && application.Version == applicationVersion && application.OperatingSystem == operatingSystem)
                {
                    result = application;
                }
            }

            return result;
        }

        public async Task<TestPackage> GetTestPackage(Tenant tenant, string name, string version, CancellationToken cancellationToken)
        {
            List<TestPackage> testPackages = await this.GetTestPackages(tenant, cancellationToken).ConfigureAwait(false);
            TestPackage result = null;
            foreach (TestPackage testPackage in testPackages)
            {
                if (testPackage.Name == name && testPackage.Version == version)
                {
                    result = testPackage;
                }
            }

            return result;
        }

        public async Task CancelTestRunSchedule(Tenant tenant, int scheduleId,CancellationToken cancellationToken)
        {
            await this.CloudConnection.DeleteRequest("/project/" + tenant.Name + "/api/schedule/" + scheduleId, cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<TestRunSchedule>> GetTestRunSchedules(Tenant tenant, CancellationToken cancellationToken)
        {
            string response = await this.CloudConnection.GetRequest("/project/" + tenant.Name + "/api/schedule", cancellationToken).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<List<TestRunSchedule>>(response);
        }

        public async Task<TestRun> ScheduleTestRun(Tenant tenant, TestPackage testPackage, Application application, DeviceGroup deviceGroup, string schedule, ResultsCallBack resultsCallBack, Dictionary<string, string> testScriptEnvironmentVariables, string testScriptParameters, CancellationToken cancellationToken)
        {
            CreateTestRunRequest createTestRunRequest = new CreateTestRunRequest()
            {
                App = application,
                DeviceGroup = deviceGroup,
                Schedule = schedule,
                TestPackage = testPackage,
                TestScriptEnvironmentVariables = testScriptEnvironmentVariables,
                TestScriptParameters = testScriptParameters,
                ResultsCallBack = resultsCallBack
            };

            string response = await this.CloudConnection.PostJsonRequest("/project/" + tenant.Name + "/api/testRun", createTestRunRequest, cancellationToken).ConfigureAwait(false);

            return JsonConvert.DeserializeObject<TestRun>(response);
        }

        public async Task<TestRun> ScheduleTestRun(Tenant tenant, TestPackage testPackage, Application application, DeviceGroup deviceGroup, CancellationToken cancellationToken)
        {
            return await this.ScheduleTestRun(tenant, testPackage, application, deviceGroup, string.Empty, null, new Dictionary<string, string>(), string.Empty, cancellationToken).ConfigureAwait(false);
        }

        public async Task<TestRun> ScheduleTestRun(Tenant tenant, TestPackage testPackage, Application application, string deviceGroupName, List<string> deviceGroupTags, CancellationToken cancellationToken)
        {
            var deviceGroup = new DeviceGroup()
            {
                DeviceGroupId = Guid.NewGuid(),
                Name = deviceGroupName,
                Devices = new List<DeviceSelection>()
                {
                    new DeviceSelection()
                    {
                        DeviceSelectionId = Guid.NewGuid().ToString(),
                        Name = deviceGroupName,
                        Tags = deviceGroupTags
                    }
                }
            };

            return await this.ScheduleTestRun(tenant, testPackage, application, deviceGroup, string.Empty, null, new Dictionary<string, string>(), string.Empty, cancellationToken).ConfigureAwait(false);
        }

        public async Task<TestRun> GetTestRun(Tenant tenant, Guid testRunId, CancellationToken cancellationToken)
        {
            string response = await this.CloudConnection.GetRequest("/project/" + tenant.Name + "/api/testRun", cancellationToken).ConfigureAwait(false);
            var testRuns = JsonConvert.DeserializeObject<TestRunResponse>(response);

            TestRun result = null;
            foreach (TestRun testRun in testRuns)
            {
                if (testRun.TestRunId == testRunId)
                {
                    result = testRun;
                }
            }

            return result;
        }

        public async Task<List<TestJob>> GetTestJobs(Tenant tenant, TestRun testRun, CancellationToken cancellationToken)
        {
            string response = await this.CloudConnection.GetRequest("/project/" + tenant.Name + "/api/testRun/" + testRun.TestRunId + "/jobs", cancellationToken).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<TestJobResponse>(response);
        }

        public async Task<TestJob> GetTestJob(Tenant tenant, int jobId, CancellationToken cancellationToken)
        {
            string response = await this.CloudConnection.GetRequest("/project/" + tenant.Name + "/api/job/" + jobId, cancellationToken).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<TestJob>(response);
        }

        public async Task<string> GetJobLog(Tenant tenant, TestJob testJob, CancellationToken cancellationToken)
        {
            return await this.CloudConnection.GetRequest("/project/" + tenant.Name + "/api/job/" + testJob.Id + "/log", cancellationToken).ConfigureAwait(false);
        }
    }
}