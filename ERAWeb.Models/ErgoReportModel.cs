using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ERAWeb.Models
{
    public class ErgoReportModel
    {
        public DateTime AssessmentDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeNumber { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public List<Entries> HumanVariables { get; set; }
        public List<PositionEntries> SittingPosition { get; set; }
        public List<PositionEntries> StandingPosition { get; set; }
        public List<PositionEntries> ComputerPosition { get; set; }
        public string HumanVariableSummary { get; set; } = "Human Variables: ";
        public string PositionSummary { get; set; } = "Sitting Position: ";
        public string ComputerPositionSummary { get; set; } = "Computer Position: ";
        public string OverAllSummary { get; set; } = "Overall Ergonomics Rating: ";
    }

    public class Entries
    {
        public string Description { get; set; }
        public string Response { get; set; }
    }

    public class PositionEntries : Entries
    {
        public string Comments { get; set; }
    }
}
