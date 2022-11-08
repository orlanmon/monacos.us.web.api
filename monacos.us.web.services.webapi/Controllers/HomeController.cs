using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace monacos.us.web.services.webapi.Controllers
{

    
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Route("/")]
        public IActionResult IntroPage()
        {

            
            try
            {

                return new OkObjectResult("monacos.us.web.sevices.webapi");

            }
            catch (Exception ex)
            {

                return new ObjectResult(ex.Message);

            }

        }

    }
}
