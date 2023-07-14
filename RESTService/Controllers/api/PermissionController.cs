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
    public class PermissionController : BaseController
    {
        public PermissionController(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Gets a list of all of the permissions
        /// </summary>
        [HttpGet("GetAllPermissions")]
        public IActionResult GetAllPermissions()
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.Permission>>();

            using (var siteBAL = new RESTBLL.Permissions(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                var permissions = siteBAL.GetAllPermissions();

                if (permissions == null)
                    return Json(response);

                var data = new List<MDO.RESTDataEntities.Standard.Permission>();
                data = permissions;

                response.Data = data;
            }

            return Json(response);
        }

        /// <summary>
        /// Gets a list of all of the permissions in a single dimension list
        /// </summary>
        [HttpGet("GetAllDBPermissions")]
        public IActionResult GetAllDBPermissions()
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.Permission>>();

            using (var siteBAL = new RESTBLL.Permissions(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                var permissions = siteBAL.GetAllDBPermissions();

                if (permissions == null)
                    return Json(response);

                var data = new List<MDO.RESTDataEntities.Standard.Permission>();
                data = permissions;

                response.Data = data;
            }

            return Json(response);
        }


        /// <summary>
        /// Gets a parent permissions with its children
        /// </summary>
        [HttpGet("GetAllPermissionsByParentPermissionID")]
        public IActionResult GetAllPermissionsByParentPermissionID(int parentID)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<List<MDO.RESTDataEntities.Standard.Permission>>();

            using (var siteBAL = new RESTBLL.Permissions(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                var permissions = siteBAL.GetAllPermissionsByParentPermissionID(parentID);

                if (permissions == null)
                    return Json(response);

                var data = new List<MDO.RESTDataEntities.Standard.Permission>();
                data = permissions;

                response.Data = data;
            }

            return Json(response);
        }
    }
}
