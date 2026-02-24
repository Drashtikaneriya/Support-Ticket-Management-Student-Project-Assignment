using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Student_Project_Assignment.Enums;

namespace Student_Project_Assignment.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public RoleType Name { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
