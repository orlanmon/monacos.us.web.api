using Microsoft.Extensions.Configuration;
using monacos.us.web.api.Services;
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
    public class NavigationServiceUnitTest
    {

        readonly NavigationService _navigationService;


        public NavigationServiceUnitTest()
        {

            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = builder.Build();

            string? dBConnectionString = configuration.GetConnectionString("DBConnection");

            _navigationService = new NavigationService(dBConnectionString);


        }



        [Fact]
        public void BuildNavigationMenu()
        {
            List<MenuItemDTO>? items = null;


            items = _navigationService.BuildNavigationMenu(1, 3);

            Assert.NotNull(items);
           
        }

        [Fact]
        public void BuildTreeView()
        {

            List<TreeViewItemDTO>? items = null;


            items = _navigationService.BuildTreeView(2);

            Assert.NotNull(items);


        }

    }
}
