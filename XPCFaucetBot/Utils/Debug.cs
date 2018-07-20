using System;
using System.Collections.Generic;
using System.Text;
using Discord;

namespace XPCFaucetBot.Utils
{
    static class Debug
    {
        internal static void Log(object o)
        {
            Console.WriteLine($"{DateTime.Now}:{o}");
        }
    }
}
