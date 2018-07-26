using System;
using System.Collections.Generic;
using System.Text;
using DotNetEnv;

namespace XPCFaucetBot.Utils
{
    static class EnvManager
    {
        internal static readonly string DiscordToken;
        internal const ulong XpcJapanId = 470484687727362069;
        static EnvManager()
        {
            Env.Load();
            DiscordToken = Env.GetString("DISCORD_TOKEN");
        }
    }
}
