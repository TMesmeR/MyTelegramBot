using MyTelegramBot.Logic;
using MyTelegramBot.Visual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyTelegramBot.Contollers
{
    internal class TextMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        private bool _countCharacters = false;
        private bool _sumNumbers = false;

        public TextMessageController(ITelegramBotClient telegramBotClient)
        {
            _telegramClient = telegramBotClient;
        }
        public async Task Handle(ITelegramBotClient botClient, Message message, CancellationToken ct)
        {
            switch (message.Text)
            {
                case "/start":
                   await MainMenu.SendMainMenu(message.Chat.Id,_telegramClient, cancellationToken: ct);
                    break;
                case "Подсчет символов":
                    _countCharacters = true;
                    _sumNumbers = false;
                    await botClient.SendTextMessageAsync(message.Chat.Id,"Введите текст для подсчета символов" ,cancellationToken: ct);
                    break;
                case "Сумма чисел":
                    _sumNumbers = true;
                    _countCharacters = false;
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Введите числа через пробел для суммирования.", cancellationToken: ct);
                    break;
                default:
                    if (_countCharacters)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"В вашем сообщении {message.Text.Replace(" ","").Length} символов, без учета пробелов", cancellationToken: ct);
                    }
                    else if (_sumNumbers)
                    {
                        await SumNumbers.SumNumb(message,_telegramClient, ct);
                    }
                    break;
            }
        }
    }
}
