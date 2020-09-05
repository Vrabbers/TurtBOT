using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtBOT.Modules
{
    public class MainModule : ModuleBase<SocketCommandContext>
    {
        public CommandService CommandService { get; set; }
        public BotConfig Config { get; set; }


        [Command("ping")]
        public async Task PingCommand()
        {
            await ReplyAsync(Config.BotPrefix + "PPPPPPPING!!!!!!!!!!!!!!!!!!!!!");
            await ReplyAsync(string.Join(" ", CommandService.Commands.Select(x => x.Name)));
        }
    }
}
