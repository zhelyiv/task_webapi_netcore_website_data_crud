using Shared.ApiModel;
using Shared.ApiModelValidation;
using Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace BusinessLogic.ApiModelValidation
{
    public class LoginValidator: ILoginValidator
    {
        public void Validate(WebsiteLoginProxy loginProxy)
        {
            if (string.IsNullOrWhiteSpace(loginProxy.Email))
            {
                throw new BadRequestError($"Empty {nameof(loginProxy.Email)}");
            }

            if(!IsValidEmail(loginProxy.Email.Trim()))
            {
                throw new BadRequestError($"'{loginProxy.Email}' - Invalid email format");
            } 
        }

        public void Validate(ICollection<WebsiteLoginProxy> loginProxies, 
            bool allowNullOrEmpty)
        {
            if (allowNullOrEmpty && (loginProxies is null || loginProxies.Count == 0))
            {
                return;
            }

            if(loginProxies is null || !loginProxies.Any())
            {
                throw new BadRequestError("At least one login is required");
            }

            foreach (var item in loginProxies)
            {
                Validate(item);
            }

            foreach (var group in loginProxies.GroupBy(x => x.Email.Trim()))
            {
                if (group.Count() > 1)
                {
                    throw new BadRequestError($"{nameof(WebsiteLoginProxy.Email)} '{group.Key}' is duplicated");
                }
            }
        }

        /// <summary>
        /// https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
