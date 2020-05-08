using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using ERAWeb.Logger;
using ERAWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using static ERAWeb.App.Utilities.EnumHelpers;

namespace ERAWeb.App.Controllers
{
    public class BaseController : Controller
    {
        #region fields

        public ILoggerManager logger;
        public IConfiguration config;
        public UserModel userModel;
        #endregion

        #region constructor
        public BaseController()
        {

        }
        #endregion

        #region properties
        public UserModel UserInfo
        {
            get
            {
                if (userModel == null)
                {
                    userModel = GetUserSession();
                }
                return userModel;
            }
        }
        #endregion

        #region utility methods

        public bool IsLoggedIn()
        {
            if (UserInfo != null)
                return true;
            else
                return false;
        }

        public bool CheckIfAdmin()
        {
            if (UserInfo.UserTypeId == Convert.ToInt32(UserType.Administrator))
                return true;
            else
                return false;
        }

        public bool CheckIfTestTaken()
        {
            return UserInfo.IsTestTaken;
        }

        public void SetUserSession(UserModel user)
        {
            const string sessionKey = "UserSession";
            DestroyAllSession();
            TempData.Add(sessionKey, JsonConvert.SerializeObject(user));
            TempData.Keep();
            //var serialisedData = JsonConvert.SerializeObject(user);
            //HttpContext.Session.SetString(sessionKey, serialisedData);
        }

        public async Task SetUserSessionAsync(UserModel user)
        {
            userModel = user;
            const string sessionKey = "UserSession";
            await DestroyAllSessionAsync();
            TempData.Add(sessionKey, JsonConvert.SerializeObject(user));
            TempData.Keep();
            //var serialisedData = JsonConvert.SerializeObject(user);
            //HttpContext.Session.SetString(sessionKey, serialisedData);
        }

        public async Task<UserModel> GetUserSessionAsync()
        {
            const string sessionKey = "UserSession";
            var value = (string)TempData.Peek(sessionKey);
            //var value = HttpContext.Session.GetString(sessionKey);
            if (value != null)
                return JsonConvert.DeserializeObject<UserModel>(value);
            return null;
        }

        public UserModel GetUserSession()
        {
            const string sessionKey = "UserSession";
            var value = (string)TempData.Peek(sessionKey);
            //var value = HttpContext.Session.GetString(sessionKey);
            if (value != null)
                return JsonConvert.DeserializeObject<UserModel>(value);
            return null;
        }

        public void DestroyAllSession()
        {
            if (TempData.Keys.Any())
                TempData.Clear();
            //HttpContext.Session.Remove(sessionKey);
        }

        public async Task DestroyAllSessionAsync()
        {
            if (TempData.Keys.Any())
                TempData.Clear();
            //HttpContext.Session.Remove(sessionKey);
        }

        public async Task<bool> IsEligibleToTakeTest(IConfiguration config)
        {
            var result = true;
            if (UserInfo.LastAssessmentDate != null && UserInfo.LastAssessmentDate.HasValue)
            {
                int testEligibilityDays = Convert.ToInt32(config.GetValue<string>("AppSettings:TestEligibleDays"));
                var actualDate = UserInfo.LastAssessmentDate.Value.AddDays(testEligibilityDays);
                if (actualDate > DateTime.Now)
                {
                    result = false;
                }
            }
            return result;
        }
        public void SetNotification(string message, NotificationType type = NotificationType.Success, string title = "Success", string redirectURL = "")
        {
            var notificationObject = JsonConvert.SerializeObject(new { Message = message, Type = type, Title = title, Redirection = redirectURL });
            if (TempData["Notification"] != null)
                TempData.Remove("Notification");
            TempData.Add("Notification", notificationObject);
        }
        #endregion

    }
}