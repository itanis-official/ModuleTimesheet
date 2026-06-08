using Microsoft.AspNetCore.Mvc;
using ModuleHelpDeskTimesheet.Models;
using ModuleHelpDeskTimesheet.Repositories;

namespace ModuleHelpDeskTimesheet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimesheetController : ControllerBase
    {
        private readonly ITimesheetRepository _repo;

        public TimesheetController(ITimesheetRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Timesheet/agent/1?start=2024-01-01&end=2024-01-31
        [HttpGet("agent/{agentId}")]
        public async Task<ActionResult<IEnumerable<TimesheetEntry>>> GetByAgent(
            int agentId,
            [FromQuery] DateTime start,
            [FromQuery] DateTime end)
        {
            var entries = await _repo.GetEntriesByAgentAsync(agentId, start, end);
            return Ok(entries);
        }

        // GET: api/Timesheet/ticket/3
        [HttpGet("ticket/{ticketId}")]
        public async Task<ActionResult<IEnumerable<TimesheetEntry>>> GetByTicket(int ticketId)
        {
            var entries = await _repo.GetEntriesByTicketAsync(ticketId);
            return Ok(entries);
        }

        // POST: api/Timesheet
        [HttpPost]
        public async Task<ActionResult<TimesheetEntry>> CreateEntry([FromBody] TimesheetEntry entry)
        {
            if (entry == null) return BadRequest();

            var created = await _repo.CreateEntryAsync(entry);
            return CreatedAtAction(nameof(GetByAgent), new { agentId = created.AgentId }, created);
        }

       // PUT: api/Timesheet/5/revue?statut=1
[HttpPut("{id}/revue")]
public async Task<IActionResult> Revue(int id, [FromQuery] StatutTimesheet statut, [FromBody] string commentaireAdmin)
{
    var exist = await _repo.GetEntryByIdAsync(id);
    if (exist == null) return NotFound();

    await _repo.RevueTimesheetAsync(id, statut, commentaireAdmin);
    
    return NoContent();
}
    }
}