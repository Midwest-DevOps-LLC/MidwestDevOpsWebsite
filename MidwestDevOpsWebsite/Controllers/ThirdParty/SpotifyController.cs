using MDO.ThirdParty.Spotify.Standard.Spotify;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MidwestDevOpsWebsite.Controllers.ThirdParty
{
    [Route("ThirdParty/[controller]/[action]")]
    public class SpotifyController : BaseController
    {
        public SpotifyController(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Login()
        {
            var scopes = "user-read-private user-read-email user-read-recently-played user-read-playback-state user-top-read user-modify-playback-state user-read-currently-playing user-read-playback-position";

            var link = $"https://accounts.spotify.com/authorize?response_type=code&client_id={SpotifyHandler.CLIENT_ID}&show_dialog=true&scope={WebUtility.UrlEncode(scopes)}&redirect_uri={WebUtility.UrlEncode("https://midwestdevops.com/ThirdParty/Spotify/Callback")}";

            return Redirect(link);
        }

        public IActionResult Callback()
        {
            //var model = new Models.SpotifyIndexModel();

            try
            {
                if (UserSession == null)
                    return RedirectToAction("Index", "Home");

                var query = HttpContext.Request.Query;

                var code = "";
                var error = "";

                if (query.ContainsKey("code"))
                {
                    code = query["code"];

                    var response = SpotifyHandler.GetToken(code);

                    if (response == null)
                        return RedirectToAction("Index", "Home");

                    using (var thirdPartyUserBLL = new BusinessLogicLayer.ThirdPartyUser(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
                    {
                        var thirdPartyUser = new DataEntities.ThirdPartyUser();
                        thirdPartyUser.ApiKey = response.refresh_token;
                        thirdPartyUser.ThirdPartyID = 1;
                        thirdPartyUser.UserID = UserSession.UserID;

                        thirdPartyUserBLL.SaveThirdPartyUser(thirdPartyUser);
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    error = query["error"];

                    return RedirectToAction("Index", "Home");
                }
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

            return RedirectToAction("Index", "Home");
        }
    }
}
