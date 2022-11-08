using System;
using System.Collections.Generic;
using System.Linq;
using dal = monacos.us.web.services.dal;
using dmodels = monacos.us.web.services.model.Models.DB;
using dto = monacos.us.web.services.dto;



namespace monacos.us.web.services.webapi.ServiceImplementation
{
    public class ContentService : IContentService
    {

        private string _DatabaseConnectionString = "";

        AutoMapper.IMapper _objAutoMapper = dto.AutoMapperConfiguration.AutoMapperConfig.CreateMapper();


        public ContentService(string DatabaseConnectionString)
        {

            this._DatabaseConnectionString = DatabaseConnectionString;

        }

        public List<dto.DB.Content_DTO> GetActiveContentItems(Int32 ContentArea_ID, bool IncludeOnlyCurrentlyPublished)
        {

            List<dto.DB.Content_DTO> objList_Content_DTO = new List<dto.DB.Content_DTO>();
            
            List<dmodels.Content> objList_Content = new List<dmodels.Content> ();

            dal.Content_DAL objContentSystem_DAL = null;

            try
            {
                
                objContentSystem_DAL = new dal.Content_DAL(this._DatabaseConnectionString);

                objList_Content = objContentSystem_DAL.Get(ContentArea_ID, IncludeOnlyCurrentlyPublished);

                foreach (dmodels.Content ContentItem in objList_Content)
                {

                    objList_Content_DTO.Add(_objAutoMapper.Map<dmodels.Content, dto.DB.Content_DTO>(ContentItem));

                }

            }

            catch (System.Exception objException)
            {
                throw new Exception("ContentSystem_BO::GetActiveContentItems Error: " + objException.Message);
            }

            finally
            {

            }

            return objList_Content_DTO;

        }

        public dto.DB.Content_DTO SelectContentItem(Int32 Content_ID)
        {

            dmodels.Content obj_Content = null;
            dal.Content_DAL objContentSystem_DAL = null;

            try
            {

                objContentSystem_DAL = new dal.Content_DAL(this._DatabaseConnectionString);

                obj_Content = objContentSystem_DAL.Select(Content_ID);

            }

            catch (System.Exception objException)
            {
                throw new Exception("ContentSystem_BO::SelectContentItem Error: " + objException.Message);
            }

            finally
            {

            }

            return _objAutoMapper.Map<dmodels.Content, dto.DB.Content_DTO>(obj_Content);

        }

        public void DeleteContentItem(Int32 Content_ID)
        {

            dal.Content_DAL objContentSystem_DAL = null;

            try
            {

                objContentSystem_DAL = new dal.Content_DAL(this._DatabaseConnectionString);

                objContentSystem_DAL.Delete(Content_ID);

            }

            catch (System.Exception objException)
            {
                throw new Exception("ContentSystem_BO::DeleteContentItem Error: " + objException.Message);
            }

            finally
            {

            }

        }

        public int AddContentItem(dto.DB.Content_DTO objContentItem)
        {

            dal.Content_DAL objContentSystem_DAL = null;
            int id = 0;

            try
            {

                objContentSystem_DAL = new dal.Content_DAL(this._DatabaseConnectionString);

                id = objContentSystem_DAL.Add(_objAutoMapper.Map<dto.DB.Content_DTO, dmodels.Content>(objContentItem));

                return id;

            }

            catch (System.Exception objException)
            {
                throw new Exception("ContentSystem_BO::AddContentItem Error: " + objException.Message);
            }

            finally
            {

            }

        }

        public void UpdateContentItem(dto.DB.Content_DTO objContentItem)
        {

            dal.Content_DAL objContentSystem_BO = null;

            try
            {

                objContentSystem_BO = new dal.Content_DAL(this._DatabaseConnectionString);

                objContentSystem_BO.Update(_objAutoMapper.Map<dto.DB.Content_DTO, dmodels.Content>(objContentItem));

            }

            catch (System.Exception objException)
            {
                throw new Exception("ContentSystem_BO::UpdateContentItem Error: " + objException.Message);
            }

            finally
            {

            }

        }
    }
}
