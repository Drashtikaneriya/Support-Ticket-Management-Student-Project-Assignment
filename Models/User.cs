using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Student_Project_Assignment.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty; // Store BCrypt hash

        [Required]
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [InverseProperty("CreatedBy")]
        public ICollection<Ticket> CreatedTickets { get; set; } = new List<Ticket>();

        [InverseProperty("AssignedTo")]
        public ICollection<Ticket> AssignedTickets { get; set; } = new List<Ticket>();
    }
}