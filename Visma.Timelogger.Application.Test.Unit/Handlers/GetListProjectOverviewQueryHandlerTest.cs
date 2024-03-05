using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Visma.Timelogger.Application.Contracts;
using Visma.Timelogger.Application.Exceptions;
using Visma.Timelogger.Application.Features.GetListProjectOverview;
using Visma.Timelogger.Application.VieModels;
using Visma.Timelogger.Domain.Entities;

namespace Visma.Timelogger.Application.Test.Unit.Handlers
{
    public class GetListProjectOverviewQueryHandlerTest
    {
        private GetListProjectOverviewQueryHandler _SUT;
        private Mock<AbstractValidator<GetListProjectOverviewQuery>> _queryValidator;
        private Mock<ILogger<GetListProjectOverviewQueryHandler>> _loggerMock;
        private Mock<IApiRequestValidator> _validatorMock;
        private Mock<IProjectRepository> _projectRepositoryMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<GetListProjectOverviewQueryHandler>>();
            _queryValidator = new Mock<AbstractValidator<GetListProjectOverviewQuery>>();
            _validatorMock = new Mock<IApiRequestValidator>();
            _projectRepositoryMock = new Mock<IProjectRepository>();
            _mapperMock = new Mock<IMapper>();
            _SUT = new GetListProjectOverviewQueryHandler(
                _loggerMock.Object,
                _queryValidator.Object,
                _validatorMock.Object,
                _projectRepositoryMock.Object,
                _mapperMock.Object);
        }

        [Test]
        public async Task GivenValidRequest_Handle_ReturnsTrue()
        {
            var userId = Guid.NewGuid();
            var projects = new List<Project>();
            var models = new List<ProjectOverviewViewModel>();

            GetListProjectOverviewQuery request = new GetListProjectOverviewQuery(userId);

            _validatorMock.Setup(val => val
                 .ValidateRequest(request, It.IsAny<AbstractValidator<GetListProjectOverviewQuery>>(), request.RequestId))
                 .ReturnsAsync(true);

            _projectRepositoryMock.Setup(repo => repo
                .GetListForFreelancerAsync(userId))
                .ReturnsAsync(projects);

            _mapperMock.Setup(mapper => mapper
                .Map<List<ProjectOverviewViewModel>>(projects))
                .Returns(models);

            var result = await _SUT.Handle(request, CancellationToken.None);

            Assert.IsInstanceOf<List<ProjectOverviewViewModel>>(result);

            _validatorMock
                .Verify(val => val
                    .ValidateRequest(request, It.IsAny<AbstractValidator<GetListProjectOverviewQuery>>(), request.RequestId), Times.Once);
            _projectRepositoryMock
                .Verify(repo => repo
                .GetListForFreelancerAsync(userId), Times.Once);
            _mapperMock
                .Verify(mapper => mapper.Map<List<ProjectOverviewViewModel>>(projects), Times.Once);
        }

        [Test]
        public void GivenInvalidRequest_Handle_ReturnsTrue()
        {
            var userId = Guid.NewGuid();
            var projects = new List<Project>();
            var models = new List<ProjectOverviewViewModel>();

            GetListProjectOverviewQuery request = new GetListProjectOverviewQuery(userId);

            _validatorMock.Setup(val => val
                 .ValidateRequest(request, It.IsAny<AbstractValidator<GetListProjectOverviewQuery>>(), request.RequestId))
                 .ThrowsAsync(new RequestValidationException(new FluentValidation.Results.ValidationResult()));

            Assert.ThrowsAsync<RequestValidationException>(async () => await _SUT.Handle(request, CancellationToken.None));

            _validatorMock
                .Verify(val => val
                    .ValidateRequest(request, It.IsAny<AbstractValidator<GetListProjectOverviewQuery>>(), request.RequestId), Times.Once);
            _projectRepositoryMock
                .Verify(repo => repo
                .GetListForFreelancerAsync(userId), Times.Never);
            _mapperMock
                .Verify(mapper => mapper.Map<List<ProjectOverviewViewModel>>(projects), Times.Never);
        }
    }
}
