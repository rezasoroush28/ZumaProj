using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;
using Zuma.Application.Commands;
using Zuma.Application.Commands.Bot;
using Application.Common.Responses;
using Zuma.Application.Commands.Telegram;

namespace Zuma.Application.CommandHandlers.BotHandlers
{
    public class HandleTelegramUpdateCommandHandler : IRequestHandler<HandleTelegramUpdateCommandRequest, CommandResponse<HandleTelegramUpdateDto>>
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IMediator _mediator;

        public HandleTelegramUpdateCommandHandler(
            ITelegramBotClient botClient,
            IMediator mediator)
        {
            _botClient = botClient;
            _mediator = mediator;
        }

        public async Task<CommandResponse<HandleTelegramUpdateDto>> Handle(HandleTelegramUpdateCommandRequest request, CancellationToken cancellationToken)
        {
            var update = request.TelUpdate;

            if (update.Message == null || string.IsNullOrEmpty(update.Message.Text))
                return Unit.Value;

            var chatId = update.Message.Chat.Id;
            var text = update.Message.Text.Trim().ToLower();

            switch (text)
            {
                case "/start":
                    var startResponse = await _mediator.Send(new StartBotCommand
                    {
                        ChatId = chatId,
                        FirstName = update.Message.Chat.FirstName
                    });

                    await _botClient.SendTextMessageAsync(
                        chatId,
                        startResponse.Result?.WelcomeMessage ?? "سلام!",
                        cancellationToken: cancellationToken);
                    break;

                default:
                    await _botClient.SendTextMessageAsync(
                        chatId,
                        "دستور نامشخص است. از /start استفاده کن 😊",
                        cancellationToken: cancellationToken);
                    break;
            }

            return Unit.Value;
        }
    }
}
