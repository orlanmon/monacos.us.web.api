using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace monacos.us.web.services.dto.DB
{
    [DataContract(Name = "ContentArea")]
    public class ContentArea_DTO
    {
        
        [DataMember(Name = "ContentAreaId", Order = 1)]
        public int ContentAreaId { get; set; }

        [DataMember(Name = "ContentAreaDescription", Order = 1)]
        public string ContentAreaDescription { get; set; }

    }




}
