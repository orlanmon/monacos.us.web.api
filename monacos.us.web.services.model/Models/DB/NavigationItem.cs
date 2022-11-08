using System;
using System.Collections.Generic;

#nullable disable

namespace monacos.us.web.services.model.Models.DB
{
    public partial class NavigationItem
    {
        public int NavigationItemId { get; set; }
        public string NavigationItemCaption { get; set; }
        public string NavigationItemImage { get; set; }
        public string NavigationItemUri { get; set; }
        public int? ParentNavigationItemId { get; set; }
        public int NavigationItemLevelSortOrder { get; set; }
        public int NavigationTypeId { get; set; }
        public string NavigationItemUriTarget { get; set; }
        public bool? NavigationItemTreeViewVisible { get; set; }
        public bool? NavigationItemMenuVisible { get; set; }
        public string NavigationItemSiteMapCaption { get; set; }
    }
}
