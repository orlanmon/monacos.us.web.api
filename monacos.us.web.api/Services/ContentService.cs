namespace monacos.us.web.api.Services
{
    public class ContentService : IContent
    {

        readonly string _dBConnectionString;

        public ContentService(string dBConnectionString)
        {

            this._dBConnectionString = dBConnectionString;

        }


    }
}
