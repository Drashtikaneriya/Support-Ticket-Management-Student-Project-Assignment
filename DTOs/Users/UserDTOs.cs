using System;
using System.ComponentModel.DataAnnotations;

namespace Student_Project_Assignment.DTOs.Users
{
    public class UserCreateRequest
    {
        [Required]
        public string Name { get; set; }= string.Empty;

        [Required]
        [EmailAddress ]
        public string Email  { get; set; } =string.Empty;

        [Required]

        [MinLength( 6)]
        public string Password  { get; set; } = string.Empty;

        [Required ]
        public int RoleId { get; set; }
    }

    public class UserResponse
    {
        public int Id { get; set; }
        public string Name  { get; set; } =string.Empty;
        public string Email  { get; set; } =  string.Empty;
        public string  RoleName { get; set; } =  string.Empty;
        public DateTime  CreatedAt { get; set; }
    }
}
