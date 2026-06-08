using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModuleHelpDeskTimesheet.Models
{
    public class TicketCollaborateur
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int AgentId { get; set; }
        public DateTime SyncedAt { get; set; }
    }
}