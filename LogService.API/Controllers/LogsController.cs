using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LogService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateLog([FromBody] LogRequest request)
        {
            switch (request.Level.ToLower())
            {
                case "info":
                    Log.Information("{Message}", request.Message);
                    break;
                case "warning":
                    Log.Warning("{Message}", request.Message);
                    break;
                case "error":
                    Log.Error("{Message}", request.Message);
                    break;
                case "critical":
                    Log.Fatal("{Message}", request.Message);
                    break;
                default:
                    Log.Information("{Message}", request.Message);
                    break;
            }

            return Ok(new { status = "logged" });
        }

        public class LogRequest
        {
            public string Level { get; set; } = "info";
            public string Message { get; set; } = string.Empty;
        }
    } 
}