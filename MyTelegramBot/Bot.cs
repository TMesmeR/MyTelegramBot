using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Polling;
using MyTelegramBot.Contollers;

namespace MyTelegramBot
{
    internal class Bot:BackgroundService
    {
        private ITelegramBotClient telegramBotClient;
        private TextMessageController _textMessageController;
        
        private DefaultMessageContoller _defaultMessageContoller;

        public Bot (ITelegramBotClient telegramBotClient, TextMessageController textMessageController, DefaultMessageContoller defaultMessageContoller)
        {
            this.telegramBotClient = telegramBotClient;
            _textMessageController = textMessageController;
            _defaultMessageContoller = defaultMessageContoller;
          

        }



        protected override async Task ExecuteAsync (CancellationToken stoppingToken)
        {
            telegramBotClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync,
                new ReceiverOptions() { AllowedUpdates = { } }, cancellationToken: stoppingToken);
            Console.WriteLine("Бот запущен");
        }

        async Task HandleUpdateAsync(ITelegramBotClient botClient,
            Update update,
            CancellationToken cancellationToken)
        {
           

            if (update.Type == UpdateType.Message)
            {
                switch (update.Message.Type)
                {
                    case MessageType.Text:
                        await _textMessageController.Handle(botClient, update.Message, cancellationToken);
                        return;
                    default:
                        await _defaultMessageContoller.Handle(update.Message, cancellationToken);
                        return;
                }
            }

        }

        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]" +
                $"\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            Console.WriteLine("Ожидаем 10 секунд перед повторным подключением");
            Thread.Sleep(10000);

            return Task.CompletedTask;
        }
    }
}
