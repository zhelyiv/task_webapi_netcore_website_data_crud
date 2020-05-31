using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; 
using Shared.ApiModel;
using Shared.DataServices;
using Shared.Enums;

namespace WebsiteManagement.WebAPI.Controllers
{
    /// <summary>
    /// Website API
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class WebsiteController : ControllerBase
    {
        private IWebsiteDataService WebsiteDataService { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="websiteDataService"></param>
        public WebsiteController(IWebsiteDataService websiteDataService)
        {
            WebsiteDataService = websiteDataService;
        }

        /// <summary>
        /// Create new website record
        /// </summary>
        /// <param name="website">website model</param>
        /// <returns>assigned record Id</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<int> Create([FromBody] WebsiteProxy website)
        { 
            return await WebsiteDataService.Create(website);
        }

        /// <summary>
        /// Update a website record - full update(The entire record must be submitted), any missing data is considered as deleted
        /// </summary>
        /// <param name="website">website model, Id of the record is mandatory</param>
        /// <returns>updated website model</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WebsiteProxy))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<WebsiteProxy> Update([FromBody] WebsiteProxy website)
        {
            return await WebsiteDataService.Update(website);
        }

        /// <summary>
        /// Update a website record - partial update, only data submitted is gonna be overwritten
        /// </summary>
        /// <param name="website"></param>
        /// <returns>updated website model</returns>
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WebsiteProxy))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<WebsiteProxy> Patch([FromBody] WebsiteProxy website)
        {
            return await WebsiteDataService.Patch(website);
        }

        /// <summary>
        /// Remove website record
        /// </summary>
        /// <param name="id">website Id</param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task Remove(int id)
        {
            await WebsiteDataService.Remove(id);
        }

        /// <summary>
        /// Get website records count
        /// </summary>
        /// <returns>records count</returns>
        [HttpGet("TotalRecords")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<int> GetTotalRecords()
        {
            return await WebsiteDataService.GetCount();
        }

        /// <summary>
        /// Get website record by id
        /// </summary>
        /// <param name="id">website Id</param>
        /// <returns>website model</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WebsiteProxy))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<WebsiteProxy> Get(int id)
        {
            return await WebsiteDataService.Get(id); 
        }

        /// <summary>
        /// Get paged collection of website records 
        /// </summary>
        /// <param name="page">page number</param>
        /// <param name="pageSize">records per page</param>
        /// <param name="orderBy">fields by which  sort can be applied</param>
        /// <param name="orderType">ascending or descending</param>
        /// <returns>collection of website models</returns>
        [HttpGet("{page}/{pageSize}/{orderBy}/{orderType}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WebsiteProxy[]))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<WebsiteProxy[]> Get(
            int page, 
            int pageSize, 
            WebsiteOrderByEnum orderBy = WebsiteOrderByEnum.Name, 
            WebsiteOrderByAscDescEnum orderType = WebsiteOrderByAscDescEnum.Ascending)
        {
            return await WebsiteDataService.Get(
                page, pageSize, orderBy, orderType);
        }

        /// <summary>
        /// Get all website records
        /// </summary>
        /// <param name="orderBy">predefined order options</param>
        /// <param name="orderType">fields by which  sort can be applied</param>
        /// <returns>collection of website models</returns>
        [HttpGet("All/{orderBy}/{orderType}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WebsiteProxy[]))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<WebsiteProxy[]> Get(
            WebsiteOrderByEnum orderBy = WebsiteOrderByEnum.Name,
            WebsiteOrderByAscDescEnum orderType = WebsiteOrderByAscDescEnum.Ascending)
        {
            return await WebsiteDataService.Get(null, null, orderBy, orderType);
        }
    }
}
