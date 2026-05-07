using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using monacos.us.web.api.Services;
using monacos.us.web.api.Models;

namespace monacos.us.web.api.unit.test.Services
{
    public class AuthorizeServiceUnitTest
    {

        readonly AuthorizeService _authorizeService;

        public AuthorizeServiceUnitTest() {

            var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = builder.Build();

            string? dBConnectionString = configuration.GetConnectionString("DBConnection");


            _authorizeService = new AuthorizeService(dBConnectionString, configuration);



        }

        [Fact]
        public void Login()
        {

            UserDTO userDTO = new UserDTO();
            LoggedInUserDTO? loggedInUser;

            userDTO.Username = "orlanmon";
            userDTO.Password = "GoWestYoungMan_1972";

            loggedInUser = _authorizeService.Login(userDTO);

            Assert.NotNull(loggedInUser);


        }

    }

}
