using monacos.us.web.api.Models;

namespace monacos.us.web.api.Services
{
    public interface INavigation
    {

        public  List<MenuItemDTO> BuildNavigationMenu(int NavigationTypeID, int User_ID);

        public List<TreeViewItemDTO> BuildTreeView(int NavigationTypeID);


    }
}
