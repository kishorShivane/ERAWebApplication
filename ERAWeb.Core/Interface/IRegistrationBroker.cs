using ERAWeb.Models;
using System.Threading.Tasks;

namespace ERAWeb.Core.Interface
{
    public interface IRegistrationBroker
    {
        Task<ResponseMessage<UserModel>> RegisterUser(UserModel user);
        Task<string> ActivateUser(int userID);
    }
}
