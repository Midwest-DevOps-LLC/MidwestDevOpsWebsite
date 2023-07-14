using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SiteController : BaseController
    {
        //public SiteController(IConfiguration configuration) : base(configuration)
        //{
        //    _configuration = configuration;
        //}

        /// <summary>
        /// Get all sites that you have access to. If you the the permission, Site.ViewAll, then you retreive all sites. If you are just a plain user, you will recieve only the ones you have access to
        /// </summary>
        [HttpGet("GetAll")]
        public IActionResult GetAllSites(bool? active)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.Site>>();

            var sessionResponse = OnlySessions(response);

            if (sessionResponse.IsSessionValid.GetValueOrDefault() == false)
                return Json(response);

            var session = GetSession;

            using (var siteBAL = new RESTBLL.Sites(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                if (CheckIfSessionHasPermission(MDO.RESTDataEntities.Standard.Permission.PermissionLookup.Site.ViewAll))
                    response.Data = siteBAL.GetAllSites(active);
                else
                    response.Data = siteBAL.GetAllSitesByUserID(session.UserID, active);
            }

            response.Status = MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.Site>>.StatusEnum.Complete;

            return Json(response);
        }

        /// <summary>
        /// Gets a site by site id. Returns data = null if there is none or there is no access
        /// </summary>
        [HttpGet]
        public IActionResult Get(int siteID)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.Site>();

            var sessionResponse = OnlySessions(response);

            if (sessionResponse.IsSessionValid.GetValueOrDefault() == false)
                return Json(response);

            using (var siteBAL = new RESTBLL.Sites(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                if (CheckIfSessionHasPermission(MDO.RESTDataEntities.Standard.Permission.PermissionLookup.Site.ViewAll) == false)
                {
                    var userSites = siteBAL.GetAllSitesByUserID(GetSession.UserID, true);

                    if (userSites.Select(x => x.SiteID).Contains(siteID) == false) //User isn't added to the site they are trying to access
                    {
                        sessionResponse.AddError("You don't have access to this site");
                    }
                }

                response.Data = siteBAL.GetSiteByID(siteID);
            }

            response.Status = MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.Site>.StatusEnum.Complete;

            return Json(response);
        }

        /// <summary>
        ///Creates or updates a site based off if 'SiteID' has value. Perm: Site.Create
        /// </summary>
        [HttpPut]
        public IActionResult Put(MDO.RESTDataEntities.Standard.Site site)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.Site>();

            var sessionResponse = OnlyPermission(response, MDO.RESTDataEntities.Standard.Permission.PermissionLookup.Site.Create);

            if (sessionResponse.IsSessionValid.GetValueOrDefault() == false)
                return Json(response);

            using (var siteBLL = new RESTBLL.Sites(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                var siteID = siteBLL.SaveSite(site);

                if (siteID == null)
                {
                    response.Status = MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.Site>.StatusEnum.Error;
                    response.Error = "Couldn't save site";
                    return Json(response);
                }

                site.SiteID = (int)siteID.GetValueOrDefault();
                response.Data = site;
                response.Status = MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.Site>.StatusEnum.Complete;
            }

            return Json(response);
        }

        /// <summary>
        /// Tries to add a user to a site. Returns a long value representing the siteuserid if was successful. Perm: Site.User.Add
        /// </summary>
        [HttpPost("AddUser")]
        public IActionResult AddUser(MDO.RESTDataEntities.Standard.AddRemoveUserRequest request)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<long?>();

            var sessionResponse = OnlyPermission(response, (int)MDO.RESTDataEntities.Standard.Permission.PermissionLookup.Site.User.Add);

            if (sessionResponse.IsSessionValid.GetValueOrDefault() == false)
                return Json(response);

            using (var siteBLL = new RESTBLL.Sites(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                var s = siteBLL.AddUser(request.SiteID, request.UserID);
                response.Data = s;
                response.Status = MDO.RESTDataEntities.Standard.APIResponse<long?>.StatusEnum.Complete;
            }

            if (response.Data.HasValue == false)
            {
                response.Status = MDO.RESTDataEntities.Standard.APIResponse<long?>.StatusEnum.Error;
                response.Error = "Couldn't add user to site: " + request.SiteID;
            }

            return Json(response);
        }

        /// <summary>
        /// Tries to remove a user from a site. Returns false if the user wasn't added to the site. Perm: Site.User.Remove
        /// </summary>
        [HttpDelete("RemoveUser")]
        public IActionResult RemoveUser(MDO.RESTDataEntities.Standard.AddRemoveUserRequest request)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<bool>();

            var sessionResponse = OnlyPermission(response, (int)MDO.RESTDataEntities.Standard.Permission.PermissionLookup.Site.User.Remove);

            if (sessionResponse.IsSessionValid.GetValueOrDefault() == false)
                return Json(response);

            using (var siteBLL = new RESTBLL.Sites(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                var s = siteBLL.RemoveUser(request.SiteID, request.UserID);
                response.Data = s;
                response.Status = MDO.RESTDataEntities.Standard.APIResponse<bool>.StatusEnum.Complete;
            }

            if (response.Data == false)
            {
                response.Status = MDO.RESTDataEntities.Standard.APIResponse<bool>.StatusEnum.Error;
                response.Error = "Couldn't remove user from site: " + request.SiteID + ". It's possible if the user never had access to the site";
            }

            return Json(response);
        }
    }
}
