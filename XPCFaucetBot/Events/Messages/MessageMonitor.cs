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
        private const string SignCommand = "^!(?<name>(xp)) *message sign";
        private readonly Regex _signMessageCommandRegex;
        internal MessageMonitor(DiscordSocketClient discordSocketClient)
        {
            _discordSocketClient = discordSocketClient;
            _signMessageCommandRegex = new Regex(SignCommand);
        }
        internal async Task MessageReceived(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message.Author.Id == _discordSocketClient.CurrentUser.Id) return;

            var context = new CommandContext(_discordSocketClient, message);
            int argPos = 0;
            if (message.HasStringPrefix("./satoshi", ref argPos)) return;

            if (context.IsPrivate)
            {
                await message.Author.SendMessageAsync(Utils.Messages.DirectMessageReturnText);
                Debug.Log($"receivedDM user:{message.Author.Username} id:{message.Author.Id} text:{message.ToString()}");
                Debug.SaveDM($"{DateTime.Now}: user:{message.Author.Username} id:{message.Author.Id} text:{message.ToString()}");
                return;
            }

            var match = _signMessageCommandRegex.Match(message.ToString());
            if (match.Success)
            {
                var currencyName = match.Groups["name"].Value;
                Debug.Log($"signMessage user:{message.Author.Username}:{message.Author.Id} message:{message.ToString()}:{message.Id}");
                await message.Channel.SendMessageAsync(string.Format(Utils.Messages.SignMessageReturnText, message.Author.Mention, currencyName));
                return;
            }
        }
    }
}