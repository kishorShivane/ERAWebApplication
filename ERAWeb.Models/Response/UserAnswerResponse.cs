using System;
using System.Collections.Generic;
using System.Text;

namespace ERAWeb.Models
{
    public class UserAnswerResponse
    {
        public List<UserAnswerModel> Answers { get; set; }
        public int TotalRecords { get; set; }
    }
}
