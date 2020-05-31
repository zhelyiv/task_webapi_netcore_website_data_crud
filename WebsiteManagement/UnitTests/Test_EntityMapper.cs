using DAL.Ef.EntityMapper;
using Shared.ApiModel;
using Shared.Enums;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class Test_EntityMapper
    { 
        async Task<WebsiteManagementEntityMapper> GetEntityMapper()
        {
            return await InstanceFactory.GetInstance<WebsiteManagementEntityMapper>();
        }

        WebsiteProxy TestProxy => new WebsiteProxy()
        {
            Id = 4,
            Name = "Name",
            Url = "Url",
            CategoryId = WebsiteCategoryEnum.Business,
            HomepageSnapshot = new WebsiteHomepageSnapshotProxy()
            {
                Image = new byte[] { 42, 34, 64, 64 }
            },
            Logins = new Collection<WebsiteLoginProxy>()
                {
                    new WebsiteLoginProxy()
                    {
                        Email = "Email",
                        Password = "Password"
                    }
                },
            Fields = new Collection<WebsiteFieldProxy>()
                {
                    new WebsiteFieldProxy()
                    {
                        FieldName = "FieldName",
                        FieldValue = "FieldValue"
                    }
                }
        };

        [Fact]
        public async Task Test_Create_Website_entity_from_Proxy()
        {
            var entityMapper = await GetEntityMapper();
            var testProxy = TestProxy;

            var websiteEntity = entityMapper.Create(testProxy);

            Assert.Equal(4, websiteEntity.Id);
            Assert.Equal(testProxy.Name, websiteEntity.Name);
            Assert.Equal(testProxy.Url, websiteEntity.Url);
            Assert.Equal(WebsiteStatusEnum.Active, websiteEntity.StatusId);
            Assert.Equal(testProxy.CategoryId, websiteEntity.CategoryId);
            Assert.NotNull(websiteEntity.HomepageSnapshot);
            Assert.Equal(testProxy.HomepageSnapshot.Image, websiteEntity.HomepageSnapshot.Image);
            Assert.NotNull(websiteEntity.Logins);
            Assert.Equal(testProxy.Logins.Count, websiteEntity.Logins.Count);
            Assert.Equal(testProxy.Logins.First().Email, websiteEntity.Logins.First().Email);
            Assert.Equal(testProxy.Logins.First().Password, websiteEntity.Logins.First().Password);
            Assert.NotNull(websiteEntity.Fields);
            Assert.Equal(testProxy.Fields.Count, websiteEntity.Fields.Count);
            Assert.Equal(testProxy.Fields.First().FieldName, websiteEntity.Fields.First().FieldName);
            Assert.Equal(testProxy.Fields.First().FieldValue, websiteEntity.Fields.First().FieldValue);
        }

        [Fact]
        public async Task Test_Update_Name_Url_CategoryId_of_Website_entity_from_Proxy()
        {
            var entityMapper = await GetEntityMapper();
            var testProxy = TestProxy;
            var websiteEntity = entityMapper.Create(testProxy);

            testProxy.Name = "updated name";
            testProxy.Url = "updated Url";
            testProxy.CategoryId = WebsiteCategoryEnum.Commercial;

            entityMapper.Update(testProxy, websiteEntity);

            Assert.Equal(testProxy.Name, websiteEntity.Name);
            Assert.Equal(testProxy.Url, websiteEntity.Url);
            Assert.Equal(testProxy.CategoryId, websiteEntity.CategoryId);
        }

        [Fact]
        public async Task Test_Update_Name_Url_with_null_of_Website_entity_from_Proxy()
        {
            var entityMapper = await GetEntityMapper();
            var testProxy = TestProxy;
            var websiteEntity = entityMapper.Create(testProxy);

            testProxy.Name = null;
            testProxy.Url = null;

            entityMapper.Update(testProxy, websiteEntity);

            Assert.NotNull(websiteEntity.Name);
            Assert.NotNull(websiteEntity.Url);
        }

        [Fact]
        public async Task Test_Update_Logins_of_Website_entity_from_Proxy()
        {
            var entityMapper = await GetEntityMapper();
            var testProxy = TestProxy;
            var websiteEntity = entityMapper.Create(testProxy);

            testProxy.Name = websiteEntity.Name;
            testProxy.Url = websiteEntity.Url;
            testProxy.CategoryId = WebsiteCategoryEnum.Commercial;

            testProxy.Logins.Clear();
            testProxy.Logins.Add(
                new WebsiteLoginProxy()
                {
                    Email = "Email",
                    Password = "new Email password"
                });
            testProxy.Logins.Add(
                new WebsiteLoginProxy()
                {
                    Email = "new login",
                    Password = "new login password"
                });

            entityMapper.Update(testProxy, websiteEntity);

            var newLogin = websiteEntity.Logins
                .FirstOrDefault(x => x.Email == "new login");

            Assert.NotNull(newLogin);
            Assert.Equal("new login password", newLogin.Password);

            var oldLogin = websiteEntity.Logins
               .FirstOrDefault(x => x.Email == "Email");

            Assert.Equal("new Email password", oldLogin.Password);
        }
         

        [Fact]
        public async Task Test_Patch_Homepage_Snapshot_Image_of_Website_entity_from_Proxy()
        {
            var entityMapper = await GetEntityMapper();
            var testProxy = TestProxy;
            var websiteEntity = entityMapper.Create(testProxy);
            
            testProxy = new WebsiteProxy 
            {
                Name = "new name",
                HomepageSnapshot = new WebsiteHomepageSnapshotProxy()
                {
                    Image= new byte[] { 1, 2 }
                }
            };

            Assert.Null(testProxy.Logins);
            Assert.Null(testProxy.Fields);

            var id = websiteEntity.Id;
            var url = websiteEntity.Url;
            var categoryId = websiteEntity.CategoryId;
            var statusId = websiteEntity.StatusId;
            int loginsCount = websiteEntity.Logins.Count;
            int loginsFieldsCount = websiteEntity.Fields.Count;

            entityMapper.Patch(testProxy, websiteEntity);

            Assert.Equal(testProxy.Name, websiteEntity.Name);
            Assert.NotNull(websiteEntity.Logins);
            Assert.NotNull(websiteEntity.Fields); 
            Assert.Equal(id, websiteEntity.Id);
            Assert.Equal(url, websiteEntity.Url);
            Assert.Equal(categoryId, websiteEntity.CategoryId);
            Assert.Equal(statusId, websiteEntity.StatusId);
            Assert.Equal(loginsCount, websiteEntity.Logins.Count);
            Assert.Equal(loginsFieldsCount, websiteEntity.Fields.Count);
        }
    }
}
