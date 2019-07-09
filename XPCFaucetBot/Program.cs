using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using Discord.WebSocket;
using XPCFaucetBot.Events.Messages;
using XPCFaucetBot.Events.VoiceChat;
using XPCFaucetBot.Utils;

namespace XPCFaucetBot
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }


        public async Task MainAsync()
        {
            var client = new DiscordSocketClient();
            var messageMonitor = new MessageMonitor(client);
            var voiceChatMonitor = new VoiceChatMonitor(client);
            client.Log += Log;
            client.Ready += Ready;
            client.MessageReceived += messageMonitor.MessageReceived;
            client.UserVoiceStateUpdated += voiceChatMonitor.UserVoiceStateUpdated;
            await messageMonitor.AddModulesAsync();
            await client.LoginAsync(TokenType.Bot, Settings.DiscordToken);
            await client.StartAsync();
            await Task.Delay(-1);
        }


        private Task Ready()
        {
            return Task.CompletedTask;
        }

        private Task Log(LogMessage arg)
        {
            Debug.Log(arg.Message);
            return Task.CompletedTask;
        }
    }
}
