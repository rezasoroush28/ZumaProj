using Telegram.Bot;
using Zuma.Application.Interfaces;
using Telegram.Bot.Requests; 




public class TelegramMessageService  : ITelegramMessageService
{
    private readonly ITelegramBotClient _botClient;

    public TelegramMessageService(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task SendMessageAsync(long chatId, string message, CancellationToken cancellationToken = default)
    {
        var request = new SendMessageRequest
        {
            ChatId = chatId,
            Text = message,
            ParseMode = Telegram.Bot.Types.Enums.ParseMode.Markdown
        };

        await _botClient.SendRequest(request, cancellationToken);
    }
}
