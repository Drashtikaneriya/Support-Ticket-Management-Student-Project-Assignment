using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Student_Project_Assignment.Enums;

namespace Student_Project_Assignment.Models
{
    public class TicketStatusLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TicketId { get; set; }

        [ForeignKey("TicketId")]
        public Ticket Ticket { get; set; } = null!;

        [Required]
        public TicketStatus OldStatus { get; set; }

        [Required]
        public TicketStatus NewStatus { get; set; }

        [Required]
        public int ChangedById { get; set; }

        [ForeignKey("ChangedById")]
        public User ChangedBy { get; set; } = null!;

        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    }
}
