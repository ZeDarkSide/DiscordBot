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

            if (ctx.Channel.Id != allowedChannelId)
            {


                await ctx.Interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("This command can only be used in the specified channel."));
                return;
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
            embedBuilder.WithFooter("\n\nIf your roles are not correct like missing sub role please contact chancethekiller900");
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
                Color = DiscordColor.Red
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





        #region Econonmy / commands
        private readonly string[] symbols = { ":cherries:", ":lemon:", ":grapes:", ":watermelon:", ":tangerine:", ":bell:", ":seven:" };
        private readonly int[] payouts = { 2, 3, 5, 10, 15, 20, 50 };
        public static int winCounter = 0;
        [Cooldown(1, 10, CooldownBucketType.User)]
        [SlashCommand("slots", "Gamble on slots")]
        public async Task SlashSlots(InteractionContext ctx, [Option("bet", "The amount to bet")] long bet)
        {
            var customColor = new DiscordColor(255, 71, 59);
            ulong allowedChannelId = 1114778208903122964;
            string results = "";
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

            // Counter to track if the next slots should be a win


            // Check if the win counter has reached 3
            if (winCounter >= 30)
            {
                // Force a win by setting all symbols to ":seven:"
                for (int i = 0; i < 3; i++)
                {
                    resultSymbols[i] = $"**{symbols[6]}**";
                }
                // Reset the win counter
                int symbolIndex = Array.IndexOf(symbols, reels[0][0]);
                int payout = payouts[symbolIndex] * (int)bet;
                totalPayout += payout;

            }
            else
            {
                // Generate random symbols as usual
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

                // Check if all symbols in a reel match
                if (reels[0][0] == reels[1][0] && reels[0][0] == reels[2][0])
                {
                    int symbolIndex = Array.IndexOf(symbols, reels[0][0]);
                    int payout = payouts[symbolIndex] * (int)bet;
                    totalPayout += payout;
                    for (int i = 0; i < 3; i++)
                    {
                        resultSymbols[i] = $"**{reels[0][0]}**";
                    }
                    // Increase the win counter
                    // winCounter = 0;

                }
                else
                {
                    // No winning combination
                    for (int i = 0; i < 3; i++)
                    {
                        resultSymbols[i] = reels[0][i];
                    }
                    // Reset the win counter
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
        [Cooldown(1, 10, CooldownBucketType.User)]
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
                    Title = $"{ctx.User.Username} Gambled {(int)bet} and lost and now has {userPoints} ",
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

            if (user.IsBot)
            {
                var embedBuilder = new DiscordEmbedBuilder
                {
                    Title = $"ERROR 847",
                    Description = $"Bots can't interact with database Error 847...",
                    Color = customColor,

                };
                embedBuilder.WithFooter("Error codes Please user !!ErrorCode (error code) for more information");
                await ctx.Channel.SendMessageAsync(embed: embedBuilder);

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
                    Description = $"But {user.Username} caught {ctx.User.Username} and stole ${WoLs} from them!",
                    Color = customColor,

                };

                await ctx.Channel.SendMessageAsync(embed: embedBuilder);
            }
            else if (WoL == 7 || WoL == 9 || WoL == 8)
            {
                var embedbuilder = new DiscordEmbedBuilder
                {
                    Title = $"{ctx.User.Username} Tried to Rob {user.Username}",
                    Description = $"But made too many sounds and was about to be caught so they fled.",
                    Color = customColor,
                };
                await ctx.Channel.SendMessageAsync(embed: embedbuilder);
            }




        }
        #endregion





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
            embedBuilder.AddField("Raid", $"!!raid [name] [start time] use a name from this list [lw,kf,vog,vow,dsc,gos,ron] and start time [Use This](https://r.3v.fi/discord-timestamps/)", inline: false);

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embedBuilder));
        }
        #endregion



    }



}
