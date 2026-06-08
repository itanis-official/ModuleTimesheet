using ITANIS.SharedEvents;
using MassTransit;
using ModuleHelpDeskTimesheet.Data;
using ModuleHelpDeskTimesheet.Models;

namespace ModuleHelpDeskTimesheet.Consumers;

public class TicketCollaborateurSyncConsumer : IConsumer<TicketCollaborateurSyncEvent>
{
    private readonly TimesheetDbContext _db;
    private readonly ILogger<TicketCollaborateurSyncConsumer> _logger;

    public TicketCollaborateurSyncConsumer(TimesheetDbContext db, ILogger<TicketCollaborateurSyncConsumer> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<TicketCollaborateurSyncEvent> context)
    {
        var msg = context.Message;
        var action = msg.GetActionAsString();
        _logger.LogInformation("TicketCollaborateurSync {Action} id={Id} ticketId={TicketId}", action, msg.Id, msg.TicketId);

        if (action == "Deleted")
        {
            var existing = await _db.TicketCollaborateurs.FindAsync(msg.Id);
            if (existing != null)
            {
                _db.TicketCollaborateurs.Remove(existing);
                await _db.SaveChangesAsync();
            }
            return;
        }

        var entity = await _db.TicketCollaborateurs.FindAsync(msg.Id);
        if (entity == null)
        {
            entity = new TicketCollaborateur { Id = msg.Id };
            _db.TicketCollaborateurs.Add(entity);
        }

        entity.TicketId = msg.TicketId;
        entity.AgentId = msg.AgentId;
        entity.SyncedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
    }
}