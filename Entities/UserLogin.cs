using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class UserLogin
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
