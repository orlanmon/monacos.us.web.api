using System;
using System.Collections.Generic;

#nullable disable

namespace monacos.us.web.services.model.Models.DB
{
    public partial class NavigationItemResourceRoleXref
    {
        public int ResourceRoleId { get; set; }
        public int NavigationItemId { get; set; }
        public int NavigationRoleXrefId { get; set; }
    }
}
