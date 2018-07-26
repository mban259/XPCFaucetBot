using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using XPCFaucetBot.Utils;

namespace XPCFaucetBot.Events.Messages
{
    public class CommandModule : ModuleBase
    {
        [Command(CommandString.Help)]
        public async Task Help()
        {
            await Context.Channel.SendMessageAsync($"{Context.User.Mention}\n```asciidoc\n{Utils.Messages.MasterHelp}\n```");
        }

        [Command(CommandString.Help)]
        public async Task Help(string command)
        {
            if (!CommandString.Commands.Any(s => s == command)) return;
            await Context.Channel.SendMessageAsync($"{Context.User.Mention}\n```asciidoc\n{Utils.Messages.HelpMessages[command]}\n```");
        }
    }
}
