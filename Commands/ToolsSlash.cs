using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Remoting.Contexts;
using System.Xml.Linq;
using ZeDarkSide_Discord_Bot.config;

namespace ZeDarkSide_Discord_Bot.Commands
{
    public class ToolsSlash : ApplicationCommandModule
    {

        [SlashCommand("test","just a test command nothing cool")]
        public async Task TestCommands(InteractionContext ctx)
        {


            ulong allowedChannelId = 1114778208903122964;

            if (ctx.Channel.Id != allowedChannelId)
            {
                await ctx.Interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("This command can only be used in the specified channel."));
                return;
            }
            
            await ctx.Interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("hello"));

        }



        [SlashCommand("bank", "Check your bank information")]
        public async Task BankCommand(InteractionContext ctx)
        {



            ulong allowedChannelId = 1114778208903122964;

            if (ctx.Channel.Id != allowedChannelId)
            {
                await ctx.Interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("This command can only be used in the specified channel."));
                return;
            }


            ulong userId = ctx.User.Id;
            int userPoints = 0;

            string role = "1.viewer";

         
            if (userId == 653805970610192394)
            {
                role = "1.Bot Owner\n2.Streamer";
            }
            else if (userId == 762851502237679637)
            {
                role = "1.Streamer";
            }

            string filePath = "Bank.json";
            JSONStructure data;

            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                data = JsonConvert.DeserializeObject<JSONStructure>(jsonData);
            }
            else
            {
                
                data = new JSONStructure { UserPoints = new Dictionary<ulong, int>() };
            }

        
            if (data.UserPoints == null)
            {
                data.UserPoints = new Dictionary<ulong, int>();
            }

        
            if (data != null && data.UserPoints.ContainsKey(userId))
            {
                userPoints = data.UserPoints[userId];
            }
            else
            {
               
                data.UserPoints[userId] = 500;
                userPoints = 500; 

              
                string updatedJsonData = JsonConvert.SerializeObject(data);
                File.WriteAllText(filePath, updatedJsonData);
            }

            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = $"{ctx.User.Username}'s Bank!",
                Color = DiscordColor.Red
            };

            embedBuilder.AddField("Money", $"${userPoints}", inline: true);
            embedBuilder.AddField("Role", $"{role}", inline: true);
            embedBuilder.WithFooter("\n\nIf your roles are not correct like missing sub role please contact chancethekiller900");
            embedBuilder.WithThumbnail(ctx.User.AvatarUrl);

         
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embedBuilder));
        }


        [SlashCommand("rob", "Rob someone")]
        public async Task RobSomeone(InteractionContext ctx, [Option("user", "User to rob")] DiscordUser user)
        {



            ulong allowedChannelId = 1114778208903122964;

            if (ctx.Channel.Id != allowedChannelId)
            {
                await ctx.Interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("This command can only be used in the specified channel."));
                return;
            }

            ulong userId = ctx.User.Id;
            ulong Robuser = user.Id;
            int userPoints = 0;
            int robpoints = 0;

            string filePath = "Bank.json";
            JSONStructure data;

            if (user.IsBot)
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent("Bots cannot be robbed."));
                return;
            }

            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                data = JsonConvert.DeserializeObject<JSONStructure>(jsonData);
            }
            else
            {
              
                data = new JSONStructure { UserPoints = new Dictionary<ulong, int>() };
            }

          
            if (data.UserPoints == null)
            {
                data.UserPoints = new Dictionary<ulong, int>();
            }

        
            if (data.UserPoints.ContainsKey(userId))
            {
                userPoints = data.UserPoints[userId];
            }
            else
            {
              
                data.UserPoints[userId] = 500;
                userPoints = 500;
            }

          
            if (data.UserPoints.ContainsKey(Robuser))
            {
                robpoints = data.UserPoints[Robuser];
            }
            else
            {
              
                data.UserPoints[Robuser] = 500;
                robpoints = 500;
            }

          
            Random rnd = new Random();
            int WoL = rnd.Next(1, 9);
            int amountRobbed = 0;

            if (WoL == 1 || WoL == 3 || WoL == 5)
            {
                amountRobbed = rnd.Next(1, robpoints);
                data.UserPoints[userId] += amountRobbed;
                data.UserPoints[Robuser] -= amountRobbed;
            }
            else if (WoL == 2 || WoL == 4 || WoL == 6 || WoL == 7 || WoL == 9)
            {
                amountRobbed = rnd.Next(1, userPoints);
                data.UserPoints[userId] -= amountRobbed;
                data.UserPoints[Robuser] += amountRobbed;
            }

            // Serialize and save the updated data back to the JSON file
            string updatedJsonData = JsonConvert.SerializeObject(data);
            File.WriteAllText(filePath, updatedJsonData);

            // Construct and send the response message
            var responseMessage = amountRobbed > 0 ? $"{ctx.User.Username} successfully robbed {user.Username} and got {amountRobbed} points." :
                                                      $"{ctx.User.Username} attempted to rob {user.Username}, but failed.";
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .WithContent(responseMessage));
        }


        [SlashCommand("leaderboard", "Display the top 10 leaderboard")]
        public async Task Leaderboard(InteractionContext ctx)
        {



            ulong allowedChannelId = 1114778208903122964;

            if (ctx.Channel.Id != allowedChannelId)
            {
                await ctx.Interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("This command can only be used in the specified channel."));
                return;
            }


            string filePath = "Bank.json";
            JSONStructure data;

            
            if (File.Exists(filePath))
            {
               
                string jsonData = File.ReadAllText(filePath);
                data = JsonConvert.DeserializeObject<JSONStructure>(jsonData);
            }
            else
            {
               
                data = new JSONStructure { UserPoints = new Dictionary<ulong, int>() };
            }

            
            if (data.UserPoints == null)
            {
                data.UserPoints = new Dictionary<ulong, int>();
            }

            
            var sortedUsers = data.UserPoints.OrderByDescending(x => x.Value);

     
            var top10Users = sortedUsers.Take(10);

          
            var eleventhUser = sortedUsers.Skip(10).FirstOrDefault();

        
            StringBuilder leaderboardMessage = new StringBuilder();
            leaderboardMessage.AppendLine("**Top 10 Leaderboard**");
            int rank = 1;
            foreach (var user in top10Users)
            {
             
                var member = await ctx.Guild.GetMemberAsync(user.Key);
                leaderboardMessage.AppendLine($"**Rank {rank}:** {member.Username}: ${user.Value}");
                rank++;
            }

      
            while (rank <= 10)
            {
                leaderboardMessage.AppendLine($"**Rank {rank}:** *Empty Slot*");
                rank++;
            }

       
            string eleventhPlace = eleventhUser.Key != default ? $"11th Place: {await ctx.Guild.GetMemberAsync(eleventhUser.Key)}" : "*No 11th Place*";
            leaderboardMessage.AppendLine(eleventhPlace);

            
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .WithContent(leaderboardMessage.ToString()));
        }

        [SlashCommand("ping", "Checks the bot's latency.")]
        public async Task Ping(InteractionContext ctx)
        {
            
            var latency = ctx.Client.Ping;

           
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .WithContent($"Pong! Latency: {latency}ms"));
        }

        [SlashCommand("help_pyrrhus", "Displays a list of commands you can use.")]
        public async Task HelpSlashCommand(InteractionContext ctx)
        {
            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = $"Help",
                Description = $"A list of commands you can use!",
                Color = DiscordColor.Red,
            };
            embedBuilder.AddField("Bank", $"The Bank command shows your stats (money, roles)", inline: false);
            embedBuilder.AddField("Gamble", $"!!gamble [bet] gamble a set amount of money", inline: false);
            embedBuilder.AddField("Rob", $"!!rob [@someone] you can try to rob someone or they rob you 0-0", inline: false);
            embedBuilder.AddField("Slots", $"!!slots [bet] you play slots with your set bet", inline: false);
            embedBuilder.AddField("Leaderboard", $"See the top 10 richest people in the server", inline: false);         
            embedBuilder.AddField("ErrorCode", $"!!ErrorCode [Error number] displays info about a error you get", inline: false);
           // embedBuilder.AddField("Addpoints", $"**This is an owner-only command!**", inline: false);
           // embedBuilder.AddField("PauseBot", $"**This is an Owner-only command!**", inline: false);
            embedBuilder.AddField("Raid", $"!!raid [name] [start time] [raid id] use a name from this list [lw,kf,vog,vow,dsc,gos,ron] and start time user [Use This](https://r.3v.fi/discord-timestamps/)", inline: false);

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embedBuilder));
        }




    }



}
