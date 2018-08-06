using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetEnv;

namespace XPCFaucetBot.Utils
{
    static class EnvManager
    {
        internal static readonly string DiscordToken;
        internal static readonly ulong XpcJapanId;
        internal static readonly ulong NotificationChannelId;
        internal static readonly ulong[] FreeRoomId;
        internal static readonly ulong ArchiveId;
        static EnvManager()
        {
            Env.Load("Data/.env");
            DiscordToken = Env.GetString("DISCORD_TOKEN");
            XpcJapanId = ulong.Parse(Env.GetString("XPC_JP_ID"));
            NotificationChannelId = ulong.Parse(Env.GetString("NOTIFICATION_CHANNEL_ID"));
            FreeRoomId = Env.GetString("FREE_ROOM_ID").Split(',').Select(ulong.Parse).ToArray();
            ArchiveId = ulong.Parse(Env.GetString("ARCHIVE_ID"));
        }
    }
}
