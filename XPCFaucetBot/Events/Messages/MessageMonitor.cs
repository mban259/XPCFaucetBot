using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using XPCFaucetBot.Utils;

namespace XPCFaucetBot.Events.Messages
{
    class MessageMonitor
    {
        private readonly DiscordSocketClient _discordSocketClient;

        private const string ReturnText = @"仕事忍satoshiは有人の操作ではなく、自動で動作するプログラム（BOT)です。
XPCに関する質問に関しては、<#447671198566973480>へお願いします。
なお<#447671198566973480>はボランティアの方が回答する場合もありますので、お手数ですが質問の際は「何が起きていて、何で困っているのか」を記載頂けると助かります。";
        internal MessageMonitor(DiscordSocketClient discordSocketClient)
        {
            _discordSocketClient = discordSocketClient;
        }
        internal async Task MessageReceived(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message.Author.Id == _discordSocketClient.CurrentUser.Id) return;
            var context = new CommandContext(_discordSocketClient, message);
            if (context.IsPrivate)
            {
                await message.Author.SendMessageAsync(ReturnText);
                Debug.Log($"receivedDM user:{message.Author.Username} id:{message.Author.Id} text:{message.ToString()}");
                Debug.SaveDM($"{DateTime.Now}: user:{message.Author.Username} id:{message.Author.Id} text:{message.ToString()}");
            }
        }
    }
}
