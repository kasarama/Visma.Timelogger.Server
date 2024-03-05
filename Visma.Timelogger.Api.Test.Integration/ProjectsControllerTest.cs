using Azure.Core;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;
using Visma.Timelogger.Api.Middleware;
using Visma.Timelogger.Api.Test.Integration.Base;
using Visma.Timelogger.Application.RequestModels;

namespace Visma.Timelogger.Api.Test.Integration
{
    public class ProjectsControllerTest
    {
        private readonly ApiFactory<Program> _factory;
        private readonly string _dbName = "ApiControllerTestDb";
        private readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            IncludeFields = true
        };
        public ProjectsControllerTest()
        {
            _factory = new ApiFactory<Program>(_dbName);
        }


        [Test]
        public async Task GivenMissingUserHeader_GetHome_ThrowsException()
        {
            var client = _factory.GetAnonymousClient();
            var response = await client.GetAsync("/");

            Assert.That(response.StatusCode.Equals(HttpStatusCode.Unauthorized));
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.That(responseString.Equals("Authorization header is missing."));
        }
        [Test]
        public async Task GivenInvalidUserHeader_GetHome_ThrowsException()
        {
            var client = _factory.GetAnonymousClient();
            client.DefaultRequestHeaders.Add("User", "invalid");
            var response = await client.GetAsync("/");

            Assert.That(response.StatusCode.Equals(HttpStatusCode.Unauthorized));
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.That(responseString.Equals("Authorization header is invalid."));
        }

        [Test]
        public async Task GivenValidUserHeader_GetHome_ThrowsException()
        {
            var client = _factory.GetAnonymousClient();
            client.DefaultRequestHeaders.Add("User", "freelancer1");
            var response = await client.GetAsync("/");

            Assert.That(response.StatusCode.Equals(HttpStatusCode.OK));
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.That(responseString.Equals("Visma e-conomic Time Logger"));
        }

        [Test]
        public async Task GivenValidRequest_CreateTimeRecord_Returns201()
        {
            var client = _factory.GetAnonymousClient();
            client.DefaultRequestHeaders.Add("User", "freelancer1");

            var body = new CreateTimeRecordRequestModel()
            {
                DurationMinutes = 66,
                ProjectId = TestData.ActiveProjectId,
                StartTime = TestData.Now
            };

            var response = await client.PostAsync("/api/Projects/CreateTimeRecord", ContentHelper.GetStringContent(body));
            Assert.That(response.StatusCode.Equals(HttpStatusCode.Created));
        }

        [Test]
        public async Task GivenMissingUserHeader_CreateTimeRecord_Returns401()
        {
            var client = _factory.GetAnonymousClient();

            var body = new CreateTimeRecordRequestModel()
            {
                DurationMinutes = 65,
                ProjectId = TestData.ActiveProjectId,
                StartTime = TestData.Now
            };

            var response = await client.PostAsync("/api/Projects/CreateTimeRecord", ContentHelper.GetStringContent(body));
            Assert.That(response.StatusCode.Equals(HttpStatusCode.Unauthorized));
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.That(responseString.Equals("Authorization header is missing."));
        }

        [Test]
        public async Task GivenInvalidUserHeader_CreateTimeRecord_Returns401()
        {
            var client = _factory.GetAnonymousClient();
            client.DefaultRequestHeaders.Add("User", "invalid");

            var body = new CreateTimeRecordRequestModel()
            {
                DurationMinutes = 64,
                ProjectId = TestData.ActiveProjectId,
                StartTime = TestData.Now
            };

            var response = await client.PostAsync("/api/Projects/CreateTimeRecord", ContentHelper.GetStringContent(body));
            Assert.That(response.StatusCode.Equals(HttpStatusCode.Unauthorized));
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.That(responseString.Equals("Authorization header is invalid."));
        }

        [Test]
        public async Task GivenInactiveProjectId_CreateTimeRecord_Returns400()
        {
            var client = _factory.GetAnonymousClient();
            client.DefaultRequestHeaders.Add("User", "freelancer1");

            var body = new CreateTimeRecordRequestModel()
            {
                DurationMinutes = 63,
                ProjectId = TestData.InactiveProjectId,
                StartTime = TestData.Now
            };

            var response = await client.PostAsync("/api/Projects/CreateTimeRecord", ContentHelper.GetStringContent(body));
            Assert.That(response.StatusCode.Equals(HttpStatusCode.BadRequest));
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ErrorDto>(responseString, options);
            Assert.That(result.Message.Equals($"Cannot register Time for Project {body.ProjectId}."));
        }

        [Test]
        public async Task GivenFutureStartTime_CreateTimeRecord_Returns400()
        {
            var client = _factory.GetAnonymousClient();
            client.DefaultRequestHeaders.Add("User", "freelancer1");

            var body = new CreateTimeRecordRequestModel()
            {
                DurationMinutes = 62,
                ProjectId = TestData.ActiveProjectId,
                StartTime = DateTime.Now.AddDays(1)
            };

            var response = await client.PostAsync("/api/Projects/CreateTimeRecord", ContentHelper.GetStringContent(body));
            Assert.That(response.StatusCode.Equals(HttpStatusCode.BadRequest));
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ErrorDto>(responseString, options);
            Assert.That(result.Message.Equals("Time Registration cannot be in the future."));
        }

        [Test]
        public async Task GivenStartTimeOutsideProjectPeriod_CreateTimeRecord_Returns400()
        {
            var client = _factory.GetAnonymousClient();
            client.DefaultRequestHeaders.Add("User", "freelancer1");

            var body = new CreateTimeRecordRequestModel()
            {
                DurationMinutes = 61,
                ProjectId = TestData.ActiveProjectId,
                StartTime = TestData.ActiveProject().StartTime.AddDays(-1)
            };

            var response = await client.PostAsync("/api/Projects/CreateTimeRecord", ContentHelper.GetStringContent(body));
            Assert.That(response.StatusCode.Equals(HttpStatusCode.BadRequest));
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ErrorDto>(responseString, options);
            Assert.That(result.Message.Equals("Time registration is outside the project time period"));
        }

        [Test]
        public async Task GivenInvalidRequest_CreateTimeRecord_Returns400()
        {
            var client = _factory.GetAnonymousClient();
            client.DefaultRequestHeaders.Add("User", "freelancer2");

            var body = new CreateTimeRecordRequestModel()
            {
                DurationMinutes = 20
            };

            var response = await client.PostAsync("/api/Projects/CreateTimeRecord", ContentHelper.GetStringContent(body));
            Assert.That(response.StatusCode.Equals(HttpStatusCode.BadRequest));
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ValidationErrorDto>(responseString, options);
            Assert.That(result.Message.Equals("Invalid request"));
        }
    }
}
