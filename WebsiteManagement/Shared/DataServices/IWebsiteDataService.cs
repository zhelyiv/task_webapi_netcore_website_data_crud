using Shared.ApiModel;
using Shared.Enums;
using System.Threading.Tasks;

namespace Shared.DataServices
{ 
    public interface IWebsiteDataService
    {
        Task<int> Create(WebsiteProxy websiteProxy);

        Task<WebsiteProxy> Update(WebsiteProxy websiteProxy);

        Task<WebsiteProxy> Patch(WebsiteProxy websiteProxy); 

        Task Remove(int id);

        Task<WebsiteProxy> Get(int id);

        Task<int> GetCount();

        Task<WebsiteProxy[]> Get(int? page, 
            int? pageSize, 
            WebsiteOrderByEnum? orderBy,
            WebsiteOrderByAscDescEnum? orderType);
    }
}
