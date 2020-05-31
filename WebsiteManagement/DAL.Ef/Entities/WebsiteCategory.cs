using System.ComponentModel.DataAnnotations; 
using Shared.Enums;

namespace DAL.Ef.Entities
{
    public class WebsiteCategory
    {
        [Key]
        public WebsiteCategoryEnum Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
