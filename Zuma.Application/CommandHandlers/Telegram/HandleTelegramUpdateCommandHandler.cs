using MediatR;
using Telegram.Bot;
using Application.Common.Responses;
using Zuma.Application.Commands.Telegram;
using Zuma.Application.Interfaces;

namespace Zuma.Application.CommandHandlers.BotHandlers
{
    public class HandleTelegramUpdateCommandHandler : IRequestHandler<HandleTelegramUpdateCommandRequest, CommandResponse<HandleTelegramUpdateDto>>
    {
        private readonly IMediator _mediator;
        private readonly ITelegramMessageService _messageService;

        public HandleTelegramUpdateCommandHandler(
            IMediator mediator,
            ITelegramMessageService messageService)
        {
            _mediator = mediator;
            _messageService = messageService;
        }

        public async Task<CommandResponse<HandleTelegramUpdateDto>> Handle(HandleTelegramUpdateCommandRequest request, CancellationToken cancellationToken)
        {
            var update = request.TelUpdate;

            if (update.Message == null || string.IsNullOrEmpty(update.Message.Text))

                return new CommandResponse<HandleTelegramUpdateDto>
                {
                    Success = true,
                    Result = null,
                    Message = "start the bot"
                };


            var chatId = update.Message.Chat.Id;
            var text = update.Message.Text.Trim().ToLower();

            switch (text)
            {
                case "/start":
                    var startResponse = await _mediator.Send(new StartBotCommandRequest
                    {
                        ChatId = chatId,
                        Username = update.Message.Chat.Username
                    });

                    await _messageService.SendMessageAsync(chatId, startResponse.Result.WelcomeMessage ,cancellationToken);

                    //await _botClient.SendTextMessageAsync(
                    //    chatId,
                    //    startResponse.Result?.WelcomeMessage ?? "سلام!",
                    //    cancellationToken: cancellationToken);
                    break;

                default:

                    await _messageService.SendMessageAsync(chatId, "دستور نامشخص است. از /start استفاده کن 😊", cancellationToken);
                    //await _botClient.SendTextMessageAsync(
                    //    chatId,
                    //    "دستور نامشخص است. از /start استفاده کن 😊",
                    //    cancellationToken: cancellationToken);
                    break;
            }

            return new CommandResponse<HandleTelegramUpdateDto> { Success = true,};
        }
    }
}
