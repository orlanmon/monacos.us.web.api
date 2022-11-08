using System;
using System.Collections.Generic;

#nullable disable

namespace monacos.us.web.services.model.Models.DB
{
    public partial class UserRoleXref
    {
        public int UserId { get; set; }
        public int ResourceRoleId { get; set; }
    }
}
