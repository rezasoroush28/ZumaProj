using Application.Common.Responses;
using MediatR;
using Zuma.Domain.Entities;
using Zuma.Infrastructure.Context;

public class StartBotCommandHandler : IRequestHandler<StartBotCommandRequest, CommandResponse<StartBotDto>>
{
    private readonly ToDoContext _context;

    public StartBotCommandHandler(ToDoContext context)
    {
        _context = context;
    }

    public async Task<CommandResponse<StartBotDto>> Handle(StartBotCommandRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.BotUsers.FirstOrDefaultAsync(u => u.ChatId == request.ChatId, cancellationToken);

        if (user == null)
        {
            var newUser = new BotUser
            {
                ChatId = request.ChatId,
                FirstName = request.FirstName ?? "کاربر",
                JoinedAt = DateTime.UtcNow
            };
            _context.BotUsers.Add(newUser);
            await _context.SaveChangesAsync(cancellationToken);

            return CommandResponse<StartBotDto>.SuccessResponse(new StartBotDto
            {
                IsFirstTime = true,
                WelcomeMessage = "سلام! 🎉 خوش اومدی به ربات."
            });
        }

        return CommandResponse<StartBotDto>.SuccessResponse(new StartBotDto
        {
            IsFirstTime = false,
            WelcomeMessage = $"سلام مجدد {user.FirstName} 😊"
        });
    }
}
