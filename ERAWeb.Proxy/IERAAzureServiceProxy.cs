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
        Task<ResponseMessage<UserRiskResponse>> GetUserRisks(UserRiskReadRequest request);
        Task<ResponseMessage<UserRiskResponse>> InsertUserRisks(UserRiskWriteRequest request);
        Task<ResponseMessage<QuestionResponse>> GetQuestions(QuestionRequest request);
        Task<ResponseMessage<List<UserModel>>> GetUserReport(ReportRequest request);
        Task<ResponseMessage<UserModel>> GetUserProfile(UserModel user);
        Task<ResponseMessage<UserModel>> UpdateUserProfile(UserModel user);
    }
}
