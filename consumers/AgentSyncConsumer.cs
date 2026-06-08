using ITANIS.SharedEvents;
using MassTransit;
using ModuleHelpDeskTimesheet.Data;
using ModuleHelpDeskTimesheet.Models;

namespace ModuleHelpDeskTimesheet.Consumers;

public class AgentSyncConsumer : IConsumer<AgentSyncEvent>
{
    private readonly TimesheetDbContext _db;
    private readonly ILogger<AgentSyncConsumer> _logger;

    public AgentSyncConsumer(TimesheetDbContext db, ILogger<AgentSyncConsumer> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AgentSyncEvent> context)
{
    var msg = context.Message;
    var action = msg.GetActionAsString(); 

    _logger.LogInformation("AgentSync {Action} id={Id} {Nom} {Prenom}", action, msg.Id, msg.Nom, msg.Prenom);

    if (action == "Deleted")
    {
        var existing = await _db.Agents.FindAsync(msg.Id);
        if (existing != null)
        {
            _db.Agents.Remove(existing);
            await _db.SaveChangesAsync();
        }
        return;
    }

    var entity = await _db.Agents.FindAsync(msg.Id);
    if (entity == null)
    {
        entity = new Agent { Id = msg.Id };
        _db.Agents.Add(entity);
    }

    entity.Nom = msg.Nom;
    entity.Prenom = msg.Prenom;
    entity.Email = msg.Email;
    entity.Telephone = msg.Telephone;
    entity.Role = msg.Role;
    entity.Poste = msg.Poste;
    entity.Departement = msg.Departement;
    entity.IsActive = msg.IsActive;
    entity.AgentType = msg.AgentType;
    entity.CoutHoraire = msg.CoutHoraire;
    entity.Rating = msg.Rating;
    entity.SyncedAt = DateTime.UtcNow;

    await _db.SaveChangesAsync();
}
}