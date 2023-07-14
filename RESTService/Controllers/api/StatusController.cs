using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusController
    {
        /// <summary>
        /// Endpoint for checking if the service is online
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get()
        {
            return new StatusCodeResult(200);
        }
    }
}
