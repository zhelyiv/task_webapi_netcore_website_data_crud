using BusinessLogic;
using DAL.Ef;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; 

namespace ServiceRegistrator
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterBusinessLogicServices();
            services.RegisterDalServices(configuration);

            return services;
        }
    }
}
