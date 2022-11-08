using System;
using System.Collections.Generic;

#nullable disable

namespace monacos.us.web.services.model.Models.DB
{
    public partial class WebSession
    {
        public int WebSessionId { get; set; }
        public string SessionId { get; set; }
        public DateTime? EntryDateTime { get; set; }
    }
}
