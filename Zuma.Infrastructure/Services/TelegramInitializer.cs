using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Zuma.Infrastructure.Services
{
    public class TelegramInitializer : IHostedService
    {
        private readonly IConfiguration _config;
        private readonly ITelegramBotClient _telegramBotClient;

        public TelegramInitializer(IConfiguration config, ITelegramBotClient telegramBotClient)
        {
            _config = config;
            _telegramBotClient = telegramBotClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var webHookUrl = _config["Telegram:WebhookUrl"];
            if (webHookUrl is null)
                throw new Exception("null webhook url");

            try
            {
                await _telegramBotClient.SetWebhook(url: webHookUrl, dropPendingUpdates : true, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
