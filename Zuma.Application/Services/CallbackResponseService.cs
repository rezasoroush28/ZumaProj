using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Zuma.Application.Interfaces.Telegram;
using Telegram.Bot.Requests;
using Zuma.Domain.Entities;
using Zuma.Domain.Enums;
using Zuma.Domain.Interfaces.IRepositories;
using Microsoft.Extensions.Caching.Memory;

public class CallbackResponseService : ITelegramResponseService
{
    private readonly IToDoItemRepository _toDoItemRepository;
    private readonly ITelegramBotClient _botClient;
    private readonly IUserSessionService _userSessionService;
    private IMemoryCache _memoryCache;
    public CallbackResponseService(ITelegramBotClient botClient, IUserSessionService userSessionService)
    {
        _botClient = botClient;
        _userSessionService = userSessionService;
    }

    public bool CanHandle(Update update)
    {
        return update.CallbackQuery != null;
    }

    public async Task ExecuteAsync(Update update, CancellationToken cancellationToken)
    {
        var callBack = update.CallbackQuery;
        var chatId = update.Message.Chat.Id;
        var callBackRequest = callBack.Data;

        switch (callBackRequest)
        {
            case "add_todo":
                {
                    _userSessionService.SetExpectedInput(chatId, ExpectedInputType.TodoTitle);
                    _userSessionService.SetUserState(chatId, UserState.WaitingForAnswer);
                    await _botClient.SendRequest(new SendMessageRequest
                    {
                        ChatId = chatId,
                        Text = "لطفاً عنوان کاری که می‌خوای اضافه کنی رو وارد کن:",
                    }, cancellationToken);
                    break;

                }

            case "add_description":
                {
                    _userSessionService.SetExpectedInput(chatId, ExpectedInputType.TodoDescription);
                    _userSessionService.SetUserState(chatId, UserState.WaitingForAnswer);

                    await _botClient.SendRequest(new SendMessageRequest
                    {
                        ChatId = chatId,
                        Text = "لطفاً توضیح مربوط به این کار رو وارد کن:",
                        ParseMode = ParseMode.Markdown
                    }, cancellationToken);
                    break;
                }

            case "save_todo":
                {
                    if (_memoryCache.TryGetValue<ToDoItem>($"todo-temp-{chatId}", out var cachedItem))
                    {
                        cachedItem.Status = ToDoStatus.JustMade;
                        cachedItem.ChatId = chatId;

                        await _toDoItemRepository.CreateToDoItem(
                            cachedItem.Title,
                            cachedItem.Description ?? string.Empty,
                            cachedItem.ChatId,
                            (int)cachedItem.Status
                            );

                        _memoryCache.Remove($"todo-temp-{chatId}");

                        await _botClient.SendRequest(new SendMessageRequest
                        {
                            ChatId = chatId,
                            Text = $"✅ کار جدید با موفقیت ذخیره شد:\n*{cachedItem.Title}*{(string.IsNullOrWhiteSpace(cachedItem.Description) ? "" : $"\n_{cachedItem.Description}_")}",
                            ParseMode = ParseMode.Markdown
                        }, cancellationToken);
                    }
                    else
                    {
                        await _botClient.SendRequest(new SendMessageRequest
                        {
                            ChatId = chatId,
                            Text = "❌ اطلاعات موقت یافت نشد. لطفاً دوباره تلاش کنید یا /start رو بزنید."
                        }, cancellationToken);
                    }

                    _userSessionService.ClearExpectedInput(chatId);
                    _userSessionService.SetUserState(chatId, UserState.Idle);

                    break;
                }
        }
    }
}
