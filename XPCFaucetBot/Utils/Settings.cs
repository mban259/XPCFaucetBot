using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace XPCFaucetBot.Utils
{
    static class Settings
    {
        public static string DiscordToken;
        public static ulong XpcJapanId;
        public static ulong NotificationChannelId;
        public static ulong AdminId;
        public static string[] Commands;
        public static string[] VoiceChatMessages;
        internal static Dictionary<ulong, ulong> VoiceChatToTextChannel;

        public static void LoadSettings()
        {
            Debug.Log("load settings");

            string sJson = LoadFile("data/settings/settings.json");
            var jobj = JsonConvert.DeserializeObject<JObject>(sJson);

            DiscordToken = (string)jobj["token"];

            XpcJapanId = (ulong)jobj["xpc_jp_id"];


            NotificationChannelId = (ulong)jobj["notification_channel_id"];

            AdminId = (ulong)jobj["admin_id"];

            Commands = ((JArray)jobj["commands"]).Select(j => (string)j).ToArray();

            VoiceChatMessages = ((JArray)jobj["vc_messages"]).Select(j => (string)j).ToArray();

            VoiceChatToTextChannel = new Dictionary<ulong, ulong>();
            var jArray = (JArray) jobj["vc_text"];
            foreach (JToken jToken in jArray)
            {
                VoiceChatToTextChannel[(ulong) jToken["vc"]] = (ulong) jToken["text"];
            }

            Debug.Log("done");
        }

        private static string LoadFile(string path)
        {
            string result;
            using (StreamReader sr = new StreamReader(path))
            {
                result = sr.ReadToEnd();
            }

            return result;
        }

        static Settings()
        {
            LoadSettings();
        }
    }
}
