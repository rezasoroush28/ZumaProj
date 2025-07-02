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
using Telegram.Bot.Types.ReplyMarkups;

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

            case string callback when callback.StartsWith("show_todos_page_"):
                {
                    var pagePart = callback.Replace("show_todos_page_", "");
                    int.TryParse(pagePart, out int page);
                    const int pageSize = 5;

                    var allItems = await _toDoItemRepository.GetToDoItemsByChatId(chatId);
                    allItems = allItems.OrderBy(x => x.Status == ToDoStatus.JustMade ? 0 :
                                x.Status == ToDoStatus.InProgress ? 1 :
                                x.Status == ToDoStatus.Done ? 2 :
                                3).ToList();

                    var totalPages = (int)Math.Ceiling((double)allItems.Count / pageSize);
                    page = Math.Clamp(page, 0, totalPages - 1); // جلوگیری از خطا

                    var pageItems = allItems
                        .Skip(page * pageSize)
                        .Take(pageSize)
                        .Select(item => new[]
                        {
            InlineKeyboardButton.WithCallbackData(
                $"✨ {item.Title} - {item.Status}", $"todo_detail_{item.Id}")
                        }).ToList();

                    var navigationButtons = new List<InlineKeyboardButton[]>();

                    if (totalPages > 1)
                    {
                        var navRow = new List<InlineKeyboardButton>();

                        if (page > 0)
                            navRow.Add(InlineKeyboardButton.WithCallbackData("⬅️ قبلی", $"show_todos_page_{page - 1}"));

                        if (page < totalPages - 1)
                            navRow.Add(InlineKeyboardButton.WithCallbackData("بعدی ➡️", $"show_todos_page_{page + 1}"));

                        navigationButtons.Add(navRow.ToArray());
                    }

                    var markup = new InlineKeyboardMarkup(pageItems.Concat(navigationButtons));

                    await _botClient.SendRequest(new SendMessageRequest
                    {
                        ChatId = chatId,
                        Text = $"📝 لیست کارهای شما (صفحه {page + 1} از {totalPages}):",
                        ReplyMarkup = markup
                    }, cancellationToken);

                    break;
                }

            case "show_todos":
                {
                    await _botClient.SendRequest(new SendMessageRequest
                    {
                        ChatId = chatId,
                        Text = "در حال بارگذاری لیست کارها...",
                        ReplyMarkup = new InlineKeyboardMarkup(new[]
                        {
            new[] { InlineKeyboardButton.WithCallbackData("📄 نمایش صفحه اول", "show_todos_page_0") }
        })
                    }, cancellationToken);

                    break;
                }

            case string callback when callback.StartsWith("todo_detail_"):
                {
                    var idPart = callback.Replace("todo_detail_", "");
                    if (!int.TryParse(idPart, out var todoId))
                        break;

                    var item = await _toDoItemRepository.GetToDoItem(todoId, chatId);
                    if (item == null || item.ChatId != chatId)
                    {
                        await _botClient.SendRequest(new SendMessageRequest
                        {
                            ChatId = chatId,
                            Text = "❌ کار مورد نظر یافت نشد یا متعلق به شما نیست."
                        }, cancellationToken);
                        break;
                    }

                    var statusButtons = new InlineKeyboardMarkup(new[]
                                {
                    new[] { InlineKeyboardButton.WithCallbackData("✅ انجام شد", $"mark_done_{todoId}") },
                    new[] { InlineKeyboardButton.WithCallbackData("🕒 در حال انجام", $"mark_inprogress_{todoId}") },
                    new[] { InlineKeyboardButton.WithCallbackData("❌ لغو شد", $"mark_canceled_{todoId}") }
        });

                    await _botClient.SendRequest(new SendMessageRequest
                    {
                        ChatId = chatId,
                        Text = $"*{item.Title}*\n_{item.Description}_\n\nوضعیت فعلی: *{item.Status}*",
                        ParseMode = ParseMode.Markdown,
                        ReplyMarkup = statusButtons
                    }, cancellationToken);

                    break;
                }

            case string cb when cb.StartsWith("mark_done_") ||
                                cb.StartsWith("mark_inprogress_") ||
                                cb.StartsWith("mark_canceled_"):
                {
                    var parts = cb.Split('_');          // mark, <status>, <id>
                    var status = parts[1] switch
                    {
                        "done" => ToDoStatus.Done,
                        "inprogress" => ToDoStatus.InProgress,
                        "canceled" => ToDoStatus.Canceled,
                        _ => ToDoStatus.JustMade
                    };

                    int.TryParse(parts[^1], out var todoId);

                    // 3-b  fetch & validate ownership
                    var item = await _toDoItemRepository.GetToDoItem(todoId, chatId);
                    if (item is null)
                    {
                        await _botClient.SendRequest(new SendMessageRequest
                        {
                            ChatId = chatId,
                            Text = "\"❌ موردی پیدا نشد\"."
                        }, cancellationToken);

                    }

                    // 3-c  update
                    await _toDoItemRepository.UpdateToDoItem(item.Id, item.Title, item.Description, (int)status);

                    // 3-d  respond – reuse the same detail view but with fresh status
                    var keyboard = new InlineKeyboardMarkup(new[]
                    {
                        new[] { InlineKeyboardButton.WithCallbackData("✅ انجام شد",      $"mark_done_{todoId}") },
                        new[] { InlineKeyboardButton.WithCallbackData("🕒 در حال انجام",  $"mark_inprogress_{todoId}") },
                        new[] { InlineKeyboardButton.WithCallbackData("❌ لغو شد",        $"mark_canceled_{todoId}") }
                    });

                    await _botClient.SendRequest(new SendMessageRequest
                    {
                        ChatId = chatId,
                        Text = $"*{item.Title}*\n_{item.Description}_\n\nوضعیت فعلی: *{item.Status}*",
                        ParseMode = ParseMode.Markdown,
                        ReplyMarkup = keyboard
                    }, cancellationToken);


                    break;
        }


    }
}
}
