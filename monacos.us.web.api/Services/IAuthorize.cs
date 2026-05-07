using Microsoft.AspNetCore.Mvc;
using monacos.us.web.api.Models;

namespace monacos.us.web.api.Services
{
    public interface IAuthorize
    {

        public RegisteredUserDTO Register(UserDTO register_request);

        public LoggedInUserDTO? Login(UserDTO login_request);

  

    }
}
