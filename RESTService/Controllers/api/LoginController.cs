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
    public class LoginController : BaseController
    {
        public LoginController(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Login to a MDO site. Returns the auth key for you to use on session only requests
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post(MDO.RESTDataEntities.Standard.LoginRequest loginRequest)
        {
            var auth = GetAuthorizationHeader;
            var app = GetX_Application_GUIDHeader;

            if (app != null) //TODO check later if this auth user can log into app
            {
                var response = new MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.LoginResponse>("Couldn't log in");
                response.Data = new MDO.RESTDataEntities.Standard.LoginResponse();

                if (string.IsNullOrEmpty(loginRequest.Username))
                {
                    var error = new MDO.RESTDataEntities.Standard.ValidationMessage("username", "Username can't be blank", MDO.RESTDataEntities.Standard.ValidationStatus.Error);
                    response.ValidationModel.Add(error);
                }

                if (string.IsNullOrEmpty(loginRequest.Password))
                {
                    var error = new MDO.RESTDataEntities.Standard.ValidationMessage("password", "Password can't be blank", MDO.RESTDataEntities.Standard.ValidationStatus.Error);
                    response.ValidationModel.Add(error);
                }

                if (response.ValidationModel.ValidationStatus == MDO.RESTDataEntities.Standard.ValidationStatus.Error)
                {
                    response.Status = MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.LoginResponse>.StatusEnum.Error;
                    response.AddError("Errors were found");
                    return Json(response);
                }

                using (var userBll = new RESTBLL.Users(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
                {
                    var userByUserName = userBll.GetUserByUsername(loginRequest.Username.ToLower());

                    if (userByUserName != null && userByUserName != new MDO.RESTDataEntities.Standard.User())
                    {
                        if (userBll.VerifyPassword(userByUserName, loginRequest.Password)) //Correct password
                        {
                            if (userByUserName.Activated == true)
                            {
                                var siteBLL = new RESTBLL.Sites(userBll.GetConnection());

                                if (siteBLL.UserHasAccessToSite(app, userByUserName.UserID.GetValueOrDefault()) == false)
                                {
                                    response.Status = MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.LoginResponse>.StatusEnum.Error;
                                    response.AddError("User doesn't have access to this site");
                                    return Json(response);
                                }

                                var sessionBLL = new RESTBLL.UserSessions(userBll.GetConnection());

                                var session = new MDO.RESTDataEntities.Standard.UserSession();
                                session.UserID = userByUserName.UserID.GetValueOrDefault();
                                session.CreatedDate = DateTime.UtcNow;
                                session.ModifiedDate = DateTime.UtcNow;
                                session.Token = Guid.NewGuid().ToString();

                                var updatedSession = sessionBLL.SaveOrUpdateSession(session);

                                response.Data.Auth = updatedSession.Token;
                                response.Data.User = userByUserName;
                                response.Status = MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.LoginResponse>.StatusEnum.Complete;
                            }
                            else
                            {
                                //response.Data.User = userByUserName;
                                response.Status = MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.LoginResponse>.StatusEnum.Error;


                                var registrationBLL = new RESTBLL.EmailRegistrations(userBll.GetConnection());
                                var reg = registrationBLL.GetEmailRegistrationByUserID(userByUserName.UserID.Value);

                                var serviceURL = _configuration.GetValue<string>("RESTService");
                                int lastSlash = serviceURL.LastIndexOf('/');
                                serviceURL = (lastSlash > -1) ? serviceURL.Substring(0, lastSlash) : serviceURL;
                                string link = serviceURL + "/api/Registration/ResendVerificationEmail?GUID=" + reg.UUID + "&Application=" + reg.Application;

                                response.AddError("Your email isn't validated. Click <a href=\"" + link + "\">here</a> to resend the email");

                                response.ValidationModel.ValidationStatus = MDO.RESTDataEntities.Standard.ValidationStatus.Error;
                            }
                        }
                        else //Invaild password
                        {
                            response.Status = MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.LoginResponse>.StatusEnum.Error;
                            response.AddError("Username or password is incorrect");

                            response.ValidationModel.ValidationStatus = MDO.RESTDataEntities.Standard.ValidationStatus.Error;
                        }
                    }
                    else //Couldn't find by username
                    {
                        response.Status = MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.LoginResponse>.StatusEnum.Error;
                        response.AddError("Username or password is incorrect");

                        MDO.Utility.Standard.LogHandler.SaveLog(new MDO.Utility.Standard.LogHandler.Log() { time = DateTime.UtcNow, text = "Couldn't find username by: " + loginRequest.Username.ToLower() });

                        response.ValidationModel.ValidationStatus = MDO.RESTDataEntities.Standard.ValidationStatus.Error;
                    }
                }

                return Json(response);
            }

            return StatusCode(403);
        }
    }
}
