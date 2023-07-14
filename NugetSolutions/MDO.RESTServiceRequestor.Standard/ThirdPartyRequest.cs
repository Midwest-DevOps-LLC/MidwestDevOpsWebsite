using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MDO.RESTServiceRequestor.Standard
{
    public class ThirdPartyRequest : BaseRequest
    {
        public ThirdPartyRequest(string apiURL, string userToken)
        {
            this._apiURL = apiURL;
            //this._userSession = this._userSession ?? new RESTDataEntities.UserSession() { Token = userToken };
            this.RestClient = new RestClient(apiURL);
            this.RestClient.Headers.Add("Authorization", userToken);
        }

        public MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.GetThirdPartyResponse> GetThirdParty(int userID)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.GetThirdPartyResponse>();

            try
            {
                var code = this.RestClient.SetPath("api/ThirdParty/GetThirdParty").GetRequest(new Dictionary<string, string>() { { "userID", userID.ToString() } });
                //var code = this.RestClient.SetPath("api/Employee").PostRequest(JsonConvert.SerializeObject(request));

                response = JsonConvert.DeserializeObject<MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.GetThirdPartyResponse>>(code);
            }
            catch (Exception ex)
            {
                return ex.Handle(response);
            }

            return response;
        }
    }
}
