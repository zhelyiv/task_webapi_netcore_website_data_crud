using Shared.Enums;
using System.ComponentModel.DataAnnotations; 

namespace DAL.Ef.Entities
{
    public class WebsiteStatus
    {
        [Key]
        public WebsiteStatusEnum Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
