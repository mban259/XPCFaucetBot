using System;
using System.Collections.Generic;
using System.Text;

namespace XPCFaucetBot.Utils
{
    class CommandString
    {
        internal const string Prefix = "./satoshi ";
        internal const string Help = "help";
        internal static readonly string[] Commands = { "setvoice", "setpitch", "setrange", "setrate", "setvolume", "settext", "setxml", "help" };
    }
}
