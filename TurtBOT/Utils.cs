using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace TurtBOT
{
    public static class Utils
    {
        public static Embed SimpleEmbed(string title, Color color, string description)
        {
            return new EmbedBuilder()
                .WithTitle(title)
                .WithColor(color)
                .WithDescription(description)
                .Build();
        }
    }
}
