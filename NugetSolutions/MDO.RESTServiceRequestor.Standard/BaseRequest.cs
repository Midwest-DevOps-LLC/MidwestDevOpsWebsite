using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.RESTServiceRequestor.Standard
{
    public class BaseRequest
    {
        internal string _apiURL = null;
        internal MDO.RESTDataEntities.Standard.UserSession _userSession;
        internal RestClient RestClient = new RestClient();

        internal string Authorization { get; set; }
        internal string ApplicationGUID { get; set; }

        internal void SetHeaders(string auth, string applicationGUID)
        {
            if (string.IsNullOrEmpty(auth) == false)
            {
                Authorization = auth;
                RestClient.Headers.Add("Authorization", auth);
            }

            if (string.IsNullOrEmpty(applicationGUID) == false)
            {
                ApplicationGUID = applicationGUID;
                RestClient.Headers.Add("X-Application-GUID", applicationGUID);
            }
        }
    }
}
