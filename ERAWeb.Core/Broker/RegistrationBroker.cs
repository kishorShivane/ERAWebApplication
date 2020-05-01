using ERAWeb.Core.Interface;
using ERAWeb.Logger;
using ERAWeb.Models;
using ERAWeb.Proxy;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ERAWeb.Core.Broker
{
    public class RegistrationBroker : BrokerBase, IRegistrationBroker
    {
        public RegistrationBroker(IConfiguration _config, ILoggerManager loggerManager, IERAAzureServiceProxy _proxy)
        {
            logger = loggerManager;
            config = _config;
            proxy = _proxy;
        }

        public async Task<ResponseMessage<UserModel>> RegisterUser(UserModel user)
        {
            return await Task.Run(() => proxy.RegisterUser(user));
        }

        public async Task<string> ActivateUser(int userID)
        {
            return await Task.Run(() => proxy.ActivateUser(userID));
        }
    }
}
