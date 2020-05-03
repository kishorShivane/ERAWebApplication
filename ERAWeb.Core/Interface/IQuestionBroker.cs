using System.Collections.Generic;
using ERAWeb.Models;
using System.Threading.Tasks;

namespace ERAWeb.Core.Interface
{
    public interface IQuestionBroker
    {
        Task<List<QuestionModel>> GetQuestions();
    }
}
