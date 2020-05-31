using System.Net; 

namespace Shared.Exceptions
{
    public class NotFoundError : WebsiteManagementBaseException
    {
        public NotFoundError(string message): base(message) 
        { 
            HttpStatusCode = HttpStatusCode.NotFound;
        }
    }
}
