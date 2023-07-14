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
    public class SessionController : BaseController
    {
        //public SessionController(IConfiguration configuration) : base(configuration)
        //{
        //    _configuration = configuration;
        //}

        /// <summary>
        /// Get's a session by auth without updating it's session timer.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.UserSession>();

            var sessionResponse = OnlySessions(response);

            if (sessionResponse.IsSessionValid.GetValueOrDefault() == false)
                return Json(response);

            var auth = GetSession;

            var app = GetX_Application_GUIDHeader;

            using (var userSessionBLL = new RESTBLL.UserSessions(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                var session = userSessionBLL.GetUserSessionByToken(auth.Token);

                if (session != null)
                {
                    response.Data = session;
                    response.Status = MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.UserSession>.StatusEnum.Complete;

                    return Json(response);
                }

                response.Status = MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.UserSession>.StatusEnum.Error;
                response.Error = "Couldn't find session by " + auth;

                MDO.Utility.Standard.LogHandler.SaveLog(new MDO.Utility.Standard.LogHandler.Log() { time = DateTime.UtcNow, text = response.Error });

                return StatusCode(500);
            }

            return StatusCode(401);
        }

        /// <summary>
        /// Gets a session by auth and also updates it's session timer
        /// </summary>
        /// <returns></returns>
        [HttpGet("Verify")]
        public IActionResult Verify()
        {
            var auth = GetAuthorizationHeader;

            var response = new MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.UserSession>();

            response = OnlySessions(response);

            if (response.IsSessionValid.GetValueOrDefault())
                response.Data = GetSession;

            response.Status = MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.UserSession>.StatusEnum.Complete;

            return Json(response);
        }
    }
}
