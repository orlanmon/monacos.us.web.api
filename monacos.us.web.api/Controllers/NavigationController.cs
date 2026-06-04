using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using monacos.us.web.api.Models;
using monacos.us.web.api.Services;

namespace monacos.us.web.api.Controllers
{
    [ApiController]
    public class NavigationController : ControllerBase
    {


        readonly INavigation _navigationService;
        readonly ILogger _logger;


        public NavigationController(INavigation navigationService, ILogger<NavigationController> logger ) { 
        
            this._navigationService = navigationService;

            this._logger = logger;


        }

        /*
        [ActionName("BuildNavigationMenu")]
        [HttpGet("api/[controller]/[action]/{NavigationTypeID}/{User_ID}"), Authorize() ]
        public IActionResult BuildNavigationMenu(int NavigationTypeID, int User_ID)
        {
            List<MenuItemDTO> objMenuItems;


            try
            {

                objMenuItems  = _navigationService.BuildNavigationMenu(NavigationTypeID, User_ID);


                _logger.LogInformation("BuildNavigationMenu Success");


                return Ok(objMenuItems);

            }
            catch (Exception ex)
            {

                _logger.LogError("BuildNavigationMenu Error: " + ex.Message);

                return StatusCode(500, new { message = "BuildNavigationMenu Error: " + ex.Message });

            }


        }
        */


        [ActionName("BuildNavigationMenu")]
        [HttpGet("api/[controller]/[action]/{NavigationTypeID}/{User_ID}"), Authorize()]
        public async Task<ActionResult> BuildNavigationMenu(int NavigationTypeID, int User_ID)
        {
            List<MenuItemDTO> objMenuItems;


            try
            {

                Task<List<MenuItemDTO>> buildNavigationMenuTask = Task.Run(() => _navigationService.BuildNavigationMenu(NavigationTypeID, User_ID));

                objMenuItems = await buildNavigationMenuTask;

                _logger.LogInformation("BuildNavigationMenu Success");

                return Ok(objMenuItems);

            }
            catch (Exception ex)
            {

                _logger.LogError("BuildNavigationMenu Error: " + ex.Message);

                return StatusCode(500, new { message = "BuildNavigationMenu Error: " + ex.Message });

            }


        }



        /*
        [ActionName("BuildTreeView")]
        [HttpGet("api/[controller]/[action]/{NavigationTypeID}"), Authorize()]
        public IActionResult BuildTreeView(int NavigationTypeID)
        {
            List<TreeViewItemDTO> objTreeViewItems;


            try
            {

                objTreeViewItems = _navigationService.BuildTreeView(NavigationTypeID);


                _logger.LogInformation("BuildNavigationMenu Success");

                return Ok(objTreeViewItems);

            }
            catch (Exception ex)
            {

                _logger.LogError("BuildTreeView Error: " + ex.Message);

                return StatusCode(500, new { message = "BuildTreeView Error: " + ex.Message });

            }


        }
        */


        [ActionName("BuildTreeView")]
        [HttpGet("api/[controller]/[action]/{NavigationTypeID}"), Authorize()]
        public async Task<ActionResult> BuildTreeView(int NavigationTypeID)
        {
            List<TreeViewItemDTO> objTreeViewItems;


            try
            {

                Task<List<TreeViewItemDTO>> buildTreeViewTask = Task.Run(() => _navigationService.BuildTreeView(NavigationTypeID));

                objTreeViewItems = await buildTreeViewTask;


                _logger.LogInformation("BuildNavigationMenu Success");

                return Ok(objTreeViewItems);

            }
            catch (Exception ex)
            {

                _logger.LogError("BuildTreeView Error: " + ex.Message);

                return StatusCode(500, new { message = "BuildTreeView Error: " + ex.Message });

            }


        }



    }
}
