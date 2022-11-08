using System;
using System.Collections.Generic;

#nullable disable

namespace monacos.us.web.services.model.Models.DB
{
    public partial class Resource
    {
        public int ResourceId { get; set; }
        public string ResourceName { get; set; }
        public string ResourceDescription { get; set; }
    }
}
