using System;
using System.Collections.Generic;
using System.Text;

namespace ERAWeb.Models
{
    public class CompetenciesReportRequest
    {
        public int UserID { get; set; }
        public string Type { get; set; }
        public string MainGroup { get; set; }
        public string SubGroup { get; set; }
        public string Competency { get; set; }
    }
}
