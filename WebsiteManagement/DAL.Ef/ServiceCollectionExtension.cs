using AutoMapper;
using DAL.Ef.EntityMapper;
using DAL.Ef.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared;
using Shared.Repositories;

namespace DAL.Ef
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterDalServices(this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddDbContext<WebsiteManagementDbContext>(options =>
            {
                options.UseSqlServer(configuration["ConnectionStrings:WebsiteManagement"]);
            });
          
            services.AddSingleton(AutoMapperProfile.GetMapperInstance()); 

            services.AddTransient<IWebsiteRepository, WebsiteRepository>();
            services.AddTransient<IWebsiteManagementDbContext, WebsiteManagementDbContext>();
            services.AddTransient<IWebsiteManagementEntityMapper, WebsiteManagementEntityMapper>();
            
            return services;
        }
    }
}
