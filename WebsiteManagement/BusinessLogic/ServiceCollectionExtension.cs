using BusinessLogic.ApiModelValidation;
using BusinessLogic.DataServices;
using Microsoft.Extensions.DependencyInjection;
using Shared.ApiModelValidation;
using Shared.DataServices;

namespace BusinessLogic
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterBusinessLogicServices(this IServiceCollection services)
        {
            services.AddTransient<IWebsiteDataService, WebsiteDataService>();
            services.AddTransient<IWebsiteValidator, WebsiteValidator>();
            services.AddTransient<IWebsiteHomepageSnapshotValidator, WebsiteHomepageSnapshotValidator>();
            services.AddTransient<ILoginValidator, LoginValidator>();
            services.AddTransient<IFieldValidator, FieldValidator>();
            services.AddTransient<IPagingValidator, PagingValidator>();
            //services.AddTransient<iii, xxx>();

            return services;
        }
    }
}
