using ERAWeb.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERAWeb.Core.Interface
{
    public interface IUserAnswerBroker
    {
        Task<List<UserAnswerModel>> GetUserAnswers(Guid identifier);
        Task<ResponseMessage<UserAnswerResponse>> InsertUserAnswers(List<UserAnswerModel> answers);
    }
}
