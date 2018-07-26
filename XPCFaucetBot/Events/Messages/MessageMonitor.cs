using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using XPCFaucetBot.Utils;

namespace XPCFaucetBot.Events.Messages
{
    class MessageMonitor
    {
        private readonly DiscordSocketClient _discordSocketClient;
        private const string SignCommand = "^!(?<name>(xp)) *message sign";
        private readonly Regex _signMessageCommandRegex;
        private readonly IServiceProvider _serviceProvider;
        private readonly CommandService _commandService;
        internal MessageMonitor(DiscordSocketClient discordSocketClient)
        {
            _discordSocketClient = discordSocketClient;
            _signMessageCommandRegex = new Regex(SignCommand);
            _commandService = new CommandService();
            _serviceProvider = new ServiceCollection().BuildServiceProvider();
        }

        internal async Task AddModulesAsync()
        {
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        internal async Task MessageReceived(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message.Author.Id == _discordSocketClient.CurrentUser.Id) return;

            var context = new CommandContext(_discordSocketClient, message);
            int argPos = 0;
            if (message.HasStringPrefix(CommandString.Prefix, ref argPos))
            {
                var result = await _commandService.ExecuteAsync(context, argPos, _serviceProvider);
                if (result.IsSuccess)
                {
                    Debug.Log("success");
                }
                else
                {
                    Debug.Log(result.ErrorReason);
                }

                return;
            }

            if (context.IsPrivate)
            {
                #region forDirectMessage

                await message.Author.SendMessageAsync(Utils.Messages.DirectMessageReturnText);
                Debug.Log(
                    $"receivedDM user:{message.Author.Username} id:{message.Author.Id} text:{message.ToString()}");
                Debug.SaveDM(
                    $"{DateTime.Now}: user:{message.Author.Username} id:{message.Author.Id} text:{message.ToString()}");
                return;

                #endregion
            }
            var match = _signMessageCommandRegex.Match(message.ToString());
            if (context.Guild.Id == EnvManager.XpcJapanId)
            {
                #region forXPC-JP
                if (match.Success)
                {
                    var currencyName = match.Groups["name"].Value;
                    Debug.Log(
                        $"signMessage user:{message.Author.Username}:{message.Author.Id} message:{message.ToString()}:{message.Id}");
                    await message.Channel.SendMessageAsync(string.Format(Utils.Messages.SignMessageReturnText,
                        message.Author.Mention, currencyName));
                    return;
                }
                #endregion
            }
        }

    }
}
