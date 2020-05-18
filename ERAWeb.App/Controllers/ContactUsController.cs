using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERAWeb.App.Utilities;
using ERAWeb.Logger;
using ERAWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite.Internal;
using Microsoft.Extensions.Configuration;
using static ERAWeb.App.Utilities.EnumHelpers;

namespace ERAWeb.App.Controllers
{
    public class ContactUsController : BaseController
    {
        #region constructor
        public ContactUsController(IConfiguration _config, ILoggerManager loggerManager)
        {
            logger = loggerManager;
            config = _config;
        }
        #endregion

        public IActionResult Index()
        {
            var contact = new ContactModel();
            contact.Name = UserInfo.FirstName + " " + UserInfo.LastName;
            contact.Email = UserInfo.Email;
            return View(contact);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ContactModel model)
        {
            var redirectURL = Url.Action("Index", "ContactUs");
            if (model != null)
            {
                await NotificationHelper.SendEmailNotification(UserInfo, model.Email, model.Name, model.Message, config);
                SetNotification("Thank you for contacting us!!", NotificationType.Information, "Thank You", redirectURL);
            }
            return View(model);
        }
    }
}