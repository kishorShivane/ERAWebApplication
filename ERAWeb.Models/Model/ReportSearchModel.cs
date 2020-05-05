using System;
using System.Collections.Generic;
using System.Text;

namespace ERAWeb.Models
{
    public class ReportSearchModel
    {
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public List<UserModel> Users { get; set; } = null;
    }
}
