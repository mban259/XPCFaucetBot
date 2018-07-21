using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using XPCFaucetBot.Utils;

namespace XPCFaucetBot.Events.VoiceChat
{
    class VoiceChatMonitor
    {
        private readonly Random _random;
        private readonly DiscordSocketClient _discordSocketClient;
        internal VoiceChatMonitor(DiscordSocketClient discordSocketClient)
        {
            _discordSocketClient = discordSocketClient;
            _random = new Random();
        }

        private readonly Dictionary<ulong, ulong> _channel = new Dictionary<ulong, ulong>()
        {
            {469520085162262528, 469148120824021003},
            {443362672033923086, 445954353979981855},
            {445954581441282048, 445954384111730690},
            {445948688578117632, 445948513809858560}
        };

        private bool EqualVoiceChannel(SocketVoiceChannel x, SocketVoiceChannel y)
        {
            if (x == null)
            {
                if (y == null) throw new Exception();
                else return false;
            }
            else
            {
                if (y == null) return false;
                else return x.Equals(y);
            }
        }

        internal async Task UserVoiceStateUpdated(SocketUser arg1, SocketVoiceState arg2, SocketVoiceState arg3)
        {
            if (arg1.Id == _discordSocketClient.CurrentUser.Id) return;
            if (!EqualVoiceChannel(arg2.VoiceChannel, arg3.VoiceChannel))
            {
                ulong textChannelId;
                if (_channel.TryGetValue(arg3.VoiceChannel.Id, out textChannelId))
                {
                    var textChannel = _discordSocketClient.GetChannel(textChannelId) as SocketTextChannel;
                    var m = XPCFaucetBot.Utils.Messages.VoiceChatJoinMessages[_random.Next(XPCFaucetBot.Utils.Messages.VoiceChatJoinMessages.Length)];
                    var message = await textChannel.SendMessageAsync(string.Format(m, arg1.Mention));
                    Debug.Log($"send {string.Format(m, $"{arg1.Username}:{arg1.Mention}")} in {textChannel.Name}:{textChannel.Id}");
                    Delete(message);
                }
            }
        }

        internal async void Delete(IUserMessage message)
        {
            await Task.Delay(20 * 1000);
            await message.DeleteAsync();
            Debug.Log($"delete {message.ToString()} in {message.Channel}");
        }

    }
}
