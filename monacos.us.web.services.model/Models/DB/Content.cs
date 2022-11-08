using System;
using System.Collections.Generic;

#nullable disable

namespace monacos.us.web.services.model.Models.DB
{
    public partial class Content
    {
        public int ContentId { get; set; }
        public int ContentAreaId { get; set; }
        public string Description { get; set; }
        public string ContentValue { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool Active { get; set; }
    }
}
