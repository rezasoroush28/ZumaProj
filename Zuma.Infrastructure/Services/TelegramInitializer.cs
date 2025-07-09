using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Zuma.Infrastructure.Services
{
    public class TelegramInitializer : IHostedService
    {
        private readonly IConfiguration _config;
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly HttpClient _httpClient;

        public TelegramInitializer(IConfiguration config, ITelegramBotClient telegramBotClient, HttpClient httpClient)
        {
            _config = config;
            _telegramBotClient = telegramBotClient;
            _httpClient = httpClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            string ngrokApi = _config["Telegram:NgrokApi"];

            try
            {
                var response = await _httpClient.GetStringAsync(ngrokApi, cancellationToken);
                var json = JsonDocument.Parse(response);
                var tunnels = json.RootElement.GetProperty("tunnels");
                var httpsTunnel = tunnels.EnumerateArray()
                    .FirstOrDefault(t => t.GetProperty("public_url").GetString().StartsWith("https://"));

                if (httpsTunnel.ValueKind == JsonValueKind.Undefined)
                    throw new Exception("No HTTPS tunnel found from ngrok.");

                var publicUrl = httpsTunnel.GetProperty("public_url").GetString();

                var endpoint = _config["Telegram:WebhookEndpoint"];
                var fullWebhookUrl = $"{publicUrl}{endpoint}";

                await _telegramBotClient.SetWebhook(url: fullWebhookUrl, dropPendingUpdates : true, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
