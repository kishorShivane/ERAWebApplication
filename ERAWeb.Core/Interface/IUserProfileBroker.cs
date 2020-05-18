using ERAWeb.Models;
using System.Threading.Tasks;

namespace ERAWeb.Core.Interface
{
    public interface IUserProfileBroker
    {
        Task<ResponseMessage<UserModel>> GetUserProfile(UserModel user);
        Task<ResponseMessage<UserModel>> UpdateUserProfile(UserModel user);
    }
}
