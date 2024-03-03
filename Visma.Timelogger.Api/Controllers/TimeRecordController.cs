using MediatR;
using Microsoft.AspNetCore.Mvc;
using Visma.Timelogger.Application.Features.CreateTimeRecord;
using Visma.Timelogger.Application.RequestModels;

namespace Visma.Timelogger.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TimeRecordController : Controller
    {

        private readonly IMediator _mediator;

        public TimeRecordController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Create", Name = "Create New Tome Record")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> Create([FromBody] CreateTimeRecordRequestModel request)
        {
            Guid userId = (Guid)HttpContext.Items["UserId"];


            await _mediator.Send(new CreateTimeRecordCommand(request, userId));

            return StatusCode(201);
        }
    }
}
