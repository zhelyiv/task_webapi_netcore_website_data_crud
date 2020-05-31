using DAL.Ef.Entities;
using DAL.Ef.EntityMapper;
using Microsoft.EntityFrameworkCore; 
using Shared.ApiModel;
using Shared.Enums;
using Shared.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Ef.Repositories
{
    public class WebsiteRepository: IWebsiteRepository
    { 
        private IWebsiteManagementDbContext DbContext { get; }

        private IWebsiteManagementEntityMapper EntityMapper { get; }

        public WebsiteRepository(IWebsiteManagementDbContext dbContext, 
            IWebsiteManagementEntityMapper entityMapper)
        {
            DbContext = dbContext;
            EntityMapper = entityMapper;
        }

        public async Task<int> Create(WebsiteProxy websiteProxy)
        {
            if (websiteProxy is null)
            {
                throw new NullReferenceException($"{nameof(websiteProxy)} param cannot be null");
            } 

            var websiteEntity = EntityMapper.Create(websiteProxy);

            await DbContext.Website.AddAsync(websiteEntity);

            await DbContext.SaveChangesAsync();

            return websiteEntity.Id;
        }

        public async Task<WebsiteProxy> Update(WebsiteProxy websiteProxy)
        {
            if(websiteProxy is null)
            {
                throw new NullReferenceException($"{nameof(websiteProxy)} param cannot be null");
            }
            
            var websiteEntity = await GetInternal(websiteProxy.Id); 
            
            if (websiteEntity is null)
            {
                return null;
            }
              
            EntityMapper.Update(websiteProxy, websiteEntity);

            await DbContext.SaveChangesAsync();

            return EntityMapper.Map(websiteEntity).FirstOrDefault();
        }

        public async Task<WebsiteProxy> Patch(WebsiteProxy websiteProxy)
        {
            if (websiteProxy is null)
            {
                throw new NullReferenceException($"{nameof(websiteProxy)} param cannot be null");
            }

            var websiteEntity = await GetInternal(websiteProxy.Id);

            if (websiteEntity is null)
            {
                return null;
            }

            EntityMapper.Patch(websiteProxy, websiteEntity);

            await DbContext.SaveChangesAsync();

            var updatedWebsiteEntity = await GetInternal(websiteProxy.Id);

            return EntityMapper.Map(updatedWebsiteEntity).FirstOrDefault();
        }

        public async Task<bool> Remove(int id)
        {
            var websiteEntity = await GetInternal(id);
           
            if (websiteEntity != null)
            {
                websiteEntity.StatusId = WebsiteStatusEnum.Inactive;
                
                await DbContext.SaveChangesAsync();
            }

            return websiteEntity != null;
        }

        public async Task<WebsiteProxy> Get(int id)
        {
            var websiteEntity = await GetInternal(id, true);
       
            if (websiteEntity is null)
            {
                return null;
            }

            return EntityMapper.Map(websiteEntity).FirstOrDefault();
        }

        public async Task<int> GetCount()
        {
            return await GetActiveOnlyQueryInternal().CountAsync();
        }

        #region privates

        private IQueryable<Website> GetActiveOnlyQueryInternal()
        { 
            return DbContext.Website.Where(x => x.StatusId == WebsiteStatusEnum.Active);
        }

        private IQueryable<Website> GetQueryInternal(bool withNoTracking = false)
        {
            var query = GetActiveOnlyQueryInternal();
             
            query = query
                .Include(x => x.HomepageSnapshot)
                .Include(x => x.Logins)
                .Include(x => x.Fields);
  
            if (withNoTracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        private async Task<Website> GetInternal(int id, bool withNoTracking = false)
        {
            var websiteEntity = await GetQueryInternal(withNoTracking)
                .SingleOrDefaultAsync(x => x.Id == id);

            return websiteEntity;
        }

        public async Task<WebsiteProxy[]> Get(int? page, int? pageSize, WebsiteOrderByEnum? orderBy, WebsiteOrderByAscDescEnum? orderType)
        {
            var query = GetQueryInternal(true);

            if(page.HasValue && pageSize.HasValue)
            {
                var itemsPerPage = Math.Abs(pageSize.Value);
                var skipCount = Math.Abs(page.Value) * itemsPerPage;

                query = query.Skip(skipCount).Take(itemsPerPage); 
            } 
              
            if (orderBy == WebsiteOrderByEnum.Name)
            {
                query = orderType == WebsiteOrderByAscDescEnum.Descending ?
                    query.OrderByDescending(x => x.Name) :
                    query.OrderBy(x => x.Name);
            } 
            else if (orderBy == WebsiteOrderByEnum.Url)
            {
                query = orderType == WebsiteOrderByAscDescEnum.Descending ?
                    query.OrderByDescending(x => x.Url) :
                    query.OrderBy(x => x.Url);
            } 
            else if (orderBy == WebsiteOrderByEnum.Category)
            {
                query = orderType == WebsiteOrderByAscDescEnum.Descending ?
                    query.OrderByDescending(x => x.Category.Name) :
                    query.OrderBy(x => x.Category.Name);
            }

            var resultArray = await query.ToArrayAsync();
             
            return EntityMapper.Map(resultArray);
        }

  
        #endregion
    }
}
