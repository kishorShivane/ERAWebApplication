using System;
using System.Collections.Generic;
using System.Text;

namespace ERAWeb.Models
{
    public class UserRiskResponse
    {
        public List<UserRiskModel> UserRisks { get; set; }
        public int TotalRecords { get; set; }
    }
}
