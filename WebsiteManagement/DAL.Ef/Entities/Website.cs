using Shared.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Ef.Entities
{
    public class Website: BaseEntity 
    {  
        [Required]
        public string Name { get; set; }

        [Required]
        public string Url { get; set; }

        [ForeignKey(nameof(Status))]
        public WebsiteStatusEnum StatusId { get; set; }
        public WebsiteStatus Status { get; set; }

        [ForeignKey(nameof(Category))]
        public WebsiteCategoryEnum CategoryId { get; set; }
        public WebsiteCategory Category { get; set; }
         
        [ForeignKey(nameof(HomepageSnapshot))]
        public int HomepageSnapshotId { get; set; }
        public WebsiteHomepageSnapshot HomepageSnapshot { get; set; }
         
        public ICollection<WebsiteLogin> Logins { get; set; }
         
        public ICollection<WebsiteField> Fields { get; set; }
         
    }
}
