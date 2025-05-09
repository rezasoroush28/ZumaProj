using Application.Common.Responses;
using MediatR;

public class StartBotCommandRequest : IRequest<CommandResponse<StartBotDto>>
{
    public long ChatId { get; set; }
    public string Username { get; set; }
}

public class StartBotDto
{
    public string WelcomeMessage { get; set; } = string.Empty;
}