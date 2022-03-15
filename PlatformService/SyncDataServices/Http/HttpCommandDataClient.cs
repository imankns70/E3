using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;

namespace PlatformService.SyncDataServivces.Http
{

    public class HttpCommanDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public HttpCommanDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task SendPlatformToCommand(PlatformReadDto plat)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(plat), Encoding.UTF8, "application/json");
            var requestUrl = _configuration["CommandService"];
            Console.WriteLine($"requestUri::::{requestUrl}");

            var response = await _httpClient.PostAsync(requestUrl, httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync Post to CommandService was Ok!");
            }
            else
            {
                Console.WriteLine("--> Sync Post to CommandService was Not Ok!");

            }
        }

    }
}