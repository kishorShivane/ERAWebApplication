using ERAWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERAWeb.Core.Interface
{
    public interface IReportBroker
    {
        Task<List<UserModel>> GetUserReport(ReportSearchModel model);
    }
}
