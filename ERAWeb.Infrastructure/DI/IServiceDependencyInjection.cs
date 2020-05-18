using ERAWeb.Core.Broker;
using ERAWeb.Core.Interface;
using ERAWeb.Logger;
using ERAWeb.Proxy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Mail;

namespace ERAWeb.Infrastructure.DI
{
    public static class IServiceDependencyInjection
    {
        public static IServiceCollection InjectApplicationDependency(this IServiceCollection services)
        {
            services.AddTransient<IUserBroker, UserBroker>();
            services.AddTransient<IRegistrationBroker, RegistrationBroker>();
            services.AddTransient<ILoggerManager, LoggerManager>();
            services.AddTransient<IQuestionBroker, QuestionBroker>();
            services.AddTransient<IUserAnswerBroker, UserAnswerBroker>();
            services.AddTransient<IUserRiskBroker, UserRiskBroker>();
            services.AddTransient<IQuestionBroker, QuestionBroker>();
            services.AddTransient<IReportBroker, ReportBroker>();
            services.AddTransient<IUserProfileBroker, UserProfileBroker>();
            services.AddTransient<IERAAzureServiceProxy, ERAAzureServiceProxy>();

            return services;
        }

        public static IConfiguration AddLoggerConfiguration(this IConfiguration configuration)
        {
            LoggerManager.LoadConfigucation("/nlog.config");
            return configuration;
        }

        public static IServiceCollection InjectSMTPDependency(this IServiceCollection services)
        {
            services.AddTransient<SmtpClient>((serviceProvider) =>
            {
                var config = serviceProvider.GetRequiredService<IConfiguration>();
                return new SmtpClient()
                {
                    Host = config.GetValue<String>("Email:Smtp:Host"),
                    Port = config.GetValue<int>("Email:Smtp:Port"),
                    Credentials = new NetworkCredential(
                            config.GetValue<String>("Email:Smtp:Username"),
                            config.GetValue<String>("Email:Smtp:Password")
                        )
                };
            });

            return services;
        }
    }
}
