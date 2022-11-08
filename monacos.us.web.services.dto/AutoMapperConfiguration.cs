using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using AutoMapper;

namespace monacos.us.web.services.dto
{
    static public class AutoMapperConfiguration
    {

        public static MapperConfiguration AutoMapperConfig = null;

        public static void Configure()
        {

            
            AutoMapperConfig = new MapperConfiguration(cfg => {
                cfg.CreateMap<monacos.us.web.services.model.Models.DB.Content, monacos.us.web.services.dto.DB.Content_DTO>();
                cfg.CreateMap<monacos.us.web.services.dto.DB.Content_DTO, monacos.us.web.services.model.Models.DB.Content>();
                cfg.CreateMap<monacos.us.web.services.model.Models.DB.ContentArea, monacos.us.web.services.dto.DB.ContentArea_DTO>();
                cfg.CreateMap<monacos.us.web.services.dto.DB.ContentArea_DTO, monacos.us.web.services.model.Models.DB.ContentArea>();
            });
            

        }


    }
}
