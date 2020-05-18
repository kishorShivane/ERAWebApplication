using ERAWeb.Core.Interface;
using ERAWeb.Logger;
using ERAWeb.Models;
using ERAWeb.Proxy;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ERAWeb.Core.Broker
{
    public class UserProfileBroker : BrokerBase, IUserProfileBroker
    {
        public UserProfileBroker(IConfiguration _config, ILoggerManager loggerManager, IERAAzureServiceProxy _proxy)
        {
            logger = loggerManager;
            config = _config;
            proxy = _proxy;
        }

        public async Task<ResponseMessage<UserModel>> GetUserProfile(UserModel user)
        {
            return await Task.Run(() => proxy.GetUserProfile(user));
        }

        public async Task<ResponseMessage<UserModel>> UpdateUserProfile(UserModel user)
        {
            return await Task.Run(() => proxy.UpdateUserProfile(user));
        }
    }
}
