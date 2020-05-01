using System;
using System.Collections.Generic;
using System.Text;

namespace ERAWeb.Models
{

    public class QuestionDictionary
    {
        public List<ErgonomicScore> ErgonomicScores{ get; set; }
        public List<QuestionnaireModel> HumanVariables { get; set; }
        public List<QuestionnaireModel> BMI { get; set; }
        public List<QuestionnaireModel> Discomforts { get; set; }
        public List<QuestionnaireModel> WorkStation { get; set; }
        public List<QuestionnaireModel> SittingPosition { get; set; }
        public List<QuestionnaireModel> ComputerPositionWhenSitting { get; set; }
        public List<QuestionnaireModel> StandingPosition { get; set; }
        public List<QuestionnaireModel> ComputerPositionWhenStanding { get; set; }
    }
    public class QuestionnaireModel
    {
        public int QuestionID { get; set; }
        public string Question { get; set; }
        public int RiskID { get; set; }
        public string Risk { get; set; }
        public string Comment { get; set; }
        public string Category { get; set; }
        public List<Answer> Answers { get; set; }
        public string AnswerSelected { get; set; }
        public Information Information { get; set; }
    }

    public class Answer
    {
        public string Text { get; set; }
        public string Type { get; set; }
        public int Score { get; set; }
    }

    public class ErgonomicScore
    {
        public int RiskID { get; set; }
        public bool IsRange { get; set; } = false;
        public double FromValue { get; set; }
        public double ToValue { get; set; }
        public string Value { get; set; }
        public int Score { get; set; }
        public string RiskType { get; set; }
    }

    public class Information
    {
        public string Title { get; set; }
        public string InformationTitle { get; set; }
        public string InformationText { get; set; }
        public string InformationImageURL { get; set; }
        public string InformationImageText { get; set; }
        public string RecommendationTitle { get; set; }
        public string RecommendationText { get; set; }
        public string RecommendationImageURL { get; set; }
        public string RecommendationImageText { get; set; }
        public string PopoverClass { get; set; }
    }
}
