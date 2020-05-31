using Shared.ApiModel;
using Shared.Enums;

namespace Shared.ApiModelValidation
{
    public interface IWebsiteValidator
    {
        void ValidateOnUpdateOrCreate(WebsiteProxy websiteProxy);
          
        void ValidateOnPatch(WebsiteProxy websiteProxy);

        void ValidateWebsiteUrl(string url, bool allowNull);

        void ValidateWebsiteName(string name, bool allowNull);

        void ValidateCategory(WebsiteCategoryEnum? categoryId, bool allowNull);
    }
}
