using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERAWeb.Models
{
    public class QuestionnaireResponse
    {
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }
        public string Industry { get; set; }
        public string Organization { get; set; }
        public string BusinessFunction { get; set; }
        public string JobTitle { get; set; }
        public string Type { get; set; }
        public string MainGroup { get; set; }
        public string SubGroup { get; set; }
        public string Competency { get; set; }
        public byte LoW { get; set; }
        public int RequiredLevel { get; set; }
        public int CurrentLevel { get; set; }
        public System.DateTime RatingDate { get; set; }
        public int Gap { get; set; }
    }
}
