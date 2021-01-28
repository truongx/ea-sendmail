using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace EA.SendMail.WebAPI
{
    public static class Captcha
    {
        private const string CAPTCHA_ENDPOINT = "https://www.google.com/recaptcha/api/siteverify";
        private static HttpClient _httpClient = new HttpClient();

        private static async Task<bool> ValidateCaptchaAsync(string captchaToken)
        {
            var parameters = new Dictionary<string, string>
            {
                { "secret", Environment.GetEnvironmentVariable("ReCaptchaSecret") },
                { "response", captchaToken}
            };
            var encodedContent = new FormUrlEncodedContent(parameters);
            var response = await _httpClient.PostAsync(CAPTCHA_ENDPOINT, encodedContent);
            string responseBody = await response.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(responseBody);
            bool success = data.success;

            return success;
        }
    }
}