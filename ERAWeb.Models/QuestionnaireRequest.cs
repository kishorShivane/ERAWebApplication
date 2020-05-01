using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERAWeb.Models
{
    public class QuestionnaireRequest
    {
        public int UserID { get; set; }
        public string Competency { get; set; }
        public int NumberOfQuestion { get; set; }
        public int Points { get; set; }
    }
}
