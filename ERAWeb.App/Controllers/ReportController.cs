using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERAWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace ERAWeb.App.Controllers
{
    public class ReportController : BaseController
    {
        public IActionResult Index(int id)
        {
            var ergoReport = GetErgonomicReportForUser(id);
            return View(ergoReport);
        }

        private ErgoReportModel GetErgonomicReportForUser(int userID)
        {
            return new ErgoReportModel()
            {
                ComputerPosition = new List<PositionEntries>(),
                StandingPosition = new List<PositionEntries>(),
                SittingPosition = new List<PositionEntries>(),
                HumanVariables = new List<Entries>()
            };
        }
    }
}