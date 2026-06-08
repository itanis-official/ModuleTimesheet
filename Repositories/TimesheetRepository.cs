using Microsoft.EntityFrameworkCore;
using ModuleHelpDeskTimesheet.Data;
using ModuleHelpDeskTimesheet.Models;

namespace ModuleHelpDeskTimesheet.Repositories
{
    public class TimesheetRepository : ITimesheetRepository
    {
        private readonly TimesheetDbContext _context;

        public TimesheetRepository(TimesheetDbContext context)
        {
            _context = context;
        }

        #region Timesheet Logic

        public async Task<IEnumerable<TimesheetEntry>> GetAllEntriesAsync()
            => await _context.TimesheetEntries.ToListAsync();

        public async Task<TimesheetEntry?> GetEntryByIdAsync(int id)
            => await _context.TimesheetEntries.FindAsync(id);

        public async Task<IEnumerable<TimesheetEntry>> GetEntriesByAgentAsync(int agentId, DateTime start, DateTime end)
        {
            return await _context.TimesheetEntries
                .Where(e => e.AgentId == agentId && e.DateDebut >= start && e.DateFin <= end)
                .OrderByDescending(e => e.DateDebut)
                .ToListAsync();
        }

        public async Task<IEnumerable<TimesheetEntry>> GetEntriesByTicketAsync(int ticketId)
        {
            return await _context.TimesheetEntries
                .Where(e => e.TicketId == ticketId)
                .ToListAsync();
        }

        public async Task<TimesheetEntry> CreateEntryAsync(TimesheetEntry entry)
        {
            // Sécurité : Calcul de la durée si elle n'est pas fournie
            if (entry.TotalHeures <= 0)
            {
                entry.TotalHeures = (entry.DateFin - entry.DateDebut).TotalHours;
            }

            _context.TimesheetEntries.Add(entry);
            await _context.SaveChangesAsync();
            return entry;
        }

        public async Task UpdateEntryAsync(TimesheetEntry entry)
        {
            _context.Entry(entry).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task RevueTimesheetAsync(int id, StatutTimesheet nouveauStatut, string commentaireAdmin)
{
    var entree = await _context.TimesheetEntries.FindAsync(id);
    if (entree != null)
    {
        entree.Statut = nouveauStatut;
        entree.CommentaireAdmin = commentaireAdmin;
        entree.DateValidation = DateTime.Now;
        
        await _context.SaveChangesAsync();
    }
}
        #endregion

        #region Calendrier Logic

        public async Task<IEnumerable<Calendrier>> GetCalendarByAgentAsync(int agentId, DateTime start, DateTime end)
        {
            return await _context.CalendarEvents
                .Where(c => c.AgentId == agentId && c.DateDebut >= start && c.DateFin <= end)
                .OrderBy(c => c.DateDebut)
                .ToListAsync();
        }

        public async Task<Calendrier> CreateEventAsync(Calendrier eventItem)
        {
            _context.CalendarEvents.Add(eventItem);
            await _context.SaveChangesAsync();
            return eventItem;
        }

        #endregion
    }
}