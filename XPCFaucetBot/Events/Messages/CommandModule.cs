using Discord.Commands;
using System.Linq;
using System.Threading.Tasks;
using XPCFaucetBot.Utils;

namespace XPCFaucetBot.Events.Messages
{
    public class CommandModule : ModuleBase
    {
        [Command(CommandString.Ping)]
        public async Task Ping()
        {
            await Context.Channel.SendMessageAsync("pong");
        }

        [Command(CommandString.Help)]
        public async Task Help()
        {
            await Context.Channel.SendMessageAsync($"{Context.User.Mention}\n```asciidoc\n{Utils.Messages.HelpList}\n```");
        }

        [Command(CommandString.Help)]
        public async Task Help(string command)
        {
            if (!Settings.Commands.Any(s => s == command)) return;
            await Context.Channel.SendMessageAsync($"{Context.User.Mention}\n```asciidoc\n{Utils.Messages.HelpMessages[command]}\n```");
        }

        [Command(CommandString.WishList)]
        public async Task WithList()
        {
            await Context.Channel.SendMessageAsync(Utils.Messages.WishList);
        }

        [Command(CommandString.Reload)]
        public async Task Reload()
        {

            if (Context.User.Id == Settings.AdminId)
            {
                await Context.Channel.SendMessageAsync("りろーど！");
                Utils.Messages.ReloadHelp();
                Utils.Messages.ReloadMessages();
                Settings.LoadSettings();
                await Context.Channel.SendMessageAsync("おわり!");
            }
        }
    }
}
