using System.Runtime.Serialization;

namespace monacos.us.web.api.Models
{

    [DataContract]
    public class MenuItemDTO
    {

            [DataMember]
            public string text { get; set; } = string.Empty;

            [DataMember]
            public string imageUrl { get; set; } = string.Empty;

            [DataMember]
            public string url { get; set; } = string.Empty;

            [DataMember]
            public string target { get; set; }  = string.Empty;

            [DataMember]
            public int level { get; set; }


            [DataMember]
            public IList<MenuItemDTO>? children { get; set; }


            // Used Internally Only
            public string NavigationItemID { get; set; }  = string.Empty;

    }
}
