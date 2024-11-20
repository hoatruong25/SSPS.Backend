using Helper.ChatGPT.Responses;
using Helper.Paypal.Responses;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Helper.ChatGPT
{
    public class ChatGPTApi : IChatGPTApi
    {
        private HttpClient _httpClient;
        public ChatGPTApi()
        {
            _httpClient = new HttpClient();
        }

        public async Task<AskChatGPTResponse?> AskChatGPT(string message)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(ChatGPTConfig.BaseUrl),
                Headers =
                {
                    { "X-RapidAPI-Key", ChatGPTConfig.RapitAPI_KEY },
                    { "X-RapidAPI-Host", ChatGPTConfig.RapitAPI_HOST },
                },
                //Content = new StringContent
                //($"[{{\"content\": {message},\"role\": \"user\"\r}}]")
                Content = new StringContent
                ("[\r\n{\r\n\"content\": \""+ message + "\",\r\n\"role\": \"user\"\r\n}\r\n]")
                {
                    Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
                }
            };

            using (var response = await _httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var resposneAsString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AskChatGPTResponse>(resposneAsString);
            }
        }
    }
}