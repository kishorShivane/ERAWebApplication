using System;
using System.Collections.Generic;
using System.Text;

namespace ERAWeb.Models
{
    public class UserRiskModel
    {
        public int UserRisksId { get; set; }
        public int UserId { get; set; }
        public System.DateTime AssesmentDate { get; set; }
        public int RiskId { get; set; }
        public int Score { get; set; }
        public int RiskValue { get; set; }
        public string Risk { get; set; }
    }
}
