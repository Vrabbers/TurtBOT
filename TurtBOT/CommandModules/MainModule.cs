using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.VisualBasic;

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
        public async Task Help([Name("(name)")][Summary("Get help for")]string cmdname = null)
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
                var usageString = cmd.Name + " ";
                var usageFields = new List<EmbedFieldBuilder>();
                for (int i = 0; i < cmd.Parameters.Count; i++ ) 
                {
                    usageString += cmd.Parameters[i].Name + " ";
                    usageFields.Add(new EmbedFieldBuilder()
                        .WithName($"{TypeName(cmd.Parameters[i].Type)} parameter {i + 1}")
                        .WithValue(cmd.Parameters[i].Summary)
                        .WithIsInline(true));
                }
                embedb.WithTitle(cmd.Name)
                    .WithDescription(cmd.Summary)
                    .WithFields(new EmbedFieldBuilder().WithName("Usage").WithValue(usageString))
                    .WithFields(usageFields);
                await ReplyAsync(embed: embedb.Build());
            }
        }

        [Command("test")]
        [Summary("This command has many arguments")]
        public async Task Test(
            [Name("1st")] [Summary("The 1st one")] string a,
            [Name("2nd")] [Summary("The 2nd one")] string b,
            [Name("3rd")] [Summary("The 3rd one")] int c)
        {
            await ReplyAsync(a + b + c);
        }


        private string TypeName(Type type)
        {
            if (type == typeof(double)) { return "Decimal"; }
            if (type == typeof(int)) { return "Integer"; }

            return type.Name;
        }
    }
}