using ERAWeb.Core.Interface;
using ERAWeb.Logger;
using ERAWeb.Models;
using ERAWeb.Proxy;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERAWeb.Core.Broker
{
    public class QuestionBroker : BrokerBase, IQuestionBroker
    {
        public QuestionBroker(IConfiguration _config, ILoggerManager loggerManager, IERAAzureServiceProxy _proxy)
        {
            logger = loggerManager;
            config = _config;
            proxy = _proxy;
        }

        public async Task<List<QuestionModel>> GetQuestions()
        {
            List<QuestionModel> questions = null;
            ResponseMessage<QuestionResponse> response;
            response = await Task.Run(() => proxy.GetQuestions(new QuestionRequest() { QuestionID = 0, RiskID = 0}));
            if (response != null && response.Content != null && response.Content.Questions != null)
            {
                questions = response.Content.Questions;
            }
            return questions;
        }
    }
}
