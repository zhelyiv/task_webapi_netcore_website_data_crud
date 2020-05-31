using BusinessLogic.ApiModelValidation;
using BusinessLogic.DataServices;
using DAL.Ef;
using DAL.Ef.Entities;
using DAL.Ef.EntityMapper;
using DAL.Ef.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WebsiteManagement.WebAPI.Controllers;

namespace UnitTests
{
    public static class InstanceFactory
    {
        public static async Task<T> GetInstance<T>() where T : class
        {
            var t = typeof(T);

            if (typeof(WebsiteManagementEntityMapper).Equals(t)) 
                return GetEntityMapper() as T;
             
            if (typeof(WebsiteManagementDbContext).Equals(t) ||
                typeof(IWebsiteManagementDbContext).Equals(t))
                return await GetDbContext() as T;

            if (typeof(WebsiteValidator).Equals(t))
                return GetWebsiteValidator() as T;
             
            return default;
        }
 
        private static WebsiteValidator GetWebsiteValidator()
        {
            return new WebsiteValidator(new LoginValidator(),
                    new FieldValidator(),
                    new WebsiteHomepageSnapshotValidator());
        }

        private static WebsiteManagementEntityMapper GetEntityMapper()
        {
            return new WebsiteManagementEntityMapper(AutoMapperProfile.GetMapperInstance());
        }
         
        private static async Task<IWebsiteManagementDbContext> GetDbContext()
        {
            var builder = new DbContextOptionsBuilder<WebsiteManagementDbContext>();
            builder.UseInMemoryDatabase($"WebsiteManagement_InMemory_{Guid.NewGuid()}"); 
            var db = new WebsiteManagementDbContext(builder.Options);

            db.Database.EnsureCreated();

            #region seed_with_dummy_records

            var listWebsites = new List<Website>();
            for (int i = 0; i < 100; i++)
            {
                var websiteEntity = new Website();
                websiteEntity.Name = "Website " + i;
                websiteEntity.Url = @$"http:\\url{i}.com";
                websiteEntity.HomepageSnapshot = new WebsiteHomepageSnapshot()
                {
                    Image = new byte[] { 32, 43, 64, 54 }
                };
                websiteEntity.StatusId = WebsiteStatusEnum.Active;
                websiteEntity.CategoryId = i % 2 == 0 ? WebsiteCategoryEnum.Blog : WebsiteCategoryEnum.Retail;
                websiteEntity.Logins = new Collection<WebsiteLogin>()
                {
                    new WebsiteLogin()
                    {
                        Email = $"email_{i}@mail.com",
                        Password = Guid.NewGuid().ToString()
                    }
                };
                websiteEntity.Fields = new Collection<WebsiteField>()
                {
                    new WebsiteField()
                    {
                        FieldName = $"{websiteEntity.Name} Field",
                        FieldValue = $"{websiteEntity.Name} FiledValue: {Guid.NewGuid()}"
                    }
                };
                listWebsites.Add(websiteEntity);
            }
            await db.Website.AddRangeAsync(listWebsites);
            await db.SaveChangesAsync();

            #endregion

            return db;
        }
    }
}
