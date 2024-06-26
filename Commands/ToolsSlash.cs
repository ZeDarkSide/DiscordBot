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
using ZeDarkSide_Discord_Bot.config;
using Microsoft.VisualBasic;
using ZeDarkSide_Discord_Bot.Data;

namespace ZeDarkSide_Discord_Bot.Commands
{
    public class ToolsSlash : ApplicationCommandModule
    {
        private const ulong AllowedChannelId = 1114778208903122964;



        [SlashCommand("bank", "Check your bank information")]
        public async Task BankCommand(InteractionContext ctx)
        {

            var customColor = new DiscordColor(255, 71, 59);
            ulong allowedChannelId = 1114778208903122964;

            if (ctx.Channel.Id != allowedChannelId )
            {
                if (ctx.User.Id == 653805970610192394)
                {

                }else
                {
                    await ctx.Interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("This command can only be used in the specified channel."));
                    return;
                }

            }

            ulong userId = ctx.User.Id;
            int userPoints = 0;

            // Get roles dynamically
            var memberRoles = ctx.Member.Roles;
            List<string> roles = new List<string>();
            var member = await ctx.Guild.GetMemberAsync(ctx.User.Id);

            if (memberRoles.Any(role => role.Name.ToLower() == "xotic040"))
            {
                roles.Add("Server Owner");
            }
            if (memberRoles.Any(role => role.Name.ToLower() == "developer"))
            {
                roles.Add("Bot Owner");
            }
            if (memberRoles.Any(role => role.Name.ToLower() == "Moderator"))
            {
                roles.Add("Server Mod");
            }
            if (memberRoles.Any(role => role.Name.ToLower() == "twitch subscriber"))
            {
                roles.Add("Twitch Sub");
            }
            if (memberRoles.Any(role => role.Name.ToLower() == "youtube member"))
            {
                roles.Add("YouTube Member");
            }
            if (memberRoles.Any(role => role.Name.ToLower() == "server booster"))
            {
                roles.Add("Server booster");
            }
            if (ctx.User.Id == 447420781325058048)
            {
                roles.Add("Twitch Sub");
                roles.Add("Beta Tester");
            }


            string roleString = string.Join("\n", roles.Select((r, index) => $"{index + 1}. {r}"));
            var userData = new Dictionary<ulong, List<string>>();

            if (File.Exists("UserData.json"))
            {
                string jsonData = File.ReadAllText("UserData.json");
                userData = JsonConvert.DeserializeObject<Dictionary<ulong, List<string>>>(jsonData);
            }
            List<string> userItems = new List<string>();
            if (userData.ContainsKey(ctx.User.Id))
            {
                userItems = userData[ctx.User.Id];
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




            if (data != null)
            {

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
                    string updatedJsonData = JsonConvert.SerializeObject(data);
                    File.WriteAllText(filePath, updatedJsonData);
                }

            }

            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = $"{ctx.User.Username}'s Bank!",
                Color = customColor,
            };

            embedBuilder.AddField("Money", $"${userPoints}", inline: true);
            embedBuilder.AddField("Roles", string.IsNullOrEmpty(roleString) ? "None" : roleString, inline: true);
            if (userItems.Count > 0)
            {
                string itemsString = string.Join("\n", userItems.Select(item => $"- {item}"));
                embedBuilder.AddField("Owned Items", itemsString);
            }
            embedBuilder.WithFooter("\n\nIf your roles are not correct like missing sub role please contact chancethekiller900", "https://images-ext-1.discordapp.net/external/OF0r3zkCIyqBD9kLTZDqZ0Sv-2sblM0-ZpdRwNRPbxs/https/img.icons8.com/cotton/64/000000/info.png");
            embedBuilder.WithThumbnail(ctx.User.AvatarUrl);

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embedBuilder));
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


            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = "Top 10 Leaderboard",
                Color = new DiscordColor("#ff473b")
            };

            int rank = 1;
            foreach (var user in top10Users)
            {

                var member = await ctx.Guild.GetMemberAsync(user.Key);


                embedBuilder.AddField($"Rank {rank}", $"{member.Username}: ${user.Value}", inline: false);
                rank++;
            }


            while (rank <= 10)
            {
                embedBuilder.AddField($"Rank {rank}", "*Empty Slot*", inline: false);
                rank++;
            }


            if (eleventhUser.Key != default)
            {
                var eleventhMember = await ctx.Guild.GetMemberAsync(eleventhUser.Key);
                embedBuilder.WithFooter($"11th Place: {eleventhMember.Username}");
            }
            else
            {
                embedBuilder.WithFooter("*No 11th Place*");
            }

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embedBuilder));

        }



        private Dictionary<ulong, DateTime> lastRobUse = new Dictionary<ulong, DateTime>();

        #region Econonmy / commands
        private readonly string[] symbols = { ":cherries:", ":lemon:", ":grapes:", ":watermelon:", ":tangerine:", ":bell:", ":seven:" };
        private readonly int[] payouts = { 2, 3, 5, 10, 15, 20, 50 };
        public static int winCounter = 0;
       
        [SlashCommand("slots", "Gamble on slots")]
        public async Task SlashSlots(InteractionContext ctx, [Option("bet", "The amount to bet")] long bet)
        {
            var customColor = new DiscordColor(255, 71, 59);
            ulong allowedChannelId = 1114778208903122964;
          
            if (ctx.Channel.Id != allowedChannelId && ctx.Channel.Id != 778062522888224788)
            {
                await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("This command can only be used in the specified channel."));
                return;
            }

            if (bet <= 0)
            {
                await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Please specify a valid bet amount."));
                return;
            }

            ulong userId = ctx.User.Id;
            int userPoints = 0;
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

            if (data != null)
            {
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
                    string updatedJsonData1 = JsonConvert.SerializeObject(data);
                    File.WriteAllText(filePath, updatedJsonData1);
                }
            }

            if (userPoints < bet)
            {
                await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("You don't have enough points to place that bet."));
                return;
            }

            var rnd = new Random();
            var reels = new List<string[]>();
            for (int i = 0; i < 3; i++)
            {
                var reelSymbols = new string[3];
                for (int j = 0; j < 3; j++)
                {
                    int index = rnd.Next(symbols.Length);
                    reelSymbols[j] = symbols[index];
                }
                reels.Add(reelSymbols);
            }

            int totalPayout = 0;
            string[] resultSymbols = { "", "", "" };

            


           
            if (winCounter >= 30)
            {
                
                Random rnds = new Random();
                int slot = rnds.Next(0, 6);
                for (int i = 0; i < 3; i++)
                {
                    resultSymbols[i] = $"**{symbols[slot]}**";
                }
                
                int symbolIndex = Array.IndexOf(symbols, reels[0][0]);
                int payout = payouts[symbolIndex] * (int)bet;
                totalPayout += payout;

            }
            else
            {
                

                for (int i = 0; i < 3; i++)
                {
                    var reelSymbols = new string[3];
                    for (int j = 0; j < 3; j++)
                    {
                        int index = rnd.Next(symbols.Length);
                        reelSymbols[j] = symbols[index];
                    }
                    reels.Add(reelSymbols);
                }


                if (reels[0][0] == reels[1][0] && reels[0][0] == reels[2][0])
                {
                    string winningSymbol = reels[0][0];
                    int symbolIndex = Array.IndexOf(symbols, winningSymbol);
                    int payout = payouts[symbolIndex] * (int)bet;
                    totalPayout += payout;
                    for (int i = 0; i < 3; i++)
                    {
                        resultSymbols[i] = $"**{winningSymbol}**";
                    }
                }
                else
                {
                    // No winning combination, set result symbols to show actual symbols
                    for (int i = 0; i < 3; i++)
                    {
                        resultSymbols[i] = reels[i][0];
                    }
                    winCounter++;
                }

            }
            Console.WriteLine(winCounter);
            if (totalPayout > 0)
            {
                data.UserPoints[userId] += totalPayout;
                userPoints = data.UserPoints[userId];
                string updatedJsonData1 = JsonConvert.SerializeObject(data);
                File.WriteAllText(filePath, updatedJsonData1);
            }
            else
            {
                data.UserPoints[userId] -= (int)bet;
                userPoints = data.UserPoints[userId];
                string updatedJsonData1 = JsonConvert.SerializeObject(data);
                File.WriteAllText(filePath, updatedJsonData1);
            }

            data.UserPoints[userId] = userPoints;
            string updatedJsonData = JsonConvert.SerializeObject(data);
            File.WriteAllText(filePath, updatedJsonData);

            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = "Slot Machine",
                Description = $"**[ {resultSymbols[0]} | {resultSymbols[1]} | {resultSymbols[2]} ]**",
                Color = customColor
            };

            if (totalPayout > 0)
            {
                embedBuilder.AddField("Congratulations!", $"You won {totalPayout} points!");
                winCounter = 0;
            }
            else
            {
                embedBuilder.AddField("Better luck next time!", "No winning combination.");

            }

            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embedBuilder));
        }


        [SlashCommand("gamble", "Gamble your life away")]
        
        public async Task GambleAway(InteractionContext ctx, [Option("bet", "The amount to bet")] long bet)
        {
            var customColor = new DiscordColor(255, 71, 59);
            ulong allowedChannelId = 1114778208903122964;

            if (ctx.Channel.Id != allowedChannelId && ctx.Channel.Id != 778062522888224788)
            {
                await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("This command can only be used in the specified channel."));
                return;
            }

            if (bet <= 0)
            {
                await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Please specify a valid bet amount."));
                return;
            }

            ulong userId = ctx.User.Id;
            int userPoints = 0;

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

            if (data != null)
            {
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
                    string updatedJsonData1 = JsonConvert.SerializeObject(data);
                    File.WriteAllText(filePath, updatedJsonData1);
                }
            }

            if (userPoints < bet)
            {
                await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("You don't have enough points to place that bet."));
                return;
            }

            Random rnd = new Random();
            int WoL = rnd.Next(1, 9);


            if (WoL == 1 || WoL == 4 || WoL == 6 || WoL == 8 || WoL == 3)
            {

                data.UserPoints[userId] += (int)bet;
                userPoints = data.UserPoints[userId];
                string updatedJsonData = JsonConvert.SerializeObject(data);
                File.WriteAllText(filePath, updatedJsonData);

                var embedBuilder = new DiscordEmbedBuilder
                {
                    Title = $"{ctx.User.Username} Gambled {(int)bet} and won and now has {userPoints} ",
                    Color = DiscordColor.Red,

                };

                await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embedBuilder));
            }
            else
            {
                data.UserPoints[userId] -= (int)bet;
                userPoints = data.UserPoints[userId];
                string updatedJsonData = JsonConvert.SerializeObject(data);
                File.WriteAllText(filePath, updatedJsonData);

                var embedBuilder = new DiscordEmbedBuilder
                {
                    Title = $"{ctx.User.Username} Gambled ${(int)bet} and lost and now has ${userPoints} ",
                    Color = DiscordColor.Red,

                };

                await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embedBuilder));
            }
        }


        [SlashCommand("rob", "Rob someone")]
        
        public async Task RobSomeone(InteractionContext ctx, [Option("user", "The user to rob")] DiscordUser user)
        {
            var customColor = new DiscordColor(255, 71, 59);
            ulong allowedChannelId = 1114778208903122964;

            // Check if the command is being used in the allowed channel
            if (ctx.Channel.Id != allowedChannelId && ctx.Channel.Id != 778062522888224788)
            {
                await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("This command can only be used in the specified channel."));
                return;
            }

            ulong userId = ctx.User.Id;
            ulong Robuser = user.Id;
            int userPoints = 0;
            int robpoints = 0;

            string filePath = "Bank.json";
            JSONStructure data;
            if (lastRobUse.ContainsKey(userId))
            {
                TimeSpan timeSinceLastUse = DateTime.Now - lastRobUse[userId];
                if (timeSinceLastUse.TotalHours < 2)
                {
                    var remainingTime = TimeSpan.FromHours(24) - timeSinceLastUse;
                    var embedBuilders = new DiscordEmbedBuilder
                    {
                        Title = $"You're on the run, so if you don't want to get caught by the cops, lay low for {remainingTime.Hours} hours and {remainingTime.Minutes} minutes.",
                        Color = customColor,
                    };
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embedBuilders));
                    return;
                }
            }
            if (user.IsBot)
            {
                var embedBuilder = new DiscordEmbedBuilder
                {
                    Title = $"ERROR 847",
                    Description = $"Bots can't interact with database Error 847...",
                    Color = customColor,
                   
                };
                embedBuilder.WithFooter("Error codes Please user !!ErrorCode (error code) for more information", "https://images-ext-1.discordapp.net/external/OF0r3zkCIyqBD9kLTZDqZ0Sv-2sblM0-ZpdRwNRPbxs/https/img.icons8.com/cotton/64/000000/info.png");
                await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embedBuilder));

            }
            if (user.Id == ctx.User.Id)
            {
                var embedBuilder = new DiscordEmbedBuilder
                {
                    Title = $"ERROR 848",
                    Description = $"Your trying to rob yourself Error 848...",
                    Color = customColor,

                };
                await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embedBuilder));
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

            if (data != null)
            {

                if (data.UserPoints == null)
                {
                    data.UserPoints = new Dictionary<ulong, int>();
                }

                if (data.UserPoints.ContainsKey(userId))
                {
                    // If user ID exists, retrieve their points
                    userPoints = data.UserPoints[userId];
                }
                else
                {

                    data.UserPoints[userId] = 500;
                    userPoints = 500;

                    string updatedJsonData = JsonConvert.SerializeObject(data);
                    File.WriteAllText(filePath, updatedJsonData);
                }

            }
            if (data != null)
            {

                if (data.UserPoints == null)
                {
                    data.UserPoints = new Dictionary<ulong, int>();
                }

                if (data.UserPoints.ContainsKey(Robuser))
                {
                    // If user ID exists, retrieve their points
                    robpoints = data.UserPoints[Robuser];
                }
                else
                {

                    data.UserPoints[Robuser] = 500;
                    robpoints = 500;


                    string updatedJsonData = JsonConvert.SerializeObject(data);
                    File.WriteAllText(filePath, updatedJsonData);
                }

            }


            Random rnd = new Random();
            int WoL = rnd.Next(1, 9);
            if (WoL == 1 || WoL == 3 || WoL == 5)
            {
                Random rnds = new Random();
                int WoLs = rnd.Next(1, robpoints);

                data.UserPoints[userId] += WoLs;
                userPoints = data.UserPoints[userId];
                string updatedJsonData1 = JsonConvert.SerializeObject(data);
                File.WriteAllText(filePath, updatedJsonData1);

                data.UserPoints[Robuser] -= WoLs;
                robpoints = data.UserPoints[Robuser];


                string updatedJsonData11 = JsonConvert.SerializeObject(data);
                File.WriteAllText(filePath, updatedJsonData11);

                var embedBuilder = new DiscordEmbedBuilder
                {
                    Title = $"{ctx.User.Username} Robbed {user.Username}",
                    Description = $"For ${WoLs}",
                    Color = customColor

                };

                await ctx.Channel.SendMessageAsync(embed: embedBuilder);
            }
            else if (WoL == 2 || WoL == 4 || WoL == 6)
            {
                Random rnds = new Random();
                int WoLs = rnd.Next(1, userPoints);

                data.UserPoints[userId] -= WoLs;
                userPoints = data.UserPoints[userId];
                string updatedJsonData1 = JsonConvert.SerializeObject(data);
                File.WriteAllText(filePath, updatedJsonData1);

                data.UserPoints[Robuser] += WoLs;
                robpoints = data.UserPoints[Robuser];
                string updatedJsonData11 = JsonConvert.SerializeObject(data);
                File.WriteAllText(filePath, updatedJsonData11);

                var embedBuilder = new DiscordEmbedBuilder
                {
                    Title = $"{ctx.User.Username} Tried to Rob {user.Username}",
                    Description = $"{user.Username} caught {ctx.User.Username} and stole ${WoLs} from them!",
                    Color = customColor,

                };
                lastRobUse[userId] = DateTime.Now;
                await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embedBuilder));
            }
            else if (WoL == 7 || WoL == 9 || WoL == 8)
            {
                var embedbuilder = new DiscordEmbedBuilder
                {
                    Title = $"{ctx.User.Username} Tried to Rob {user.Username}",
                    Description = $"They made too much noise and were about to be caught, so they fled.",
                    Color = customColor,
                };
                lastRobUse[userId] = DateTime.Now;
                await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embedbuilder));
            }




        }
        #endregion


        [SlashCommand("ModInfo", "Get information about destiny mods")]
        public async Task ModInfo(InteractionContext ctx, [Autocomplete(typeof(ModNameAutocompleteProvider)), Option("ModName", "The name of the mod to get information about", true)] string modName = null)
        {
            var customColor = new DiscordColor(255, 71, 59);

            await ctx.Interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
    .WithContent("Not available at this time!"));
            return;

            if (modName == null)
            {
                await ctx.Interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent("Please choose a mod!"));
                return;
            }

            if (DictionaryDataBase.ModDetails.TryGetValue(modName, out var modInfo))
            {
                var embedBuilder = CreateModEmbed(modInfo.Title, modInfo.Description, modInfo.Slot, modInfo.ImageUrl, modInfo.CommunityResearch, customColor);
                await ctx.Interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .AddEmbed(embedBuilder));
            }
            else
            {
                await ctx.Interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent("Mod not found."));
            }
        }

        private DiscordEmbedBuilder CreateModEmbed(string title, string description, string slot, string imageUrl, string communityResearch, DiscordColor color)
        {
            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = title,
                Description = description,
                ImageUrl = imageUrl,
                Color = color,
            };

            embedBuilder.AddField("Slot", slot, inline: true);
            if (!string.IsNullOrEmpty(communityResearch))
            {
                embedBuilder.AddField("Community Research", communityResearch, inline: true);
            }
            embedBuilder.WithFooter("This is still a WIP command. Some things may be wrong or outright missing", "https://images-ext-1.discordapp.net/external/OF0r3zkCIyqBD9kLTZDqZ0Sv-2sblM0-ZpdRwNRPbxs/https/img.icons8.com/cotton/64/000000/info.png");

            return embedBuilder;
        }


        #region Basis commands with helping and testing


        [SlashCommand("ping", "Checks the bot's latency.")]
        public async Task Ping(InteractionContext ctx)
        {

            var latency = ctx.Client.Ping;


            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .WithContent($"Pong! Latency: {latency}ms"));
        }


        [SlashCommand("test", "just a test command nothing cool")]
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


        [SlashCommand("help_pyrrhus", "Displays a list of commands you can use.")]
        public async Task HelpSlashCommand(InteractionContext ctx)
        {
            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = $"Help",
                Description = $"A list of commands you can use!",
                Color = DiscordColor.Red,
            };
            // embedBuilder.AddField("Addpoints", $"**This is an owner-only command!**", inline: false);
            // embedBuilder.AddField("PauseBot", $"**This is an Owner-only command!**", inline: false);
            embedBuilder.AddField("Raid", $"!!raid [name] [start time] [desc if you want] use a name from this list [lw,kf,vog,vow,dsc,gos,ron,se] and start time [Use This](https://r.3v.fi/discord-timestamps/)", inline: false);
            embedBuilder.AddField("Excision", $"!!exc [start time] Excision GM and start time [Use This](https://r.3v.fi/discord-timestamps/)", inline: false);
            embedBuilder.AddField("Daily", $"!!daily gives the user $1000. you can use it once every 24 Hours", inline: false);
            embedBuilder.AddField("Raid ping ", $"!!raidping <messageid> <message> will ping everyone that was in the join slot of a raid post", inline: false);
            embedBuilder.AddField("Change raid time ", $"!!changeraidtime <messageid> <new time> will change the raid post start time", inline: false);

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embedBuilder));
        }
        #endregion



    }



}
