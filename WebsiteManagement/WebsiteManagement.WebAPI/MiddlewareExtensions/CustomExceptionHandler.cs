using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Exceptions;
using System; 
using System.Linq; 
using System.Net;
using System.Text; 
using System.Threading.Tasks;

namespace WebsiteManagement.WebAPI.MiddlewareExtensions
{
    /// <summary>
    /// Custom ExceptionHandler
    /// </summary>
    public class CustomExceptionHandler
    {
        private ILogger Logger { get; }
        private HttpContext HttpContext { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        public CustomExceptionHandler(HttpContext httpContext)
        {
            HttpContext = httpContext; 
            Logger = httpContext.RequestServices
                .GetRequiredService<ILogger<CustomExceptionHandler>>();
        }

        /// <summary>
        /// Handle the Context error and transform it ot standartized JsonResult regarding the exception
        /// </summary>
        /// <returns></returns>
        public async Task ProcessError()
        {
            var contextFeaute = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = contextFeaute.Error;
             
            var exceptionMessage = exception.Message;

            if (exception is WebsiteManagementBaseException wmex)
            { 
                HttpContext.Response.StatusCode = (int)wmex.HttpStatusCode;
            }
            else if (exception is DbUpdateException dbue)
            {
                exceptionMessage = dbue.GetDbUpdateExceptionMessage();
            }
             
            if (contextFeaute != null)
            {
                var innerExceptionMessages = exception.GetInnerExceptions().Select(x => x.Message);
                var logError = $"{exceptionMessage}{Environment.NewLine}Inner Exception:{Environment.NewLine}{string.Join(Environment.NewLine, innerExceptionMessages)}";
                Logger.LogError(new StringBuilder()
                    .AppendLine($"Endpoint [{HttpContext.Request.Path}] Error:")
                    .AppendLine(logError)
                    .AppendLine(exception.StackTrace)
                    .ToString());

                HttpContext.Response.ContentType = "application/json";
                HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var actionResult = new JsonResult(exceptionMessage)
                {
                    ContentType = HttpContext.Response.ContentType,
                    StatusCode = HttpContext.Response.StatusCode 
                };
                 
                await HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(actionResult));
            }
        } 
    }
}
