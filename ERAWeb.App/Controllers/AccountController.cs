using ERAWeb.App.Utilities;
using ERAWeb.Core.Interface;
using ERAWeb.Logger;
using ERAWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using static ERAWeb.App.Utilities.EnumHelpers;

namespace ERAWeb.App.Controllers
{
    public class AccountController : BaseController
    {
        #region fields
        IUserBroker userService = null;
        #endregion

        #region constructor
        public AccountController(IConfiguration _config, IUserBroker userBroker, ILoggerManager loggerManager)
        {
            logger = loggerManager;
            userService = userBroker;
            config = _config;
        }
        #endregion



        #region action methods
        public IActionResult Login()
        {
            //logger.LogInfo("Hello, world!");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserModel user)
        {

            var validUser = await userService.ValidateUserCredentials(user);
            if (validUser.Content != null)
            {
                //if (validUser.Content.UserActive == 1)
                //{
                //    SetUserSession(validUser.Content);
                //    return RedirectToAction("Landing", "Home");
                //}
                //else
                //{
                //    var userModel = validUser.Content;
                //    var activationURL = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/{"Registration"}/{"Activate"}?{"userID="}{userModel.UserID}";
                //    var notificationSent = await NotificationHelper.SendRegisterNotification(userModel.UserEmail, userModel.UserName, activationURL, config);
                //    if (!notificationSent)
                //    {
                //        logger.LogError("Sending notification failed for userID:" + user.UserID);
                //    }
                //    return RedirectToAction("Activate", "Account");
                //}

                SetUserSession(validUser.Content);
                if (validUser.Content.UserTypeId == System.Convert.ToInt32(UserType.Administrator))
                    return RedirectToAction("Search", "Report");
                else
                    return RedirectToAction("Index", "Questionnaire");
            }
            else
            {
                SetNotification("Invalid UserID or Password..!!", NotificationType.Failure, "Failed");
                return View();
            }
        }

        public IActionResult Logout()
        {
            var user = GetUserSession();
            if (user != null)
            {
                DestroyUserSession();
            }
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ResetPassword user)
        {
            var message = await userService.UpdatePassword(user);
            if (message == "Updated")
            {
                var redirectURL = Url.Action("Login", "Account");
                SetNotification("Password Updated Successfully!!", NotificationType.Success, "Success", redirectURL);
                return View();
            }
            else
            {
                SetNotification(message, NotificationType.Failure, "Failed");
                return View();
            }
        }

        #endregion

    }
}