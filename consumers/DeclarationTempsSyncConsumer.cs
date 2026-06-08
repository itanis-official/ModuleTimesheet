using ITANIS.SharedEvents;
using MassTransit;
using ModuleHelpDeskTimesheet.Data;
using ModuleHelpDeskTimesheet.Models;

namespace ModuleHelpDeskTimesheet.Consumers;

public class DeclarationTempsSyncConsumer : IConsumer<DeclarationTempsSyncEvent>
{
    private readonly TimesheetDbContext _db;
    private readonly ILogger<DeclarationTempsSyncConsumer> _logger;

    public DeclarationTempsSyncConsumer(TimesheetDbContext db, ILogger<DeclarationTempsSyncConsumer> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<DeclarationTempsSyncEvent> context)
    {
        var msg = context.Message;
        _logger.LogInformation("DeclarationTempsSync id={Id} employe={Employe} projet={Projet}",
            msg.DeclarationId, msg.EmployeNomComplet, msg.ProjetNom);

        var entity = await _db.DeclarationTemps.FindAsync(msg.DeclarationId);
        if (entity == null)
        {
            entity = new DeclarationTemps { Id = msg.DeclarationId };
            _db.DeclarationTemps.Add(entity);
        }

        entity.EmployeIdRh = msg.EmployeIdRh;
        entity.EmployeNomComplet = msg.EmployeNomComplet;
        entity.EmployeAgentType = msg.EmployeAgentType;
        entity.SousTacheId = msg.SousTacheId;
        entity.SousTacheTitre = msg.SousTacheTitre;
        entity.ProjetId = msg.ProjetId;
        entity.ProjetNom = msg.ProjetNom;
        entity.Date = msg.Date;
        entity.DureeHeures = msg.DureeHeures;
        entity.Type = msg.Type;
        entity.DeclaredAt = msg.DeclaredAt;
        entity.SyncedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
    }
}