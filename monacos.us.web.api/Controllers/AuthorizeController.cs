using Microsoft.AspNetCore.Mvc;
using monacos.us.web.api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using monacos.us.web.api.Services;


namespace monacos.us.web.api.Controllers
{
    [ApiController]
    public class AuthorizeController : ControllerBase
    {

        readonly IAuthorize _authorizeservice;
        readonly ILogger _logger;


        public AuthorizeController(IAuthorize authorizeservice, ILogger<AuthorizeController> logger) {

            _authorizeservice = authorizeservice;
            _logger = logger;

        }

        /*
        [ActionName("Register")]
        [HttpPost("api/[controller]/[action]")]
        public IActionResult Register(UserDTO request)
        {

            RegisteredUserDTO? registeredUser = null;

            try
            {

                registeredUser = _authorizeservice.Register(request);


                _logger.LogInformation("Register Success");

                return Ok(registeredUser);

            }
            catch (Exception ex)
            {

                _logger.LogError("Register Error: " + ex.Message);

                return StatusCode(500, new { message = "Register Error: " + ex.Message });

            }

        }
        */

        [ActionName("Register")]
        [HttpPost("api/[controller]/[action]")]
        public async Task<ActionResult> Register(UserDTO request)
        {

            RegisteredUserDTO? registeredUser = null;

            try
            {

                Task<RegisteredUserDTO> registerTask = Task.Run(() => _authorizeservice.Register(request));


                registeredUser = await registerTask;



                _logger.LogInformation("Register Success");

                return Ok(registeredUser);

            }
            catch (Exception ex)
            {

                _logger.LogError("Register Error: " + ex.Message);

                return StatusCode(500, new { message = "Register Error: " + ex.Message });

            }

        }

        /*
        [ActionName("Login")]
        [HttpPost("api/[controller]/[action]")]
        public IActionResult Login(UserDTO request)
        {

            LoggedInUserDTO loggedInUserDTO = null;


            try {

                loggedInUserDTO = _authorizeservice.Login(request);

                if (loggedInUserDTO == null)
                {
                    BadRequest("Bad User Name of Password");
                }

                _logger.LogInformation("Login Success");

                return Ok(loggedInUserDTO);


            }
            catch (Exception ex)
            {


                _logger.LogError("Login Error: " + ex.Message);

                return StatusCode(500, new { message = "Login Error: " + ex.Message });

            }


        }
        */


        [ActionName("Login")]
        [HttpPost("api/[controller]/[action]")]
        public async Task<ActionResult> Login(UserDTO request)
        {

            LoggedInUserDTO loggedInUserDTO = null;

            try
            {

                
                Task<LoggedInUserDTO> loginTask = Task.Run(() => _authorizeservice.Login(request));


                loggedInUserDTO = await loginTask;


                if (loggedInUserDTO == null)
                {
                    BadRequest("Bad User Name of Password");
                }

                _logger.LogInformation("Login Success");

                return Ok(loggedInUserDTO);


            }
            catch (Exception ex)
            {


                _logger.LogError("Login Error: " + ex.Message);

                return StatusCode(500, new { message = "Login Error: " + ex.Message });

            }


        }











        // Test Async Method Use for Template.


        /*
         

        public Task<string> GetDataAsync()
{
    var data = "Synchronous Data";
    // Wraps the result in a completed task
    return Task.FromResult(data);
}

        */



        /*
        



        [ActionName("PullJSON")]
        [HttpGet("api/[controller]/[action]")]
        public async Task<ActionResult> PullJSON()
        {

            try
            {
                string? JSONResult;

                HttpClient httpClient = new HttpClient();


                JSONResult = await httpClient.GetFromJsonAsync<string>("https://jsonplaceholder.typicode.com/todos/1");

               
                return Ok(JSONResult);


            }
            catch(Exception ex)
            {


                return StatusCode(500, new { message = $"PullJSON Error: {ex.Message}" });



            }

        }
        */



    }
}
