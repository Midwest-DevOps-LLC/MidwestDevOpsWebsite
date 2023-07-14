using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTService.Controllers
{
    public class ExternalApplicationController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
