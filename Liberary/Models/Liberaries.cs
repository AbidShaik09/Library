using System.ComponentModel.DataAnnotations;

namespace Liberary.Models
{
    public class Liberaries
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? BookName { get; set; }
        public string? Discription { get; set; }
        [Required]
        public string? ImgPath { get; set; }

        public DateTime CreatedTime= DateTime.Now;


    }
}
