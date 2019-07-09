using System.Collections.Generic;
using System.IO;
using System.Text;

namespace XPCFaucetBot.Utils
{
    static class Messages
    {
        public static string AnswerToDM;
        public static Dictionary<string, string> HelpMessages;
        public static string HelpList;
        public static string WishList;
        internal static void ReloadMessages()
        {
            Debug.Log("load messages");
            using (StreamReader sr = new StreamReader("data/messages/answertodm.txt"))
            {
                AnswerToDM = sr.ReadToEnd();
            }

            using (StreamReader sr = new StreamReader("data/messages/wishlist.txt"))
            {
                WishList = sr.ReadToEnd();
            }
            Debug.Log("done");
        }

        internal static void ReloadHelp()
        {
            HelpMessages = new Dictionary<string, string>();
            Debug.Log("loadhelp");
            foreach (var command in Settings.Commands)
            {
                using (StreamReader sr = new StreamReader($"data/help/{command}.txt", Encoding.UTF8))
                {
                    Debug.Log($"load {command}.txt");
                    HelpMessages[command] = sr.ReadToEnd();
                }
            }

            using (StreamReader sr = new StreamReader("data/help/helplist.txt", Encoding.UTF8))
            {
                Debug.Log($"load helplist.txt");
                HelpList = sr.ReadToEnd();
            }
            Debug.Log("done");
        }

        static Messages()
        {
            ReloadHelp();
            ReloadMessages();
        }
    }
}
