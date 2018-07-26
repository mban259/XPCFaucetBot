using System;
using System.Collections.Generic;
using System.Text;
using DotNetEnv;

namespace XPCFaucetBot.Utils
{
    static class EnvManager
    {
        internal static readonly string DiscordToken;
        internal const ulong XpcJapanId = 443362672033923082;
        static EnvManager()
        {
            Env.Load();
            DiscordToken = Env.GetString("DISCORD_TOKEN");
        }
    }
}
