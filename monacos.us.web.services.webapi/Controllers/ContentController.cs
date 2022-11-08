using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dto = monacos.us.web.services.dto;
using monacos.us.web.services.webapi.ServiceImplementation;



namespace monacos.us.web.services.webapi.Controllers
{

    //[Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IContentService _ContentService;



        public ContentController(IConfiguration configuration, IContentService ContentService)
        {
            
            this._configuration = configuration;
            this._ContentService = ContentService;

        }


        // Attribute Based Routing
        //[HttpGet("api/[controller]/[action]/")]
        //[Route("api/[controller]/GetAll")]
        [HttpGet]
        [Produces("application/json")]
        [Route("api/Content/GetAll/{ContentArea_ID}/{IncludeOnlyCurrentlyPublished}")]
        public IActionResult GetActiveContentItems(Int32 ContentArea_ID, bool IncludeOnlyCurrentlyPublished)
        {

            try
            {

                string DatabaseConnectionString = "";
                List<dto.DB.Content_DTO> colContent = new List<dto.DB.Content_DTO>();


                //DatabaseConnectionString = _configuration.GetConnectionString("DatabaseConnection");

                //ContentService objContentService = new ContentService(DatabaseConnectionString);


                colContent = this._ContentService.GetActiveContentItems(ContentArea_ID, IncludeOnlyCurrentlyPublished);

                return new OkObjectResult(colContent);

            }
            catch (Exception ex)
            {

                return new ObjectResult(ex.Message);

            }

        }

        // Attribute Based Routing
        [HttpGet]
        [Produces("application/json")]
        [Route("api/Content/Get/{id}")]
        public IActionResult SelectContentItem(int id)
        {

            try
            {

                string DatabaseConnectionString = "";

                dto.DB.Content_DTO objContentItem = null;

                //DatabaseConnectionString = _configuration.GetConnectionString("DatabaseConnection");

                //ContentService objContentService = new ContentService(DatabaseConnectionString);

                objContentItem = _ContentService.SelectContentItem(id);

                return new OkObjectResult(objContentItem);


            }
            catch (Exception ex)
            {

                return new ObjectResult(ex.Message);


            }

        }

        // Attribute Based Routing
        [HttpGet]
        [Produces("application/json")]
        [Route("api/Content/Delete/{id}")]
        public IActionResult DeleteContentItem(int id)
        {

            try
            {

                string DatabaseConnectionString = "";

                dto.DB.Content_DTO objContentItem = null;

                //DatabaseConnectionString = _configuration.GetConnectionString("DatabaseConnection");

                //ContentService objContentService = new ContentService(DatabaseConnectionString);

                _ContentService.DeleteContentItem(id);

                return new OkObjectResult(string.Format("Content Item {0} Deleted", id) );

            }
            catch (Exception ex)
            {

                return new ObjectResult(ex.Message);


            }

        }

        // Attribute Based Routing
        [HttpPost]
        [Produces("application/json")]
        [Route("api/Content/Add")]
        public IActionResult Add(dto.DB.Content_DTO objContentItem)
        {

            try
            {

                string DatabaseConnectionString = "";
                int id = 0;

                // DatabaseConnectionString = _configuration.GetConnectionString("DatabaseConnection");

                // ContentService objContentService = new ContentService(DatabaseConnectionString);

                id = _ContentService.AddContentItem(objContentItem);

                return new OkObjectResult(string.Format("Content Item {0} Added", id));

            }
            catch (Exception ex)
            {

                return new ObjectResult(ex.Message);


            }

        }

        // Attribute Based Routing
        [HttpPost]
        [Produces("application/json")]
        [Route("api/Content/Update")]
        public IActionResult Update(dto.DB.Content_DTO objContentItem)
        {

            try
            {

                // DatabaseConnectionString = _configuration.GetConnectionString("DatabaseConnection");

                // ContentService objContentService = new ContentService(DatabaseConnectionString);

                _ContentService.UpdateContentItem(objContentItem);

                return new OkObjectResult(string.Format("Content Item {0} Updated", objContentItem.ContentId));

            }
            catch (Exception ex)
            {

                return new ObjectResult(ex.Message);


            }

        }
    }
}
