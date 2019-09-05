using System.Collections.Generic;
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
        public async Task Help([Summary("Command to get extra help for")]string name = null)
        {
            var embedb = new EmbedBuilder();
            if (name == null)
            {
                string help = default;
                foreach (var c in CommandService.Commands)
                {
                    help += c.Name + "\n";
                }
                embedb.WithTitle("Help")
                    .WithColor(Color.Blue)
                    .WithDescription(help);
                await ReplyAsync(embed: embedb.Build());
            }
            else
            {
                var cmd = CommandService.Commands.First(c => c.Name == name);
                var usageString = $"{Bot.BotConfig.Prefix}{cmd.Name} ";
                var usageFields = new List<EmbedFieldBuilder>();
                foreach (var parameter in cmd.Parameters)
                {
                    usageString += parameter.IsOptional ? $"({parameter.Name}) " : $"{parameter.Name} ";
                    usageFields.Add(new EmbedFieldBuilder()
                        .WithName($"{Utils.TypeName(parameter.Type)} {parameter.Name}")
                        .WithValue(parameter.Summary)
                        .WithIsInline(true));
                }
                embedb.WithTitle(cmd.Name)
                    .WithDescription(cmd.Summary)
                    .WithFields(new EmbedFieldBuilder().WithName("Usage").WithValue(usageString))
                    .WithColor(Color.Blue)
                    .WithFields(usageFields);
                if(cmd.Parameters.Count != 0) { embedb.WithFooter("Parameters in (parentheses) are optional.");}
                await ReplyAsync(embed: embedb.Build());
            }
        }

        [Command("uptime")]
        [Summary("Gets the uptime")]
        public async Task Uptime()
        {
            var formattedTime = Bot.GetUptime().ToString(@"hh\:mm\:ss");
            await ReplyAsync(embed: new EmbedBuilder()
                .WithTitle("Uptime")
                .WithColor(Color.Blue)
                .WithDescription($"I've been up for {formattedTime}")
                .Build());
        }
    }
}