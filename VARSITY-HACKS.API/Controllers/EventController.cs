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

        // Post api/event/IsEventConflicting
        [HttpPost("IsEventConflicting")]
        public async Task<IActionResult> IsEventConflicting(UserEventAddModel model)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userName)) return BadRequest("user not found");

            var response = await _event.IsEventConflictingAsync(userName, model);
            if (!response.IsSuccess) return BadRequest(response);
            
            return Ok(response);
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

        // Delete api/event/DeleteEvent
        [HttpDelete("DeleteEvent")]
        public async Task<IActionResult> DeleteEvent(int userEventId)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userName)) return BadRequest("user not found");

            var response = await _event.DeleteAsync(userName, userEventId);
            if (!response.IsSuccess) return BadRequest(response);
            return Ok(response);
        }

        // Delete api/event/DeleteCalendarEvent
        [HttpDelete("DeleteCalendarEvent")]
        public async Task<IActionResult> DeleteCalenderEvent(int calendarEventId)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userName)) return BadRequest("user not found");

            var response = await _event.DeleteCalendarEventAsync(userName, calendarEventId);
            if (!response.IsSuccess) return BadRequest(response);
            return Ok(response);
        }

        // Edit api/event/EditCalendarEvent 
        [HttpPut("EditCalendarEvent")]
        public async Task<IActionResult> EditCalendarEvent(UserCalendarEventEditModel model)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userName)) return BadRequest("user not found");

            var response = await _event.EditCalendarEventAsync(userName, model);
            if (!response.IsSuccess) return BadRequest(response);
            return Ok(response);
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

        // GET api/event/get-all-type
        [HttpGet("get-all-type")]
        public async Task<IActionResult> GetTypeWiseEvents()
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userName)) return BadRequest("user not found");

            var response = await _event.GetTypeWiseEventsAsync(userName);

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
