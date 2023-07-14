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
    public static class AddTrackToEndOfQueueAPI
    {
        public static Models.ApiResponse<bool> AddTrackToEndOfQueue(int userID, string songURI)
        {
            var methodResponse = new Models.ApiResponse<bool>();

            try
            {
                var clientToken = SpotifyHandler.Clients.GetClientToken(userID);

                if (clientToken == null)
                {
                    methodResponse.ErrorMessage = "Couldn't find client";
                    return methodResponse;
                }

                RESTServiceRequestor.Standard.RestClient restClient = new RESTServiceRequestor.Standard.RestClient("https://api.spotify.com/v1/me/player/queue?uri=" + WebUtility.UrlEncode(songURI));
                restClient.OverrideContentType = "application/json";

                restClient.Headers.Add("Authorization", $"Bearer {clientToken}");

                NameValueCollection outgoingQueryString = HttpUtility.ParseQueryString(String.Empty);
                //outgoingQueryString.Add("uri", songURI);

                var apiResponse = restClient.PostRequest(outgoingQueryString.ToString());

                methodResponse.Data = true;
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
