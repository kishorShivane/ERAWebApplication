using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ERAWeb.App.Models;
using Microsoft.AspNetCore.Mvc;

namespace ERAWeb.App.Controllers
{
    public class ErrorController : BaseController
    {
        #region action methods
        public IActionResult PageNotFound()
        {
            return View();
        }

        public IActionResult ServerError()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}