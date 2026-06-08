using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModuleHelpDeskTimesheet.Models
{
    public class DeclarationTemps
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; } 

        public int EmployeIdRh { get; set; }
        public string EmployeNomComplet { get; set; } = string.Empty;
        public string EmployeAgentType { get; set; } = "interne";

        public int SousTacheId { get; set; }
        public string SousTacheTitre { get; set; } = string.Empty;

        public int ProjetId { get; set; }
        public string ProjetNom { get; set; } = string.Empty;

        public DateTime Date { get; set; }
        public decimal DureeHeures { get; set; }
        public string Type { get; set; } = "Travail";

        public DateTime DeclaredAt { get; set; }
        public DateTime SyncedAt { get; set; }
    }
}