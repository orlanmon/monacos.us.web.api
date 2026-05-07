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

    }
}
