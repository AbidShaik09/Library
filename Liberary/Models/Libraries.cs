using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Liberary.Models
{
    public class Libraries
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(UserModel))]
        public string UserName { get; set; }
        
        [Required]
        public string? ArticleName { get; set; }

        [DisplayName("Content")]
        public string Description { get; set; }

        public string? ImgPath { get; set; } = "";

        public DateTime CreatedTime= DateTime.Now;


    }
}
