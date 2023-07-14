using MDO.ThirdParty.Spotify.Standard.Spotify;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace MDO.ThirdParty.Spotify.Standard.EndPoints
{
    public static class GetMeTokenAPI
    {
        public static Models.ApiResponse<MDO.RESTDataEntities.Standard.ThirdParty.Spotify.GetMeTokenResponse> GetMeToken(int userID)
        {
            var methodResponse = new Models.ApiResponse<MDO.RESTDataEntities.Standard.ThirdParty.Spotify.GetMeTokenResponse>();

            try
            {
                var clientToken = SpotifyHandler.Clients.GetClientToken(userID);

                if (clientToken == null)
                {
                    methodResponse.ErrorMessage = "Couldn't find client";
                    return methodResponse;
                }

                methodResponse.Data = new RESTDataEntities.Standard.ThirdParty.Spotify.GetMeTokenResponse()
                {
                    ClientToken = clientToken
                };

                return methodResponse;
            }
            catch (WebException wex)
            {
                try
                {
                    var resp = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();

                    MDO.Utility.Standard.LogHandler.SaveException(wex);
                    MDO.Utility.Standard.LogHandler.SaveLog(new MDO.Utility.Standard.LogHandler.Log()
                    {
                        text = resp,
                        time = DateTime.UtcNow
                    });

                    var obj = JsonConvert.DeserializeObject<Models.ErrorResponse>(resp);

                    methodResponse.ErrorMessage = obj.error.message;
                }
                catch (WebException ex)
                {
                    methodResponse.Exception = ex;
                }
            }
            catch (Exception ex)
            {
                MDO.Utility.Standard.LogHandler.SaveException(ex);

                methodResponse.Exception = ex;
            }

            return methodResponse;
        }
    }
}
