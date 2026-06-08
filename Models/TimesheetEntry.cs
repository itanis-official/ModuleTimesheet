using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModuleHelpDeskTimesheet.Models
{
    public enum StatutTimesheet 
    { 
        EnAttente,   // Par défaut à la création
        Approuve,    // Validé par l'admin
        Rejete       // Refusé pour correction
    }

    public enum OrigineSaisie 
    { 
        Manuel,       // Saisie libre par l'agent
        HelpDesk,     // Généré depuis un ticket de support
        GestionProjet // Généré depuis une tâche projet (soustache)
    }

    public class TimesheetEntry
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AgentId { get; set; }

        [Required]
        [StringLength(200)]
        public string NomTache { get; set; } = string.Empty; 

        public string? Description { get; set; }

        [Required]
        public OrigineSaisie Source { get; set; } = OrigineSaisie.Manuel;

        public int? TicketId { get; set; }      
        public int? SoustacheId { get; set; }   

        [Required]
        public DateTime DateDebut { get; set; } 
        
        [Required]
        public DateTime DateFin { get; set; }

        [Required]
        public double TotalHeures { get; set; } 

        [Required]
        public StatutTimesheet Statut { get; set; } = StatutTimesheet.EnAttente;

        public string? CommentaireAdmin { get; set; } 
        
        public DateTime? DateValidation { get; set; } 
    }
}