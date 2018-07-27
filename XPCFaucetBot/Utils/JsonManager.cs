using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace XPCFaucetBot.Utils
{
    static class JsonManager
    {
        internal static string[] Commands;
        internal static Dictionary<ulong, ulong> VoiceChatToTextChannel;
        internal static string[] VoiceChatJoinMessages;

        internal static void ReloadVoiceChatJoinMessages()
        {
            string sJson;
            using (StreamReader sr = new StreamReader("Messages/VoiceChatJoinMessages.json"))
            {
                sJson = sr.ReadToEnd();
            }

            VoiceChatJoinMessages = JsonConvert.DeserializeObject<string[]>(sJson);
        }

        internal static void ReloadCommands()
        {
            string sJson;
            using (StreamReader sr = new StreamReader("Commands.json"))
            {
                sJson = sr.ReadToEnd();
            }
            Commands = JsonConvert.DeserializeObject<string[]>(sJson);
        }

        internal static void ReloadVoiceChatToTextChannel()
        {
            string sJson;
            using (StreamReader sr = new StreamReader("Settings/VoiceChat.json"))
            {
                sJson = sr.ReadToEnd();
            }
            VoiceChatToTextChannel = new Dictionary<ulong, ulong>();
            var jArray = JsonConvert.DeserializeObject<JArray>(sJson);
            foreach (var jToken in jArray)
            {
                VoiceChatToTextChannel[(ulong)jToken["vc"]] = (ulong)jToken["text"];
            }
        }

        static JsonManager()
        {
            ReloadCommands();
            ReloadVoiceChatToTextChannel();
            ReloadVoiceChatJoinMessages();
        }
    }
}
