using Application.Common.Responses;
using MediatR;
using Zuma.Domain.Entities;
using Zuma.Domain.Interfaces.IRepositories;

public class StartBotCommandHandler : IRequestHandler<StartBotCommandRequest, CommandResponse<StartBotDto>>
{
    private readonly IBotUserRepository _botUserRepository;

    public StartBotCommandHandler(IBotUserRepository botUserRepository)
    {
        _botUserRepository = botUserRepository;
    }

    public async Task<CommandResponse<StartBotDto>> Handle(StartBotCommandRequest request, CancellationToken cancellationToken)
    {
        var user = await _botUserRepository.GetBotUserByChatId(request.ChatId);
        
        if (user == null)
        {
            var newUser = new BotUser
            {
                ChatId = request.ChatId,
                FirstName = request.Username ?? "کاربر",
                JoinedAt = DateTime.UtcNow
            };

            _botUserRepository.AddAsync(newUser);


            return CommandResponse<StartBotDto>.Ok(new StartBotDto
            {
                WelcomeMessage = "سلام! 🎉 خوش اومدی به ربات."
            });
        }

        return CommandResponse<StartBotDto>.Ok(new StartBotDto
        {
            WelcomeMessage = $"سلام مجدد {user.FirstName } 😊"
        });
    }
}
