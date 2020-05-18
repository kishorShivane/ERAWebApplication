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
        #region fields
        IRegistrationBroker registerService = null;
        #endregion

        #region constructor
        public RegistrationController(IConfiguration _config, IRegistrationBroker registrationBroker, ILoggerManager loggerManager)
        {
            logger = loggerManager;
            registerService = registrationBroker;
            config = _config;
        }
        #endregion

        #region action methods
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserModel user)
        {
            if (!IsLoggedIn())
            {
                user.RegisteredDate = DateTime.Now;
                user.UserTypeId = Convert.ToInt32(EnumHelpers.UserType.Employee);
                var validUser = await registerService.RegisterUser(user);
                if (validUser != null && validUser.Content != null)
                {
                    return RedirectToAction("Successfull", "Registration");
                }
                else
                {
                    SetNotification(validUser.Message, NotificationType.Failure, "Failed");
                    return RedirectToAction("Error", "Error");
                }
            }
            else
            {
                return RedirectToAction("Index", "Questionnaire");
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
        #endregion
    }
}