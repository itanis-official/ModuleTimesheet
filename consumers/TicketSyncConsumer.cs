using ITANIS.SharedEvents;
using MassTransit;
using ModuleHelpDeskTimesheet.Data;
using ModuleHelpDeskTimesheet.Models;

namespace ModuleHelpDeskTimesheet.Consumers;

public class TicketSyncConsumer : IConsumer<TicketSyncEvent>
{
    private readonly TimesheetDbContext _db;
    private readonly ILogger<TicketSyncConsumer> _logger;

    public TicketSyncConsumer(TimesheetDbContext db, ILogger<TicketSyncConsumer> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<TicketSyncEvent> context)
    {
        var msg = context.Message;
        var action = msg.GetActionAsString();
        _logger.LogInformation("TicketSync {Action} id={Id} titre={Titre}", action, msg.Id, msg.Titre);

        if (action == "Deleted")
        {
            var existing = await _db.Tickets.FindAsync(msg.Id);
            if (existing != null)
            {
                _db.Tickets.Remove(existing);
                await _db.SaveChangesAsync();
            }
            return;
        }

        var entity = await _db.Tickets.FindAsync(msg.Id);
        if (entity == null)
        {
            entity = new Ticket { Id = msg.Id };
            _db.Tickets.Add(entity);
        }

        entity.InterventionId = msg.InterventionId;
        entity.Categorie = msg.Categorie;
        entity.Titre = msg.Titre;
        entity.Description = msg.Description;
        entity.ClientId = msg.ClientId;
        entity.SousClientId = msg.SousClientId;
        entity.Statut = msg.Statut;
        entity.Priorite = msg.Priorite;
        entity.DateCreation = msg.DateCreation;
        entity.DateFermeture = msg.DateFermeture;
        entity.DureeReelleMinutes = msg.DureeReelleMinutes;
        entity.CoutFinal = msg.CoutFinal;
        entity.AgentPrincipalId = msg.AgentPrincipalId;
        entity.CodeUnidesk = msg.CodeUnidesk;
        entity.SyncedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
    }
}