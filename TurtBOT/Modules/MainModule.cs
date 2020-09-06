using Discord.Commands;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TurtBOT.Modules
{
    public class MainModule : ModuleBase<SocketCommandContext>
    {
        public CommandService CommandService { get; set; }
        public BotConfig Config { get; set; }


        [Command("ping")]
        public async Task Ping()
        {
            var watch = new Stopwatch();
            watch.Start();
            var message = await ReplyAsync("pinging");
            watch.Stop();
            await message.ModifyAsync(msg => msg.Content = $"Ping took {watch.ElapsedMilliseconds}ms!");
        }
    }
}
