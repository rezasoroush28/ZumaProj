
namespace Zuma.Application.Interfaces
{
    public interface ITelegramMessageService
    {
        Task SendMessageAsync(long chatId, string message, CancellationToken cancellationToken = default);
    }
}
