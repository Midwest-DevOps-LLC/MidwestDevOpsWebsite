using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MidwestDevOpsWebsite.Controllers
{
    public class UserController : BaseController
    {
        public UserController(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public IActionResult ViewAll()
        {
            if (HasPermission(38) == false)
            {
                return RedirectToAction("Home", "Login");
            }

            using (var userBll = new BusinessLogicLayer.Users(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                var users = userBll.GetAllUsers();

                var model = new List<Models.UserModel>();

                foreach (var user in users)
                {
                    model.Add(new Models.UserModel(user));
                }

                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            //if (UserSession == null)
            //{
            //    return RedirectToAction("Home", "Login");
            //}

            if (UserSession.UserID == id || UserSession.UserID == 2)
            {
                using (var userBll = new BusinessLogicLayer.Users(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
                {
                    var user = userBll.GetUserByID(id);

                    if (user != null && user != new DataEntities.User())
                    {
                        var thirdPartyBLL = new BusinessLogicLayer.ThirdParty(userBll.GetConnection());

                        var model = new Models.UserModel(user);
                        model.ThirdParties = thirdPartyBLL.GetAllThirdParties();

                        var thirdPartyUserBLL = new BusinessLogicLayer.ThirdPartyUser(userBll.GetConnection());
                        model.ThirdPartyUser = thirdPartyUserBLL.GetAllThirdPartyUsersByUserID(UserSession.UserID);

                        var sessionPerms = GetUserPermissions;

                        return View(model);
                    }
                }

                return NotFound();
            }

            return Unauthorized();
        }

        [HttpPost]
        public IActionResult UserInfo(Models.UserModel model)
        {
            if (UserSession == null)
            {
                return Unauthorized();
            }

            //Models.ValidationModel validationModel = new Models.ValidationModel("Couldn't update", "Couldn't update account");

            MDO.RESTDataEntities.Standard.APIResponse<bool> response = new MDO.RESTDataEntities.Standard.APIResponse<bool>();
            response.ValidationModel.ErrorFullMessage = "Couldn't update account";
            response.ValidationModel.ErrorMessage = "Couldn't update";

            var canEditEmail = HasPermission(35) || model.UserID == UserSession.UserID;
            var canEditName = HasPermission(37) || model.UserID == UserSession.UserID;
            var canEditUserName = HasPermission(40) || model.UserID == UserSession.UserID;

            if (canEditEmail == false && canEditName == false && canEditUserName == false)
            {
                response.AddError("You don't have permission to change another user's username, email, or name");
                response.Status = MDO.RESTDataEntities.Standard.APIResponse<bool>.StatusEnum.Error;
                response.ValidationModel.ValidationStatus = MDO.RESTDataEntities.Standard.ValidationStatus.Error;

                return Json(response.ValidationModel);
            }

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Username))
                {
                    var error = new MDO.RESTDataEntities.Standard.ValidationMessage("username", "Username can't be blank", MDO.RESTDataEntities.Standard.ValidationStatus.Error);
                    response.ValidationModel.Add(error);
                }

                if (string.IsNullOrEmpty(model.Email))
                {
                    var error = new MDO.RESTDataEntities.Standard.ValidationMessage("email", "Email can't be blank", MDO.RESTDataEntities.Standard.ValidationStatus.Error);
                    response.ValidationModel.Add(error);
                }
                else if(MDO.EmailHandler.Standard.EmailHandler.IsValidEmail(model.Email) == false)
                {
                    var error = new MDO.RESTDataEntities.Standard.ValidationMessage("email", "Email is not valid", MDO.RESTDataEntities.Standard.ValidationStatus.Error);
                    response.ValidationModel.Add(error);
                }

                if (response.ValidationModel.ValidationStatus == MDO.RESTDataEntities.Standard.ValidationStatus.Success)
                {
                    using (var userBLL = new BusinessLogicLayer.Users(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
                    {
                        var userFromID = userBLL.GetUserByUUID(model.UUID);

                        if (userFromID != null && userFromID != new DataEntities.User())
                        {
                            var userFromEmail = userBLL.GetUserByEmail(model.Email.ToLower());

                            if (userFromEmail == null || userFromEmail == new DataEntities.User() || userFromEmail.UserID == userFromID.UserID)
                            {
                                if (userFromID.UserID.Value == model.UserID.Value)
                                {
                                    if (canEditEmail)
                                    {
                                        if (userFromID.Email.ToLower() != model.Email.ToLower())
                                        {
                                            //var loginRequestor = new MDO.RESTServiceRequestor.Standard.RegistrationRequestor(this._configuration.GetValue<string>("RESTService"), GetAuth, this._configuration.GetValue<string>("ApplicationGUID"));
                                            //loginRequestor.


                                            userFromID.Activated = false;

                                            System.Net.Mail.MailAddress toAddress = new System.Net.Mail.MailAddress(model.Email);
                                            var worked = SendRegistrationEmail(toAddress, userFromID.UserID.Value, "MDO");
                                        
                                            if (worked == false)
                                            {
                                                response.ValidationModel.ErrorFullMessage = "Couldn't send validation email.";
                                                response.ValidationModel.ValidationStatus = MDO.RESTDataEntities.Standard.ValidationStatus.Error;
                                                return Json(response.ValidationModel);
                                            }
                                        }
                                    }

                                    if (canEditUserName)
                                    {
                                        userFromID.Username = model.Username;
                                    }

                                    if (canEditName)
                                    {
                                        userFromID.FirstName = model.FirstName;
                                        userFromID.MiddleName = model.MiddleName;
                                        userFromID.LastName = model.LastName;
                                    }

                                    if (canEditEmail)
                                    {
                                        userFromID.Email = model.Email.ToLower();
                                    }

                                    userFromID.ModifiedBy = model.UserID.Value;
                                    userFromID.ModifiedDate = DateTime.UtcNow;

                                    var userID = userBLL.SaveUser(userFromID);

                                    if (userID != null)
                                    {
                                        response.ValidationModel.SuccessFullMessage = "Successfully updated account<br>If you changed your email you have to verify the account again";
                                        response.ValidationModel.SuccessMessage = "Success!";
                                    }
                                    else
                                    {
                                        response.ValidationModel.ErrorFullMessage = "Can't update user in database";
                                        response.ValidationModel.ValidationStatus = MDO.RESTDataEntities.Standard.ValidationStatus.Error;
                                    }
                                }
                                else
                                {
                                    var error = new MDO.RESTDataEntities.Standard.ValidationMessage("username", "Username already taken", MDO.RESTDataEntities.Standard.ValidationStatus.Error);
                                    response.ValidationModel.Add(error);
                                    response.ValidationModel.ValidationStatus = MDO.RESTDataEntities.Standard.ValidationStatus.Error;
                                }
                            }
                            else
                            {
                                var error = new MDO.RESTDataEntities.Standard.ValidationMessage("email", "Email already in use", MDO.RESTDataEntities.Standard.ValidationStatus.Error);
                                response.ValidationModel.Add(error);
                                response.ValidationModel.ValidationStatus = MDO.RESTDataEntities.Standard.ValidationStatus.Error;
                            }
                        }
                        else
                        {
                            response.ValidationModel.ErrorFullMessage = "Can't find user in database";
                            response.ValidationModel.ValidationStatus = MDO.RESTDataEntities.Standard.ValidationStatus.Error;
                        }
                    }
                }        
            }

            return Json(response.ValidationModel);
        }

        [HttpPost]
        public IActionResult AccountSecurity(Models.UpdatePasswordModel model)
        {
            Models.ValidationModel validationModel = new Models.ValidationModel("Couldn't update", "Couldn't update password");

            MDO.RESTDataEntities.Standard.APIResponse<bool> response = new MDO.RESTDataEntities.Standard.APIResponse<bool>();

            if (HasPermission(33) == false && model.UserID != UserSession.UserID)
            {
                response.AddError("You don't have permission to change another user's password");
                response.Status = MDO.RESTDataEntities.Standard.APIResponse<bool>.StatusEnum.Error;
                response.ValidationModel.ValidationStatus = MDO.RESTDataEntities.Standard.ValidationStatus.Error;

                return Json(response.ValidationModel);
            }

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Password))
                {
                    var error = new MDO.RESTDataEntities.Standard.ValidationMessage("password", "Password can't be blank", MDO.RESTDataEntities.Standard.ValidationStatus.Error);
                    response.ValidationModel.Add(error);
                }

                if (string.IsNullOrEmpty(model.RetypePassword))
                {
                    var error = new MDO.RESTDataEntities.Standard.ValidationMessage("retype-password", "Password can't be blank", MDO.RESTDataEntities.Standard.ValidationStatus.Error);
                    response.ValidationModel.Add(error);
                }

                if (validationModel.validationStatus == Models.ValidationStatus.Success)
                {
                    using (var userBLL = new BusinessLogicLayer.Users(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
                    {
                        var userFromID = userBLL.GetUserByUUID(model.UUID);

                        if (userFromID != null && userFromID != new DataEntities.User())
                        {
                            if (userFromID.UserID.Value == model.UserID)
                            {
                                userFromID.Password = MDO.Utility.Standard.TextHasher.Hash(model.Password, model.UUID);
                                userFromID.ModifiedBy = model.UserID;
                                userFromID.ModifiedDate = DateTime.UtcNow;

                                var userID = userBLL.SaveUser(userFromID);

                                if (userID != null)
                                {
                                    response.ValidationModel.SuccessFullMessage = "Successfully updated password";
                                    response.ValidationModel.SuccessMessage = "Success!";
                                }
                                else
                                {
                                    response.ValidationModel.ErrorFullMessage = "Can't update password in database";
                                    response.ValidationModel.ValidationStatus = MDO.RESTDataEntities.Standard.ValidationStatus.Error;
                                }
                            }
                            else
                            {
                                var error = new MDO.RESTDataEntities.Standard.ValidationMessage("username", "Stop what you are doing", MDO.RESTDataEntities.Standard.ValidationStatus.Error);
                                response.ValidationModel.Add(error);
                                response.ValidationModel.ValidationStatus = MDO.RESTDataEntities.Standard.ValidationStatus.Error;
                            }
                        }
                        else
                        {
                            response.ValidationModel.ErrorFullMessage = "Can't find user in database";
                            response.ValidationModel.ValidationStatus = MDO.RESTDataEntities.Standard.ValidationStatus.Error;
                        }
                    }
                }
            }

            return Json(validationModel);
        }

        public bool SendRegistrationEmail(System.Net.Mail.MailAddress toAddress, int userID, string application)
        {
            using (var emailBLL = new BusinessLogicLayer.EmailRegistrations(MDO.Utility.Standard.ConnectionHandler.GetConnectionString()))
            {
                var rowsAffected = emailBLL.SetAllEmailRegistractionsToInactive(userID);

                var UUID = System.Guid.NewGuid().ToString();

                var newEmailRegistration = new DataEntities.EmailRegistration() { EmailRegistrationID = null, UserID = userID, UUID = UUID, Active = true, Application = application };

                var emailID = emailBLL.SaveEmailRegistration(newEmailRegistration);

                if (emailID != null)
                {
                    var fromAddress = new System.Net.Mail.MailAddress("midwestdevops@gmail.com");

                    var emailText = DataEntities.PreWrittenEmails.RegistrationEmail(UUID, this._configuration.GetValue<string>("ApplicationGUID"));
                    var emailFrom = _configuration.GetValue<string>("EmailUsername");
                    var emailPassword = _configuration.GetValue<string>("EmailPassword");
                    MDO.EmailHandler.Standard.EmailHandler.SendEmail(emailFrom, MDO.EmailHandler.Standard.EmailHandler.EmailAddresses.alerts, toAddress.Address, emailPassword, "Verify Email - Midwest DevOps, LLC", emailText, true);

                    return true;
                }
            }

            return false;
        }
    }
}