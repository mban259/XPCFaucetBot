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
            if (arg3.VoiceChannel.Guild.Id == EnvManager.XpcJapanId)
            {
                #region for XPC-JP        
                if (!EqualVoiceChannel(arg2.VoiceChannel, arg3.VoiceChannel))
                {
                    ulong textChannelId;
                    if (JsonManager.VoiceChatToTextChannel.TryGetValue(arg3.VoiceChannel.Id, out textChannelId))
                    {
                        var textChannel = _discordSocketClient.GetChannel(textChannelId) as SocketTextChannel;
                        var m = XPCFaucetBot.Utils.JsonManager.VoiceChatJoinMessages[
                            _random.Next(XPCFaucetBot.Utils.JsonManager.VoiceChatJoinMessages.Length)];
                        var message = await textChannel.SendMessageAsync(string.Format(m, arg1.Mention));
                        Debug.Log(
                            $"send {string.Format(m, $"{arg1.Username}:{arg1.Mention}")} in {textChannel.Name}:{textChannel.Id}");
                        Delete(message);
                    }
                }
                #endregion
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
