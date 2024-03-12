using Microsoft.AspNetCore.Mvc;
using Visma.Timelogger.Application.Contracts;
using Visma.Timelogger.Application.Events.Sub;

namespace Visma.Timelogger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : Controller
    {
        private readonly IEventBusService _eventBusService;
        private int count = 0;
        public TestController(IEventBusService eventBusService)
        {
            _eventBusService = eventBusService;
        }

        [HttpGet("CreateRandomProject", Name = "Create New Project For freelancer2 and customer1")]

        public async Task PublishRandomProject()
        {
            await _eventBusService.PublishEvent(new ProjectCreatedEvent()
            {
                EventId = Guid.NewGuid(),
                AggregateId = Guid.NewGuid(),
                FreelancerId = Guid.Parse("DD330056-EE5A-451B-AC2C-AF0CB20EB213"),
                CustomerId = Guid.Parse("671758F5-320D-4D1C-8C1A-54CDC55F2F75"),
                Name = $"Magda's project nr {count}",
                StartTime = DateTime.UtcNow.Date,
                Deadline = DateTime.UtcNow.Date.AddDays(45)
            }); ;
        }
    }
}
