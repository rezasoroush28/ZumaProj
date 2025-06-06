using MediatR;
using Telegram.Bot;
using Application.Common.Responses;
using Zuma.Application.Commands.Telegram;
using Telegram.Bot.Types.ReplyMarkups;
using Zuma.Application.Interfaces.Telegram;

namespace Zuma.Application.CommandHandlers.BotHandlers
{
    public class HandleTelegramUpdateCommandHandler : IRequestHandler<HandleTelegramUpdateCommandRequest, CommandResponse<HandleTelegramUpdateDto>>
    {
        private readonly IEnumerable<ITelegramResponseService> _responseServices;

        public HandleTelegramUpdateCommandHandler(IEnumerable<ITelegramResponseService> responseServices)
        {
            _responseServices = responseServices;
        }

        public async Task<CommandResponse<HandleTelegramUpdateDto>> Handle(HandleTelegramUpdateCommandRequest request, CancellationToken cancellationToken)
        {
            var update = request.TelUpdate;

            var chatId = update.Message?.Chat.Id
                ?? update.CallbackQuery?.Message?.Id
                ?? 0;


            var responseService = _responseServices.FirstOrDefault(s => s.CanHandle(request.TelUpdate));
            if (responseService is null)
                return CommandResponse<HandleTelegramUpdateDto>.Fail("Not Supported");

            await responseService.ExecuteAsync(update, cancellationToken);
            return null;

            //if (update.CallbackQuery?.Data == "add_todo")
            //{
            //    var CallBackChatId = update.CallbackQuery.Message.Chat.Id;

            //    await _messageService.SendMessageAsync(CallBackChatId,
            //        "📝 لطفاً متن کار جدیدت رو بنویس و برام بفرست:",
            //        cancellationToken);

            //    await _messageService.AnswerCallbackQueryAsync(update.CallbackQuery.Id, cancellationToken);

            //}

            //else if (update.Message != null && !string.IsNullOrEmpty(update.Message.Text))
            //{
            //    var chatId = update.Message.Chat.Id;
            //    var text = update.Message.Text.Trim().ToLower();

            //    switch (text)
            //    {
            //        case "/start":
            //            var startResponse = await _mediator.Send(new StartBotCommandRequest
            //            {
            //                ChatId = chatId,
            //                Username = update.Message.Chat.Username
            //            });
            //            var keyboard = new InlineKeyboardMarkup(new[]
            //               {
            //                new[] { InlineKeyboardButton.WithCallbackData("➕ افزودن کار جدید", "add_todo") }
            //           });

            //            await _messageService.SendMessageWithKeyboardAsync(chatId, startResponse.Result.WelcomeMessage, keyboard, cancellationToken);

            //            //await _botClient.SendTextMessageAsync(
            //            //    chatId,
            //            //    startResponse.Result?.WelcomeMessage ?? "سلام!",
            //            //    cancellationToken: cancellationToken);
            //            break;

            //        default:

            //            await _messageService.SendMessageAsync(chatId, "دستور نامشخص است. از /start استفاده کن 😊", cancellationToken);
            //            //await _botClient.SendTextMessageAsync(
            //            //    chatId,
            //            //    "دستور نامشخص است. از /start استفاده کن 😊",
            //            //    cancellationToken: cancellationToken);
            //            break;
            //    }
            //}
            //else if (update.Message == null || string.IsNullOrEmpty(update.Message.Text))
            //{
            //    return new CommandResponse<HandleTelegramUpdateDto>
            //    {
            //        Success = true,
            //        Result = null,
            //        Message = "start the bot"
            //    };
            //}
            //return new CommandResponse<HandleTelegramUpdateDto> { Success = true, };










        }
    }
}
