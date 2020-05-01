using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERAWeb.Models
{
    public class JobCompetencyMatrixModel
    {
        public int JobID { get; set; }
        public string Type { get; set; }
        public string Maingroup { get; set; }
        public string Subgroup { get; set; }
        public string Competency { get; set; }
        public byte LoW { get; set; }
        public int RequiredLevel { get; set; }
    }
}
