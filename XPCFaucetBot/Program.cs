using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using Discord.WebSocket;
using XPCFaucetBot.Events.VoiceChat;
using XPCFaucetBot.Utils;

namespace XPCFaucetBot
{
    class Program
    {
        
        private readonly DiscordSocketClient _discordSocketClient;
        private readonly VoiceChatMonitor _voiceChatMonitor;

        static void Main(string[] args)
        {

            var program = new Program();
            program.Awake();
            program.MainAsync().GetAwaiter().GetResult();
        }

        internal void Awake()
        {
            _discordSocketClient.UserVoiceStateUpdated += _voiceChatMonitor.UserVoiceStateUpdated;
            _discordSocketClient.Log += Log;
        }

        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        internal async Task MainAsync()
        {
            await _discordSocketClient.LoginAsync(TokenType.Bot, EnvManager.DiscordToken);
            await _discordSocketClient.StartAsync();
            await Task.Delay(-1);
        }

        internal Program()
        {
            _discordSocketClient = new DiscordSocketClient();
            _voiceChatMonitor = new VoiceChatMonitor(_discordSocketClient);
        }
    }
}
