using Microsoft.AspNetCore.Builder;

namespace WebsiteManagement.WebAPI.MiddlewareExtensions
{
    /// <summary>
    /// ExceptionHandler Middleware
    /// </summary>
    public static class ExceptionMiddlewareExtensions
    {
        /// <summary>
        /// Use Custom ExceptionHandler
        /// </summary>
        /// <param name="app">IApplicationBuilder instance</param>
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                { 
                    await new CustomExceptionHandler(context).ProcessError();
                });
            }); 
        }
    }
}
