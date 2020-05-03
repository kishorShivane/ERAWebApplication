using ERAWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERAWeb.Core.Interface
{
    public interface IUserRiskBroker
    {
        Task<List<UserRiskModel>> GetUserRisks(int userID);
        Task<ResponseMessage<UserRiskResponse>> InsertUserRisks(List<UserRiskModel> userRisks);
    }
}
