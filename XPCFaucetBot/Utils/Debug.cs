using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Discord;

namespace XPCFaucetBot.Utils
{
    static class Debug
    {
        private static readonly StreamWriter LogWriter;
        private static readonly StreamWriter ReceiveDMWriter;
        internal static void Log(object o)
        {
            Console.WriteLine($"{DateTime.Now}:{o}");
            LogWriter.WriteLine($"{DateTime.Now}:{o}");
        }

        internal static void SaveDM(string s)
        {
            ReceiveDMWriter.WriteLine(s);
        }

        static Debug()
        {
            LogWriter = new StreamWriter("Data/debug.log", true);
            LogWriter.AutoFlush = true;
            ReceiveDMWriter = new StreamWriter("Data/receivedDM.log", true);
            ReceiveDMWriter.AutoFlush = true;
        }
    }
}
