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
    public static class RecentlyPlayedAPI
    {
        public static Models.ApiResponse<MDO.RESTDataEntities.Standard.ThirdParty.Spotify.RecentlyPlayedResponse> RecentlyPlayed(int userID)
        {
            var methodResponse = new Models.ApiResponse<MDO.RESTDataEntities.Standard.ThirdParty.Spotify.RecentlyPlayedResponse>();

            try
            {
                var clientToken = SpotifyHandler.Clients.GetClientToken(userID);

                if (clientToken == null)
                {
                    methodResponse.ErrorMessage = "Couldn't find client";
                    return methodResponse;
                }

                RESTServiceRequestor.Standard.RestClient restClient = new RESTServiceRequestor.Standard.RestClient("https://api.spotify.com/v1/me/player/recently-played");
                //restClient.OverrideContentType = "application/x-www-form-urlencoded";

                restClient.Headers.Add("Authorization", $"Bearer {clientToken}");

                NameValueCollection outgoingQueryString = HttpUtility.ParseQueryString(String.Empty);

                var response = restClient.GetRequest();

                var deserialized = JsonConvert.DeserializeObject<MDO.RESTDataEntities.Standard.ThirdParty.Spotify.RecentlyPlayedResponse>(response);

                methodResponse.Data = deserialized;

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
