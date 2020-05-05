using System;
using System.Collections.Generic;
using System.Text;

namespace ERAWeb.Models
{
    public class UserRiskModel
    {
        public int UserRisksID { get; set; }
        public int UserID { get; set; }
        public System.DateTime AssesmentDate { get; set; }
        public int RiskID { get; set; }
        public double Score { get; set; }
        public int RiskValue { get; set; }
        public string Risk { get; set; }
        public Guid TestIdentifier { get; set; }
    }
}
