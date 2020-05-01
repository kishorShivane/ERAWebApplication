using ERAWeb.Logger;
using ERAWeb.Models;
using ERAWeb.Proxy;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ERAWeb.Proxy
{
    public class ERAAzureServiceProxy : IERAAzureServiceProxy
    {
        public static ILoggerManager logger;
        public static IConfiguration config;
        public ERAAzureServiceProxy(IConfiguration _config, ILoggerManager loggerManager)
        {
            logger = loggerManager;
            config = _config;
        }

        public static void SerializeJsonIntoStream(object value, Stream stream)
        {
            using (var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            using (var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None })
            {
                var js = new JsonSerializer();
                js.Serialize(jtw, value);
                jtw.Flush();
            }
        }

        private static HttpContent CreateHttpContent(object content)
        {
            HttpContent httpContent = null;

            if (content != null)
            {
                var ms = new MemoryStream();
                SerializeJsonIntoStream(content, ms);
                ms.Seek(0, SeekOrigin.Begin);
                httpContent = new StreamContent(ms);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return httpContent;
        }

        public async Task<ResponseMessage<UserModel>> ValidateUserCredentials(UserModel user)
        {
            ResponseMessage<UserModel> result;
            string azureBaseUrl = config.GetValue<string>("ERAAzureAPIURL:ERAAzureAPIBaseURL");
            string urlQueryStringParams = config.GetValue<string>("ERAAzureAPIURL:ERALoginAPIURL");

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"{azureBaseUrl}{urlQueryStringParams}"))
            using (var httpContent = CreateHttpContent(user))
            {
                request.Content = httpContent;

                using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        result = JsonConvert.DeserializeObject<ResponseMessage<UserModel>>(jsonString);
                    }
                    else
                    {
                        result = new ResponseMessage<UserModel>();
                        result = JsonConvert.DeserializeObject<ResponseMessage<UserModel>>(jsonString);
                    }
                }
            }
            return result;
        }

        public async Task<ResponseMessage<UserModel>> RegisterUser(UserModel user)
        {
            ResponseMessage<UserModel> result;
            string azureBaseUrl = config.GetValue<string>("ERAAzureAPIURL:ERAAzureAPIBaseURL");
            string urlQueryStringParams = config.GetValue<string>("ERAAzureAPIURL:ERARegisterAPIURL");

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"{azureBaseUrl}{urlQueryStringParams}"))
            using (var httpContent = CreateHttpContent(user))
            {
                request.Content = httpContent;

                using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        result = JsonConvert.DeserializeObject<ResponseMessage<UserModel>>(jsonString);
                    }
                    else
                    {
                        result = new ResponseMessage<UserModel>();
                        result = JsonConvert.DeserializeObject<ResponseMessage<UserModel>>(jsonString);
                    }
                }
            }
            return result;
        }

        public async Task<string> UpdatePassword(ResetPassword user)
        {
            string result;
            string azureBaseUrl = config.GetValue<string>("ERAAzureAPIURL:ERAAzureAPIBaseURL");
            string urlQueryStringParams = config.GetValue<string>("ERAAzureAPIURL:ERAResetPasswordAPIURL");

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"{azureBaseUrl}{urlQueryStringParams}"))
            using (var httpContent = CreateHttpContent(user))
            {
                request.Content = httpContent;

                using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        result = "Updated";
                    }
                    else
                    {
                        ;
                        result = JsonConvert.DeserializeObject<string>(jsonString);
                    }
                }
            }
            return result;
        }

        public async Task<string> ActivateUser(int userID)
        {
            string result;
            string azureBaseUrl = config.GetValue<string>("ERAAzureAPIURL:ERAAzureAPIBaseURL");
            string urlQueryStringParams = config.GetValue<string>("ERAAzureAPIURL:ERAActivateAccountAPIURL");

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"{azureBaseUrl}{urlQueryStringParams}"))
            using (var httpContent = CreateHttpContent(userID))
            {
                request.Content = httpContent;

                using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        result = "Updated";
                    }
                    else
                    {
                        result = JsonConvert.DeserializeObject<string>(jsonString);
                    }
                }
            }
            return result;
        }

        public async Task<ResponseMessage<UserAnswerResponse>> GetUserAnswers(UserAnswerReadRequest readRequest)
        {
            ResponseMessage<UserAnswerResponse> result;
            string azureBaseUrl = config.GetValue<string>("ERAAzureAPIURL:ERAAzureAPIBaseURL");
            string urlQueryStringParams = config.GetValue<string>("ERAAzureAPIURL:ERAUserAnswerReadAPIURL");

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"{azureBaseUrl}{urlQueryStringParams}"))
            using (var httpContent = CreateHttpContent(readRequest))
            {
                request.Content = httpContent;

                using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        result = JsonConvert.DeserializeObject<ResponseMessage<UserAnswerResponse>>(jsonString);
                    }
                    else
                    {
                        result = new ResponseMessage<UserAnswerResponse>();
                        result = JsonConvert.DeserializeObject<ResponseMessage<UserAnswerResponse>>(jsonString);
                    }
                }
            }
            return result;
        }

        public async Task<ResponseMessage<UserAnswerResponse>> InsertUserAnswers(UserAnswerWriteRequest writeRequest)
        {
            ResponseMessage<UserAnswerResponse> result;
            string azureBaseUrl = config.GetValue<string>("ERAAzureAPIURL:ERAAzureAPIBaseURL");
            string urlQueryStringParams = config.GetValue<string>("ERAAzureAPIURL:ERAUserAnswerWriteAPIURL");

            using (var client = new HttpClient())
            //using (var request = new HttpRequestMessage(HttpMethod.Post, $"{azureBaseUrl}{urlQueryStringParams}"))
            using (var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:7071/api/ERAUserAnswerWrite"))
            using (var httpContent = CreateHttpContent(writeRequest))
            {
                request.Content = httpContent;

                using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        result = JsonConvert.DeserializeObject<ResponseMessage<UserAnswerResponse>>(jsonString);
                    }
                    else
                    {
                        result = new ResponseMessage<UserAnswerResponse>();
                        result = JsonConvert.DeserializeObject<ResponseMessage<UserAnswerResponse>>(jsonString);
                    }
                }
            }
            return result;
        }
    }
}
