using System;
using System.Collections.Generic;
using System.Text;

namespace ERAWeb.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public System.DateTime RegisteredDate { get; set; }
        public int UserTypeId { get; set; }
        public string UserType { get; set; }
        public string CompanyName { get; set; }
        public bool IsTestTaken { get; set; }
        public DateTime? LastAssessmentDate { get; set; } = null;
        public Guid? LatestTestIdentifier { get; set; } = null;
    }
}
