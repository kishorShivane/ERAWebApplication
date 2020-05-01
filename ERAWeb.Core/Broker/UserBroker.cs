using ERAWeb.Core.Interface;
using ERAWeb.Logger;
using ERAWeb.Models;
using ERAWeb.Proxy;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ERAWeb.Core.Broker
{
    public class UserBroker : BrokerBase, IUserBroker
    {
        public UserBroker(IConfiguration _config, ILoggerManager loggerManager, IERAAzureServiceProxy _proxy)
        {
            logger = loggerManager;
            config = _config;
            proxy = _proxy;
        }

        public async Task<ResponseMessage<UserModel>> ValidateUserCredentials(UserModel user)
        {
            return await Task.Run(() => proxy.ValidateUserCredentials(user));
        }

        public async Task<string> UpdatePassword(ResetPassword user)
        {
            return await Task.Run(() => proxy.UpdatePassword(user));
        }
    }
}
