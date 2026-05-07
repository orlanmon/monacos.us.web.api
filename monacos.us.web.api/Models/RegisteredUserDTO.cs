using System.Runtime.Serialization;

namespace monacos.us.web.api.Models
{

    [DataContract]
    public class RegisteredUserDTO
    {
        [DataMember]
        public string Username { get; set; } = string.Empty;
        [DataMember]
        public string PasswordHash { get; set; } = string.Empty;

        [DataMember]
        public Int32 UserID { get; set; }

    }
}
