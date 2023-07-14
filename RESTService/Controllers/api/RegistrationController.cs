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
    public class RegistrationController : BaseController
    {
        public RegistrationController(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Regsiter a new user
        /// </summary>
        [HttpPost]
        public IActionResult Post(MDO.RESTDataEntities.Standard.RegisterRequest request)
        {
            var response = new MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.RegistrationResponse>("Couldn't register");
            response.Data = new MDO.RESTDataEntities.Standard.RegistrationResponse();

            if (string.IsNullOrEmpty(request.Username))
            {
                var error = new MDO.RESTDataEntities.Standard.ValidationMessage("regusername", "Username can't be blank", MDO.RESTDataEntities.Standard.ValidationStatus.Error);
                response.ValidationModel.Add(error);
            }

            if (string.IsNullOrEmpty(request.Email))
            {
                var error = new MDO.RESTDataEntities.Standard.ValidationMessage("email", "Email can't be blank", MDO.RESTDataEntities.Standard.ValidationStatus.Error);
                response.ValidationModel.Add(error);
            }
            else if (MDO.EmailHandler.Standard.EmailHandler.IsValidEmail(request.Email) == false)
            {
                var error = new MDO.RESTDataEntities.Standard.ValidationMessage("email", "Email isn't valid", MDO.RESTDataEntities.Standard.ValidationStatus.Error);
                response.ValidationModel.Add(error);
            }

            if (string.IsNullOrEmpty(request.Password))
            {
                var error = new MDO.RESTDataEntities.Standard.ValidationMessage("regpassword", "Password can't be blank", MDO.RESTDataEntities.Standard.ValidationStatus.Error);
                response.ValidationModel.Add(error);
            }
            if (string.IsNullOrEmpty(request.RetypePassword))
            {
                var error = new MDO.RESTDataEntities.Standard.ValidationMessage("retype-password", "Password can't be blank", MDO.RESTDataEntities.Standard.ValidationStatus.Error);
                response.ValidationModel.Add(error);
            }
            if (string.IsNullOrEmpty(request.Password) == false && string.IsNullOrEmpty(request.RetypePassword) == false && request.RetypePassword != request.Password)
            {
                var error = new MDO.RESTDataEntities.Standard.ValidationMessage("regpassword", "Passwords don't match", MDO.RESTDataEntities.Standard.ValidationStatus.Error);
                response.ValidationModel.Add(error);
            }

            if (string.IsNullOrEmpty(request.Application))
            {
                response.ValidationModel.ErrorFullMessage = "Application GUID is not set";
                response.ValidationModel.ValidationStatus = MDO.RESTDataEntities.Standard.ValidationStatus.Error;
            }

            if (response.ValidationModel.ValidationStatus == MDO.RESTDataEntities.Standard.ValidationStatus.Error)
            {
                response.Status = MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.RegistrationResponse>.StatusEnum.Error;
                response.AddError("Errors were found");
                return Json(response);
            }

            using (var userBll = new RESTBLL.Users(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                var userByUserName = userBll.GetUserByUsername(request.Username.ToLower());

                if (userByUserName != null && userByUserName != new MDO.RESTDataEntities.Standard.User())
                {
                    var error = new MDO.RESTDataEntities.Standard.ValidationMessage("regusername", "Username is already in use", MDO.RESTDataEntities.Standard.ValidationStatus.Error);
                    response.ValidationModel.Add(error);
                    response.ValidationModel.ValidationStatus = MDO.RESTDataEntities.Standard.ValidationStatus.Error;
                }
                else
                {
                    var userByEmail = userBll.GetUserByEmail(request.Email.ToLower());

                    if (userByEmail == null || userByEmail == new MDO.RESTDataEntities.Standard.User())
                    {
                        var newUser = new MDO.RESTDataEntities.Standard.User()
                        {
                            Active = true,
                            Username = request.Username.ToLower(),
                            Email = request.Email.ToLower(),
                            CreatedBy = 0,
                            CreatedDate = null,
                            ModifiedBy = null,
                            ModifiedDate = null,
                            UserID = null,
                            Password = "TEMP",
                            Activated = false
                        };

                        var userID = userBll.SaveUser(newUser);

                        if (userID != null)
                        {
                            var user = userBll.GetUserByID(Convert.ToInt32(userID.Value));

                            if (user != null && user != new MDO.RESTDataEntities.Standard.User())
                            {
                                var newPassword = MDO.Utility.Standard.TextHasher.Hash(request.Password, user.UUID);
                                user.Password = newPassword;

                                var savedAgain = userBll.SaveUser(user);

                                if (savedAgain != null)
                                {
                                    var toAddress = new System.Net.Mail.MailAddress(request.Email);
                                    if (SendRegistrationEmail(toAddress, user.UserID.Value, request.Application))
                                    {
                                        response.ValidationModel.SuccessFullMessage = "Successfully created " + request.Username + "<br>Please check your email to verify your account<br><b>Note:</b> it could be in spam";
                                        response.ValidationModel.SuccessMessage = "Success!";
                                    }
                                    else
                                    {
                                        response.ValidationModel.ErrorFullMessage = "Account created but couldn't send registration email. Please try again later";
                                        response.ValidationModel.ValidationStatus = MDO.RESTDataEntities.Standard.ValidationStatus.Error;
                                    }
                                }
                                else
                                {
                                    response.ValidationModel.ErrorFullMessage = "Couldn't save user to server. Please try again later";
                                    response.ValidationModel.ValidationStatus = MDO.RESTDataEntities.Standard.ValidationStatus.Error;
                                }
                            }
                            else
                            {
                                response.ValidationModel.ErrorFullMessage = "Couldn't save user. Please try again later";
                                response.ValidationModel.ValidationStatus = MDO.RESTDataEntities.Standard.ValidationStatus.Error;
                            }
                        }
                        else
                        {
                            response.ValidationModel.ErrorFullMessage = "Couldn't create user. Please try again later";
                            response.ValidationModel.ValidationStatus = MDO.RESTDataEntities.Standard.ValidationStatus.Error;
                        }
                    }
                    else
                    {
                        var error = new MDO.RESTDataEntities.Standard.ValidationMessage("email", "Email is already in use", MDO.RESTDataEntities.Standard.ValidationStatus.Error);
                        response.ValidationModel.Add(error);
                        response.ValidationModel.ValidationStatus = MDO.RESTDataEntities.Standard.ValidationStatus.Error;
                    }
                }
            }

            return Json(response);
        }

        /// <summary>
        /// When a user goes to this page it will resend their activation email. TODO Make an endpoint to send a new verification for a new email for an existing user
        /// </summary>
        [HttpGet("ResendVerificationEmail")]
        public IActionResult ResendVerificationEmail(string GUID, string Application)
        {
            if (!string.IsNullOrEmpty(GUID) && !string.IsNullOrEmpty(Application))
            {
                using (var registrationBLL = new RESTBLL.EmailRegistrations(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
                {
                    var reg = registrationBLL.GetEmailRegistrationByUUID(GUID);

                    if (reg != null)
                    {
                        var userBLL = new RESTBLL.Users(registrationBLL.GetConnection());
                        var user = userBLL.GetUserByID(reg.UserID);

                        var siteBLL = new RESTBLL.Sites(registrationBLL.GetConnection());
                        var site = siteBLL.GetSiteByGUID(Application);

                        int lastSlash = site.URL.LastIndexOf('/');
                        site.URL = (lastSlash > -1) ? site.URL.Substring(0, lastSlash) : site.URL;

                        var siteURI = site.URL + site.RegisterSuccessPath;

                        if (SendRegistrationEmail(new System.Net.Mail.MailAddress(user.Email), user.UserID.Value, Application))
                        {
                            return Redirect(siteURI + MDO.RESTDataEntities.Standard.EmailRegistration.RegistrationStatus.ReSentEmail.ToString());
                        }

                        return Redirect(siteURI + MDO.RESTDataEntities.Standard.EmailRegistration.RegistrationStatus.Error.ToString());
                    }
                }
            }

            return Redirect("https://www.midwestdevops.com");
        }

        /// <summary>
        /// When a user goes to this page it will set their user Activated status to true so that they can log in with their account
        /// </summary>
        [HttpGet("Verify")]
        public IActionResult Verify(string GUID, string Application)
        {
            if (!string.IsNullOrEmpty(GUID) && !string.IsNullOrEmpty(Application))
            {
                using (var registrationBLL = new RESTBLL.EmailRegistrations(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
                {
                    var reg = registrationBLL.GetEmailRegistrationByUUID(GUID);

                    if (reg != null)
                    {
                        reg.Active = false;

                        var s = registrationBLL.SaveEmailRegistration(reg);

                        var siteBLL = new RESTBLL.Sites(registrationBLL.GetConnection());
                        var site = siteBLL.GetSiteByGUID(Application);

                        int lastSlash = site.URL.LastIndexOf('/');
                        site.URL = (lastSlash > -1) ? site.URL.Substring(0, lastSlash) : site.URL;

                        var siteURI = site.URL + site.RegisterSuccessPath;

                        if (s != null)
                        {
                            using (var userBLL = new RESTBLL.Users(registrationBLL.GetConnection()))
                            {
                                var user = userBLL.GetUserByID(reg.UserID);
                                user.Activated = true;

                                userBLL.SaveUser(user);

                                return Redirect(siteURI + MDO.RESTDataEntities.Standard.EmailRegistration.RegistrationStatus.Success.ToString());
                            }
                        }
                        else
                        {
                            return Redirect(siteURI + MDO.RESTDataEntities.Standard.EmailRegistration.RegistrationStatus.Error.ToString());
                        }
                    }
                }
            }

            return Redirect("https://www.midwestdevops.com");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public bool SendRegistrationEmail(System.Net.Mail.MailAddress toAddress, int userID, string application)
        {
            try
            {
                using (var emailBLL = new RESTBLL.EmailRegistrations(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
                {

                    var sitesBLL = new RESTBLL.Sites(emailBLL.GetConnection());
                    var siteByApplication = sitesBLL.GetSiteByGUID(application);

                    if (siteByApplication == null)
                    {
                        return false;
                    }

                    var rowsAffected = emailBLL.SetAllEmailRegistractionsToInactive(userID);

                    var UUID = System.Guid.NewGuid().ToString();

                    var newEmailRegistration = new MDO.RESTDataEntities.Standard.EmailRegistration() { EmailRegistrationID = null, UserID = userID, UUID = UUID, Active = true, Application = application };

                    var emailID = emailBLL.SaveEmailRegistration(newEmailRegistration);

                    if (emailID != null)
                    {
                        var serviceURL = _configuration.GetValue<string>("RESTService");
                        var emailFrom = _configuration.GetValue<string>("EmailUsername");
                        var emailPassword = _configuration.GetValue<string>("EmailPassword");
                        var emailText = MDO.EmailHandler.Standard.PreWrittenEmails.RegistrationEmail(serviceURL, UUID, application, siteByApplication.Name, siteByApplication.URL);
                        return MDO.EmailHandler.Standard.EmailHandler.SendEmail(emailFrom, MDO.EmailHandler.Standard.EmailHandler.EmailAddresses.noreply, toAddress.Address, emailPassword, $"Verify Email - {siteByApplication.Name}", emailText, true);
                    }
                }
            }
            catch (Exception ex)
            {
                MDO.Utility.Standard.LogHandler.SaveLog(new MDO.Utility.Standard.LogHandler.Log() { exception = ex, text = "HAHAHA", time = DateTime.UtcNow });
                MDO.Utility.Standard.LogHandler.SaveException(ex);
            }

            return false;
        }
    }
}
