using System.Diagnostics;
using System.Linq;
using System.Text;
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
        public async Task Help(string command = null)
        {
            var embedb = new EmbedBuilder();
            if (command == null)
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
        }
    }
}