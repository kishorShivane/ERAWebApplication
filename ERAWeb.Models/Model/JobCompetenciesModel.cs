namespace ERAWeb.Models
{
    public class JobCompetenciesModel
    {
        public string Type { get; set; }
        public string MainGroup { get; set; }
        public string SubGroup { get; set; }
        public string Competency { get; set; }
        public byte LoW { get; set; }
        public byte Senior_Executive { get; set; }
        public byte Executive { get; set; }
        public byte Chief_Specialist { get; set; }
        public byte Senior_Manager { get; set; }
        public byte Senior_Specialist { get; set; }
        public byte Manager { get; set; }
        public byte Professional_Specialist { get; set; }
        public byte Team_Leader { get; set; }
        public byte Technical_Specialist { get; set; }
        public byte Office_Administrator { get; set; }
        public byte Business_Process_Implementor { get; set; }
        public byte General_Worker { get; set; }
    }
}
