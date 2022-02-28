using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VARSITY_HACKS.BusinessLogic;
using VARSITY_HACKS.BusinessLogic.Registration;
using VARSITY_HACKS.ViewModel;

namespace VARSITY_HACKS.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {

        private readonly IEventCore _event;

        public EventController(IEventCore @event)
        {
            _event = @event;
        }
        // GET api/event/get
        [HttpPost("post")]
        public async Task<IActionResult> AddEvent([FromBody] UserEventAddModel model)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userName)) return BadRequest("user not found");

            var response = await _event.AddAsync(userName,model);
            if (!response.IsSuccess) return BadRequest(response.Message);
            return Created("",response);
        }
        // GET api/event/get
        [HttpGet("get")]
        public async Task<IActionResult> GetEvents()
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userName)) return BadRequest("user not found");

            var response = await _event.GetEventsAsync(userName);
            if (!response.IsSuccess) return BadRequest(response.Message);
            return Ok();
        }

        // GET api/event/get
        [HttpGet("calendar-events")]
        public async Task<IActionResult> GetCalendarEvents()
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userName)) return BadRequest("user not found");

            var response = await _event.GetCalendarEventsAsync(userName);
            if (!response.IsSuccess) return BadRequest(response.Message);
            return Ok();
        }
    }
}
