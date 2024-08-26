using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using esquire.common.Models;
using esquire.common;

namespace esquire.services.Services
{
    public interface IGoogleService
    {

        public Task<GoogleResponse> GetUserInfoAsync(string baseUrl, string code, string urlRedirect);
    }
    public class GoogleService : IGoogleService
    {
        private readonly ConfigurationSettings _configuration;

        public GoogleService(ConfigurationSettings config)
        {
            _configuration = config;
        }
        public async Task<string> GetAccessTokenAsync(string baseUrl, string code, string urlRedirect)
        {
            var redirectUri = $"{baseUrl}/{urlRedirect}";
            var tokenUrl = "https://accounts.google.com/o/oauth2/token";

            using (var httpClient = new HttpClient())
            {
                var requestBody = $"code={code}&client_id={_configuration.GoogleClientId}&client_secret={_configuration.GoogleClientSecret}&redirect_uri={redirectUri}&grant_type=authorization_code";
                var response = await httpClient.PostAsync(tokenUrl, new StringContent(requestBody, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded"));

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var tokenData = JObject.Parse(responseContent);
                    return tokenData["access_token"].Value<string>();
                }
                else
                {
                    // Handle unsuccessful response
                    return null;
                }
            }
        }

        public async Task<GoogleResponse> GetUserInfoAsync(string baseUrl, string code, string urlRedirect)
        {
            var userInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo";
            string accessToken = GetAccessTokenAsync(baseUrl, code, urlRedirect).Result;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var response = await httpClient.GetAsync(userInfoUrl);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<GoogleResponse>(responseContent);
                }
                else
                {
                    // Handle unsuccessful response
                    return null;
                }
            }
        }
    }
}
