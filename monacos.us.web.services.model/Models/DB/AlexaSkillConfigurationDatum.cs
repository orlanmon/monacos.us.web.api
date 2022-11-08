using System;
using System.Collections.Generic;

#nullable disable

namespace monacos.us.web.services.model.Models.DB
{
    public partial class AlexaSkillConfigurationDatum
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int AlexaSkillAccountId { get; set; }
        public bool? Delete { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual AlexaSkillAccount AlexaSkillAccount { get; set; }
    }
}
