using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Zuma.Application.Interfaces.Telegram;
using Zuma.Domain.Entities;
using Zuma.Domain.Interfaces.IRepositories;
using Telegram.Bot.Requests;
using Microsoft.Extensions.Caching.Memory;
using Zuma.Domain.Enums;

public class PlainTextResponseService : ITelegramResponseService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IUserSessionService _userSessionService;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IBotUserRepository _botUserRepository;
    private readonly IToDoItemRepository _toDoItemRepository;

    public PlainTextResponseService(IUserSessionService userSessionService, ITelegramBotClient telegramBotClient, IBotUserRepository botUserRepository, IMemoryCache memoryCache, IToDoItemRepository toDoItemRepository)
    {
        _userSessionService = userSessionService;
        _telegramBotClient = telegramBotClient;
        _botUserRepository = botUserRepository;
        _memoryCache = memoryCache;
        _toDoItemRepository = toDoItemRepository;
    }

    public bool CanHandle(Update update) => update.Message?.Text != null;

    public async Task ExecuteAsync(Update update, CancellationToken cancellationToken)
    {
        var chatId = update.Message.Chat.Id;
        var messageText = update.Message.Text.Trim().ToLower();

        if (messageText == "/start")
        {
            var user = await _botUserRepository.GetBotUserByChatId(chatId);
            string welcomeMessage;

            if (user == null)
            {
                var newUser = new BotUser
                {
                    ChatId = chatId,
                    FirstName = update.Message.Chat.FirstName ?? "کاربر",
                    JoinedAt = DateTime.UtcNow
                };

                await _botUserRepository.AddAsync(newUser);

                welcomeMessage = $"سلام 👋 خوش اومدی {newUser.Username}!";
            }
            else
            {
                welcomeMessage = $"سلام مجدد {user.FirstName} 😊 خوش برگشتی!";
            }

            var request = new SendMessageRequest
            {
                ChatId = chatId,
                Text = welcomeMessage,
                ParseMode = ParseMode.Markdown,
                ReplyMarkup = new InlineKeyboardMarkup(new[]
                {
                    new[]
                        {
                            InlineKeyboardButton.WithCallbackData("➕ افزودن کار جدید", "add_todo")
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("📋 نمایش کارهای قبلی", "show_todos")
                        }
                })
            };

            await _telegramBotClient.SendRequest(request, cancellationToken);
            _userSessionService.SetUserState(chatId, UserState.Idle);
        }


        else if (_userSessionService.GetUserState(chatId) == UserState.WaitingForAnswer)
        {
            var expectedInput = _userSessionService.GetExpectedInput(chatId);
            _userSessionService.SetUserState(chatId, UserState.Idle);

            switch (expectedInput)
            {
                case ExpectedInputType.TodoTitle:
                    var tempToDoItem = new ToDoItem { Title = messageText };
                    _memoryCache.Set($"todo-temp-{chatId}", tempToDoItem, TimeSpan.FromMinutes(10));
                    _userSessionService.SetExpectedInput(chatId, ExpectedInputType.TodoDescription);
                    var keyboard = new InlineKeyboardMarkup(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("💬 افزودن توضیحات", "add_description"),
                        InlineKeyboardButton.WithCallbackData("✅ ذخیره", "save_todo")
                    }
                });
                    await _telegramBotClient.SendRequest(new SendMessageRequest
                    {
                        ChatId = chatId,
                        Text = "عنوان دریافت شد. حالا چه کاری انجام بدم؟",
                        ReplyMarkup = keyboard
                    }, cancellationToken);

                    break;

                case ExpectedInputType.TodoDescription:
                    if (_memoryCache.TryGetValue<ToDoItem>($"todo-temp-{chatId}", out var cachedItem))
                    {
                        cachedItem.Description = messageText;
                        cachedItem.Status = ToDoStatus.JustMade;
                        _memoryCache.Remove($"todo-temp-{chatId}");
                        await _toDoItemRepository.CreateToDoItem(cachedItem.Title, cachedItem.Description, chatId, (int)ToDoStatus.JustMade);
                        var nesxtKeyboard = new InlineKeyboardMarkup(new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("✅ ذخیره", "save_todo")
                            } 
                        });

                        await _telegramBotClient.SendRequest(new SendMessageRequest
                        {
                            ChatId = chatId,
                            Text = $"✅ کار جدید با موفقیت ثبت شد:\n*{cachedItem.Title}",
                            ParseMode = ParseMode.Markdown,
                            ReplyMarkup = nesxtKeyboard
                        }, cancellationToken);
                    }
                    else
                    {
                        await _telegramBotClient.SendRequest(new SendMessageRequest
                        {
                            ChatId = chatId,
                            Text = "⚠️ خطا در بازیابی اطلاعات. لطفاً دوباره /start رو بزنید."
                        }, cancellationToken);

                    }
                    _userSessionService.ClearExpectedInput(chatId);
                    _userSessionService.SetUserState(chatId, UserState.Idle);

                    break;

                default:
                    var fallbackRequest = new SendMessageRequest
                    {
                        ChatId = chatId,
                        Text = "دستور نامشخصه. لطفاً از /start شروع ک�� 😊"
                    };
                    await _telegramBotClient.SendRequest(fallbackRequest, cancellationToken);
                    break;
            }



        }
    }
}


