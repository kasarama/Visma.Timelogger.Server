﻿using Azure.Core;
using System.Net;
using System.Text.Json;
using Visma.Timelogger.Api.Middleware;
using Visma.Timelogger.Api.Test.Integration.Base;
using Visma.Timelogger.Application.RequestModels;
using Visma.Timelogger.Application.VieModels;

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
        public async Task GivenValidRequest_CreateTimeRecord_ReturnsGuid()
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
            Assert.That(response.StatusCode.Equals(HttpStatusCode.OK));
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Guid>(responseString, options);
            Assert.IsInstanceOf<Guid>(result);
            Assert.That(result, Is.Not.EqualTo(Guid.Empty));
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
                StartTime = DateTime.UtcNow.AddDays(1)
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
                StartTime = TestData.ActiveProject.StartTime.AddDays(-1)
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

        [Test]
        public async Task GivenValidRequest_GetProjectOverview_Returns200()
        {
            var client = _factory.GetAnonymousClient();
            client.DefaultRequestHeaders.Add("User", "freelancer1");

            var response = await client.GetAsync($"/api/Projects/GetProjectOverview/{TestData.ActiveProject.Id}");
            Assert.That(response.StatusCode.Equals(HttpStatusCode.OK));
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ProjectOverviewViewModel>(responseString, options);
            Assert.That(result.Id.Equals(TestData.ActiveProject.Id));
        }

        [Test]
        public async Task GivenInvalidRequest_GetProjectOverview_Returns400()
        {
            var client = _factory.GetAnonymousClient();
            client.DefaultRequestHeaders.Add("User", "freelancer1");

            var response = await client.GetAsync($"/api/Projects/GetProjectOverview/{Guid.Empty}");
            Assert.That(response.StatusCode.Equals(HttpStatusCode.BadRequest));
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ValidationErrorDto>(responseString, options);
            Assert.That(result.Message.Equals("Invalid request"));
        }

        [Test]
        public async Task GivenInvalidProjectId_GetProjectOverview_Returns400()
        {
            var client = _factory.GetAnonymousClient();
            client.DefaultRequestHeaders.Add("User", "freelancer1");
            Guid invalidProjectId = Guid.NewGuid();
            var response = await client.GetAsync($"/api/Projects/GetProjectOverview/{invalidProjectId}");
            Assert.That(response.StatusCode.Equals(HttpStatusCode.BadRequest));
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ErrorDto>(responseString, options);
            Assert.That(result.Message.Equals($"Cannot find Project {invalidProjectId} for Freelancer {TestData.FreelancerId}."));
        }

        [Test]
        public async Task GivenValidRequest_GetListProjectOverview_Returns200()
        {
            var client = _factory.GetAnonymousClient();
            client.DefaultRequestHeaders.Add("User", "freelancer1");

            var response = await client.GetAsync($"/api/Projects/GetListProjectOverview");
            Assert.That(response.StatusCode.Equals(HttpStatusCode.OK));
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<ProjectOverviewViewModel>>(responseString, options);
            Assert.That(result.Count.Equals(2));
        }

        [Test]
        public async Task GivenInvalidRequest_GetListProjectOverview_Returns401()
        {
            var client = _factory.GetAnonymousClient();
            client.DefaultRequestHeaders.Add("User", "invalid");

            var response = await client.GetAsync($"/api/Projects/GetListProjectOverview");
            Assert.That(response.StatusCode.Equals(HttpStatusCode.Unauthorized));
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.That(responseString.Equals("Authorization header is invalid."));
        }
    }
}
