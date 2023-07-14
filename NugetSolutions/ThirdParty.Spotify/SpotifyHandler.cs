using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace MDO.ThirdParty.Spotify.Standard.Spotify
{
    public static class SpotifyHandler
    {
        public static string CLIENT_ID = "";
        public static string CLIENT_SECRET = "";

        public static ClientsClass Clients = new ClientsClass();

        #region ResponseClasses

        public class TokenResponse
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public long expires_in { get; set; }
            public string refresh_token { get; set; }
            public string scope { get; set; }
        }

        public class RefreshTokenResponse
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public long expires_in { get; set; }
            public string scope { get; set; }
        }

        #endregion

        public class ClientToken
        {
            public int UserID { get; set; }
            public string GUID { get; set; }
            public string ClientCode { get; set; }
            public string Token { get; set; }
            public string RefreshToken { get; set; }
            public DateTime ExpiresAt { get; set; }
        }

        public class ClientsClass //Remember a client could have multiple tokens
        {
            private List<ClientToken> ClientTokens { get; set; } = new List<ClientToken>();

            public int? AddClient(string refreshToken, int userID)
            {
                LoadFile();

                if (ClientTokens.Any(x => x.RefreshToken == refreshToken))
                {
                    return ClientTokens.Where(x => x.RefreshToken == refreshToken).FirstOrDefault().UserID;
                }

                var tokenResponse = RefreshToken(refreshToken);

                if (tokenResponse == null)
                    return null;

                string guid = Guid.NewGuid().ToString();

                var newClientToken = new ClientToken();
                newClientToken.ClientCode = "";
                newClientToken.RefreshToken = refreshToken;
                newClientToken.Token = tokenResponse.access_token;
                newClientToken.ExpiresAt = DateTime.UtcNow.AddSeconds(tokenResponse.expires_in);
                newClientToken.GUID = guid;
                newClientToken.UserID = userID;

                ClientTokens.Add(newClientToken);

                SaveToFile();

                return userID;
            }

            private void SaveToFile()
            {
                using (StreamWriter sw = new StreamWriter("SpotifyClients.sav"))
                {
                    var serial = JsonConvert.SerializeObject(ClientTokens, Formatting.Indented);
                    sw.Write(serial);
                }
            }

            private void LoadFile()
            {
                if (File.Exists("SpotifyClients.sav") == false)
                {
                    using (StreamWriter sw = new StreamWriter("SpotifyClients.sav"))
                    {
                        sw.Write("");
                    }
                }

                using (StreamReader sw = new StreamReader("SpotifyClients.sav"))
                {
                    var serial = JsonConvert.DeserializeObject<List<ClientToken>>(sw.ReadToEnd());
                    this.ClientTokens = serial;

                    if (this.ClientTokens == null)
                        this.ClientTokens = new List<ClientToken>();
                }
            }

            public ClientToken GetClient(int userID)
            {
                return ClientTokens.FirstOrDefault(x => x.UserID == userID);
            }

            public string GetClientToken(int userID)
            {
                var foundClient = GetClient(userID);

                if (foundClient == null)
                    return null;

                if (foundClient.ExpiresAt < DateTime.UtcNow) //Expired
                {
                    var refreshToken = RefreshToken(foundClient.RefreshToken);

                    foundClient.Token = refreshToken.access_token;
                    foundClient.ExpiresAt = DateTime.UtcNow.AddSeconds(refreshToken.expires_in);
                }

                return foundClient.Token;
            }

            private RefreshTokenResponse RefreshToken(string refreshToken)
            {
                try
                {
                    RESTServiceRequestor.Standard.RestClient restClient = new RESTServiceRequestor.Standard.RestClient("https://accounts.spotify.com/api/token");
                    restClient.OverrideContentType = "application/x-www-form-urlencoded";

                    restClient.Headers.Add("Authorization", $"Basic {Base64Encode($"{CLIENT_ID}:{CLIENT_SECRET}")}");

                    NameValueCollection outgoingQueryString = HttpUtility.ParseQueryString(String.Empty);

                    outgoingQueryString.Add("grant_type", "refresh_token");
                    outgoingQueryString.Add("refresh_token", refreshToken);
                    //outgoingQueryString.Add("client_id", CLIENT_ID);
                    //outgoingQueryString.Add("client_secret", CLIENT_SECRET);

                    var response = restClient.PostRequest(outgoingQueryString.ToString());

                    var deserialized = JsonConvert.DeserializeObject<RefreshTokenResponse>(response);

                    return deserialized;
                }
                catch (WebException wex)
                {
                    var resp = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();

                    MDO.Utility.Standard.LogHandler.SaveException(wex);
                    MDO.Utility.Standard.LogHandler.SaveLog(new MDO.Utility.Standard.LogHandler.Log()
                    {
                        text = resp,
                        time = DateTime.UtcNow
                    });

                    //throw;
                }
                catch (Exception ex)
                {
                    MDO.Utility.Standard.LogHandler.SaveException(ex);

                    //throw;
                }

                return null;
            }
        }


        public static Models.TokenResponse GetToken(string clientCode)
        {
            try
            {
                RESTServiceRequestor.Standard.RestClient restClient = new RESTServiceRequestor.Standard.RestClient("https://accounts.spotify.com/api/token");
                restClient.OverrideContentType = "application/x-www-form-urlencoded";

                NameValueCollection outgoingQueryString = HttpUtility.ParseQueryString(String.Empty);
                outgoingQueryString.Add("grant_type", "authorization_code");
                outgoingQueryString.Add("code", clientCode);
                outgoingQueryString.Add("redirect_uri", "https://midwestdevops.com/ThirdParty/Spotify/Callback");
                outgoingQueryString.Add("client_id", CLIENT_ID);
                outgoingQueryString.Add("client_secret", CLIENT_SECRET);

                var response = restClient.PostRequest(outgoingQueryString.ToString());

                var deserialized = JsonConvert.DeserializeObject<Models.TokenResponse>(response);

                return deserialized;
            }
            catch (WebException wex)
            {
                var resp = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();

                MDO.Utility.Standard.LogHandler.SaveException(wex);
                MDO.Utility.Standard.LogHandler.SaveLog(new MDO.Utility.Standard.LogHandler.Log()
                {
                    text = resp,
                    time = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                MDO.Utility.Standard.LogHandler.SaveException(ex);
            }

            return null;
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
