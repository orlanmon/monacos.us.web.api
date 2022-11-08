using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace monacos.us.web.services.dto.DB
{
    [DataContract(Name = "Content")]
    public class Content_DTO
    {
        [DataMember(Name = "ContentId", Order = 1)]
        public Int32 ContentId { get; set; }
        [DataMember(Name = "ContentAreaId", Order = 2)]
        public Int32 ContentAreaId { get; set; }
        [DataMember(Name = "ContentValue", Order = 3)]
        public String ContentValue { get; set; }
        [DataMember(Name = "CreateDate", Order = 4)]
        public String CreateDate { get; set; }
        [DataMember(Name = "PublishDate", Order = 5)]
        public String PublishDate { get; set; }
        [DataMember(Name = "ExpirationDate", Order = 6)]
        public String? ExpirationDate { get; set; }
        [DataMember(Name = "Description", Order = 7)]
        public string Description { get; set; }
        [DataMember(Name = "Active", Order = 8)]
        public bool Active { get; set; }
    }

}
