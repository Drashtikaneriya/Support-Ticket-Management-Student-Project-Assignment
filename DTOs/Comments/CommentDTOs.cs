using System;
using System.ComponentModel.DataAnnotations;

namespace Student_Project_Assignment.DTOs.Comments
{
    public class CommentCreateRequest
    {
        [Required]
        public string Comment { get; set; } = string.Empty;
    }

    public class CommentUpdateRequest
    {
        [Required]
        public string Comment { get; set; } = string.Empty;
    }

    public class CommentResponse
    {
        public int Id { get; set; }
        public string Comment { get; set; } = string.Empty;
        public int TicketId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
