using Microsoft.Extensions.DependencyInjection;
using Shared.ApiModel;
using Shared.ApiModelValidation;
using Shared.Enums;
using Shared.Exceptions;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace BusinessLogic.ApiModelValidation
{
    public class WebsiteValidator: IWebsiteValidator
    {
        #region private
 
        private ILoginValidator LoginValidator { get; }

        private IFieldValidator FieldValidator { get; }

        private IWebsiteHomepageSnapshotValidator WebsiteHomepageSnapshotValidator { get; }

        #endregion

        public WebsiteValidator(ILoginValidator loginValidator,
            IFieldValidator fieldValidator, 
            IWebsiteHomepageSnapshotValidator websiteHomepageSnapshotValidator)
        {
            LoginValidator = loginValidator;
            FieldValidator = fieldValidator;
            WebsiteHomepageSnapshotValidator = websiteHomepageSnapshotValidator;
        }

        public void ValidateOnUpdateOrCreate(WebsiteProxy websiteProxy)
        {
            ValidateWebsiteUrl(websiteProxy.Url);
            ValidateWebsiteName(websiteProxy.Name);
            ValidateCategory(websiteProxy.CategoryId);
            LoginValidator.Validate(websiteProxy.Logins?.ToArray(), false);
            FieldValidator.Validate(websiteProxy.Fields?.ToArray(), false);
            WebsiteHomepageSnapshotValidator.Validate(websiteProxy.HomepageSnapshot, false);
        }
        public void ValidateOnPatch(WebsiteProxy websiteProxy)
        {
            ValidateWebsiteUrl(websiteProxy.Url, true);
            ValidateWebsiteName(websiteProxy.Name, true);
            ValidateCategory(websiteProxy.CategoryId, true);
            LoginValidator.Validate(websiteProxy.Logins?.ToArray(), true);
            FieldValidator.Validate(websiteProxy.Fields?.ToArray(), true);
            WebsiteHomepageSnapshotValidator.Validate(websiteProxy.HomepageSnapshot, true);
        }

        public void ValidateWebsiteUrl(string url, bool allowNull = false)
        {
            if (allowNull && url is null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new BadRequestError($"Empty {nameof(url)}");
            }

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                throw new BadRequestError($"Invalid '{url}', must be a valid absolute url");
            }
        }

        public void ValidateWebsiteName(string name, bool allowNull = false)
        {
            if (allowNull && name is null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BadRequestError($"Empty {nameof(name)}");
            }

            if(Regex.Matches(name, @"[a-zA-Z]").Count == 0)
            {
                throw new BadRequestError($"Invalid {nameof(name)}");
            }
        }

        public void ValidateCategory(WebsiteCategoryEnum? categoryId, bool allowNull = false)
        {
            if (categoryId is null && allowNull)
            {
                return;
            }

            if (!categoryId.HasValue)
            {
                throw new BadRequestError($"Empty {nameof(categoryId)}");
            }
        }
    }
}
