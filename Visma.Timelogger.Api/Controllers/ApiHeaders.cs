using Microsoft.AspNetCore.Mvc;

namespace Visma.Timelogger.Api.Controllers
{
    public class ApiHeaders
    {
        [FromHeader]
        public string User { get; set; } = string.Empty;
    }
}
