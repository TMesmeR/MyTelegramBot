using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyTelegramBot.Visual
{
    internal class MainMenu
    {

        internal static async Task SendMainMenu(long chatId,ITelegramBotClient _telegramClient, CancellationToken cancellationToken)
        {
            var button = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton("Подсчет символов"),
                new KeyboardButton("Сумма чисел")
            })
            {

                ResizeKeyboard = true,
                OneTimeKeyboard = false
            };


            await _telegramClient.SendTextMessageAsync(chatId, "Выберите действие", replyMarkup: button, cancellationToken: cancellationToken);
        }
    }
}
