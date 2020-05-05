using ERAWeb.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERAWeb.Core.Interface
{
    public interface IUserRiskBroker
    {
        Task<List<UserRiskModel>> GetUserRisks(Guid identifier);
        Task<ResponseMessage<UserRiskResponse>> InsertUserRisks(List<UserRiskModel> userRisks);
    }
}
