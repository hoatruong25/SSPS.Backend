using Helper.Paypal.Responses;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using IdentityModel.Client;
using Helper.Paypal.Requests;

namespace Helper.Paypal
{
    // Cần optimise lại vì mỗi lần gọi thì cần authen sẽ tốn nhiều tài nguyên
    public class PayPalApi : IPayPalApi
    {
        private HttpClient _httpClient;

        public PayPalApi()
        {
            _httpClient = new HttpClient();
        }

        private async Task<AuthorizationPayPalResponse?> GetAuthorization()
        {
            try
            {
                var byteArray = Encoding.ASCII.GetBytes($"{PayPalConfig.ClientId}:{PayPalConfig.ClientSecret}");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                var keyValuePairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
            };

                var resposne = await _httpClient.PostAsync($"{PayPalConfig.BaseUrl}/v1/oauth2/token", new FormUrlEncodedContent(keyValuePairs));

                var resposneAsString = await resposne.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<AuthorizationPayPalResponse>(resposneAsString);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<CreatePlanPayPalResponse?> CreatePlan(CreatePlanRequest request)
        {
            // Viết thêm func check token đã hết hạn chưa
            try
            {
                var getAuthen = await GetAuthorization();

                if (getAuthen == null)
                    return null;

                _httpClient.SetBearerToken(getAuthen.access_token);

                var requestContent = JsonConvert.SerializeObject(request);

                var httpRequestMessage = new HttpRequestMessage
                {
                    Content = new StringContent(requestContent, Encoding.UTF8, "application/json")
                };

                httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await _httpClient.PostAsync($"{PayPalConfig.BaseUrl}/v1/billing/plans", httpRequestMessage.Content);

                var resposneAsString = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<CreatePlanPayPalResponse>(resposneAsString);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<GetPlanDetailsResponse?> GetPlanDetails(string planId)
        {
            var getAuthen = await GetAuthorization();

            if (getAuthen == null)
                return null;

            _httpClient.SetBearerToken(getAuthen.access_token);

            var response = await _httpClient.GetAsync($"{PayPalConfig.BaseUrl}/v1/billing/plans/{planId}");

            var resposneAsString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<GetPlanDetailsResponse>(resposneAsString);
        }

        public async Task<CreateSubscriptionResponse?> CreateSubscrition(CreateSubscriptionRequest request)
        {
            var getAuthen = await GetAuthorization();

            if (getAuthen == null)
                return null;

            _httpClient.SetBearerToken(getAuthen.access_token);

            var requestContent = JsonConvert.SerializeObject(request);

            var httpRequestMessage = new HttpRequestMessage
            {
                Content = new StringContent(requestContent, Encoding.UTF8, "application/json")
            };

            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.PostAsync($"{PayPalConfig.BaseUrl}/v1/billing/subscriptions", httpRequestMessage.Content);

            var resposneAsString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<CreateSubscriptionResponse>(resposneAsString);
        }
    }
}
