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
        private object _lockObj = new object();
        private bool _monitoring;

        internal MessageMonitor(DiscordSocketClient discordSocketClient)
        {
            _discordSocketClient = discordSocketClient;
            _signMessageCommandRegex = new Regex(SignCommand);
            _commandService = new CommandService();
            _serviceProvider = new ServiceCollection().BuildServiceProvider();
        }

        internal async Task AddModulesAsync()
        {
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);
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

                if (message.MentionedUsers.Any(u => u.Id == _discordSocketClient.CurrentUser.Id) || message.MentionedRoles.Any(r => r.Members.Any(m => m.Id == _discordSocketClient.CurrentUser.Id)))
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

        private DateTime NextMonday()
        {
            var today = DateTime.Today;
            int diff;
            if (today.DayOfWeek == DayOfWeek.Sunday)
            {
                diff = 1;
            }
            else
            {
                diff = DayOfWeek.Monday - today.DayOfWeek + 7;
            }
            return today.AddDays(diff);
        }

        internal async Task FreeRoomMonitor()
        {
            lock (_lockObj)
            {
                if (_monitoring)
                {
                    Debug.Log("already monitoring");
                    return;
                }

                _monitoring = true;
            }

            Debug.Log("Start FreeRoom Monitor");
            var next = NextMonday();


            while (true)
            {
                if (DateTime.Now >= next)
                {
                    Debug.Log("Check!!");
                    try
                    {
                        var alertChannels = new List<SocketTextChannel>();
                        var archiveChannels = new List<SocketTextChannel>();
                        foreach (var freeRoomId in EnvManager.FreeRoomId)
                        {
                            var category = _discordSocketClient.GetChannel(freeRoomId) as SocketCategoryChannel;
                            foreach (var socketGuildChannel in category.Channels)
                            {
                                if (socketGuildChannel is SocketTextChannel)
                                {
                                    var textChannel = socketGuildChannel as SocketTextChannel;
                                    var state = await GetState(textChannel, next);
                                    switch (state)
                                    {
                                        case State.Alert:
                                            alertChannels.Add(textChannel);
                                            break;
                                        case State.Archive:
                                            archiveChannels.Add(textChannel);
                                            break;
                                    }
                                }

                            }
                        }

                        foreach (var socketTextChannel in alertChannels)
                        {
                            Debug.Log($"alert:{socketTextChannel.Name}");
                            try
                            {
                                await socketTextChannel.SendMessageAsync("先週に書き込みをしたユニークユーザーが5人未満でした");
                            }
                            catch (Exception e)
                            {
                                Debug.Log(e);
                            }
                        }

                        foreach (var socketTextChannel in archiveChannels)
                        {
                            Debug.Log($"alert_archive:{socketTextChannel.Name}");
                            try
                            {
                                await socketTextChannel.SendMessageAsync("先週、先々週に書き込みをしたユニークユーザーが5人未満でした");
                            }
                            catch (Exception e)
                            {
                                Debug.Log(e);
                            }
                        }

                        //foreach (var socketTextChannel in archiveChannels)
                        //{
                        //    Debug.Log($"archive:{socketTextChannel.Name}");
                        //    try
                        //    {                        
                        //        await socketTextChannel.ModifyAsync((p) =>
                        //        {
                        //            p.CategoryId = new Optional<ulong?>(EnvManager.ArchiveId);
                        //        });
                        //    }
                        //    catch (Exception e)
                        //    {
                        //        Debug.Log(e);
                        //    }
                        //}


                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                    }
                    finally
                    {
                        next = next.AddDays(7);
                        Debug.Log($"next:{next}");
                    }
                }
                else
                {
                    await Task.Delay(1 * 1000);
                }
            }
        }

        internal async Task<State> GetState(SocketTextChannel channel, DateTime thisMonday)
        {
            DateTime lastMonday = thisMonday.AddDays(-7);
            DateTime beforeLastMonday = lastMonday.AddDays(-7);
            IMessage[] messages;
            for (int i = 1; true; i++)
            {
                messages = await GetMessage(channel, i * 100);
                if (messages.Length < i * 100)
                {
                    break;
                }

                if (messages.Min(m => m.Timestamp) < beforeLastMonday)
                {
                    break;
                }
            }
            Array.Sort(messages, (a, b) => -a.Timestamp.CompareTo(b.Timestamp));
            HashSet<ulong> lastWeek = new HashSet<ulong>();
            HashSet<ulong> beforeLastWeek = new HashSet<ulong>();
            foreach (var message in messages)
            {
                if (message.Author.IsBot) continue;
                if (lastMonday <= message.Timestamp && message.Timestamp < thisMonday)
                {
                    if (lastWeek.Add(message.Author.Id))
                    {
                        if (lastWeek.Count >= 5) return State.None;
                    }
                }
                if (beforeLastMonday <= message.Timestamp && message.Timestamp < lastMonday)
                {
                    if (beforeLastWeek.Add(message.Author.Id))
                    {
                        if (beforeLastWeek.Count >= 5 && lastWeek.Count < 5) return State.Alert;
                    }
                }
            }

            if (lastWeek.Count < 5 && beforeLastWeek.Count < 5 && channel.CreatedAt < beforeLastMonday)
            {
                return State.Archive;
            }

            if (lastWeek.Count < 5 && channel.CreatedAt < lastMonday)
            {
                return State.Alert;
            }

            return State.None;
        }

        private async Task<IMessage[]> GetMessage(SocketTextChannel channel, int limit)
        {
            var messageArray = await channel.GetMessagesAsync(limit).ToArray();
            int length = messageArray.Sum(m => m.Count);
            var result = new IMessage[length];
            int index = 0;
            foreach (var readOnlyCollection in messageArray)
            {
                foreach (var message in readOnlyCollection)
                {
                    result[index] = message;
                    index++;
                }
            }

            return result;
        }

    }

    enum State
    {
        None,
        Alert,
        Archive
    }
}

