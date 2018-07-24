using System;
using System.IO;
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

        private readonly DiscordSocketClient _discordSocketClient;
        private readonly VoiceChatMonitor _voiceChatMonitor;
        private readonly MessageMonitor _messageMonitor;

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
            _discordSocketClient.MessageReceived += _messageMonitor.MessageReceived;
            _discordSocketClient.Ready += Ready;
        }

        private async Task Ready()
        {
            Debug.Log($"{_discordSocketClient.CurrentUser.Username}:{_discordSocketClient.CurrentUser.Id}");
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
            _messageMonitor = new MessageMonitor(_discordSocketClient);
        }
    }
}
