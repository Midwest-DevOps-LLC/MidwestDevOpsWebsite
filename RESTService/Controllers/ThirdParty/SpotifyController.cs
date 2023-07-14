using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTService.Controllers.ThirdParty
{
    [ApiController]
    [Route("api/ThirdParty/[controller]")]
    public class SpotifyController : BaseController
    {
        public SpotifyController(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Confirms that a MDO account has a Spotify account linked
        /// </summary>
        [HttpGet("Get")]
        public IActionResult Get(int userID)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<bool>();
            response.Data = true;

            var error = CheckIfHasAccountIfNotMakeOne(userID);

            if (string.IsNullOrEmpty(error) == false)
            {
                response.AddError(error);
                response.Data = false;
            }

            return Json(response);
        }

        private string CheckIfHasAccountIfNotMakeOne(int userID)
        {
            var check = MDO.ThirdParty.Spotify.Standard.Spotify.SpotifyHandler.Clients.GetClient(userID);

            if (check == null)
            {
                using (var siteBAL = new RESTBLL.ThirdPartyUser(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
                {
                    var userKeys = siteBAL.GetAllThirdPartyUsersByUserID(userID);

                    if (userKeys == null)
                        return "Account doesn't have a Spotify account";

                    if (userKeys.Where(x => x.ThirdPartyID == 1).Count() < 1)
                    {
                        return "Account doesn't have a Spotify account";
                    }

                    var spotifyKey = userKeys.Where(x => x.ThirdPartyID == 1).FirstOrDefault().ApiKey;

                    var guid = MDO.ThirdParty.Spotify.Standard.Spotify.SpotifyHandler.Clients.AddClient(spotifyKey, userID);

                    if (guid.HasValue == false)
                    {
                        return "Couldn't get token from Spotify";
                    }
                }
            }

            return "";
        }

        /// <summary>
        /// Get the spotify user account. Only valid sessions can access their own account
        /// </summary>
        [HttpGet("GetMe")]
        public IActionResult GetMe(int userID)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.ThirdParty.Spotify.GetMeResponse>();

            var sessionResponse = OnlySessions(response);

            if (sessionResponse.IsSessionValid.GetValueOrDefault() == false)
                return Json(response);

            if ((GetSession.UserID == userID || GetSession.UserID == 2) == false)
            {
                response.AddError("You can't access other user's account");
                return Json(response);
            }

            var error = CheckIfHasAccountIfNotMakeOne(userID);

            if (string.IsNullOrEmpty(error) == false)
            {
                response.AddError(error);
                return Json(response);
            }

            var resp = MDO.ThirdParty.Spotify.Standard.EndPoints.GetMeAPI.GetMe(userID);

            if (string.IsNullOrEmpty(resp.ErrorMessage) == false)
            {
                response.AddError(resp.ErrorMessage);
            }
            else
            {
                response.Data = resp.Data;
            }

            return Json(response);
        }

        /// <summary>
        /// Get the spotify user's token. Only valid sessions can access their own account
        /// </summary>
        [HttpGet("GetMeToken")]
        public IActionResult GetMeToken(int userID)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.ThirdParty.Spotify.GetMeTokenResponse>();

            var sessionResponse = OnlySessions(response);

            if (sessionResponse.IsSessionValid.GetValueOrDefault() == false)
                return Json(response);

            if ((GetSession.UserID == userID || GetSession.UserID == 2) == false)
            {
                response.AddError("You can't access other user's account");
                return Json(response);
            }

            var error = CheckIfHasAccountIfNotMakeOne(userID);

            if (string.IsNullOrEmpty(error) == false)
            {
                response.AddError(error);
                return Json(response);
            }

            var resp = MDO.ThirdParty.Spotify.Standard.EndPoints.GetMeTokenAPI.GetMeToken(userID);

            if (string.IsNullOrEmpty(resp.ErrorMessage) == false)
            {
                response.AddError(resp.ErrorMessage);
            }
            else
            {
                response.Data = resp.Data;
            }

            return Json(response);
        }

        /// <summary>
        /// Get the currently playing track. Only valid sessions can access their own account
        /// </summary>
        [HttpGet("CurrentlyPlaying")]
        public IActionResult CurrentlyPlaying(int userID)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.ThirdParty.Spotify.CurrentlyPlayingResponse>();

            var sessionResponse = OnlySessions(response);

            if (sessionResponse.IsSessionValid.GetValueOrDefault() == false)
                return Json(response);

            if ((GetSession.UserID == userID || GetSession.UserID == 2) == false)
            {
                response.AddError("You can't access other user's account");
                return Json(response);
            }

            var error = CheckIfHasAccountIfNotMakeOne(userID);

            if (string.IsNullOrEmpty(error) == false)
            {
                response.AddError(error);
                return Json(response);
            }

            var resp = MDO.ThirdParty.Spotify.Standard.EndPoints.CurrentlyPlayingTrackAPI.CurrentlyPlayingTrack(userID);

            if (string.IsNullOrEmpty(resp.ErrorMessage) == false)
            {
                response.AddError(resp.ErrorMessage);
            }
            else
            {
                response.Data = resp.Data;
            }

            return Json(response);
        }

        /// <summary>
        /// Get a list of recently played tracks. Only valid sessions can access their own account
        /// </summary>
        [HttpGet("RecentlyPlayed")]
        public IActionResult RecentlyPlayed(int userID)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.ThirdParty.Spotify.RecentlyPlayedResponse>();

            var sessionResponse = OnlySessions(response);

            if (sessionResponse.IsSessionValid.GetValueOrDefault() == false)
                return Json(response);

            if ((GetSession.UserID == userID || GetSession.UserID == 2) == false)
            {
                response.AddError("You can't access other user's account");
                return Json(response);
            }

            var error = CheckIfHasAccountIfNotMakeOne(userID);

            if (string.IsNullOrEmpty(error) == false)
            {
                response.AddError(error);
                return Json(response);
            }

            var resp = MDO.ThirdParty.Spotify.Standard.EndPoints.RecentlyPlayedAPI.RecentlyPlayed(userID);

            if (string.IsNullOrEmpty(resp.ErrorMessage) == false)
            {
                response.AddError(resp.ErrorMessage);
            }
            else
            {
                response.Data = resp.Data;
            }

            return Json(response);
        }

        /// <summary>
        /// Get a list of tracks or artists. Only valid sessions can access their own account
        /// </summary>
        [HttpGet("Search")]
        public IActionResult Search(int userID, string q, MDO.ThirdParty.Spotify.Standard.EndPoints.SearchAPI.QueryType queryType)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.ThirdParty.Spotify.SearchResponse>();

            var sessionResponse = OnlySessions(response);

            if (sessionResponse.IsSessionValid.GetValueOrDefault() == false)
                return Json(response);

            if ((GetSession.UserID == userID || GetSession.UserID == 2) == false)
            {
                response.AddError("You can't access other user's account");
                return Json(response);
            }

            var error = CheckIfHasAccountIfNotMakeOne(userID);

            if (string.IsNullOrEmpty(error) == false)
            {
                response.AddError(error);
                return Json(response);
            }

            var resp = MDO.ThirdParty.Spotify.Standard.EndPoints.SearchAPI.Search(userID, q, queryType);

            if (string.IsNullOrEmpty(resp.ErrorMessage) == false)
            {
                response.AddError(resp.ErrorMessage);
            }
            else
            {
                response.Data = resp.Data;
            }

            return Json(response);
        }

        /// <summary>
        /// Adds a track to the end of the queue. Only valid sessions can access their own account
        /// </summary>
        [HttpGet("AddTrackToEndOfQueue")]
        public IActionResult AddTrackToEndOfQueue(int userID, string trackURI)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<bool>();

            var sessionResponse = OnlySessions(response);

            if (sessionResponse.IsSessionValid.GetValueOrDefault() == false)
                return Json(response);

            if ((GetSession.UserID == userID || GetSession.UserID == 2) == false)
            {
                response.AddError("You can't access other user's account");
                return Json(response);
            }

            var error = CheckIfHasAccountIfNotMakeOne(userID);

            if (string.IsNullOrEmpty(error) == false)
            {
                response.AddError(error);
                return Json(response);
            }

            var resp = MDO.ThirdParty.Spotify.Standard.EndPoints.AddTrackToEndOfQueueAPI.AddTrackToEndOfQueue(userID, trackURI);

            if (string.IsNullOrEmpty(resp.ErrorMessage) == false)
            {
                response.AddError(resp.ErrorMessage);
            }
            else
            {
                response.Data = resp.Data;
            }

            return Json(response);
        }
    }
}
