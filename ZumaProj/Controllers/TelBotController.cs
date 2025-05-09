using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Zuma.Application.Commands.Telegram;


[ApiController]
[Route("bot")]
public class TelBotController : ControllerBase
{
    private readonly ITelegramBotClient _botClient;
    private readonly ISender _mediator;
    public TelBotController(ITelegramBotClient botClient, ISender mediator)
    {
        _botClient = botClient;
        _mediator = mediator;
    }

    [HttpPost("post")]
    public async Task<IActionResult> Post([FromBody] Update update, CancellationToken cancellationToken)
    {
        var command = new HandleTelegramUpdateCommandRequest { TelUpdate = update };
        await _mediator.Send(command, cancellationToken);
        return Ok();

    }

    //[HttpPost("post")]
    //public async Task<IActionResult> Post([FromBody] Update update, CancellationToken cancellationToken)
    //{
    //    Console.WriteLine("✅ Got update with ID: " + update.Id);
    //    Console.WriteLine("Message text: " + update.Message?.Text);

    //    await Task.CompletedTask;
    //    return Ok();
    //}






}
