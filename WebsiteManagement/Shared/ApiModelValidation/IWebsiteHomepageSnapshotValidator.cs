using Shared.ApiModel;

namespace Shared.ApiModelValidation
{
    public interface IWebsiteHomepageSnapshotValidator
    {
        void Validate(WebsiteHomepageSnapshotProxy homepageSnapshotProxy, bool allowNull);
    }
}
