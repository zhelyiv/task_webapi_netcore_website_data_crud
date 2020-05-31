using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Ef.Entities
{
    public class WebsiteLogin : BaseEntity
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [ForeignKey(nameof(Website))]
        public int WebsiteId { get; set; } 
        public Website Website { get; set; }
    }
}
