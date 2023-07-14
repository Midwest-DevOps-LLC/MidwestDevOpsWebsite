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
    public class ThirdPartyController : BaseController
    {
        public ThirdPartyController(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Gets all third party accounts for the user ID. Only valid sessions can access their own account unless they have Perm: ThirdParty.GetAllByUser
        /// </summary>
        [HttpGet("GetThirdParty")]
        public IActionResult GetThirdParty(int userID)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.GetThirdPartyResponse>();

            var sessionResponse = OnlySessions(response);

            if (sessionResponse.IsSessionValid.GetValueOrDefault() == false)
                return Json(response);

            var canViewOthers = CheckIfSessionHasPermission(41);

            if ((GetSession.UserID == userID || canViewOthers) == false)
            {
                response.AddError("You can't access other user's account");
                return Json(response);
            }

            using (var siteBAL = new RESTBLL.ThirdPartyUser(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                var userKeys = siteBAL.GetAllThirdPartyUsersByUserID(userID);

                if (userKeys == null)
                    return Json(response);

                var data = new MDO.RESTDataEntities.Standard.GetThirdPartyResponse();
                data.ThirdPartyUsers = userKeys;

                response.Data = data;
            }

            return Json(response);
        }
    }
}
