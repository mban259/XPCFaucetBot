using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace XPCFaucetBot.Events.VoiceChat
{
    class VoiceChatMonitor
    {
        internal VoiceChatMonitor(DiscordSocketClient discordSocketClient)
        {
            _discordSocketClient = discordSocketClient;
        }

        private readonly DiscordSocketClient _discordSocketClient;
        private readonly Dictionary<ulong, ulong> _channel = new Dictionary<ulong, ulong>()
        {
            {469520085162262528, 469148120824021003},
            {443362672033923086, 445954353979981855},
            {445954581441282048, 445954384111730690},
            {445948688578117632, 445948513809858560}
        };
        internal async Task UserVoiceStateUpdated(SocketUser arg1, SocketVoiceState arg2, SocketVoiceState arg3)
        {
            if (arg2.VoiceChannel != arg3.VoiceChannel)
            {
                ulong textChannelId;
                if (_channel.TryGetValue(arg3.VoiceChannel.Id, out textChannelId))
                {
                    var textChannel = _discordSocketClient.GetChannel(textChannelId) as SocketTextChannel;
                    var message = await textChannel.SendMessageAsync($"{arg1.Mention}さんいらっしゃい");
                    Delete(message);
                }
            }
        }

        internal async void Delete(IUserMessage message)
        {
            Console.WriteLine(message.ToString());
            await Task.Delay(20 * 1000);
            await message.DeleteAsync();
            Console.WriteLine("delete");
        }

    }
}
