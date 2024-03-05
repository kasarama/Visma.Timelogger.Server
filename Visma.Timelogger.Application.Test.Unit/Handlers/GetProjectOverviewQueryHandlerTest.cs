using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Visma.Timelogger.Application.Contracts;
using Visma.Timelogger.Application.Exceptions;
using Visma.Timelogger.Application.Features.GetProjectOverview;
using Visma.Timelogger.Application.VieModels;
using Visma.Timelogger.Domain.Entities;

namespace Visma.Timelogger.Application.Test.Unit.Handlers
{
    public class GetProjectOverviewQueryHandlerTest
    {
        private GetProjectOverviewQueryHandler _SUT;
        private Mock<AbstractValidator<GetProjectOverviewQuery>> _queryValidator;
        private Mock<ILogger<GetProjectOverviewQueryHandler>> _loggerMock;
        private Mock<IApiRequestValidator> _validatorMock;
        private Mock<IProjectRepository> _projectRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Project _existingProject;
        ProjectOverviewViewModel _existingProjectOverviewVM;
        private DateTime _now = DateTime.Now.Date;


        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<GetProjectOverviewQueryHandler>>();
            _queryValidator = new Mock<AbstractValidator<GetProjectOverviewQuery>>();
            _validatorMock = new Mock<IApiRequestValidator>();
            _projectRepositoryMock = new Mock<IProjectRepository>();
            _mapperMock = new Mock<IMapper>();

            _existingProject = new Project()
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                Deadline = _now.AddDays(2),
                FreelancerId = Guid.NewGuid(),
                IsActive = true,
                StartTime = _now.AddDays(-2)
            };

            _existingProjectOverviewVM = new ProjectOverviewViewModel()
            {
                Id = _existingProject.Id,
                CustomerId = _existingProject.CustomerId,
                Deadline = _existingProject.Deadline,
                FreelancerId = _existingProject.FreelancerId,
                IsActive = _existingProject.IsActive,
                StartTime = _existingProject.StartTime
            };

            _SUT = new GetProjectOverviewQueryHandler(
                _loggerMock.Object,
                _queryValidator.Object,
                _validatorMock.Object,
                _projectRepositoryMock.Object,
                _mapperMock.Object
            );
        }

        [Test]
        public async Task GivenValidRequest_Handle_ReturnsTrue()
        {
            var userId = _existingProject.FreelancerId;
            var projectId = _existingProject.Id;
            GetProjectOverviewQuery request = new GetProjectOverviewQuery(projectId, userId);

            _validatorMock.Setup(val => val
                 .ValidateRequest(request, It.IsAny<AbstractValidator<GetProjectOverviewQuery>>(), request.RequestId))
                 .ReturnsAsync(true);

            _projectRepositoryMock.Setup(repo => repo
                .GetByIdForFreelancerAsync(request.ProjectId, request.UserId))
                .ReturnsAsync(_existingProject);

            _mapperMock.Setup(mapper => mapper
                .Map<ProjectOverviewViewModel>(_existingProject))
                .Returns(_existingProjectOverviewVM);

            var result = await _SUT.Handle(request, CancellationToken.None);

            Assert.IsInstanceOf<ProjectOverviewViewModel>(result);

            _validatorMock
                .Verify(val => val
                    .ValidateRequest(request, It.IsAny<AbstractValidator<GetProjectOverviewQuery>>(), request.RequestId), Times.Once);
            _projectRepositoryMock
                .Verify(repo => repo
                    .GetByIdForFreelancerAsync(request.ProjectId, request.UserId), Times.Once);
            _mapperMock
                .Verify(mapper => mapper.Map<ProjectOverviewViewModel>(_existingProject), Times.Once);
        }

        [Test]
        public void GivenInvalidRequest_Handle_ThrowsException()
        {
            var userId = _existingProject.FreelancerId;
            var projectId = Guid.Empty;
            GetProjectOverviewQuery request = new GetProjectOverviewQuery(projectId, userId);

            _validatorMock.Setup(val => val
                 .ValidateRequest(request, It.IsAny<AbstractValidator<GetProjectOverviewQuery>>(), request.RequestId))
                 .ThrowsAsync(new RequestValidationException(new FluentValidation.Results.ValidationResult()));

            Assert.ThrowsAsync<RequestValidationException>(async () => await _SUT.Handle(request, CancellationToken.None));            

            _validatorMock
                .Verify(val => val
                    .ValidateRequest(request, It.IsAny<AbstractValidator<GetProjectOverviewQuery>>(), request.RequestId), Times.Once);
            _projectRepositoryMock
                .Verify(repo => repo
                    .GetByIdForFreelancerAsync(request.ProjectId, request.UserId), Times.Never);
            _mapperMock
                .Verify(mapper => mapper.Map<ProjectOverviewViewModel>(_existingProject), Times.Never);
        }

        [Test]
        public void GivenInvalidProjectId_Handle_ThrowsException()
        {
            var userId = _existingProject.FreelancerId;
            var projectId = Guid.NewGuid();
            GetProjectOverviewQuery request = new GetProjectOverviewQuery(projectId, userId);

            _validatorMock.Setup(val => val
                 .ValidateRequest(request, It.IsAny<AbstractValidator<GetProjectOverviewQuery>>(), request.RequestId))
                 .ReturnsAsync(true);

            _projectRepositoryMock.Setup(repo => repo
                .GetByIdForFreelancerAsync(request.ProjectId, request.UserId))
                .ReturnsAsync((Project)null);

            Assert.ThrowsAsync<BadRequestException>(async () => await _SUT.Handle(request, CancellationToken.None));

            _validatorMock
                .Verify(val => val
                    .ValidateRequest(request, It.IsAny<AbstractValidator<GetProjectOverviewQuery>>(), request.RequestId), Times.Once);
            _projectRepositoryMock
                .Verify(repo => repo
                    .GetByIdForFreelancerAsync(request.ProjectId, request.UserId), Times.Once);
            _mapperMock
                .Verify(mapper => mapper.Map<ProjectOverviewViewModel>(_existingProject), Times.Never);
        }

        [Test]
        public void GivenInvalidProject_Handle_ThrowsException()
        {
            var userId = _existingProject.FreelancerId;
            var projectId = Guid.NewGuid();
            GetProjectOverviewQuery request = new GetProjectOverviewQuery(projectId, userId);

            _validatorMock.Setup(val => val
                 .ValidateRequest(request, It.IsAny<AbstractValidator<GetProjectOverviewQuery>>(), request.RequestId))
                 .ReturnsAsync(true);

            _projectRepositoryMock.Setup(repo => repo
                .GetByIdForFreelancerAsync(request.ProjectId, request.UserId))
                .ReturnsAsync(_existingProject);

            _mapperMock.Setup(mapper => mapper
                .Map<ProjectOverviewViewModel>(_existingProject))
                .Throws(new AutoMapperMappingException());

            Assert.ThrowsAsync<AutoMapperMappingException>(async () => await _SUT.Handle(request, CancellationToken.None));

            _validatorMock
                .Verify(val => val
                    .ValidateRequest(request, It.IsAny<AbstractValidator<GetProjectOverviewQuery>>(), request.RequestId), Times.Once);
            _projectRepositoryMock
                .Verify(repo => repo
                    .GetByIdForFreelancerAsync(request.ProjectId, request.UserId), Times.Once);
            _mapperMock
                .Verify(mapper => mapper.Map<ProjectOverviewViewModel>(_existingProject), Times.Once);
        }
    }
}
