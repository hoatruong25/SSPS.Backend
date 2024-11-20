using BusinessLogic;
using DTO.Params.ExternalParam;
using DTO.Results.ExternalResult;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using ApiGateway.Middleware;
using DTO.Results.SecurityResult;

namespace ApiGateway.Controllers.External
{
    [ApiController]
    [ControllerName("External")]
    [Route("api/external")]
    public class ExternalController : Controller
    {
        private readonly ILogic<AskChatGptParam, AskChatGptResult> _askChatGPTLogic;
        private readonly HttpClient _httpClient;
        public ExternalController(ILogic<AskChatGptParam, AskChatGptResult> askChatGPTLogic, HttpClient httpClient)
        {
            _askChatGPTLogic = askChatGPTLogic;
            _httpClient = httpClient;
        }

        [HttpPost]
        [Route("ask-chat-gpt")]
        [TypeFilter(typeof(AuthorizeUserAttribute))]
        public ActionResult<AskChatGptResult?> AskChatGPT(AskChatGptParam param)
        {
            var returnData = _askChatGPTLogic.Execute(param)?.Result;

            return returnData;
        }

        [HttpPost]
        [Route("chat-bot-user")]
        [TypeFilter(typeof(AuthorizeUserAttribute))]
        public async Task<ActionResult<AskChatBotResult?>> AskChatBotUserAsync(AskChatBotParam param)
        {
            var userContext = HttpContext.Items["User"] as UserContextResult;
            var userId = userContext != null ? userContext.Id : "";

            var requestBody = new
            {
                message = param.Message,
                userId
            };
        
            // Serialize the object to JSON
            var requestBodyJson = JsonConvert.SerializeObject(requestBody);

            var uri = "http://127.0.0.1:5000/chatbot-user";

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(uri),
                Headers =
                {
                },
                Content = new StringContent(requestBodyJson, Encoding.UTF8, "application/json")
                {
                    Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
                }
            };
        
            using (var response = await _httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var responseAsString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AskChatBotResult>(responseAsString);
            }
        }

        [HttpPost]
        [Route("chat-bot-admin")]
        [TypeFilter(typeof(AuthorizeAdminAttribute))]
        public async Task<ActionResult<AskChatBotResult?>> AskChatBotAdminAsync(AskChatBotParam param)
        {
            var userContext = HttpContext.Items["User"] as UserContextResult;
            var userId = userContext != null ? userContext.Id : "";

            var requestBody = new
            {
                message = param.Message,
                // userId
            };
        
            // Serialize the object to JSON
            var requestBodyJson = JsonConvert.SerializeObject(requestBody);

            var uri = "http://127.0.0.1:5001/chatbot-admin";

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(uri),
                Headers =
                {
                },
                Content = new StringContent(requestBodyJson, Encoding.UTF8, "application/json")
                {
                    Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
                }
            };
        
            using (var response = await _httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var responseAsString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AskChatBotResult>(responseAsString);
            }
        }
    }
}