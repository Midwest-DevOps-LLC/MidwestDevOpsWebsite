using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.RESTServiceRequestor.Standard
{
    public class RegistrationRequestor : BaseRequest
    {
        public RegistrationRequestor(string apiURL, string userToken, string applicationGUID)
        {
            this._apiURL = apiURL;
            this.RestClient = new RestClient(apiURL);
            base.SetHeaders(userToken, applicationGUID);
        }

        public MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.RegistrationResponse> Login(MDO.RESTDataEntities.Standard.RegisterRequest request)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.RegistrationResponse>();

            try
            {
                var code = this.RestClient.SetPath("api/Registration").PostRequest(JsonConvert.SerializeObject(request));

                response = JsonConvert.DeserializeObject<MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.RegistrationResponse>>(code);
            }
            catch (Exception ex)
            {
                return ex.Handle(response);
            }

            return response;
        }
    }
}
