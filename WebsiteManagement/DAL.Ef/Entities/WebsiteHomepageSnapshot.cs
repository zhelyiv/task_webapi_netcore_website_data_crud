using System.ComponentModel.DataAnnotations;

namespace DAL.Ef.Entities
{
    public class WebsiteHomepageSnapshot : BaseEntity 
    {
        [Required]
        public byte[] Image { get; set; }
    }
}
