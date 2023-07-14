using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.RESTServiceRequestor.Standard.ThirdParty.Spotify
{
    public class GetMeTokenRequestor : BaseRequest
    {
        public GetMeTokenRequestor(string apiURL, string userToken)
        {
            this._apiURL = apiURL;
            //this._userSession = this._userSession ?? new RESTDataEntities.UserSession() { Token = userToken };
            this.RestClient = new RestClient(apiURL);
            this.RestClient.Headers.Add("Authorization", userToken);
        }

        public MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.ThirdParty.Spotify.GetMeTokenResponse> GetMeToken(int userID)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.ThirdParty.Spotify.GetMeTokenResponse>();

            try
            {
                var code = this.RestClient.SetPath("api/ThirdParty/Spotify/GetMeToken").GetRequest(new Dictionary<string, string>() { { "userID", userID.ToString() } });
                //var code = this.RestClient.SetPath("api/Employee").PostRequest(JsonConvert.SerializeObject(request));

                response = JsonConvert.DeserializeObject<MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.ThirdParty.Spotify.GetMeTokenResponse>>(code);
            }
            catch (Exception ex)
            {
                return ex.Handle(response);
            }

            return response;
        }
    }
}
