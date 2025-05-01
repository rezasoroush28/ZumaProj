using Application.Common.Responses;
using MediatR;

public class StartBotCommandRequest : IRequest<CommandResponse<StartBotDto>>
{
    public long ChatId { get; set; }
    public string? FirstName { get; set; }
}

public class StartBotDto
{
    public bool IsFirstTime { get; set; }
    public string WelcomeMessage { get; set; } = string.Empty;
}