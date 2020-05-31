using System.Net;

namespace Shared.Exceptions
{
    public class BadRequestError : WebsiteManagementBaseException
    {
        public BadRequestError(string message): base(message) 
        {
            HttpStatusCode = HttpStatusCode.BadRequest;
        }
    }
}
