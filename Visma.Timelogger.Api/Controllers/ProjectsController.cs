﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Visma.Timelogger.Application.Features.CreateTimeRecord;
using Visma.Timelogger.Application.Features.GetProjectOverview;
using Visma.Timelogger.Application.RequestModels;
using Visma.Timelogger.Application.VieModels;

namespace Visma.Timelogger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : Controller
    {

        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
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

        [HttpGet("GetProjectOverview/{projectId}", Name = "Get Overview Of Project With Time Registrations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProjectOverviewViewModel>> GetProjectOverview([FromRoute] Guid projectId)
        {
            Guid userId = (Guid)HttpContext.Items["UserId"];
            ProjectOverviewViewModel result = await _mediator.Send(new GetProjectOverviewQuery(projectId, userId));
            return Ok(result);
        }

    }
}
