using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Project_Assignment.Data;
using Student_Project_Assignment.DTOs.Tickets;
using Student_Project_Assignment.Enums;
using Student_Project_Assignment.Models;
using Student_Project_Assignment.Services;

namespace Student_Project_Assignment.Controllers
{
    [ApiController]
    [Route("api/tickets")]
    [Authorize]
    public class TicketsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TicketService _ticketService;

        public TicketsController(AppDbContext context, TicketService ticketService)
        {
            _context = context;
            _ticketService = ticketService;
        }

        private int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        private string UserRole => User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketResponse>>> GetTickets()
        {
            var query = _context.Tickets
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .AsQueryable();

            // Role based filtering according to specification:
            // MANAGER → all
            // SUPPORT → assigned only
            // USER → own tickets
            if (UserRole == RoleType.USER.ToString())
            {
                query = query.Where(t => t.CreatedById == CurrentUserId);
            }
            else if (UserRole == RoleType.SUPPORT.ToString())
            {
                query = query.Where(t => t.AssignedToId == CurrentUserId);
            }

            var tickets = await query
                .Select(t => new TicketResponse
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Status = t.Status.ToString(),
                    Priority = t.Priority.ToString(),
                    CreatedById = t.CreatedById,
                    CreatedByName = t.CreatedBy.Name,
                    AssignedToId = t.AssignedToId,
                    AssignedToName = t.AssignedTo != null ? t.AssignedTo.Name : null,
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync();

            return Ok(tickets);
        }

        [HttpPost]
        [Authorize(Roles = "USER,MANAGER")]
        public async Task<ActionResult<TicketResponse>> CreateTicket([FromBody] TicketCreateRequest request)
        {
            var ticket = new Ticket
            {
                Title = request.Title,
                Description = request.Description,
                Priority = request.Priority,
                CreatedById = CurrentUserId,
                Status = TicketStatus.OPEN
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTickets), new { }, new TicketResponse
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                Status = ticket.Status.ToString(),
                Priority = ticket.Priority.ToString(),
                CreatedById = ticket.CreatedById,
                CreatedByName = User.FindFirst(ClaimTypes.Name)?.Value ?? "",
                CreatedAt = ticket.CreatedAt
            });
        }

        [HttpPatch("{id}/assign")]
        [Authorize(Roles = "MANAGER,SUPPORT")]
        public async Task<IActionResult> AssignTicket(int id, [FromBody] TicketAssignRequest request)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return NotFound();

            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == request.AssignedToId);
            if (user == null) return BadRequest(new { message = "User not found" });

            if (!_ticketService.CanAssignToRole(user.Role.Name))
            {
                return BadRequest(new { message = "Cannot assign ticket to USER role" });
            }

            ticket.AssignedToId = request.AssignedToId;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = "SUPPORT,MANAGER")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] TicketStatusUpdateRequest request)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return NotFound();

            if (!_ticketService.IsValidStatusTransition(ticket.Status, request.NewStatus))
            {
                return BadRequest(new { message = $"Invalid status transition from {ticket.Status} to {request.NewStatus}" });
            }

            var log = new TicketStatusLog
            {
                TicketId = ticket.Id,
                OldStatus = ticket.Status,
                NewStatus = request.NewStatus,
                ChangedById = CurrentUserId
            };

            ticket.Status = request.NewStatus;
            _context.TicketStatusLogs.Add(log);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "MANAGER")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return NotFound();

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
