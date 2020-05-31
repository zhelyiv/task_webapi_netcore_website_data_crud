using BusinessLogic.ApiModelValidation;
using BusinessLogic.DataServices;
using DAL.Ef;
using DAL.Ef.EntityMapper;
using DAL.Ef.Repositories; 
using Microsoft.EntityFrameworkCore.Internal;
using Shared.ApiModel;
using Shared.Enums;
using System;
using System.Collections.ObjectModel;
using System.Linq; 
using System.Threading.Tasks;
using WebsiteManagement.WebAPI.Controllers;
using Xunit;

namespace UnitTests
{
    public class Test_InMemory_Integration : IDisposable
    {
        private IWebsiteManagementDbContext _db;
        private async Task<IWebsiteManagementDbContext> GetDbContext()
        {
            if (_db is null)
            {
                _db = await InstanceFactory.GetInstance<IWebsiteManagementDbContext>();
            }
            return _db;
        }
        private async Task<WebsiteController> GetWebsiteController()
        {
            var entityMapper = await InstanceFactory.GetInstance<WebsiteManagementEntityMapper>();
            var websiteRepository = new WebsiteRepository(await GetDbContext(), entityMapper);
            var websiteValidator = await InstanceFactory.GetInstance<WebsiteValidator>();
            var websiteDataService = new WebsiteDataService(websiteRepository, websiteValidator, new PagingValidator());
            return new WebsiteController(websiteDataService);
        }

        public void Dispose()
        {
            _db?.Dispose();
        }

        [Fact]
        public async Task Test_records_count()
        {
            var controller = await GetWebsiteController();

            var count = await controller.GetTotalRecords();

            Assert.True(count > 0);
        }

        [Fact]
        public async Task Test_get_all()
        {
            var controller = await GetWebsiteController();

            var all = await controller.Get();

            var count = await controller.GetTotalRecords();

            Assert.Equal(count, all.Length);
            Assert.Equal(100, all.Length);
        }

        [Fact]
        public async Task Test_get_single()
        {
            var controller = await GetWebsiteController();

            var websiteProxy = await controller.Get(1);

            Assert.NotNull(websiteProxy);
            Assert.Equal(1, websiteProxy.Id);
            Assert.Equal("Website 0", websiteProxy.Name);
            Assert.Equal("http:\\\\url0.com", websiteProxy.Url);
            Assert.Equal(WebsiteCategoryEnum.Blog, websiteProxy.CategoryId);
            Assert.NotNull(websiteProxy.HomepageSnapshot);
            Assert.NotNull(websiteProxy.HomepageSnapshot.Image);
            Assert.NotNull(websiteProxy.Logins);
            Assert.Equal(1, websiteProxy.Logins.Count);
            Assert.Equal("email_0@mail.com", websiteProxy.Logins.First().Email);
            Assert.NotNull(websiteProxy.Fields);
            Assert.Equal(1, websiteProxy.Fields.Count);
            Assert.Equal("Website 0 Field", websiteProxy.Fields.First().FieldName);
        }

        [Fact]
        public async Task Test_get_page()
        {
            var controller = await GetWebsiteController();

            var page = await controller.Get(0, 12);

            Assert.Equal(12, page.Length);
            Assert.Equal("Website 0", page.First().Name);
            Assert.Equal("Website 9", page.Last().Name);

            page = await controller.Get(1, 12);

            Assert.Equal(12, page.Length);

            Assert.Equal("Website 12", page.First().Name);
            Assert.Equal("Website 23", page.Last().Name);
        }

        [Fact]
        public async Task Test_create()
        {
            var testProxy = new WebsiteProxy()
            {
                Name = "stackoverflow",
                Url = "https://stackoverflow.com",
                CategoryId = WebsiteCategoryEnum.None,
                HomepageSnapshot = new WebsiteHomepageSnapshotProxy()
                {
                    Image = new byte[] { 42, 34, 64, 64 }
                },
                Logins = new Collection<WebsiteLoginProxy>()
                {
                    new WebsiteLoginProxy()
                    {
                        Email = "mystackoverflowlogin@mail.com",
                        Password = "345667889"
                    }
                },
                Fields = new Collection<WebsiteFieldProxy>()
                {
                    new WebsiteFieldProxy()
                    {
                        FieldName = "Field Name",
                        FieldValue = "Field Value"
                    }
                }
            };

            var controller = await GetWebsiteController();
            var id = await controller.Create(testProxy);

            Assert.True(id > 100);

            var addedWebsiteProxy = await controller.Get(id);

            Assert.Equal(id, addedWebsiteProxy.Id);
            Assert.Equal(testProxy.Name, addedWebsiteProxy.Name);
            Assert.Equal(testProxy.Url, addedWebsiteProxy.Url);
            Assert.Equal(testProxy.CategoryId, addedWebsiteProxy.CategoryId);
            Assert.NotNull(addedWebsiteProxy.HomepageSnapshot);
            Assert.Equal(testProxy.HomepageSnapshot.Image, addedWebsiteProxy.HomepageSnapshot.Image);
            Assert.NotNull(addedWebsiteProxy.Logins);
            Assert.Equal(testProxy.Logins.Count, addedWebsiteProxy.Logins.Count);
            Assert.Equal(testProxy.Logins.First().Email, addedWebsiteProxy.Logins.First().Email);
            Assert.Equal(testProxy.Logins.First().Password, addedWebsiteProxy.Logins.First().Password);
            Assert.NotNull(addedWebsiteProxy.Fields);
            Assert.Equal(testProxy.Fields.Count, addedWebsiteProxy.Fields.Count);
            Assert.Equal(testProxy.Fields.First().FieldName, addedWebsiteProxy.Fields.First().FieldName);
            Assert.Equal(testProxy.Fields.First().FieldValue, addedWebsiteProxy.Fields.First().FieldValue);
        }
         
        [Fact]
        public async Task Test_patch()
        {
            var controller = await GetWebsiteController();
            var proxyToPatch = new WebsiteProxy()
            {
                Id = 1,
                Name = "New Name",
                Logins = new Collection<WebsiteLoginProxy>()
                {
                    new WebsiteLoginProxy()
                    {
                        Email = "email_0@mail.com",
                        Password = "new password"
                    },
                    new WebsiteLoginProxy()
                    {
                        Email = "new_mail@mail.com",
                        Password = "fdsfsdfsd"
                    }
                }
            };

            var patchedProxy = await controller.Patch(proxyToPatch);
             
            Assert.Equal(proxyToPatch.Name, patchedProxy.Name);
            Assert.Equal("http:\\\\url0.com", patchedProxy.Url);
            Assert.Equal(WebsiteCategoryEnum.Blog, patchedProxy.CategoryId);
            Assert.NotNull(patchedProxy.HomepageSnapshot);
            Assert.NotNull(patchedProxy.HomepageSnapshot.Image);
            Assert.NotNull(patchedProxy.Logins);
            Assert.Equal(2, patchedProxy.Logins.Count);
            var checkOldEmail = patchedProxy.Logins
                .Any(x => x.Email == "email_0@mail.com" && x.Password == "new password");
            Assert.True(checkOldEmail);
            var checkNewEmail = patchedProxy.Logins
                .Any(x => x.Email == "new_mail@mail.com" && x.Password == "fdsfsdfsd");
            Assert.True(checkNewEmail); 
            Assert.NotNull(patchedProxy.Fields);
            Assert.Equal(1, patchedProxy.Fields.Count);
            Assert.Equal("Website 0 Field", patchedProxy.Fields.First().FieldName);
        }

        [Fact]
        public async Task Test_update()
        {
            var controller = await GetWebsiteController();

            var proxyToUpdate = new WebsiteProxy();
            proxyToUpdate.Id = 1;
            proxyToUpdate.Name = "Website 0";
            proxyToUpdate.Url = @"https://github.com/";
            proxyToUpdate.HomepageSnapshot = new WebsiteHomepageSnapshotProxy()
            {
                Image = new byte[] { 32, 43, 64, 54 }
            };
            proxyToUpdate.CategoryId = WebsiteCategoryEnum.Business;
            proxyToUpdate.Logins = new Collection<WebsiteLoginProxy>()
                {
                    new WebsiteLoginProxy()
                    {
                        Email = $"another_email@mail.com",
                        Password = "ddd"
                    }
                };

            proxyToUpdate.Fields = null;

            var updatedProxy = await controller.Update(proxyToUpdate);

            Assert.Equal(proxyToUpdate.Name, updatedProxy.Name);
            Assert.Equal(proxyToUpdate.Url, updatedProxy.Url);
            Assert.Equal(proxyToUpdate.CategoryId, updatedProxy.CategoryId);
            Assert.NotNull(updatedProxy.HomepageSnapshot);
            Assert.NotNull(updatedProxy.HomepageSnapshot.Image);
            Assert.NotNull(updatedProxy.Logins);
            Assert.Equal(proxyToUpdate.Logins.Count, proxyToUpdate.Logins.Count);
            var checkOldEmail = !updatedProxy.Logins
                .Any(x => x.Email == "email_0@mail.com");
            Assert.True(checkOldEmail);
            var checkNewEmail = updatedProxy.Logins
                .Any(x => x.Email == "another_email@mail.com" && x.Password == "ddd");
            Assert.True(checkNewEmail);
            Assert.NotNull(updatedProxy.Fields);
            Assert.Equal(0, updatedProxy.Fields.Count);
        }

    }
}
