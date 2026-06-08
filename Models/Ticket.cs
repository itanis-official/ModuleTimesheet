using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModuleHelpDeskTimesheet.Models
{
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int? InterventionId { get; set; }
        public string Categorie { get; set; } = string.Empty;
        public string Titre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ClientId { get; set; }
        public int? SousClientId { get; set; }
        public string Statut { get; set; } = string.Empty;
        public string Priorite { get; set; } = string.Empty;
        public DateTime DateCreation { get; set; }
        public DateTime? DateFermeture { get; set; }
        public double DureeReelleMinutes { get; set; }
        public double CoutFinal { get; set; }
        public int? AgentPrincipalId { get; set; }
        public string? CodeUnidesk { get; set; }
        public DateTime SyncedAt { get; set; }
    }
}