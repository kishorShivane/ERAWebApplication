using System;
using System.Collections.Generic;
using System.Text;

namespace ERAWeb.Models
{
    public class ReportSearchModel
    {
        public string Email { get; set; } = "";
        public int UserID { get; set; }
        public List<UserModel> Users { get; set; } = null;
    }
}
