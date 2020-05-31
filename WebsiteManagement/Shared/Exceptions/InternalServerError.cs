using System.Net; 

namespace Shared.Exceptions
{
    public class InternalServerError: WebsiteManagementBaseException
    {
        public InternalServerError(string message): base(message)
        {
            HttpStatusCode = HttpStatusCode.InternalServerError;
        }
    }
}
