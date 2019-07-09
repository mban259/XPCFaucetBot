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
        internal static void Log(object o)
        {
            Console.WriteLine($"{DateTime.Now}:{o}");
            LogWriter.WriteLine($"{DateTime.Now}:{o}");
        }

        static Debug()
        {
            LogWriter = new StreamWriter($"data/debug.log", true);
            LogWriter.AutoFlush = true;
        }
    }
}
