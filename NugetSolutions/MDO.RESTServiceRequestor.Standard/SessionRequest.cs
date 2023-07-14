using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MDO.RESTServiceRequestor.Standard
{
    public class SessionRequest : BaseRequest
    {
        public SessionRequest(string apiURL, string userToken, string applicationGUID)
        {
            this._apiURL = apiURL;
            this.RestClient = new RestClient(apiURL);
            base.SetHeaders(userToken, applicationGUID);
        }

        public MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.UserSession> Get()
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.UserSession>();

            try
            {
                var code = this.RestClient.SetPath("api/Session").GetRequest();

                response = JsonConvert.DeserializeObject<MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.UserSession>>(code);
            }
            catch (Exception ex)
            {
                return ex.Handle(response);
            }

            return response;
        }

        public MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.UserSession> Verify()
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.UserSession>();

            try
            {
                var code = this.RestClient.SetPath("api/Session/Verify").GetRequest();

                response = JsonConvert.DeserializeObject<MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.UserSession>>(code);
            }
            catch (Exception ex)
            {
                return ex.Handle(response);
            }

            return response;
        }
    }
}
