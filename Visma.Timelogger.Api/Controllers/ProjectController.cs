using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Visma.Timelogger.Application.Contracts;
using Visma.Timelogger.Application.RequestModels;
using Visma.Timelogger.Domain.Entities;
using Visma.Timelogger.Persistence;

namespace Visma.Timelogger.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProjectController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IProjectRepository _projectRepository;

        public ProjectController(IMediator mediator, ProjectDbContext projectDbContext, IProjectRepository projectRepository)
        {
            _mediator = mediator;
            _projectRepository = projectRepository;
        }

        [HttpPost("Create", Name = "Create New Project")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> Create([FromBody] CreateProjectRequestModel request)
        {
            await _mediator.Send(request);
            return StatusCode(202);
        }

        [HttpGet("AllProjects", Name = "Get All Projects")]
        public async Task<ActionResult<Project[]>> GetAll()
        {
           var result = await _projectRepository.ListAllWithTimeRecordsAsync();
            return  Ok(result);
        }

    }
}
