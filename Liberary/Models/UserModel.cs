using System.ComponentModel.DataAnnotations;

namespace Liberary.Models
{
    public class UserModel
    {
        [Key]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime createdDate { get; set; }

        public string profilepic { get; set; } = "";
    }
}
