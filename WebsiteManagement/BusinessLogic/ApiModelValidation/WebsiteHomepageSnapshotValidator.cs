using Shared.ApiModel;
using Shared.ApiModelValidation;
using Shared.Exceptions;
using System.Linq;

namespace BusinessLogic.ApiModelValidation
{
    public class WebsiteHomepageSnapshotValidator: IWebsiteHomepageSnapshotValidator
    {
        public void Validate(WebsiteHomepageSnapshotProxy homepageSnapshotProxy, bool allowNull)
        {
            if (allowNull && homepageSnapshotProxy is null)
            {
                return;
            }

            if (homepageSnapshotProxy is null)
            {
                throw new BadRequestError("Homepage snapshot image is required");
            }

            if(homepageSnapshotProxy.Image is null || 
                homepageSnapshotProxy.Image.Count() == 0)
            {
                throw new BadRequestError("Homepage snapshot image bytes cannot be empty");
            }
        }
    }
}
