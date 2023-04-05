using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Liberary.Models
{
    public class Liberaries
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? BookName { get; set; }
        [DisplayName("Content")]
        
        public string? Discription { get; set; }
        
        public string? ImgPath { get; set; }
		[DisplayName("Author")]
		[Required]
        public string? AuthorName { get; set; }

        public DateTime CreatedTime= DateTime.Now;


    }
}
