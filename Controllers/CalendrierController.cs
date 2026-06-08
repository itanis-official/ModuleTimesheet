using Microsoft.AspNetCore.Mvc;
using ModuleHelpDeskTimesheet.Models;
using ModuleHelpDeskTimesheet.Repositories;

namespace ModuleHelpDeskTimesheet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalendrierController : ControllerBase
    {
        private readonly ITimesheetRepository _repo;

        public CalendrierController(ITimesheetRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Calendrier/agent/1?start=2024-01-01&end=2024-01-31
        [HttpGet("agent/{agentId}")]
        public async Task<ActionResult<IEnumerable<Calendrier>>> GetByAgent(
            int agentId,
            [FromQuery] DateTime start,
            [FromQuery] DateTime end)
        {
            var events = await _repo.GetCalendarByAgentAsync(agentId, start, end);
            return Ok(events);
        }

        // POST: api/Calendrier
        [HttpPost]
        public async Task<ActionResult<Calendrier>> CreateEvent([FromBody] Calendrier eventItem)
        {
            if (eventItem == null) return BadRequest();

            var created = await _repo.CreateEventAsync(eventItem);
            
            // Retourne un 201 Created
            return CreatedAtAction(
                nameof(GetByAgent), 
                new { agentId = created.AgentId, start = created.DateDebut, end = created.DateFin }, 
                created);
        }
    }
}