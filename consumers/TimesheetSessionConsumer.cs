using System.Threading.Tasks;
using MassTransit;
using ITANIS.SharedEvents;
using ModuleHelpDeskTimesheet.Models;
using ModuleHelpDeskTimesheet.Data;

namespace ModuleHelpDeskTimesheet.Consumers
{
    public class TimesheetSessionConsumer : IConsumer<TimesheetSessionEvent>
    {
        private readonly TimesheetDbContext _context; 

        public TimesheetSessionConsumer(TimesheetDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<TimesheetSessionEvent> context)
        {
            var message = context.Message;

            var newTimesheetEntry = new TimesheetEntry
            {
                AgentId = message.AgentId,
                NomTache = message.NomTache,
                Description = message.Description,
                TicketId = message.TicketId,
                SoustacheId = null, 
                DateDebut = message.DateDebut,
                DateFin = message.DateFin,
                TotalHeures = message.TotalHeures, 
                Statut = StatutTimesheet.EnAttente, 
                Source = OrigineSaisie.HelpDesk 
            };

            // Cleaner approach: Use the explicit property declared on your TimesheetDbContext
            _context.TimesheetEntries.Add(newTimesheetEntry);
            
            await _context.SaveChangesAsync();
        }
    }
}