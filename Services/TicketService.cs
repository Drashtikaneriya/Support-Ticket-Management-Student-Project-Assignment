using System;
using Student_Project_Assignment.Enums;

namespace Student_Project_Assignment.Services
{
    public class TicketService
    {
        public bool IsValidStatusTransition(TicketStatus currentStatus, TicketStatus newStatus)
        {
            // Strict forward transition only:
            // OPEN → IN_PROGRESS → RESOLVED → CLOSED

            if (currentStatus == TicketStatus.OPEN && newStatus == TicketStatus.IN_PROGRESS) return true;
            if (currentStatus == TicketStatus.IN_PROGRESS && newStatus == TicketStatus.RESOLVED) return true;
            if (currentStatus == TicketStatus.RESOLVED && newStatus == TicketStatus.CLOSED) return true;

            return false;
        }

        public bool CanAssignToRole(RoleType roleType)
        {
            // Cannot assign ticket to USER role
            return roleType != RoleType.USER;
        }
    }
}
