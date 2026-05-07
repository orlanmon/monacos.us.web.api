using System.Runtime.Serialization;

namespace monacos.us.web.api.Models
{
    [DataContract]
    public class UserDTO
    {
        [DataMember]
        public string Username { get; set; } = string.Empty;

        [DataMember]
        public string Password { get; set; } = string.Empty;

    }
}
