using Telegram.Bot.Types;

namespace Zuma.Application.Interfaces.Telegram
{
    public interface ITelegramResponseService
    {
        /// <summary>
        /// مشخص‌کننده‌ی نوع سناریویی که این سرویس پاسخ می‌دهد (مثلاً Text, Callback, Voice...)
        /// </summary>
        bool CanHandle(Update update); // این اضافه می‌شه

        /// <summary>
        /// اجرای منطق پاسخ به آپدیت دریافتی از تلگرام
        /// </summary>
        Task ExecuteAsync(Update update, CancellationToken cancellationToken);
    }
}


