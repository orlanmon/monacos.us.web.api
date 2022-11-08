using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace monacos.us.web.services.webapi.ServiceImplementation
{
    public interface IContentService
    {
        public List<dto.DB.Content_DTO> GetActiveContentItems(Int32 ContentArea_ID, bool IncludeOnlyCurrentlyPublished);
        public dto.DB.Content_DTO SelectContentItem(Int32 Content_ID);
        public void DeleteContentItem(Int32 Content_ID);
        public int AddContentItem(dto.DB.Content_DTO objContentItem);
        public void UpdateContentItem(dto.DB.Content_DTO objContentItem);


    }
}
