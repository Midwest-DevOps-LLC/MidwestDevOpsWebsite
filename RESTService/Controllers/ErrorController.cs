using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RESTService.Controllers
{
    public class ErrorController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult ErrorRedirect()
        {
            var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            var query = HttpContext.Request.Query;

            var code = "";

            if (query.ContainsKey("statusCode"))
            {
                code = query["statusCode"];
            }

            var response = new MDO.RESTDataEntities.Standard.EndpointErrorResponse();
            //response.Data = new MDO.RESTDataEntities.Standard.EndpointErrorResponse();

            //response.Status = MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.EndpointErrorResponse>.StatusEnum.Error;
            //response.ValidationModel.ValidationStatus = MDO.RESTDataEntities.Standard.ValidationStatus.Error;

            if (code == "404")
            {
                //response.AddError("Endpoint could not be found");
            }

            response.StatusCode = code;
            response.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            response.Path = feature?.OriginalPath;

            //response.AddError("Endpoint not found");

            //response.EndpointErrorResponse = response.Data;

            return Json(response); //Returning APIResponse is boomer way
        }
    }
}
