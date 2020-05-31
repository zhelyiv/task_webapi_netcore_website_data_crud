using Shared.ApiModel;
using System.Collections.Generic;

namespace Shared.ApiModelValidation
{
    public interface ILoginValidator
    {
        void Validate(WebsiteLoginProxy loginProxy);
        void Validate(ICollection<WebsiteLoginProxy> loginProxies, bool allowNullOrEmpty);
    }
}
