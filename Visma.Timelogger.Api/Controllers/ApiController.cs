using MediatR;
using Microsoft.AspNetCore.Mvc;
using Visma.Timelogger.Application.Features.CreateTimeRecord;
using Visma.Timelogger.Application.RequestModels;

namespace Visma.Timelogger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : Controller
    {

        private readonly IMediator _mediator;

        public ApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateTimeRecord", Name = "Create Time Record")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> CreateTimeRecord([FromBody] CreateTimeRecordRequestModel request)
        {
            Guid userId = (Guid)HttpContext.Items["UserId"];

            await _mediator.Send(new CreateTimeRecordCommand(request, userId));

            return StatusCode(201);
        }
    }
}
