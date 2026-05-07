using System.Runtime.Serialization;

namespace monacos.us.web.api.Models
{

    [DataContract]
    public class LoggedInUserDTO
    {

        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Token { get; set; }

        [DataMember]
        public int UserID { get; set; }

        [DataMember]
        public IList<int> UserRoles { get; set; }


    }
}
