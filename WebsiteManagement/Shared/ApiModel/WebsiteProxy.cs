using Shared.Enums;
using System.Collections.Generic; 

namespace Shared.ApiModel
{
    public class WebsiteProxy
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; } 

        public WebsiteCategoryEnum? CategoryId { get; set; } 

        public WebsiteHomepageSnapshotProxy HomepageSnapshot { get; set; }
 
        public ICollection<WebsiteLoginProxy> Logins { get; set; }

        public ICollection<WebsiteFieldProxy> Fields { get; set; }
    }
}
