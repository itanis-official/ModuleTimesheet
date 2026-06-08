using ModuleHelpDeskTimesheet.Models;

public interface ITimesheetRepository
{
    Task<IEnumerable<TimesheetEntry>> GetAllEntriesAsync();
    Task<TimesheetEntry?> GetEntryByIdAsync(int id);
    Task<IEnumerable<TimesheetEntry>> GetEntriesByAgentAsync(int agentId, DateTime start, DateTime end);
    Task<IEnumerable<TimesheetEntry>> GetEntriesByTicketAsync(int ticketId);
    Task<TimesheetEntry> CreateEntryAsync(TimesheetEntry entry);
    Task UpdateEntryAsync(TimesheetEntry entry);

    Task RevueTimesheetAsync(int id, StatutTimesheet nouveauStatut, string commentaireAdmin);


    // Calendrier
    Task<IEnumerable<Calendrier>> GetCalendarByAgentAsync(int agentId, DateTime start, DateTime end);
    Task<Calendrier> CreateEventAsync(Calendrier eventItem);
}