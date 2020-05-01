using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERAWeb.App.Utilities;
using ERAWeb.Core.Interface;
using ERAWeb.Logger;
using ERAWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static ERAWeb.App.Utilities.EnumHelpers;

namespace ERAWeb.App.Controllers
{
    public class RegistrationController : BaseController
    {
        IRegistrationBroker registerService = null;
        public RegistrationController(IConfiguration _config, IRegistrationBroker registrationBroker, ILoggerManager loggerManager)
        {
            logger = loggerManager;
            registerService = registrationBroker;
            config = _config;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserModel user)
        {
            user.RegisteredDate = DateTime.Now;
            user.UserTypeId = Convert.ToInt32(EnumHelpers.UserType.Employee);
            var validUser = await registerService.RegisterUser(user);
            if (validUser.Content != null)
            {
                //var userModel = validUser.Content;
                //var activationURL = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/{"Registration"}/{"Activate"}?{"userID="}{userModel.UserID}";
                //var notificationSent = await NotificationHelper.SendRegisterNotification(userModel.UserEmail, userModel.UserName, activationURL, config);
                //if (!notificationSent)
                //{
                //    logger.LogError("Sending notification failed for userID:" + user.UserID);
                //}
                //ViewData["Notification"] = validUser.Message;
                return RedirectToAction("Successfull", "Registration");
            }
            else
            {
                SetNotification(validUser.Message, NotificationType.Failure, "Failed");
                return RedirectToAction("Error", "Error");
            }
        }

        public IActionResult Successfull()
        {
            return View();
        }

        public async Task<IActionResult> Activate(int userID)
        {
            var message = await registerService.ActivateUser(userID);
            if (message == "Updated")
            {
                return View();
            }
            else
            {
                var homePageURL = Url.Action("Index", "Home");
                SetNotification(message, NotificationType.Failure, "Failed", homePageURL);
                return View();
            }
        }
    }
}