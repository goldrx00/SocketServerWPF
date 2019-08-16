using SocketServerWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServerWPF
{
    class TelegramAPI
    {

        MainWindow mainWindow;
        Telegram.Bot.TelegramBotClient Bot;
        string token;

        public TelegramAPI(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            //token = mainWindow.classINI.iniGet("Token", "Token");
            Bot = new Telegram.Bot.TelegramBotClient(mainWindow.mClassINI.iniGet("Token", "Token"));
        }        

        // init methods...
        public async void telegramAPIAsync()
        {
            //Token Key 를 이용하여 봇을 가져온다.
            //var Bot = new Telegram.Bot.TelegramBotClient("802319057:AAF2_2iNBUEJ0lJlR5TnWrE82OxlFu5pJwY");
            //Bot 에 대한 정보를 가져온다.
            var me = await Bot.GetMeAsync();
            //Bot 의 이름을 출력한다.
            Console.WriteLine("Hello my name is {0}", me.FirstName);
        }

        public void setTelegramEvent()
        {
            Bot.OnMessage += Bot_OnMessage; // 이벤트를 추가해줍니다.
            Bot.StartReceiving(); // 이 함수가 실행이 되어야 사용자로부터 메세지를 받을 수 있습니다.
        }


        private async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e.Message;
            if (message == null || message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return;

            Console.WriteLine(message.Chat.Id + " " + message.Text);
            await Bot.SendTextMessageAsync(message.Chat.Id, message.Text);
        }
    }
}
