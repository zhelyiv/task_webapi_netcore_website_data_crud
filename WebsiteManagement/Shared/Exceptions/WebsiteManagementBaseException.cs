using System; 
using System.Net; 

namespace Shared.Exceptions
{
    public abstract class WebsiteManagementBaseException: Exception
    {
        protected WebsiteManagementBaseException(string message): base(message)
        { 
        } 
        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
