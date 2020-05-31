using Shared.ApiModel;
using Shared.Enums; 
using System.Threading.Tasks;

namespace Shared.Repositories
{
    public interface IWebsiteRepository
    {
        Task<int> Create(WebsiteProxy websiteProxy);

        Task<WebsiteProxy> Update(WebsiteProxy websiteProxy);

        Task<WebsiteProxy> Patch(WebsiteProxy websiteProxy);

        Task<bool> Remove(int id);

        Task<WebsiteProxy> Get(int id);

        Task<int> GetCount();

        Task<WebsiteProxy[]> Get(int? page, int? pageSize, WebsiteOrderByEnum? orderBy, WebsiteOrderByAscDescEnum? orderType);
    }
}
