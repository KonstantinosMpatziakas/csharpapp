using CSharpApp.Application.Products;
using CSharpApp.Core.Settings;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Application.Token
{
    public class TokenService : ITokenService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly RestApiSettings _restApiSettings;

        public TokenService(IOptions<RestApiSettings> restApiSettings, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _restApiSettings = restApiSettings.Value;
        }

        public async Task<string> GetTokenAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_restApiSettings.BaseUrl!);

                var request = new
                {
                    email = _restApiSettings.Username,
                    password = _restApiSettings.Password
                };

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var jsonContent = JsonSerializer.Serialize(request, options);
                var data = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(_restApiSettings.Auth!, data);
                response.EnsureSuccessStatusCode();
                var token = await response.Content.ReadAsStringAsync();

                return token;
            }
            catch (Exception ex) {
                throw;
            }
        }
    }
}
