using System;
using System.Collections.Generic;

#nullable disable

namespace monacos.us.web.services.model.Models.DB
{
    public partial class AlexaSkillAuthorization
    {
        public int Id { get; set; }
        public string AmazonAlexaClientId { get; set; }
        public string AmazonAlexaState { get; set; }
        public string AmazonAlexaSkillAccessToken { get; set; }
        public string AmazonAlexaUserId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int AlexaSkillAccountId { get; set; }
        public string AmazonAlexaSkillScope { get; set; }
        public string AmazonAlexaAuthRedirectUri { get; set; }
        public bool? Delete { get; set; }

        public virtual AlexaSkillAccount AlexaSkillAccount { get; set; }
    }
}
