using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MidwestDevOpsWebsite.Models;

namespace MidwestDevOpsWebsite.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IConfiguration configuration) : base (configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult ContactUs(ContactUsModel model)
        {
            Models.ValidationModel validationModel = new  Models.ValidationModel("Something went wrong!", "Couldn't send form");

            if (ModelState.IsValid)
            {
                var fromAddress = new MailAddress("midwestdevops@gmail.com", "Midwest DevOps, LLC");
                var toAddress = new MailAddress("midwestdevops@gmail.com", "Midwest DevOps, LLC");

                const string fromPassword = "Thisisapassword420!";
                const string subject = "Contact Form";

                string body = String.Format("<span style='color:blue'>Name:</span>  {0} <br><span style='color:blue'>Email:</span> {1} <br><span style='color:blue'>Comment:</span>  {2}", model.Name, model.Email, model.Comment);

                if (string.IsNullOrEmpty(model.Email))
                {
                    var error = new Models.ValidationMessage("email", "Email can't be blank", Models.ValidationStatus.Error);
                    validationModel.Add(error);
                }

                if (string.IsNullOrEmpty(model.Name))
                {
                    var error = new Models.ValidationMessage("name", "Name can't be blank", Models.ValidationStatus.Error);
                    validationModel.Add(error);
                }

                if (string.IsNullOrEmpty(model.Comment))
                {
                    var error = new Models.ValidationMessage("comments", "Comment can't be blank", Models.ValidationStatus.Error);
                    validationModel.Add(error);
                }

                if (validationModel.validationStatus == Models.ValidationStatus.Success)
                {
                    validationModel.ValidationAlertMessage = "Successfully sent form";
                    validationModel.ValidationModalMessage = "Success!";

                    var emailFrom = _configuration.GetValue<string>("EmailUsername");
                    var emailPassword = _configuration.GetValue<string>("EmailPassword");

                    MDO.EmailHandler.Standard.EmailHandler.SendEmail(emailFrom, MDO.EmailHandler.Standard.EmailHandler.EmailAddresses.alerts, "midwestdevops@gmail.com", emailPassword, subject, body, true);
                }
            }

            return Json(validationModel);
        }

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

            return View(new ErrorViewModel { StatusCode = code, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Path = feature?.OriginalPath });
        }
    }
}
