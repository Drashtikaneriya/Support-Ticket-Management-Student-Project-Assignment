using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Student_Project_Assignment.Enums;

namespace Student_Project_Assignment.DTOs.Tickets
{
    public class TicketCreateRequest
    {
        [Required]
        [MinLength(5)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(10)]
        public string Description { get; set; } = string.Empty;

        public TicketPriority Priority { get; set; } = TicketPriority.MEDIUM;
    }

    public class TicketResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public int CreatedById { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public int? AssignedToId { get; set; }
        public string? AssignedToName { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class TicketAssignRequest
    {
        [Required]
        public int AssignedToId { get; set; }
    }

    public class TicketStatusUpdateRequest
    {
        [Required]
        public TicketStatus NewStatus { get; set; }
    }
}
