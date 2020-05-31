using AutoMapper;
using Microsoft.EntityFrameworkCore.Internal;  
using System.Linq;
using System.Collections.Generic;
using Shared.ApiModel;
using DAL.Ef.Entities;

namespace DAL.Ef.EntityMapper
{
    public class WebsiteManagementEntityMapper: IWebsiteManagementEntityMapper
    {
        private IMapper AutoMapper { get; }

        public WebsiteManagementEntityMapper(IMapper autoMapper)
        {
            AutoMapper = autoMapper;
        }

        public Website Create(WebsiteProxy websiteProxy)
        {
            return AutoMapper.Map<Website>(websiteProxy);
        }

        public WebsiteProxy[] Map(params Website[] websites)
        {
            return AutoMapper.Map<WebsiteProxy[]>(websites);
        }
     
        public void Update(WebsiteProxy srcProxy, Website dest)
        {
            var src = Create(srcProxy);
             
            AutoMapper.Map(src, dest);

            RemoveMissingFields(dest.Fields, src.Fields);
            RemoveMissingLogins(dest.Logins, src.Logins);
            
            UpdateFields(dest.Fields, src.Fields);
            UpdateLogins(dest.Logins, src.Logins);
            
            AddNewFields(dest.Fields, src.Fields);
            AddNewLogins(dest.Logins, src.Logins);
        }

        public void Patch(WebsiteProxy srcProxy, Website dest)
        {
            var src = Create(srcProxy);

            var categoryId = dest.CategoryId;

            AutoMapper.Map(src, dest);

            if (!srcProxy.CategoryId.HasValue)
            {
                dest.CategoryId = categoryId;
            }

            UpdateFields(dest.Fields, src.Fields);
            UpdateLogins(dest.Logins, src.Logins);

            AddNewFields(dest.Fields, src.Fields);
            AddNewLogins(dest.Logins, src.Logins);
        }
         
        public void RemoveMissingFields(ICollection<WebsiteField> oldFields, ICollection<WebsiteField> updatedFields)
        {
            var fieldsToRemove = oldFields
                .Where(x => !updatedFields.Any(z => z.FieldName == x.FieldName))
                .ToArray();

            foreach (var field in fieldsToRemove)
            {
                oldFields.Remove(field);
            }
        }
        
        public void RemoveMissingLogins(ICollection<WebsiteLogin> oldLogins, ICollection<WebsiteLogin> updatedLogins)
        {
            var loginsToRemove = oldLogins
                .Where(x => !updatedLogins.Any(z => z.Email == x.Email))
                .ToArray();

            foreach (var login in loginsToRemove)
            {
                oldLogins.Remove(login);
            }
        }
        
        public void UpdateFields(ICollection<WebsiteField> oldFields, ICollection<WebsiteField> updatedFields)
        {
            var fieldsToUpdate = updatedFields
                .Where(x => oldFields.Any(z => z.FieldName?.Trim().ToUpper() == x.FieldName?.Trim().ToUpper()))
                .ToDictionary(k => k.FieldName);

            foreach (var field in oldFields)
            {
                if (fieldsToUpdate.ContainsKey(field.FieldName))
                {
                    AutoMapper.Map(fieldsToUpdate[field.FieldName], field);
                }
            }
        }
       
        public void UpdateLogins(ICollection<WebsiteLogin> oldLogins, ICollection<WebsiteLogin> updatedLogins)
        {
            var loginsToUpdate = updatedLogins
                .Where(x => oldLogins.Any(z => z.Email?.Trim().ToUpper() == x.Email?.Trim().ToUpper()))
                .ToDictionary(k => k.Email);

            foreach (var login in oldLogins)
            {
                if (loginsToUpdate.ContainsKey(login.Email))
                {
                    AutoMapper.Map(loginsToUpdate[login.Email], login);
                }
            }
        }
        
        public void AddNewFields(ICollection<WebsiteField> oldFields, ICollection<WebsiteField> updatedFields)
        {
            var newFields = updatedFields
                .Where(x => !oldFields.Any(z => z.FieldName == x.FieldName));

            foreach(var field in newFields)
            {
                oldFields.Add(field);
            }
        }
        
        public void AddNewLogins(ICollection<WebsiteLogin> oldLogins, ICollection<WebsiteLogin> updatedLogins)
        {
            var newFields = updatedLogins
                .Where(x => !oldLogins.Any(z => z.Email == x.Email));

            foreach (var login in newFields)
            {
                oldLogins.Add(login);
            }
        }
    }
}
