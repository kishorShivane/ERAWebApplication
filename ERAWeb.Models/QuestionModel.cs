using System;
using System.Collections.Generic;
using System.Text;

namespace ERAWeb.Models
{
    public class QuestionModel
    {
        public int QuestionID { get; set; }
        public int RiskID { get; set; }
        public string Question { get; set; }
        public string Comment { get; set; }
    }
}
