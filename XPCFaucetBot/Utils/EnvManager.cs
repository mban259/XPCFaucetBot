using System;
using System.Collections.Generic;
using System.Text;
using DotNetEnv;

namespace XPCFaucetBot.Utils
{
    static class EnvManager
    {
        internal static readonly string DiscordToken;
        internal static readonly ulong XpcJapanId;
        internal static readonly ulong NotificationChannelId;
        static EnvManager()
        {
            Env.Load();
            DiscordToken = Env.GetString("DISCORD_TOKEN");
            XpcJapanId = ulong.Parse(Env.GetString("XPC_JP_ID"));
            NotificationChannelId = ulong.Parse(Env.GetString("NOTIFICATION_CHANNEL_ID"));
        }
    }
}
