using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERAWeb.Core.Interface;
using ERAWeb.Logger;
using ERAWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static ERAWeb.App.Utilities.EnumHelpers;

namespace ERAWeb.App.Controllers
{
    public class ProfileController : BaseController
    {
        #region fields
        IUserProfileBroker service = null;
        #endregion

        #region constructor
        public ProfileController(IConfiguration _config, IUserProfileBroker _service, ILoggerManager loggerManager)
        {
            logger = loggerManager;
            service = _service;
            config = _config;
        }
        #endregion

        #region Action Methods
        public IActionResult Index()
        {
            if (IsLoggedIn())
            { return View(UserInfo); }
            else
            { return RedirectToAction("Login", "Account"); }
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserModel user)
        {
            if (user != null)
            {
                var response = await service.UpdateUserProfile(user);
                if (response != null && response.Content != null)
                {
                    await UpdateUserIdentityCache(response.Content);
                    SetNotification(response.Message, NotificationType.Success, "Successfull");
                }
                else
                {
                    SetNotification(response.Message, NotificationType.Failure, "Failed");
                    return RedirectToAction("Error", "Error");
                }
            }

            return View(user);
        }
        #endregion

        #region Private methods
        private async Task UpdateUserIdentityCache(UserModel userModel)
        {
            var user = await GetUserSessionAsync();
            if (user != null)
            {
                user.CompanyName = userModel.CompanyName;
                user.Email = userModel.Email;
                user.LastName = userModel.LastName;
                user.FirstName = userModel.FirstName;
                user.Password = userModel.Password;
                user.EmployeeNumber = userModel.EmployeeNumber;
            }
            await SetUserSessionAsync(user);
        }
        #endregion

    }
}