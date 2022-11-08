using System;
using System.Configuration;
using monacos.us.web.services.model.Models.DB;
using System.Collections.Generic;
using System.Linq;


namespace monacos.us.web.services.dal
{
    public class Content_DAL
    {

        private string _DatabaseConnectionString = "";

        public Content_DAL(string DatabaseConnectionString)
        {

            this._DatabaseConnectionString = DatabaseConnectionString;

        }


        public Int32 Add(monacos.us.web.services.model.Models.DB.Content objContent)
        {
            
            try
            {

                //DatabaseConnectionString = ConfigurationManager.AppSettings.Get("DatabaseConnectionString");

                monacos.us.web.services.model.Models.DB.monacosusContext objDataContext = new monacos.us.web.services.model.Models.DB.monacosusContext(this._DatabaseConnectionString);

                objDataContext.Contents.Add(objContent);

                objDataContext.SaveChanges();

            }
            catch (Exception objException)
            {
                throw new Exception("Content_DAO::Add" + objException.Message);

            }


            return objContent.ContentId;


        }


        public void Delete(Int32 Content_ID)
        {

            monacos.us.web.services.model.Models.DB.Content objContent = null;

            try
            {


                monacos.us.web.services.model.Models.DB.monacosusContext objDataContext = new monacos.us.web.services.model.Models.DB.monacosusContext(this._DatabaseConnectionString);

                objContent = objDataContext.Contents.Single(contentitem => contentitem.ContentId == Content_ID);

                objDataContext.Contents.Remove(objContent);

                objDataContext.SaveChanges();

            }
            catch (Exception objException)
            {
                throw new Exception("Content_DAO::Delete" + objException.Message);

            }


        }

        public void Update(monacos.us.web.services.model.Models.DB.Content objContent)
        {

            
            monacos.us.web.services.model.Models.DB.Content objContentUpdate = null;

            try
            {

          
                monacos.us.web.services.model.Models.DB.monacosusContext objDataContext = new monacos.us.web.services.model.Models.DB.monacosusContext(this._DatabaseConnectionString);

                objContentUpdate = objDataContext.Contents.Single(contentitem => contentitem.ContentId == objContent.ContentId);

                objContentUpdate.ContentAreaId = objContent.ContentAreaId;
                objContentUpdate.ContentValue = objContent.ContentValue;
                objContentUpdate.PublishDate = objContent.PublishDate;
                objContentUpdate.ExpirationDate = objContent.ExpirationDate;
                objContentUpdate.Description = objContent.Description;
                objContentUpdate.Active = objContent.Active;

                objDataContext.SaveChanges();


            }
            catch (Exception objException)
            {
                throw new Exception("Content_DAO::Update" + objException.Message);

            }

        }

        public monacos.us.web.services.model.Models.DB.Content Select(Int32 Content_ID)
        {

            
            monacos.us.web.services.model.Models.DB.Content objContent = null;

            try
            {

                monacos.us.web.services.model.Models.DB.monacosusContext objDataContext = new monacos.us.web.services.model.Models.DB.monacosusContext(this._DatabaseConnectionString);

                objContent = objDataContext.Contents.Single(contentitem => contentitem.ContentId == Content_ID);
            }
            catch (Exception objException)
            {
                throw new Exception("Content_DAO::Select" + objException.Message);

            }

            return objContent;

        }


        public List<monacos.us.web.services.model.Models.DB.Content> Get(Int32 ContentArea_ID, bool IncludeOnlyCurrentlyPublished)
        {

            
            List<monacos.us.web.services.model.Models.DB.Content> objContents = null;

            try
            {

                monacos.us.web.services.model.Models.DB.monacosusContext objDataContext = new monacos.us.web.services.model.Models.DB.monacosusContext(this._DatabaseConnectionString);

                //contentitem.Expiration_Date >= System.DateTime.Now

                if (!IncludeOnlyCurrentlyPublished)
                {
                    // This Includes Everything Except InActive 

                    objContents = objDataContext.Contents.Where(contentitem => contentitem.Active == true && contentitem.ContentAreaId == ContentArea_ID).OrderByDescending(contentitem => contentitem.PublishDate).ToList();

                }
                else
                {

                    // Only Active Content That is Currently Published
                    // Two Scenarios:
                    // #1 Between and Including Publish Date and Expiration Date 
                    // #2 After and Including Publish Date but No Expiration Date so never expires.

                    /*
                    objContents = objDataContext.Contents.Where(contentitem => contentitem.Active == true 
                     && contentitem.ContentArea_ID == ContentArea_ID 
                     &&
                     (
                           (  
                            ( (((System.DateTime)contentitem.Expiration_Date).Subtract(System.DateTime.Now).Days >= 0))
                            && (System.DateTime.Now.Subtract((DateTime)contentitem.Publish_Date).Days >= 0) 
                           )
                           ||
                           (
                            ( contentitem.Expiration_Date == null )
                            && (System.DateTime.Now.Subtract((DateTime)contentitem.Publish_Date).Days >= 0) 
                           )
                    )

                     ).OrderByDescending(contentitem => contentitem.Publish_Date).ToList();
                    */

                    objContents = objDataContext.Contents.Where(contentitem => contentitem.Active == true
                     && contentitem.ContentAreaId == ContentArea_ID
                     &&
                     (
                           (
                            (contentitem.ExpirationDate != null)
                            &&
                            (System.DateTime.Now <= contentitem.ExpirationDate)
                            &&
                            (System.DateTime.Now >= contentitem.PublishDate)
                           )
                           ||
                           (
                            (contentitem.ExpirationDate == null)
                            && (System.DateTime.Now >= contentitem.PublishDate)
                           )
                    )

                     ).OrderByDescending(contentitem => contentitem.PublishDate).ToList();



                }

                // contentitem.Expiration_Date == null ||
            }
            catch (Exception objException)
            {
                throw new Exception("Content_DAO::Get" + objException.Message);

            }

            return objContents;

        }

    }
}
