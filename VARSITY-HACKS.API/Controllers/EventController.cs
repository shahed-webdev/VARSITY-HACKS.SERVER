using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VARSITY_HACKS.BusinessLogic;
using VARSITY_HACKS.DATA;
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
       
        // POST api/event/add
        [HttpPost("add")]
        public async Task<IActionResult> AddEvent(UserEventAddModel model)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userName)) return BadRequest("user not found");

            var response = await _event.AddAsync(userName,model);
            if (!response.IsSuccess) return BadRequest(response);
            return Created("",response);
        }
       
        // GET api/event/get
        [HttpGet("get/{type}")]
        public async Task<IActionResult> GetEvents(EventType type)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userName)) return BadRequest("user not found");

            var response = await _event.GetEventsAsync(userName, type);
            if (!response.IsSuccess) return BadRequest(response);
            return Ok(response);
        }

        // GET api/event/calendar-events
        [HttpGet("calendar-events")]
        public async Task<IActionResult> GetCalendarEvents()
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userName)) return BadRequest("user not found");

            var response = await _event.GetCalendarEventsAsync(userName);
            if (!response.IsSuccess) return BadRequest(response);
            return Ok(response);
        }
    }
}
