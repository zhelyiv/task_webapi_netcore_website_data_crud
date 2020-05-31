using BusinessLogic.ApiModelValidation;
using Shared.ApiModel;
using Shared.Enums;
using Shared.Exceptions;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class Test_ApiModelValidators
    { 
        async Task<WebsiteValidator> GetWebsiteValidator()
        {
            return await InstanceFactory.GetInstance<WebsiteValidator>();
        }

        WebsiteProxy TestProxy => new WebsiteProxy()
        {
            Id = 4,
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

        [Fact]
        public async Task Test_With_valid_WesiteProxy()
        {
            var testProxy = TestProxy;
            (await GetWebsiteValidator()).ValidateOnUpdateOrCreate(testProxy);
        }

        [Fact]
        public async Task Test_with_invalid_website_name()
        {
            var validator = await GetWebsiteValidator();
            var testProxy = TestProxy;
            testProxy.Name = "&*%&&87465";
            Assert.Throws<BadRequestError>(() =>
            validator.ValidateOnUpdateOrCreate(testProxy));
        }

        [Fact]
        public async Task Test_with_invalid_website_url()
        {
            var validator = await GetWebsiteValidator();
            var testProxy = TestProxy;
            testProxy.Url = "fdsfsd";
            Assert.Throws<BadRequestError>(() =>
            validator.ValidateOnUpdateOrCreate(testProxy));
        }

        [Fact]
        public async Task Test_with_empty_category()
        {
            var validator = await GetWebsiteValidator();
            var testProxy = TestProxy;
            testProxy.CategoryId = null;
            Assert.Throws<BadRequestError>(() =>
            validator.ValidateOnUpdateOrCreate(testProxy));
        }

        [Fact]
        public async Task Test_with_missing_HomepageSnapshot()
        {
            var validator = await GetWebsiteValidator();
            var testProxy = TestProxy;
            testProxy.HomepageSnapshot = null;
            Assert.Throws<BadRequestError>(() =>
            validator.ValidateOnUpdateOrCreate(testProxy));
        }

        [Fact]
        public async Task Test_with_missing_HomepageSnapshot_Image()
        {
            var validator = await GetWebsiteValidator();
            var testProxy = TestProxy;
            testProxy.HomepageSnapshot.Image = null;
            Assert.Throws<BadRequestError>(() =>
            validator.ValidateOnUpdateOrCreate(testProxy));
        }

        [Fact]
        public async Task Test_with_empty_HomepageSnapshot_Image()
        {
            var validator = await GetWebsiteValidator();
            var testProxy = TestProxy;
            testProxy.HomepageSnapshot.Image = new byte[] { };
            Assert.Throws<BadRequestError>(() =>
            validator.ValidateOnUpdateOrCreate(testProxy));
        }

        [Fact]
        public async Task Test_with_missing_Logins()
        {
            var validator = await GetWebsiteValidator();
            var testProxy = TestProxy;
            testProxy.Logins = null;
            Assert.Throws<BadRequestError>(() =>
            validator.ValidateOnUpdateOrCreate(testProxy));
        }

        [Fact]
        public async Task Test_with_empty_Logins()
        {
            var validator = await GetWebsiteValidator();
            var testProxy = TestProxy;
            testProxy.Logins = new Collection<WebsiteLoginProxy>();
            Assert.Throws<BadRequestError>(() =>
            validator.ValidateOnUpdateOrCreate(testProxy));
        }

        [Fact]
        public async Task Test_with_invalid_email_in_Logins()
        {
            var validator = await GetWebsiteValidator();
            var testProxy = TestProxy;
            testProxy.Logins.First().Email = "bad email";
            Assert.Throws<BadRequestError>(() =>
            validator.ValidateOnUpdateOrCreate(testProxy));
        }

        [Fact]
        public async Task Test_with_missing_email_in_Logins()
        {
            var validator = await GetWebsiteValidator();
            var testProxy = TestProxy;
            testProxy.Logins.First().Email = " ";
            Assert.Throws<BadRequestError>(() =>
            validator.ValidateOnUpdateOrCreate(testProxy));
        }

        [Fact]
        public async Task Test_with_duplicated_email_in_Logins()
        {
            var validator = await GetWebsiteValidator();
            var testProxy = TestProxy;
            testProxy.Logins.Add(new WebsiteLoginProxy() { Email = "mystackoverflowlogin@mail.com" });
            Assert.Throws<BadRequestError>(() =>
            validator.ValidateOnUpdateOrCreate(testProxy));
        }

        [Fact]
        public async Task Test_with_duplicated_FieldName_in_Logins()
        {
            var validator = await GetWebsiteValidator();
            var testProxy = TestProxy;
            testProxy.Fields.Add(new WebsiteFieldProxy() { FieldName = "Field Name" });
            Assert.Throws<BadRequestError>(() =>
            validator.ValidateOnUpdateOrCreate(testProxy));
        }

        [Fact]
        public async Task Test_with_empty_FieldName_in_Logins()
        {
            var validator = await GetWebsiteValidator();
            var testProxy = TestProxy;
            testProxy.Fields.First().FieldName = " ";
            Assert.Throws<BadRequestError>(() =>
            validator.ValidateOnUpdateOrCreate(testProxy));
        }

    }
}
