using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeDarkSide_Discord_Bot.Commands
{
    public class Tools : BaseCommandModule
    {

        [Command("test")]
        public async Task TestCommands(CommandContext ctx)
        {

            await ctx.Channel.SendMessageAsync($"this is a bot test {ctx.User.Username}");

        }


        [Command("EmbedTest")]
        [Cooldown(1,5, CooldownBucketType.Global)]
        public async Task EmbedTestCommands(CommandContext ctx, string name)
        {


            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = name,
                Color = DiscordColor.Red
            };
            await ctx.Channel.SendMessageAsync(embed : embedBuilder);

        }



        [Command("ErrorCode")]
        [Cooldown(1, 5, CooldownBucketType.Global)]
        public async Task Error(CommandContext ctx, [RemainingText] string text)
        {
            if (text == "789")
            {
                var embedBuilder = new DiscordEmbedBuilder
                {
                    Title = $"ERROR 789",
                    Description = $"You will get this error when\nYou tried using or typed a @ in your message. This is not allowed. This can happen when you do something like !!say GET PINGED @User",
                    Color = DiscordColor.Red,
                    // Fields = ("Money",$"{userPoints}",true)
                };
                
                await ctx.Channel.SendMessageAsync(embed: embedBuilder);
            }else if (text == "847")
            {
                var embedBuilder = new DiscordEmbedBuilder
                {
                    Title = $"ERROR 847",
                    Description = $"You will get this error when\nYou tried making a/the bot use a bot command. This is not allowed. This can happen when you do something like !!rob @ZeDarkSide",
                    Color = DiscordColor.Red,
                    // Fields = ("Money",$"{userPoints}",true)
                };

                await ctx.Channel.SendMessageAsync(embed: embedBuilder);
            }
            else
            {
                var embedBuilder = new DiscordEmbedBuilder
                {
                    Title = $"ERROR Missing var",
                    Description = $"You either forgot to put a error code in or that error code does not exist!",
                    Color = DiscordColor.Red,
                    // Fields = ("Money",$"{userPoints}",true)
                };

                await ctx.Channel.SendMessageAsync(embed: embedBuilder);
            }
        }




        [Command("Help")]
        public async Task help(CommandContext ctx)
        {
            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = $"Help",
                Description = $"A list of commands you can use!",
                Color = DiscordColor.Red,
                // Fields = ("Money",$"{userPoints}",true)
            };
            embedBuilder.AddField("Bank", $"The Bank command shows your stats(money,roles)", inline: false);          
            embedBuilder.AddField("Gamble", $"!!gamble [bet] gamble a set amount of money", inline: false);
            embedBuilder.AddField("Rob", $"!!rob [@someone] you can try to rob someone or they rob you 0-0", inline: false);
            embedBuilder.AddField("Slots", $"!!slots [bet] you play slots with your set bet", inline: false);
            embedBuilder.AddField("Leaderboard", $"See the top 10 richest people in the server", inline: false);          
            embedBuilder.AddField("ErrorCode", $"!!ErrorCode [Error number] displays info about a error you get", inline: false);
          //  embedBuilder.AddField("Addpoints", $"**This is a owner only command!**", inline: false);
          //  embedBuilder.AddField("PauseBot", $"**This is a Owner only command!**", inline: false);
            embedBuilder.AddField("Raid", $"!!raid [name] [start time] [raid id] use a name from this list [lw,kf,vog,vow,dsc,gos,ron] and start time user [Use This](https://r.3v.fi/discord-timestamps/)", inline: false);
            await ctx.Channel.SendMessageAsync(embed: embedBuilder);
        }








        //Test commands and templates up here ^^^^^^^^^^^^^^^^^^^^

    }




}
