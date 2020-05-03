using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ERAWeb.Models
{
    public class ErgoReportModel
    {
        public ErgoReportModel()
        {
            ComputerPosition = new List<PositionEntries>();
            StandingPosition = new List<PositionEntries>();
            SittingPosition = new List<PositionEntries>();
            HumanVariables = new List<Entries>();
        }
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
        public Summary HumanVariableSummary { get; set; } = new Summary();
        public Summary PositionSummary { get; set; } = new Summary();
        public Summary ComputerPositionSummary { get; set; } = new Summary();
        public Summary OverAllSummary { get; set; } = new Summary();
    }

    public class Entries
    {
        public string Description { get; set; }
        public string Response { get; set; }
    }

    public class PositionEntries : Entries
    {
        public string Comments { get; set; }
        public bool IsPositive { get; set; }
    }

    public class Summary
    {
        public int Value { get; set; }
        public string Status { get; set; }
    }
}
