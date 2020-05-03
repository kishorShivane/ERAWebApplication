﻿using System;
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
        #region fields
        private static QuestionDictionary questionnaire = null;
        IHostingEnvironment host;
        IUserAnswerBroker userAnswerservice;
        IUserRiskBroker userRiskservice;
        IQuestionBroker questionService;
        #endregion

        #region constructor
        public QuestionnaireController(IConfiguration _config, ILoggerManager loggerManager, IHostingEnvironment _host,
                IUserAnswerBroker _userAnswerService, IUserRiskBroker _userRiskservice, IQuestionBroker _questionService)
        {
            logger = loggerManager;
            config = _config;
            host = _host;
            userAnswerservice = _userAnswerService;
            userRiskservice = _userRiskservice;
            questionService = _questionService;
        }

        #endregion

        #region action methods
        public async Task<IActionResult> Index()
        {
            if (!CheckIfTestTaken())
            {
                if (questionnaire == null)
                    await BootStrapQuestionnaireModel();
                var model = questionnaire;
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Report", new { id = UserInfo.UserId });
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(QuestionDictionary model)
        {
            List<UserAnswerModel> userAnswers = null;
            List<UserRiskModel> userRisks = null;
            if (model != null)
            {
                userAnswers = GetUserAnswersFromModel(model);
                if (userAnswers.Any())
                    userRisks = GetUserRisksFromAnswers(userAnswers);
            }
            if (userRisks.Any())
            {
                await userAnswerservice.InsertUserAnswers(userAnswers);
                await userRiskservice.InsertUserRisks(userRisks);
                await UpdateUserTestStatus();
                SetNotification("Thank you for completing test successfully.!!", NotificationType.Success, "Successfully Submitted");
                return RedirectToActionPermanent("Index", "Report", new { id = UserInfo.UserId });
            }
            return View(questionnaire);
        }
        #endregion

        #region private methods
        private async Task UpdateUserTestStatus()
        {
            var user = await GetUserSessionAsync();
            if (user != null)
            {
                user.IsTestTaken = true;
            }
            await SetUserSessionAsync(user);
        }

        private List<UserRiskModel> GetUserRisksFromAnswers(List<UserAnswerModel> userAnswers)
        {
            List<UserRiskModel> userRisks = null;
            var todaysDate = DateTime.Now;
            var groupByRisk = userAnswers.GroupBy(x => x.RiskID);
            Tuple<int, string> riskScore = null;
            double finalValue = 0;
            if (groupByRisk.Any())
            {
                userRisks = new List<UserRiskModel>();
                foreach (var risk in groupByRisk)
                {
                    var ergonomics = questionnaire.ErgonomicScores.Where(z => z.RiskID == risk.Key).ToList();
                    switch (risk.Key)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            finalValue = risk.FirstOrDefault(x => x.RiskID == risk.Key).Score;
                            var riskText = ergonomics.FirstOrDefault(x => x.RiskID == risk.Key && x.Score == finalValue).RiskType;
                            riskScore = new Tuple<int, string>(Convert.ToInt32(finalValue), riskText);
                            break;
                        case 5:
                            var weight = Convert.ToDouble(risk.FirstOrDefault(z => z.QuestionID == 5).Answer);
                            var height = Convert.ToDouble(risk.FirstOrDefault(z => z.QuestionID == 6).Answer);
                            finalValue = Math.Round(Convert.ToDouble(weight / (height * height)), 4);
                            riskScore = GetScoreForRangeFromRisk(ergonomics, finalValue);
                            break;
                        case 6:
                            finalValue = risk.Sum(x => x.Score);
                            riskScore = GetScoreForRangeFromRisk(ergonomics, finalValue);
                            break;
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                            finalValue = (Convert.ToDouble(risk.Sum(x => x.Score)) / Convert.ToDouble(risk.Count()));
                            riskScore = GetScoreForRangeFromRisk(ergonomics, finalValue);
                            break;
                        default:
                            break;
                    }
                    userRisks.Add(new UserRiskModel()
                    {
                        RiskID = risk.Key,
                        UserID = UserInfo.UserId,
                        AssesmentDate = todaysDate,
                        Score = finalValue,
                        RiskValue = riskScore.Item1,
                        Risk = riskScore.Item2
                    });
                }
            }

            return userRisks;
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
                    UserID = UserInfo.UserId,
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
                score = (ans != null) ? ans.Score : score;
            }
            return score;
        }

        private void UpdateScoreForSpecialRisk(List<UserAnswerModel> sectionAnswers)
        {
            int score = 0;
            var groupByRisk = sectionAnswers.GroupBy(x => x.RiskID).ToList();
            groupByRisk.ForEach(x =>
            {
                var ergonomics = questionnaire.ErgonomicScores.Where(z => z.RiskID == x.Key).ToList();
                switch (x.Key)
                {
                    case 5:
                        var weight = Convert.ToDouble(x.FirstOrDefault(z => z.QuestionID == 5).Answer);
                        var height = Convert.ToDouble(x.FirstOrDefault(z => z.QuestionID == 6).Answer);
                        var bmiScore = Convert.ToDouble(weight / (height * height));
                        score = GetScoreForRangeFromRisk(ergonomics, bmiScore).Item1;
                        sectionAnswers.Where(y => y.RiskID == x.Key).Select(z => { z.Score = score; return z; }).ToList();
                        break;
                    default:
                        break;
                }

            });
        }

        private Tuple<int, string> GetScoreForRangeFromRisk(List<ErgonomicScore> ergonomics, double value)
        {
            int score = 0;
            string risk = "";
            if (ergonomics.Any() && ergonomics.FirstOrDefault().IsRange)
            {
                foreach (var ergo in ergonomics)
                {
                    if (value > ergo.FromValue && value < ergo.ToValue)
                    {
                        score = ergo.Score;
                        risk = ergo.RiskType;
                        break;
                    }
                }
            }
            return new Tuple<int, string>(score, risk);
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