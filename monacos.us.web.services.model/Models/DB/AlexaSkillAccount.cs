using System;
using System.Collections.Generic;

#nullable disable

namespace monacos.us.web.services.model.Models.DB
{
    public partial class AlexaSkillAccount
    {
        public AlexaSkillAccount()
        {
            AlexaSkillAuthorizations = new HashSet<AlexaSkillAuthorization>();
            AlexaSkillConfigurationData = new HashSet<AlexaSkillConfigurationDatum>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string AmazonAlexaSkillName { get; set; }
        public bool? Delete { get; set; }

        public virtual ICollection<AlexaSkillAuthorization> AlexaSkillAuthorizations { get; set; }
        public virtual ICollection<AlexaSkillConfigurationDatum> AlexaSkillConfigurationData { get; set; }
    }
}
