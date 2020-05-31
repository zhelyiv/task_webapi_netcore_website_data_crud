using DAL.Ef.Entities;
using Shared.Enums;
using System;
using System.Linq;

namespace DAL.Ef
{
    public static class WebsiteManagementDbContextSeedHelper
    {
        public static WebsiteStatus[] GetWebsiteStatusesFromEnum()
        {
            return Enum.GetValues(typeof(WebsiteStatusEnum))
               .Cast<WebsiteStatusEnum>()
               .Select(x => new WebsiteStatus { Id = x, Name = x.ToString() })
               .ToArray();
        }
        public static WebsiteCategory[] GetWebsiteCategoriesFromEnum()
        {
            return Enum.GetValues(typeof(WebsiteCategoryEnum))
              .Cast<WebsiteCategoryEnum>()
              .Select(x => new WebsiteCategory { Id = x, Name = x.ToString() })
              .ToArray();
        }
    }
}
