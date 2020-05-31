using DAL.Ef.Entities; 
using Shared.ApiModel;
using System.Collections.Generic; 

namespace DAL.Ef.EntityMapper
{
    public interface IWebsiteManagementEntityMapper
    {
        Website Create(WebsiteProxy websiteProxy);

        WebsiteProxy[] Map(params Website[] websites);

        void Update(WebsiteProxy src, Website dest);

        void Patch(WebsiteProxy src, Website dest);

        void RemoveMissingFields(ICollection<WebsiteField> oldFields, ICollection<WebsiteField> updatedFields);

        void RemoveMissingLogins(ICollection<WebsiteLogin> oldLogins, ICollection<WebsiteLogin> updatedLogins);

        void UpdateFields(ICollection<WebsiteField> oldFields, ICollection<WebsiteField> updatedFields);

        void UpdateLogins(ICollection<WebsiteLogin> oldLogins, ICollection<WebsiteLogin> updatedLogins);

        void AddNewFields(ICollection<WebsiteField> oldFields, ICollection<WebsiteField> updatedFields);

        void AddNewLogins(ICollection<WebsiteLogin> oldLogins, ICollection<WebsiteLogin> updatedLogins);
 
    }
}
