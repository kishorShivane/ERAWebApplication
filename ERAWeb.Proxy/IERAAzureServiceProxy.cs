using ERAWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERAWeb.Proxy
{
    public interface IERAAzureServiceProxy
    {
        Task<ResponseMessage<UserModel>> ValidateUserCredentials(UserModel user);
        Task<ResponseMessage<UserModel>> RegisterUser(UserModel user);
        Task<string> UpdatePassword(ResetPassword user);
        Task<string> ActivateUser(int userID);
        Task<ResponseMessage<UserAnswerResponse>> GetUserAnswers(UserAnswerReadRequest request);
        Task<ResponseMessage<UserAnswerResponse>> InsertUserAnswers(UserAnswerWriteRequest request);
    }
}
