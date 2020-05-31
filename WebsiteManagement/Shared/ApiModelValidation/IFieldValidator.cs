using Shared.ApiModel;
using System.Collections.Generic;

namespace Shared.ApiModelValidation
{
    public interface IFieldValidator
    {
        void Validate(WebsiteFieldProxy fieldProxy);
        void Validate(ICollection<WebsiteFieldProxy> fieldProxies, bool allowNullOrEmpty);
    }
}
