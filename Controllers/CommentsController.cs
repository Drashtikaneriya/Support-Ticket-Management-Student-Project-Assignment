using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Project_Assignment.Data;
using Student_Project_Assignment.DTOs.Comments;
using Student_Project_Assignment.Models;

namespace Student_Project_Assignment.Controllers
{
    [ApiController]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CommentsController(AppDbContext context)
        {
            _context = context;
        }

        private int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        [HttpGet("api/tickets/{id}/comments")]
        public async Task<ActionResult<IEnumerable<CommentResponse>>> GetComments(int id)
        {
            var comments = await _context.TicketComments
                .Include(c => c.User)
                .Where(c => c.TicketId == id)
                .Select(c => new CommentResponse
                {
                    Id = c.Id,
                    Comment = c.Comment,
                    TicketId = c.TicketId,
                    UserId = c.UserId,
                    UserName = c.User.Name,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();

            return Ok(comments);
        }

        [HttpPost("api/tickets/{id}/comments")]
        public async Task<ActionResult<CommentResponse>> CreateComment(int id, [FromBody] CommentCreateRequest request)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return NotFound();

            var comment = new TicketComment
            {
                TicketId = id,
                UserId = CurrentUserId,
                Comment = request.Comment
            };

            _context.TicketComments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComments), new { id = comment.TicketId }, new CommentResponse
            {
                Id = comment.Id,
                Comment = comment.Comment,
                TicketId = comment.TicketId,
                UserId = comment.UserId,
                UserName = User.FindFirst(ClaimTypes.Name)?.Value ?? "",
                CreatedAt = comment.CreatedAt
            });
        }

        [HttpPatch("api/comments/{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] CommentUpdateRequest request)
        {
            var comment = await _context.TicketComments.FindAsync(id);
            if (comment == null) return NotFound();

            if (comment.UserId != CurrentUserId && !User.IsInRole("MANAGER"))
            {
                return Forbid();
            }

            comment.Comment = request.Comment;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("api/comments/{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.TicketComments.FindAsync(id);
            if (comment == null) return NotFound();

            if (comment.UserId != CurrentUserId && !User.IsInRole("MANAGER"))
            {
                return Forbid();
            }

            _context.TicketComments.Remove(comment);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
