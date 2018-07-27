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
        private const string SignCommand = "^!xp *message sign";
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


            if (context.IsPrivate)
            {
                #region forDirectMessage

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

                await message.Author.SendMessageAsync(Utils.Messages.DirectMessageReturnText);
                Debug.Log(
                    $"receivedDM user:{message.Author.Username} id:{message.Author.Id} text:{message.ToString()}");
                Debug.SaveDM(
                    $"{DateTime.Now}: user:{message.Author.Username} id:{message.Author.Id} text:{message.ToString()}");
                return;

                #endregion
                return;
            }
            var match = _signMessageCommandRegex.Match(message.ToString());
            if (context.Guild.Id == EnvManager.XpcJapanId)
            {
                #region forXPC-JP
                if (match.Success)
                {
                    Debug.Log($"signMessage user:{message.Author.Username}:{message.Author.Id} message:{message.ToString()}:{message.Id}");
                    await message.Channel.SendMessageAsync($"{message.Author.Mention} {Utils.Messages.SignMessageReturnText}");
                    return;
                }

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

                if (message.MentionedUsers.Any(u => u.Id == _discordSocketClient.CurrentUser.Id))
                {
                    Debug.Log($"receiveMention user:{message.Author.Username}:{message.Author.Id} message:{message.ToString()}:{message.Id}");
                    var xpc = _discordSocketClient.GetGuild(EnvManager.XpcJapanId);
                    var notificationChannel = xpc.GetTextChannel(EnvManager.NotificationChannelId);
                    await notificationChannel.SendMessageAsync(
                        $"{xpc.EveryoneRole.Mention} {message.Author.Mention}さんからメンションきたよ\nmessage:{message.ToString()}\nlink:https://discordapp.com/channels/{xpc.Id}/{message.Channel.Id}/{message.Id}");
                    return;
                }
                #endregion
                return;
            }
        }

    }
}
