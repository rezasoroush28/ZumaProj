using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
    private readonly IMemoryCache _memoryCache;
    public TelBotController(ITelegramBotClient botClient, ISender mediator, IMemoryCache memoryCache)
    {
        _botClient = botClient;
        _mediator = mediator;
        _memoryCache = memoryCache;
    }

    [HttpPost("post")]
    public async Task<IActionResult> Post([FromBody] Update update, CancellationToken cancellationToken)
    {
        var updateId = update.Id;
        var key = $"handled-update-{updateId}";

        if (_memoryCache.TryGetValue(key, out _))
        {
            return Ok();
        }

        _memoryCache.Set(key, true, TimeSpan.FromSeconds(40));
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
