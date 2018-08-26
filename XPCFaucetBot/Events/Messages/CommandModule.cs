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
        [Command("ping")]
        public async Task Ping()
        {
            await Context.Channel.SendMessageAsync("pong");
        }

        [Command(CommandString.Help)]
        public async Task Help()
        {
            await Context.Channel.SendMessageAsync($"{Context.User.Mention}\n```asciidoc\n{Utils.Messages.MasterHelp}\n```");
        }

        [Command(CommandString.Help)]
        public async Task Help(string command)
        {
            if (!JsonManager.Commands.Any(s => s == command)) return;
            await Context.Channel.SendMessageAsync($"{Context.User.Mention}\n```asciidoc\n{Utils.Messages.HelpMessages[command]}\n```");
        }

        #region for python irasshai
        [Command("setvoice")]
        public Task SetVoice(string name)
        {
            return Task.CompletedTask;
        }

        [Command("setpitch")]
        public Task SetPitch(string value)
        {
            return Task.CompletedTask;
        }

        [Command("setrange")]
        public Task SetRange(string value)
        {
            return Task.CompletedTask;
        }

        [Command("setrate")]
        public Task SetRate(string value)
        {
            return Task.CompletedTask;
        }

        [Command("setvolume")]
        public Task SetVolume(string value)
        {
            return Task.CompletedTask;
        }

        [Command("settext")]
        public Task SetText(params string[] text)
        {
            return Task.CompletedTask;
        }

        [Command("setxml")]
        public Task SetVoice(params string[] text)
        {
            return Task.CompletedTask;
        }

        [Command("reset")]
        public Task Reset()
        {
            return Task.CompletedTask;
        }

        [Command("getvcsetting")]
        public Task GetVCSetting()
        {
            return Task.CompletedTask;
        }

        [Command("say")]
        public Task Say(params string[] text)
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}
