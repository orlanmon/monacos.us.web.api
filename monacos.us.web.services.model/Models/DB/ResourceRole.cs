using System;
using System.Collections.Generic;

#nullable disable

namespace monacos.us.web.services.model.Models.DB
{
    public partial class ResourceRole
    {
        public int ResourceRoleId { get; set; }
        public int ResourceId { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
    }
}
