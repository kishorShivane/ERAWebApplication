using ERAWeb.Core.Interface;
using ERAWeb.Logger;
using ERAWeb.Models;
using ERAWeb.Proxy;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERAWeb.Core.Broker
{
    public class UserAnswerBroker : BrokerBase, IUserAnswerBroker
    {
        public UserAnswerBroker(IConfiguration _config, ILoggerManager loggerManager, IERAAzureServiceProxy _proxy)
        {
            logger = loggerManager;
            config = _config;
            proxy = _proxy;
        }

        public async Task<ResponseMessage<UserAnswerResponse>> GetUserAnswers(int userID)
        {
            return await Task.Run(() => proxy.GetUserAnswers(new UserAnswerReadRequest() { UserID = userID }));
        }

        public async Task<ResponseMessage<UserAnswerResponse>> InsertUserAnswers(List<UserAnswerModel> answers)
        {
            return await Task.Run(() => proxy.InsertUserAnswers(new UserAnswerWriteRequest() { Answers = answers }));
        }
    }
}
