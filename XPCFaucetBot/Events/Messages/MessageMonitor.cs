using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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
        private const string SignCommand = "^!(xp) *message sign .+$";
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
                await message.Author.SendMessageAsync(Utils.Messages.DirectMessageReturnText);
                Debug.Log($"receivedDM user:{message.Author.Username} id:{message.Author.Id} text:{message.ToString()}");
                Debug.SaveDM($"{DateTime.Now}: user:{message.Author.Username} id:{message.Author.Id} text:{message.ToString()}");
                return;
            }

            var r = new Regex(SignCommand);
            if (r.Match(message.ToString()).Success)
            {
                await message.Channel.SendMessageAsync(string.Format(Utils.Messages.SignMessageReturnText, message.Author.Mention));
                return;
            }
        }
    }
}