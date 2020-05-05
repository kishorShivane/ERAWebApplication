using ERAWeb.Core.Interface;
using ERAWeb.Logger;
using ERAWeb.Models;
using ERAWeb.Proxy;
using Microsoft.Extensions.Configuration;
using System;
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

        public async Task<List<UserAnswerModel>> GetUserAnswers(Guid identifier)
        {
            List<UserAnswerModel> userAnswers = null;
            ResponseMessage<UserAnswerResponse> response;
            response = await Task.Run(() => proxy.GetUserAnswers(new UserAnswerReadRequest() { TestIdentifier = identifier }));
            if (response != null && response.Content != null && response.Content.Answers != null)
            {
                userAnswers = response.Content.Answers;
            }
            return userAnswers;
        }

        public async Task<ResponseMessage<UserAnswerResponse>> InsertUserAnswers(List<UserAnswerModel> answers)
        {
            return await Task.Run(() => proxy.InsertUserAnswers(new UserAnswerWriteRequest() { Answers = answers }));
        }
    }
}
