using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }

        [Required,EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required,MaxLength(20, ErrorMessage = "first name must be less than 20 characters long")]

        public string FirstName { get; set; }
        [Required,MaxLength(20, ErrorMessage = "first name must be less than 20 characters long")]

        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
        [NotMapped]
        public string Token { get; set; }

    }
}
