using ERAWeb.Core.Interface;
using ERAWeb.Logger;
using ERAWeb.Models;
using ERAWeb.Proxy;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERAWeb.Core.Broker
{
    public class UserRiskBroker : BrokerBase, IUserRiskBroker
    {
        public UserRiskBroker(IConfiguration _config, ILoggerManager loggerManager, IERAAzureServiceProxy _proxy)
        {
            logger = loggerManager;
            config = _config;
            proxy = _proxy;
        }

        public async Task<List<UserRiskModel>> GetUserRisks(Guid identifier)
        {
            List<UserRiskModel> userRisks = null;
            ResponseMessage<UserRiskResponse> response;
            response = await Task.Run(() => proxy.GetUserRisks(new UserRiskReadRequest() { TestIdentifier = identifier }));
            if (response != null && response.Content != null && response.Content.UserRisks != null)
            {
                userRisks = response.Content.UserRisks;
            }
            return userRisks;
        }

        public async Task<ResponseMessage<UserRiskResponse>> InsertUserRisks(List<UserRiskModel> userRisks)
        {
            return await Task.Run(() => proxy.InsertUserRisks(new UserRiskWriteRequest() { UserRisks = userRisks }));
        }
    }
}
