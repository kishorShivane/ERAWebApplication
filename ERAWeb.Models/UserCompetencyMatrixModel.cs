using System;
using System.Collections.Generic;
using System.Text;

namespace ERAWeb.Models
{
    public class UserCompetencyMatrixModel
    {
        public int ID { get; set; }
        public byte UserID { get; set; }
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
