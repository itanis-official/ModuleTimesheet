using Microsoft.EntityFrameworkCore;
using ModuleHelpDeskTimesheet.Models;

namespace ModuleHelpDeskTimesheet.Data
{
    public class TimesheetDbContext : DbContext
    {
        public TimesheetDbContext(DbContextOptions<TimesheetDbContext> options)
            : base(options) { }

        public DbSet<TimesheetEntry> TimesheetEntries { get; set; }
        public DbSet<Calendrier> CalendarEvents { get; set; }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<DeclarationTemps> DeclarationTemps { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
public DbSet<TicketCollaborateur> TicketCollaborateurs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agent>(entity =>
            {
                entity.Property(a => a.CoutHoraire).HasPrecision(18, 2);
                entity.Property(a => a.Rating).HasPrecision(3, 2);
            });

            modelBuilder.Entity<DeclarationTemps>(entity =>
            {
                entity.Property(d => d.DureeHeures).HasPrecision(6, 2);
            });
        }
    }
}