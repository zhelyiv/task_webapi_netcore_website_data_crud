using Shared.ApiModel;
using Shared.ApiModelValidation;
using Shared.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.ApiModelValidation
{
    public class FieldValidator: IFieldValidator
    {
        public void Validate(WebsiteFieldProxy fieldProxy)
        {
            if (string.IsNullOrWhiteSpace(fieldProxy.FieldName))
            {
                throw new BadRequestError($"Empty {nameof(fieldProxy.FieldName)}");
            }
        }

        public void Validate(ICollection<WebsiteFieldProxy> fieldProxies, 
            bool allowNullOrEmpty)
        {
            if(allowNullOrEmpty && (fieldProxies is null || fieldProxies.Count == 0))
            {
                return;
            }

            if(fieldProxies is null)
            {
                return;
            }

            foreach(var item in fieldProxies)
            {
                Validate(item);
            }
             
            foreach(var group in fieldProxies.GroupBy(x => x.FieldName.Trim()))
            {
                if(group.Count() > 1)
                {
                    throw new BadRequestError($"{nameof(WebsiteFieldProxy.FieldName)} '{group.Key}' is duplicated");
                }
            }
        } 
    }
}
