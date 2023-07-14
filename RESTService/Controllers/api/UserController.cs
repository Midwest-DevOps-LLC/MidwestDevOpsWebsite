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
    public class UserController : BaseController
    {
        public UserController(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Gets the name fields from a user
        /// </summary>
        [HttpGet("GetName")]
        public IActionResult GetName(int userID)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.GetNameResponse>();

            using (var siteBAL = new RESTBLL.Users(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                var user = siteBAL.GetUserByID(userID);

                if (user == null)
                    return Json(response);

                var data = new MDO.RESTDataEntities.Standard.GetNameResponse();
                data.Username = user.Username;
                data.FirstName = user.FirstName;
                data.MiddleName = user.MiddleName;
                data.LastName = user.LastName;

                response.Data = data;
            }

            return Json(response);
        }
    }
}
