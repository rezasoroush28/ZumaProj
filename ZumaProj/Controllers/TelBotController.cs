using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;

[ApiController]
[Route("api/bot/update")]
public class TelBotController : ControllerBase
{
    private readonly ITelegramBotClient _botClient;

    public TelBotController(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update)
    {
        

        // 1. پیام کاربر رو پردازش کن
        // 2. اگر متن داشت مثلا "لیست تسک ها" بود، بری از CQRS یک لیست بیاری
        // 3. جواب رو با botClient.SendTextMessageAsync بفرستی

        return Ok();
    }
}
