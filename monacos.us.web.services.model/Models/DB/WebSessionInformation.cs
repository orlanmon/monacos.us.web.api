using System;
using System.Collections.Generic;

#nullable disable

namespace monacos.us.web.services.model.Models.DB
{
    public partial class WebSessionInformation
    {
        public int WebSessionInformationId { get; set; }
        public int WebSessionId { get; set; }
        public string WebSessionServerVariableName { get; set; }
        public string WebSessionServerVariableValue { get; set; }
        public DateTime? EntryDateTime { get; set; }
    }
}
