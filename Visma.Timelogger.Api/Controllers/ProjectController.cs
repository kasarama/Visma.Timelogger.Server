using MediatR;
using Microsoft.AspNetCore.Mvc;
using Visma.Timelogger.Application.RequestModels;

namespace Visma.Timelogger.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProjectController : Controller
    {
        private readonly IMediator _mediator;

        public ProjectController(IMediator mediator)
        {
            _mediator = mediator;
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


    }
}
