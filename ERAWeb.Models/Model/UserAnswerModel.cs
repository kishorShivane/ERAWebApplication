using System;
using System.Collections.Generic;
using System.Text;

namespace ERAWeb.Models
{
    public class UserAnswerModel
    {
        public int UserAnswerID { get; set; }
        public int UserID { get; set; }
        public System.DateTime AssesmentDate { get; set; }
        public int RiskID { get; set; }
        public int QuestionID { get; set; }
        public string Answer { get; set; }
        public int Score { get; set; }
        public Guid TestIdentifier { get; set; }
    }
}
