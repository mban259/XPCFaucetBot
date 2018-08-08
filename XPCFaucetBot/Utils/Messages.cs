using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;

namespace XPCFaucetBot.Utils
{
    static class Messages
    {
        internal static string DirectMessageReturnText;
        internal static string SignMessageReturnText;

        internal static Dictionary<string, string> HelpMessages;
        internal static string MasterHelp;

        internal static void ReloadMessages()
        {
            using (StreamReader sr = new StreamReader("Data/Messages/DirectMessageReturn.txt"))
            {
                DirectMessageReturnText = sr.ReadToEnd();
            }

            using (StreamReader sr = new StreamReader("Data/Messages/SignMessageReturn.txt"))
            {
                SignMessageReturnText = sr.ReadToEnd();
            }
        }

        internal static void ReloadHelp()
        {
            HelpMessages = new Dictionary<string, string>();
            Debug.Log("loadhelp");
            foreach (var command in JsonManager.Commands)
            {
                using (StreamReader sr = new StreamReader($"Data/Help/{command}.txt", Encoding.UTF8))
                {
                    Debug.Log($"load {command}.txt");
                    HelpMessages[command] = sr.ReadToEnd();
                }
            }

            using (StreamReader sr = new StreamReader("Data/Help/helplist.txt", Encoding.UTF8))
            {
                Debug.Log($"load helplist.txt");
                MasterHelp = sr.ReadToEnd();
            }
        }

        static Messages()
        {
            try
            {
                ReloadHelp();
                ReloadMessages();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
