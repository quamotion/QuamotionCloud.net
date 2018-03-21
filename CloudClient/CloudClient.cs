using Quamotion.Cloud.Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task Login(string apiToken)
        {
            await this.CloudConnection.Login(apiToken).ConfigureAwait(false);
        }

        public async Task<List<Tenant>> GetTenants()
        {
            string response = await this.CloudConnection.GetRequest("/api/project").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<ProjectResponse>(response);
        }


        public async Task<TestPackage> PublishTestPackage(Tenant tenant, string filePath)
        {
            string response = await this.CloudConnection.SendFile("/project/" + tenant.Name + "/api/testPackage", filePath).ConfigureAwait(false);
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

        public async Task<Application> PublishApplication(Tenant tenant, string filePath)
        {
            string response = await this.CloudConnection.SendFile("/project/" + tenant.Name + "/api/app", filePath).ConfigureAwait(false);

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

        public async Task<List<Application>> GetApplications(Tenant tenant)
        {
            string response = await this.CloudConnection.GetRequest("/project/" + tenant.Name + "/api/app").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<ApplicationResponse>(response);
        }

        public async Task<List<TestPackage>> GetTestPackages(Tenant tenant)
        {
            string response = await this.CloudConnection.GetRequest("/project/" + tenant.Name + "/api/testPackage").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<TestPackageResponse>(response);
        }

        public async Task<List<DeviceGroup>> GetDeviceGroups(Tenant tenant)
        {
            string response = await this.CloudConnection.GetRequest("/project/" + tenant.Name + "/api/deviceGroup").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<DeviceGroupResponse>(response);
        }

        public async Task<DeviceGroup> GetDeviceGroup(Tenant tenant, Guid deviceGroupId)
        {
            List<DeviceGroup> deviceGroups = await this.GetDeviceGroups(tenant).ConfigureAwait(false);
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

        public async Task<Application> GetApplication(Tenant tenant, string applicationId, string applicationVersion, string operatingSystem)
        {
            List<Application> applications = await this.GetApplications(tenant).ConfigureAwait(false);
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

        public async Task<TestPackage> GetTestPackage(Tenant tenant, string name, string version)
        {
            List<TestPackage> testPackages = await this.GetTestPackages(tenant).ConfigureAwait(false);
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

        public async Task<TestRun> ScheduleTestRun(Tenant tenant, TestPackage testPackage, Application application, Guid deviceGroupId, string schedule, Dictionary<string, string> testScriptEnvironmentVariables, string testScriptParameters)
        {
            CreateTestRunRequest createTestRunRequest = new CreateTestRunRequest()
            {
                App = application,
                DeviceGroupId = deviceGroupId,
                Schedule = schedule,
                TestPackage = testPackage,
                TestScriptEnvironmentVariables = testScriptEnvironmentVariables,
                TestScriptParameters = testScriptParameters
            };

            string response = await this.CloudConnection.PostJsonRequest("/project/" + tenant.Name + "/api/testRun", createTestRunRequest).ConfigureAwait(false);

            return JsonConvert.DeserializeObject<TestRun>(response);
        }

        public async Task<TestRun> ScheduleTestRun(Tenant tenant, TestPackage testPackage, Application application, DeviceGroup deviceGroup)
        {
            return await this.ScheduleTestRun(tenant, testPackage, application, deviceGroup.DeviceGroupId, string.Empty, new Dictionary<string, string>(), string.Empty).ConfigureAwait(false);
        }

        public async Task<TestRun> GetTestRun(Tenant tenant, Guid testRunId)
        {
            string response = await this.CloudConnection.GetRequest("/project/" + tenant.Name + "/api/testRun").ConfigureAwait(false);
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

        public async Task<List<TestJob>> GetTestJobs(Tenant tenant, TestRun testRun)
        {
            string response = await this.CloudConnection.GetRequest("/project/" + tenant.Name + "/api/testRun/" + testRun.TestRunId + "/jobs").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<TestJobResponse>(response);
        }

        public async Task<TestJob> GetTestJob(Tenant tenant, int jobId)
        {
            string response = await this.CloudConnection.GetRequest("/project/" + tenant.Name + "/api/job/" + jobId).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<TestJob>(response);
        }

        public async Task<string> GetJobLog(Tenant tenant, TestJob testJob)
        {
            return await this.CloudConnection.GetRequest("/project/" + tenant.Name + "/api/job/" + testJob.Id + "/log").ConfigureAwait(false);
        }
    }
}