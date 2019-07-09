using System;
using System.Linq;
using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using XPCFaucetBot.Utils;

namespace XPCFaucetBot.Events.Messages
{
    class MessageMonitor
    {
        private readonly DiscordSocketClient client;
        private readonly IServiceProvider provider;
        private readonly CommandService service;
        public MessageMonitor(DiscordSocketClient discordSocketClient)
        {
            client = discordSocketClient;
            service = new CommandService();
            provider = new ServiceCollection().BuildServiceProvider();
        }

        public async Task AddModulesAsync()
        {
            await service.AddModulesAsync(Assembly.GetEntryAssembly(), provider);
        }

        public async Task MessageReceived(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;

            // 自分自身ならreturn
            if (message.Author.Id == client.CurrentUser.Id)
                return;

            var context = new CommandContext(client, message);

            if (context.IsPrivate)
            {
                //DM
                await DirectMessageReceived(message, context);
                return;
            }
            else
            {
                // サーバー
                await GuildMessageReceived(message, context);
                return;
            }
        }

        private async Task DirectMessageReceived(SocketUserMessage message, CommandContext context)
        {
            int argPos = 0;
            if (message.HasStringPrefix(CommandString.Prefix, ref argPos))
            {
                var result = await service.ExecuteAsync(context, argPos, provider);
                if (result.IsSuccess)
                {
                    Debug.Log("success");
                }
                else
                {
                    Debug.Log(result.ErrorReason);
                }

            }
            WriteDirectMessage(message);
            await DirectMessageReceived(message);
        }

        private async Task GuildMessageReceived(SocketUserMessage message, CommandContext context)
        {
            if (message.MentionedUsers.Any(u => u.Id == client.CurrentUser.Id) || message.MentionedRoles.Any(r => r.Members.Any(u => u.Id == client.CurrentUser.Id)))
            {
                await MentionReceived(message, context.Guild.Id, context.Channel.Id);
                WriteGuildMessage(message, context);
            }
            if (context.Guild.Id == Settings.XpcJapanId)
            {
                #region XPC-JP
                int argPos = 0;
                if (message.HasStringPrefix(CommandString.Prefix, ref argPos))
                {
                    WriteGuildMessage(message, context);
                    var result = await service.ExecuteAsync(context, argPos, provider);
                    if (result.IsSuccess)
                    {
                        Debug.Log("success");
                    }
                    else
                    {
                        Debug.Log(result.ErrorReason);
                    }
                }
                #endregion
            }
        }

        private void WriteGuildMessage(SocketUserMessage message, CommandContext context)
        {
            Debug.Log("Guild Message\n" +
                    $"guild:{context.Guild.Id}:{context.Guild.Name}\n" +
                      $"channel:{context.Channel.Id}:{context.Channel.Name}\n" +
                      $"author:{message.Author.Id}:{message.Author.Username}\n" +
                      $"id:{message.Id}\n" +
                      $"text:{message.ToString()}");

        }

        private void WriteDirectMessage(SocketUserMessage message)
        {

            Debug.Log("Direct Message\n" +
                      $"author:{message.Author.Id}:{message.Author.Username}\n" +
                      $"id:{message.Id}\n" +
                      $"text:{message.ToString()}");
        }

        private async Task DirectMessageReceived(SocketUserMessage message)
        {
            var xpcJp = client.GetGuild(Settings.XpcJapanId);
            var notificationChannel = xpcJp.GetChannel(Settings.NotificationChannelId) as SocketTextChannel;
            await notificationChannel.SendMessageAsync(
                $"{xpcJp.EveryoneRole.Mention} {message.Author.Mention}さんからDMきたよ\n" +
                $"text:{message.ToString()}");
            await message.Author.SendMessageAsync(Utils.Messages.AnswerToDM);
        }

        private async Task MentionReceived(SocketUserMessage message, ulong guildId, ulong channelId)
        {
            var xpcJp = client.GetGuild(Settings.XpcJapanId);
            var notificationChannel = xpcJp.GetChannel(Settings.NotificationChannelId) as SocketTextChannel;
            await notificationChannel.SendMessageAsync($"{xpcJp.EveryoneRole.Mention} {message.Author.Mention}さんからメンションきたよ\n" +
                                                       $"text:{message.ToString()}\n" +
                                                       $"link:https://discordapp.com/channels/{guildId}/{channelId}/{message.Id}");
        }
    }

}

