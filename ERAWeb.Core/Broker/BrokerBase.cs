using ERAWeb.Logger;
using ERAWeb.Proxy;
using Microsoft.Extensions.Configuration;

namespace ERAWeb.Core.Broker
{
    public class BrokerBase
    {
        public ILoggerManager logger;
        public IConfiguration config;
        public IERAAzureServiceProxy proxy;
    }
}
