using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
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

        private bool SameVoiceChannel(SocketVoiceChannel x, SocketVoiceChannel y)
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
            try
            {
                // 自分自身ならreturn
                if (arg1.Id == _discordSocketClient.CurrentUser.Id)
                    return;
                // arg3のVoiceChannelがnull = VCから出た
                // ならreturn
                if (arg3.VoiceChannel == null)
                    return;
                if (arg3.VoiceChannel.Guild.Id == Settings.XpcJapanId)
                {
                    #region for XPC-JP        
                    if (!SameVoiceChannel(arg2.VoiceChannel, arg3.VoiceChannel))
                    {
                        await EnteredVC(arg1, arg3.VoiceChannel);
                    }
                    #endregion
                }

            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }
        }

        private async Task EnteredVC(SocketUser user, SocketVoiceChannel voiceChannel)
        {
            ulong textChannelId;
            if (Settings.VoiceChatToTextChannel.TryGetValue(voiceChannel.Id, out textChannelId))
            {
                var textChannel = _discordSocketClient.GetChannel(textChannelId) as SocketTextChannel;
                var m = XPCFaucetBot.Utils.Settings.VoiceChatMessages[
                    _random.Next(XPCFaucetBot.Utils.Settings.VoiceChatMessages.Length)];
                var message = await textChannel.SendMessageAsync(string.Format(m, user.Mention));
                Debug.Log(
                    $"send {string.Format(m, $"{user.Username}:{user.Mention}")} in {textChannel.Name}:{textChannel.Id}");
                var res = Task.Run(() => Delete(message));
            }
        }

        internal async Task Delete(IUserMessage message)
        {
            await Task.Delay(20 * 1000);
            await message.DeleteAsync();
            Debug.Log($"delete {message.ToString()} in {message.Channel}");
        }

    }
}
