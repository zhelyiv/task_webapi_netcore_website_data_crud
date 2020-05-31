using Shared.ApiModel;
using Shared.ApiModelValidation;
using Shared.DataServices;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Repositories;
using System; 
using System.Linq; 
using System.Threading.Tasks;

namespace BusinessLogic.DataServices
{
    public class WebsiteDataService : IWebsiteDataService
    {
        #region members

        private IServiceProvider ServiceProvider { get; }

        private IWebsiteRepository WebsiteRepository { get; }

        private IWebsiteValidator WebsiteValidator { get; }

        private IPagingValidator PagingValidator { get; }

        #endregion

        public WebsiteDataService(IWebsiteRepository websiteRepository,
            IWebsiteValidator websiteValidator,
            IPagingValidator pagingValidator) 
        {
            WebsiteRepository = websiteRepository;
            WebsiteValidator = websiteValidator;
            PagingValidator = pagingValidator;
        }
         
        public async Task<int> Create(WebsiteProxy websiteProxy)
        {
            WebsiteValidator.ValidateOnUpdateOrCreate(websiteProxy);

            var id = await WebsiteRepository.Create(websiteProxy);

            return id;
        }

        public async Task<WebsiteProxy> Update(WebsiteProxy websiteProxy)
        {
            if(websiteProxy.Id == 0)
            {
                throw new NotFoundError($"Update Failed. Website {websiteProxy.Id} not provided");
            }

            WebsiteValidator.ValidateOnUpdateOrCreate(websiteProxy);

            var updatedObject = await WebsiteRepository.Update(websiteProxy);

            if (updatedObject is null)
            {
                throw new NotFoundError($"Update Failed. Website with id {websiteProxy.Id} not found");
            }

            return updatedObject;
        }

        public async Task<WebsiteProxy> Patch(WebsiteProxy websiteProxy)
        {
            WebsiteValidator.ValidateOnPatch(websiteProxy);

            var updatedObject = await WebsiteRepository.Patch(websiteProxy);

            if (updatedObject is null)
            {
                throw new NotFoundError($"Update Failed. Website with id {websiteProxy.Id} not found");
            }

            return updatedObject;
        }

        public async Task Remove(int id)
        {
            if (!await WebsiteRepository.Remove(id))
            {
                throw new NotFoundError($"Delete Failed. Website with id {id} not found");
            }
        }

        public async Task<WebsiteProxy> Get(int id)
        {
            var website = await WebsiteRepository.Get(id);

            if (website is null)
            {
                throw new NotFoundError($"Website with id {id} not found");
            }

            return website;
        }

        public async Task<int> GetCount()
        {
            return await WebsiteRepository.GetCount();
        }

        public async Task<WebsiteProxy[]> Get(int? page,
            int? pageSize,
            WebsiteOrderByEnum? orderBy,
            WebsiteOrderByAscDescEnum? orderType)
        { 
            var totalRecords = await GetCount();
            if (totalRecords == 0)
            {
                return Enumerable.Empty<WebsiteProxy>().ToArray();
            }

            PagingValidator.Validate(totalRecords, page, pageSize);

            return await WebsiteRepository.Get(page, pageSize, orderBy, orderType);
        }
    }
}
