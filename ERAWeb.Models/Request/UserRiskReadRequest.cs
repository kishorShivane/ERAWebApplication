using System;
using System.Collections.Generic;
using System.Text;

namespace ERAWeb.Models
{
    public class UserRiskReadRequest
    {
        public int RiskID { get; set; } = 0;
        public int UserID { get; set; } = 0;
        public Guid? TestIdentifier { get; set; } = null;
    }
}
