using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace TurtBOT.CommandModules
{
    public class MainModule : ModuleBase
    {
        public CommandService CommandService { get; set; }

        [Command("ping")]
        [Summary("Pings the bot")]
        public async Task Ping()
        {
            var watch = new Stopwatch();
            watch.Start();
            var task = await ReplyAsync("pinging");
            watch.Stop();
            await task.ModifyAsync(msg => msg.Content = $"Ping with {watch.ElapsedMilliseconds}ms!");
        }

        [Command("help")]
        [Summary("Gets help")]
        public async Task Help([Name("Command Name")][Summary("Get help for")]string cmdname = null)
        {
            var embedb = new EmbedBuilder();
            if (cmdname == null)
            {
                string help = default;
                foreach (var c in CommandService.Commands)
                {
                    help += c.Name + "\n";
                }
                embedb.WithTitle("Help")
                    .WithDescription(help);
                await ReplyAsync(embed: embedb.Build());
            }
            else
            {
                var cmd = CommandService.Commands.First(c => c.Name == cmdname);
                embedb.WithTitle(cmd.Name);
                embedb.WithDescription(cmd.Summary);
                await ReplyAsync(embed: embedb.Build());
            }
        }
    }
}