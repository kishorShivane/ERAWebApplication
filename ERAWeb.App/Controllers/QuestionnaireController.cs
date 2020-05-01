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
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using static ERAWeb.App.Utilities.EnumHelpers;

namespace ERAWeb.App.Controllers
{
    public class QuestionnaireController : BaseController
    {
        private static QuestionDictionary questionnaire;
        IHostingEnvironment host;
        IUserAnswerBroker service;
        private int userID = 0;

        public int UserID
        {
            get
            {
                if (userID == 0)
                {
                    var user = GetUserSession();
                    if (user != null)
                    {
                        userID = user.UserId;
                    }
                }
                return userID;
            }
        }

        public QuestionnaireController(IConfiguration _config, ILoggerManager loggerManager, IHostingEnvironment _host, IUserAnswerBroker _service)
        {
            logger = loggerManager;
            config = _config;
            host = _host;
            service = _service;
        }
        public IActionResult Index()
        {
            BootStrapQuestionnaireModel();
            var model = questionnaire;
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(QuestionDictionary model)
        {
            List<UserAnswerModel> userAnswers = null;
            if (model != null)
            {
                userAnswers = GetUserAnswersFromModel(model);
            }
            if (userAnswers.Any())
            {
                await service.InsertUserAnswers(userAnswers);
            }
            return View("Index", model);
        }

        private List<UserAnswerModel> GetUserAnswersFromModel(QuestionDictionary model)
        {
            List<UserAnswerModel> userAnswers = null;
            if (model != null)
            {
                userAnswers = new List<UserAnswerModel>();
                userAnswers.AddRange(GetIndividualSectionAnswers(model.HumanVariables, questionnaire.HumanVariables));
                userAnswers.AddRange(GetIndividualSectionAnswers(model.BMI, questionnaire.BMI));
                userAnswers.AddRange(GetIndividualSectionAnswers(model.Discomforts, questionnaire.Discomforts));

                if (model.WorkStation.FirstOrDefault().AnswerSelected == "Sitting Workstation")
                {
                    userAnswers.AddRange(GetIndividualSectionAnswers(model.SittingPosition, questionnaire.SittingPosition));
                    userAnswers.AddRange(GetIndividualSectionAnswers(model.ComputerPositionWhenSitting, questionnaire.ComputerPositionWhenSitting));
                }
                else
                {
                    userAnswers.AddRange(GetIndividualSectionAnswers(model.StandingPosition, questionnaire.StandingPosition));
                    userAnswers.AddRange(GetIndividualSectionAnswers(model.ComputerPositionWhenStanding, questionnaire.ComputerPositionWhenStanding));
                }

                if (userAnswers.Any())
                {
                    UpdateScoreForSpecialRisk(userAnswers);
                }
            }
            return userAnswers;
        }

        private IEnumerable<UserAnswerModel> GetIndividualSectionAnswers(List<QuestionnaireModel> answerModel, List<QuestionnaireModel> questionModel)
        {
            List<UserAnswerModel> sectionAnswers = null;
            var todaysDate = DateTime.Now;
            if (answerModel.Any()) sectionAnswers = new List<UserAnswerModel>();
            answerModel.ForEach(x =>
            {
                int score = GetScoreForAnswerToQuestion(x.QuestionID, x.AnswerSelected, questionModel);
                sectionAnswers.Add(new UserAnswerModel()
                {
                    Answer = x.AnswerSelected,
                    AssesmentDate = todaysDate,
                    QuestionID = x.QuestionID,
                    RiskID = x.RiskID,
                    UserID = UserID,
                    Score = score
                });
            });
            return sectionAnswers;
        }

        private int GetScoreForAnswerToQuestion(int questionID, string answerSelected, List<QuestionnaireModel> questionModel)
        {
            int score = 0;
            var question = questionModel.FirstOrDefault(z => z.QuestionID == questionID);
            if (question != null && question.Answers != null)
            {
                var ans = question.Answers.FirstOrDefault(x => x.Text.Equals(answerSelected));
                score = (ans != null)? ans.Score : score;
            }
            return score;
        }

        private void UpdateScoreForSpecialRisk(List<UserAnswerModel> sectionAnswers)
        {
            int score = 0;
            var groupByRisk = sectionAnswers.GroupBy(x => x.RiskID).ToList();
            groupByRisk.ForEach(x =>
            {
                switch (x.Key)
                {
                    case 5:
                        var weight = Convert.ToDouble(x.FirstOrDefault(z => z.QuestionID == 5).Answer);
                        var height = Convert.ToDouble(x.FirstOrDefault(z => z.QuestionID == 6).Answer);
                        var bmiScore = weight / (height * height);
                        score = GetScoreForRangeFromRisk(x.Key, bmiScore);
                        sectionAnswers.Where(y => y.RiskID == x.Key).Select(z => { z.Score = score; return z; }).ToList();
                        break;
                    default:
                        break;
                }

            });
        }

        private int GetScoreForRangeFromRisk(int riskID, double value)
        {
            int score = 0;
            var ergonomics = questionnaire.ErgonomicScores.Where(x => x.RiskID == riskID).ToList();
            foreach (var ergo in ergonomics)
            {
                score = (value > ergo.FromValue && value < ergo.ToValue) ? ergo.Score : score;
            }
            return score;
        }

        private void BootStrapQuestionnaireModel()
        {
            var wwwrootPath = host.WebRootPath;
            using (StreamReader r = new StreamReader(wwwrootPath + @"\staticFiles\QuestionnaireSettings.json"))
            {
                string json = r.ReadToEnd();
                questionnaire = JsonConvert.DeserializeObject<QuestionDictionary>(json);
            }
        }


    }
}