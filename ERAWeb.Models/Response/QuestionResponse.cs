using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERAWeb.Models
{
    public class QuestionResponse
    {
        public List<QuestionModel> Questions { get; set; }
        public int TotalRecords { get; set; }
    }
}
