using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ERAWeb.Core.Interface;
using ERAWeb.Logger;
using ERAWeb.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using static ERAWeb.App.Utilities.EnumHelpers;

namespace ERAWeb.App.Controllers
{
    public class ReportController : BaseController
    {
        #region fields
        private static QuestionDictionary questionnaire;
        IHostingEnvironment host;
        IUserAnswerBroker userAnswerservice;
        IUserRiskBroker userRiskservice;
        IQuestionBroker questionService;
        IReportBroker reportService;
        #endregion

        #region constructor
        public ReportController(IConfiguration _config, ILoggerManager loggerManager, IHostingEnvironment _host,
            IUserAnswerBroker _userAnswerService, IUserRiskBroker _userRiskservice, IQuestionBroker _questionService, IReportBroker _reportService)
        {
            logger = loggerManager;
            config = _config;
            host = _host;
            userAnswerservice = _userAnswerService;
            userRiskservice = _userRiskservice;
            questionService = _questionService;
            reportService = _reportService;
        }
        #endregion

        #region action methods
        public async Task<IActionResult> Index(Guid id)
        {
            if (CheckIfAdmin())
            {
                ErgoReportModel ergoReport = await GetReportData(id);
                return View(ergoReport);
            }
            else
            {
                if (IsLoggedIn() && CheckIfTestTaken())
                {
                    if (UserInfo.LatestTestIdentifier.Value == id)
                    {
                        ErgoReportModel ergoReport = await GetReportData(id);
                        return View(ergoReport);
                    }
                    else
                    {
                        return RedirectToAction("PageNotFound", "Error");
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
        }

        private async Task<ErgoReportModel> GetReportData(Guid id)
        {
            var ergoReport = new ErgoReportModel();
            var value = (string)TempData.Peek(id.ToString());
            if (value != null)
            {
                ergoReport = JsonConvert.DeserializeObject<ErgoReportModel>(value);
            }
            else
            {
                ergoReport = await GetErgonomicReportDataForUser(id);
                TempData.Add(id.ToString(), JsonConvert.SerializeObject(ergoReport));
                TempData.Keep();
            }
            return ergoReport;
        }

        public async Task<IActionResult> ErgonomicReport(Guid id)
        {
            if (CheckIfAdmin() || CheckIfTestTaken())
            {
                ErgoReportModel ergoReport = await GetReportData(id);
                return new ViewAsPdf("ErgonomicReport", ergoReport)
                {
                    FileName = UserInfo.FirstName + "_" + UserInfo.LastName + "_ErgonomicReport.pdf",
                    CustomSwitches = "--viewport-size 1280x1024",
                    PageMargins = new Margins() { Left = 10, Right = 10, Top = 20, Bottom = 20 }
                };
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Search()
        {
            if (CheckIfAdmin())
            {
                ReportSearchModel model = new ReportSearchModel();
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Questionnaire");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Search(ReportSearchModel model)
        {
            var users = await reportService.GetUserReport(model);
            if (users != null && users.Any())
            {
                model.Users = users;
            }
            else
            {
                SetNotification("No user report found for criteria.!", NotificationType.Information, "Information");
            }
            return View(model);
        }

        #endregion

        #region private methods
        private async Task<ErgoReportModel> GetErgonomicReportDataForUser(Guid identifier)
        {
            ErgoReportModel reportData = null;
            await BootStrapQuestionnaireModel();
            var userAnswers = await userAnswerservice.GetUserAnswers(identifier);
            var userRisks = await userRiskservice.GetUserRisks(identifier);
            if (userRisks != null && userRisks.Any() && userAnswers != null && userAnswers.Any())
            {
                reportData = new ErgoReportModel();
                var bmiRiskID = questionnaire.BMI.FirstOrDefault().RiskID;
                var bmiRisk = userRisks.FirstOrDefault(x => x.RiskID == bmiRiskID);
                var bmiErgo = questionnaire.ErgonomicScores.FirstOrDefault(x => x.RiskID == bmiRiskID && x.Score == bmiRisk.RiskValue);
                var discomfortsRiskID = questionnaire.Discomforts.FirstOrDefault().RiskID;
                var discomfortRisk = userRisks.FirstOrDefault(x => x.RiskID == discomfortsRiskID);
                var sittingRiskID = questionnaire.SittingPosition.FirstOrDefault().RiskID;
                var standingRiskID = questionnaire.StandingPosition.FirstOrDefault().RiskID;
                var sittingCompRiskID = questionnaire.ComputerPositionWhenSitting.FirstOrDefault().RiskID;
                var standingCompRiskID = questionnaire.ComputerPositionWhenStanding.FirstOrDefault().RiskID;

                #region report data properties
                reportData.FirstName = UserInfo.FirstName;
                reportData.LastName = UserInfo.LastName;
                reportData.AssessmentDate = userRisks.FirstOrDefault().AssesmentDate;
                reportData.EmployeeNumber = UserInfo.EmployeeNumber;
                reportData.CompanyName = UserInfo.CompanyName;
                reportData.Email = UserInfo.Email;
                reportData.TestIdentifier = identifier;
                #endregion


                #region Human Variables Report Data
                questionnaire.HumanVariables.ForEach(x =>
                {
                    var userAnswer = userAnswers.FirstOrDefault(z => z.QuestionID == x.QuestionID);
                    reportData.HumanVariables.Add(new Entries()
                    {
                        Description = x.Description,
                        Response = userAnswer.Answer
                    });
                    reportData.HumanVariableSummary.Value = reportData.HumanVariableSummary.Value + userAnswer.Score;
                });

                reportData.HumanVariables.Add(new Entries()
                {
                    Description = questionnaire.BMI.FirstOrDefault().Category,
                    Response = bmiRisk.Score + " is between " + bmiErgo.FromValue + " and " + bmiErgo.ToValue
                });
                reportData.HumanVariableSummary.Value = reportData.HumanVariableSummary.Value + bmiRisk.RiskValue;

                reportData.HumanVariables.Add(new Entries()
                {
                    Description = questionnaire.Discomforts.FirstOrDefault().Category,
                    Response = discomfortRisk.Score.ToString()
                });
                reportData.HumanVariableSummary.Value = reportData.HumanVariableSummary.Value + discomfortRisk.RiskValue;

                reportData.HumanVariableSummary.Value = Convert.ToInt32(Math.Round(((decimal)reportData.HumanVariableSummary.Value / 23) * 100));
                reportData.HumanVariableSummary.Status = SetSummaryStatus(reportData.HumanVariableSummary.Value);
                #endregion

                #region Position Report Data
                if (userRisks.FirstOrDefault(x => x.RiskID == sittingRiskID) != null)
                {
                    //Sitting Position
                    reportData.SittingPosition.AddRange(GetSectionReportData(questionnaire.SittingPosition, userAnswers));
                    reportData.PositionSummary.Value = Convert.ToInt32(Math.Round((decimal)userRisks.FirstOrDefault(x => x.RiskID == sittingRiskID).Score * 100));
                    reportData.PositionSummary.Status = SetSummaryStatus(reportData.PositionSummary.Value);

                    reportData.ComputerPosition.AddRange(GetSectionReportData(questionnaire.ComputerPositionWhenSitting, userAnswers));
                    reportData.ComputerPositionSummary.Value = Convert.ToInt32(Math.Round((decimal)userRisks.FirstOrDefault(x => x.RiskID == sittingCompRiskID).Score * 100));
                    reportData.ComputerPositionSummary.Status = SetSummaryStatus(reportData.ComputerPositionSummary.Value);
                }
                else
                {
                    //Standing Position
                    reportData.StandingPosition.AddRange(GetSectionReportData(questionnaire.StandingPosition, userAnswers));
                    reportData.PositionSummary.Value = Convert.ToInt32(Math.Round((decimal)userRisks.FirstOrDefault(x => x.RiskID == standingRiskID).Score * 100));
                    reportData.PositionSummary.Status = SetSummaryStatus(reportData.PositionSummary.Value);

                    reportData.ComputerPosition.AddRange(GetSectionReportData(questionnaire.ComputerPositionWhenStanding, userAnswers));
                    reportData.ComputerPositionSummary.Value = Convert.ToInt32(Math.Round((decimal)userRisks.FirstOrDefault(x => x.RiskID == standingCompRiskID).Score * 100));
                    reportData.ComputerPositionSummary.Status = SetSummaryStatus(reportData.ComputerPositionSummary.Value);
                }
                #endregion

                reportData.OverAllSummary.Value = Convert.ToInt32(Math.Round((decimal)(reportData.HumanVariableSummary.Value + reportData.PositionSummary.Value + reportData.ComputerPositionSummary.Value) / 3));
                reportData.OverAllSummary.Status = SetSummaryStatus(reportData.OverAllSummary.Value);
            }
            return reportData;
        }

        private string SetSummaryStatus(int value)
        { return value <= 30 ? "btn-success" : value > 30 && value <= 40 ? "btn-warning" : "btn-danger"; }

        private IEnumerable<PositionEntries> GetSectionReportData(List<QuestionnaireModel> sittingPosition, List<UserAnswerModel> userAnswers)
        {
            var sectionData = new List<PositionEntries>();
            sittingPosition.ForEach(x =>
            {
                var answer = userAnswers.FirstOrDefault(z => z.QuestionID == x.QuestionID).Answer;
                var isPositive = x.Answers.FirstOrDefault(z => z.Text.Equals(answer)).Type == "switch-success" ? true : false;
                sectionData.Add(new PositionEntries()
                {
                    Description = x.Question,
                    Response = answer,
                    IsPositive = isPositive,
                    Comments = isPositive ? "Good" : x.Comment
                });
            });
            return sectionData;
        }

        private async Task BootStrapQuestionnaireModel()
        {
            try
            {
                var wwwrootPath = host.WebRootPath;
                using (StreamReader r = new StreamReader(wwwrootPath + @"\staticFiles\QuestionnaireSettings.json"))
                {
                    string json = r.ReadToEnd();
                    questionnaire = JsonConvert.DeserializeObject<QuestionDictionary>(json);
                }

                var questions = await questionService.GetQuestions();
                if (questions != null && questions.Any())
                {
                    UpdateQuestionBase(questionnaire.HumanVariables, questions);
                    UpdateQuestionBase(questionnaire.BMI, questions);
                    UpdateQuestionBase(questionnaire.Discomforts, questions);
                    UpdateQuestionBase(questionnaire.SittingPosition, questions);
                    UpdateQuestionBase(questionnaire.ComputerPositionWhenSitting, questions);
                    UpdateQuestionBase(questionnaire.StandingPosition, questions);
                    UpdateQuestionBase(questionnaire.ComputerPositionWhenStanding, questions);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void UpdateQuestionBase(List<QuestionnaireModel> variables, List<QuestionModel> questions)
        {
            variables.ForEach(x =>
            {
                var question = questions.FirstOrDefault(z => z.QuestionID == x.QuestionID);
                x.Question = question.Question;
                x.Comment = question.Comment;
                x.RiskID = question.RiskID;
            });
        }
        #endregion

    }
}