using ERAWeb.Models;
using System.Threading.Tasks;

namespace ERAWeb.Core.Interface
{
    public interface IUserBroker
    {
        Task<ResponseMessage<UserModel>> ValidateUserCredentials(UserModel user);

        Task<string> UpdatePassword(ResetPassword user);
    }
}
