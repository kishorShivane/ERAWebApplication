using System;
using System.Collections.Generic;
using System.Text;

namespace ERAWeb.Models
{
    public class UserAnswerReadRequest
    {
        public int RiskID { get; set; } = 0;
        public int QuestionID { get; set; } = 0;
        public int UserID { get; set; } = 0;
    }
}
