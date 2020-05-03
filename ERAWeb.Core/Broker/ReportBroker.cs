using ERAWeb.Core.Interface;
using ERAWeb.Logger;
using ERAWeb.Models;
using ERAWeb.Proxy;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERAWeb.Core.Broker
{
    public class ReportBroker : BrokerBase, IReportBroker
    {
        public ReportBroker(IConfiguration _config, ILoggerManager loggerManager, IERAAzureServiceProxy _proxy)
        {
            logger = loggerManager;
            config = _config;
            proxy = _proxy;
        }

        public async Task<List<UserModel>> GetUserReport(ReportSearchModel model)
        {
            List<UserModel> users = null;
            ResponseMessage<List<UserModel>> response;
            response = await Task.Run(() => proxy.GetUserReport(new ReportRequest() { UserID = model.UserID, Email = model.Email }));
            if (response != null && response.Content != null && response.Content != null)
            {
                users = response.Content;
            }
            return users;
        }
    }
}
