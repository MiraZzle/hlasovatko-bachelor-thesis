using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace server.Controllers
{
    [Route("sessions")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        [HttpGet("/test")]
        public IActionResult Test()
        {
            return Ok("Test successful!");
        }
    }
}
