using System;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Scripting;

namespace TurtBOT.CommandModules
{
    public class OwnerModule : ModuleBase
    {
        [Command("eval")]
        [Summary("Evaluates a C# expression")]
        [RequireOwner]
        public async Task Eval(
            [Remainder]
            [Name("code")]
            [Summary("Code to evaluate")] string code)
        {
            var embedBuilder = new EmbedBuilder()
                .WithTitle("Eval")
                .WithFields(new EmbedFieldBuilder()
                    .WithName("📥 Input")
                    .WithValue($"```csharp\n{code}```"));
            try
            {
                var eval = await CSharpScript.EvaluateAsync(code, options, new EvalGlobals(Context));
                embedBuilder.WithColor(Color.Green)
                    .WithFields(new EmbedFieldBuilder()
                        .WithName("📤 Output")
                        .WithValue($"```csharp\n{eval}```"));
            }
            catch (Exception e)
            {
                embedBuilder.WithColor(Color.Red)
                    .WithFields(new EmbedFieldBuilder()
                        .WithName(Bot.BotConfig.ErrorMessage)
                        .WithValue($"```{e.GetType().Name} {e.Message}```"));
            }

            await ReplyAsync(embed: embedBuilder.Build());
        }
        
        readonly ScriptOptions options = ScriptOptions.Default
            .AddReferences(
                typeof(SocketCommandContext).Assembly,
                Assembly.GetEntryAssembly()
                )
            .WithImports(
                "System",
                "System.IO",
                "System.Text",
                "System.Linq",
                "System.Collections.Generic",
                "Discord",
                "Discord.Commands",
                "Discord.WebSocket");
    }

    public class EvalGlobals
    {
        public ICommandContext Context;
        
        public EvalGlobals(ICommandContext context)
        {
            Context = context;
        }
    }
}