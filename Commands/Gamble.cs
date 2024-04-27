using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.AsyncEvents;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.EventHandling;
using DSharpPlus.EventArgs;
using DSharpPlus.Exceptions;
using DSharpPlus.Net;
using Newtonsoft.Json; 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ZeDarkSide_Discord_Bot.Commands
{
    public class Gamble : BaseCommandModule
    {
        private Dictionary<string, RaidData> raidDatabase = new Dictionary<string, RaidData>();


        

        [Command("bank")]
        [Cooldown(1, 30, CooldownBucketType.User)] // Example cooldown settings
        public async Task SeeHowMuchInBank(CommandContext ctx)
        {


            ulong allowedChannelId = 1114778208903122964;

            if (ctx.Channel.Id != allowedChannelId)
            {


                await ctx.RespondAsync("This command can only be used in the specified channel.");
                return;
            }


            ulong userId = ctx.User.Id;
            int userPoints = 0;

            string role = "1.viewer";


            if (userId == 653805970610192394)
            {
                role = "1.Bot Owner\n2.Streamer";
            }
            if (userId == 762851502237679637)
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
                Color = DiscordColor.Red,
                
               
            };

            embedBuilder.AddField("Money", $"${userPoints}", inline: true);
            embedBuilder.AddField("Role", $"{role}", inline: true);
            embedBuilder.WithFooter("\n\nIf your roles are not correct like missing sub role please contact chancethekiller900");
            embedBuilder.WithThumbnail(ctx.User.AvatarUrl);

            await ctx.Channel.SendMessageAsync(embed: embedBuilder);

          

        }



        [Command("gamble")]
        [Cooldown(1, 30, CooldownBucketType.User)]
        public async Task GamblePoints(CommandContext ctx, int pointsToGamble)
        {

            ulong allowedChannelId = 1114778208903122964;

            if (ctx.Channel.Id != allowedChannelId)
            {
                await ctx.RespondAsync("This command can only be used in the specified channel.");
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

                
                    string updatedJsonData = JsonConvert.SerializeObject(data);
                    File.WriteAllText(filePath, updatedJsonData);
                }

            }

            if (userPoints < pointsToGamble)
            {
                var embedBuilder = new DiscordEmbedBuilder
                {
                    Title = $"You only have {userPoints} ",
                    Color = DiscordColor.Red,
                    
                };




                await ctx.Channel.SendMessageAsync(embed: embedBuilder);
                return;
            }


            Random rnd = new Random();
            int WoL = rnd.Next(1, 9);
            if (WoL == 1 || WoL == 4)
            {

                data.UserPoints[userId] += pointsToGamble;
                userPoints = data.UserPoints[userId]; 
                string updatedJsonData = JsonConvert.SerializeObject(data);
                File.WriteAllText(filePath, updatedJsonData);

                var embedBuilder = new DiscordEmbedBuilder
                {
                    Title = $"{ctx.User.Username} Gambled {pointsToGamble} and won and now has {userPoints} ",
                    Color = DiscordColor.Red,
                    
                };

                await ctx.Channel.SendMessageAsync(embed: embedBuilder);
            }else
            {
                data.UserPoints[userId] -= pointsToGamble;
                userPoints = data.UserPoints[userId]; 
                string updatedJsonData = JsonConvert.SerializeObject(data);
                File.WriteAllText(filePath, updatedJsonData);

                var embedBuilder = new DiscordEmbedBuilder
                {
                    Title = $"{ctx.User.Username} Gambled {pointsToGamble} and lost and now has {userPoints} ",
                    Color = DiscordColor.Red,
                    
                };

                await ctx.Channel.SendMessageAsync(embed: embedBuilder);
            }




        }


        [Command("rob")]
        [Cooldown(1, 7200, CooldownBucketType.User)]
        public async Task RobSomeone(CommandContext ctx, DiscordMember user)
        {


            ulong allowedChannelId = 1114778208903122964;

            if (ctx.Channel.Id != allowedChannelId)
            {
                await ctx.RespondAsync("This command can only be used in the specified channel.");
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
                    Color = DiscordColor.Red,
                   
                };
                embedBuilder.WithFooter("Error codes Please user !!ErrorCode (error code) for more information");
                await ctx.Channel.SendMessageAsync(embed: embedBuilder);
               
            }else
            {
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


                // await ctx.Channel.SendMessageAsync($"user {ctx.User.Username} has {userPoints} and user 2 {user.Username} has {robpoints}");

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
                        Color = DiscordColor.Red,
                       
                    };

                    await ctx.Channel.SendMessageAsync(embed: embedBuilder);
                }
                else if (WoL == 2 || WoL == 4 || WoL == 6 || WoL == 7 || WoL == 9)
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
                        Color = DiscordColor.Red,
                        
                    };

                    await ctx.Channel.SendMessageAsync(embed: embedBuilder);
                }
            }


           

        }




        [Command("leaderboard")]
        public async Task Leaderboard(CommandContext ctx)
        {


            ulong allowedChannelId = 1114778208903122964;

            if (ctx.Channel.Id != allowedChannelId)
            {
                await ctx.RespondAsync("This command can only be used in the specified channel.");
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

           
            await ctx.Channel.SendMessageAsync(embed: embedBuilder);
        }


        private readonly string[] symbols = { ":cherries:", ":lemon:", ":grapes:", ":watermelon:", ":tangerine:", ":bell:", ":seven:" };
        private readonly int[] payouts = { 2, 3, 5, 10, 15, 20, 50 }; 

        [Command("slots")]
        [Cooldown(1, 10, CooldownBucketType.User)]
        public async Task Slots(CommandContext ctx, int bet = 0, string result = null)
        {

            ulong allowedChannelId = 1114778208903122964; 

            if (ctx.Channel.Id != allowedChannelId)
            {
                await ctx.RespondAsync("This command can only be used in the specified channel.");
                return;
            }


            if (bet <= 0)
            {
                await ctx.RespondAsync("Please specify a valid bet amount.");
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
                await ctx.RespondAsync("You don't have enough points to place that bet.");
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
            for (int i = 0; i < 3; i++)
            {
                if (reels[0][i] == reels[1][i] && reels[0][i] == reels[2][i])
                {
                    int symbolIndex = Array.IndexOf(symbols, reels[0][i]);
                    int payout = payouts[symbolIndex] * bet;
                    totalPayout += payout;

                    resultSymbols[i] = $"**{reels[0][i]}**";
                }
                else
                {
                    resultSymbols[i] = reels[0][i];
                }
            }

            
            if (totalPayout > 0)
            {
               

                data.UserPoints[userId] += totalPayout;
                userPoints = data.UserPoints[userId]; 

              
                string updatedJsonData1 = JsonConvert.SerializeObject(data);
                File.WriteAllText(filePath, updatedJsonData1);
            }
            else
            {
                data.UserPoints[userId] -= bet;
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
                Color = DiscordColor.Red
            };

            if (totalPayout > 0)
            {
                embedBuilder.AddField("Congratulations!", $"You won {totalPayout} points!");
            }
            else
            {
                embedBuilder.AddField("Better luck next time!", "No winning combination.");
            }

            await ctx.Channel.SendMessageAsync(embed: embedBuilder);
        }



        
        [Command("addpoints")]//fix this later 
        public async Task Addpoints(CommandContext ctx , int PointAmount, DiscordMember user )
        {

            if (ctx.User.Id == 1114778208903122964)
            {
                if (user.Id == null || user.Id == 0)
                {
                    var noUser1 = new DiscordEmbedBuilder
                    {
                        Title = $"You did not pick a user! please try again and use a member to add points to.",
                        Color = DiscordColor.Red,
                       
                    };

                  

                    await ctx.Channel.SendMessageAsync(embed: noUser1);
                }
                if (PointAmount <= 0)
                {
                    PointAmount = 100;
                }
                ulong userId = user.Id;
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

                data.UserPoints[userId] += PointAmount;
                userPoints = data.UserPoints[userId]; 


                string updatedJsonData1 = JsonConvert.SerializeObject(data);
                File.WriteAllText(filePath, updatedJsonData1);


                var embedBuilder = new DiscordEmbedBuilder
                {
                    Title = $"Added ${PointAmount} to {user.Username}'s Bank account!",
                    Color = DiscordColor.Red,
                   
                };

              


                await ctx.Channel.SendMessageAsync(embed: embedBuilder);
            }
            else
            {
                var embedBuilder = new DiscordEmbedBuilder
                {
                    Title = $"You can't use this command only The Owner can use this command",
                    Color = DiscordColor.Red,
                  
                };

                


                await ctx.Channel.SendMessageAsync(embed: embedBuilder);
            }

        }

        [Command("bug")]
        public async Task Bug(CommandContext ctx, params string[] args)
        {
            if (args.Length < 4)
            {
                await ctx.RespondAsync("Please provide all required parameters: bug name, bug details, reproduction steps, and reporter name.\nexample:  !!bug \"bug name\" \"bug details\" \"Reproduction Steps\" \"user sending report\" ");
                return;
            }

            
            string bugName = args[0];
            string bugDetails = args[1];
            string reproductionSteps = args[2];
            string reporterName = args[3];

            
            var embed = new DiscordEmbedBuilder()
                .WithTitle(bugName)
                .WithDescription($"Bug Details:\n {bugDetails}")
                .AddField("Reproduction Steps", reproductionSteps)
                .WithFooter($"Reported by {reporterName}")
                .WithColor(DiscordColor.Red);

            
            var owner = await ctx.Guild.GetMemberAsync(653805970610192394) as DiscordMember;
            await owner.SendMessageAsync($"Bug Report by {ctx.User.Username}");
            await owner.SendMessageAsync(embed: embed);

            
            await ctx.Message.DeleteAsync();
            await ctx.RespondAsync($"Bug report for '{bugName}' has been submitted successfully. Thank you! {ctx.User.Username}");
        }


        public int emptyCharacterCount = 0;


        [Command("restids")]
        [RequireOwner]
        [RequirePermissions(Permissions.ManageMessages)]
        [RequireBotPermissions(Permissions.ManageMessages)]
        public async Task idrest(CommandContext ctx)
        {

            emptyCharacterCount = 0;

            await ctx.RespondAsync("Raid ids have been reset");
            foreach (var raidEntry in raidDatabase)
            {
                Console.WriteLine($"Raid ID: {raidEntry.Key}, Data: {raidEntry.Value}");
            }

        }




        [Command("raid")]
        [RequirePermissions(Permissions.ManageMessages)]
        [RequireBotPermissions(Permissions.ManageMessages)]
        public async Task Raid(CommandContext ctx, string raidAlias, string startTime = "undefined")
        {

            if (startTime == null || startTime == "")
            {
                startTime = "undefined";
            }


            Dictionary<string, string> raidNameAliases = new Dictionary<string, string>()
    {
        { "crota", "Crota's End" },
        { "ron", "Root of Nightmares" },
        { "vog", "Vault of Glass" },
        { "kf", "King's Fall" },
        { "lw", "Last Wish" },
        { "gos", "Garden of Salvation" },
        {"vow", "Vow of the Disciple"},
        {"dsc", "Deep Stone Crypt"}
    };

            // Check in if the provided raid name alias exists in the dictionary
            string raidName;
            if (!raidNameAliases.TryGetValue(raidAlias.ToLower(), out raidName))
            {
                await ctx.RespondAsync("Invalid raid name alias. Please use a valid raid name alias.");
                return;
            }
            emptyCharacterCount++;


            if (emptyCharacterCount >= 201)
            {
                await ctx.RespondAsync("No more raids can be made at this point in time. Please contact the owner!");
                return;
            }

            if (emptyCharacterCount != 0)
            {
                raidName += new string('\u200E', emptyCharacterCount);
            }
            if (!raidDatabase.ContainsKey(raidName))
            {
                raidDatabase[raidName] = new RaidData();
            }



            await ctx.Message.DeleteAsync();


            var interactivity = ctx.Client.GetInteractivity();

            RaidData raidData = raidDatabase[raidName];
            

            var embedBuilder = new DiscordEmbedBuilder()
                .WithTitle($"**{raidName}**")
                .WithDescription($"**Start Time:** {startTime}\n\nJoin Slots:\n{GetSlotStatus(raidData.JoinSlots, ctx.Member)}\nStandby Slots:\n{GetSlotStatus(raidData.StandbySlots, ctx.Member)}")
                .WithColor(new DiscordColor("#ff473b"))
                .WithFooter($"This raid has an ID of {emptyCharacterCount}");



            switch (raidAlias.ToLower())
            {
                case "crota":
                    embedBuilder.WithImageUrl("https://www.blueberries.gg/wp-content/uploads/2023/08/Crotas-End-Raid-gameplay-Destiny-2-1536x864.jpg");
                    break;
                case "ron":
                    embedBuilder.WithImageUrl("https://www.blueberries.gg/wp-content/uploads/2023/03/Root-of-Nightmares-Raid-Featured-Destiny-2-1536x864.jpg");
                    break;
                case "vog":
                    embedBuilder.WithImageUrl("https://www.blueberries.gg/wp-content/uploads/2022/03/Vault-of-Glass-Destiny-2-900p.jpg");
                    break;
                case "kf":
                    embedBuilder.WithImageUrl("https://www.blueberries.gg/wp-content/uploads/2022/08/Kings-Fall-Destiny-2-900p-1536x864.jpg");
                    break;
                case "lw":
                    embedBuilder.WithImageUrl("https://www.blueberries.gg/wp-content/uploads/2022/01/Last-Wish-raid-Destiny-2-900p-1536x864.jpeg");
                    break;
                case "dsc":
                    embedBuilder.WithImageUrl("https://www.blueberries.gg/wp-content/uploads/2022/03/Deep-Stone-Crypt-Destiny-2-900p.jpg");
                    break;
                case "gos":
                    embedBuilder.WithImageUrl("https://www.blueberries.gg/wp-content/uploads/2022/01/Garden-of-Salvation-Destiny-2-900p-1536x864.jpeg");
                    break;
                case "vow":
                    embedBuilder.WithImageUrl("https://www.blueberries.gg/wp-content/uploads/2022/03/Vow-of-the-Disciple-Destiny-2-900p-1-1536x864.jpg");
                    break;

            }





            var messageBuilder = new DiscordMessageBuilder()
                .WithEmbed(embedBuilder.Build())
                .AddComponents(
                    new DiscordButtonComponent(ButtonStyle.Success, "join_raid", "Join Raid"),
                    new DiscordButtonComponent(ButtonStyle.Secondary, "join_standby", "Join Standby"),
                    new DiscordButtonComponent(ButtonStyle.Danger, "delete_raid", "Delete Raid")
                );

            var message = await ctx.RespondAsync(messageBuilder);

            while (true)
            {
                var interactivityResult = await interactivity.WaitForButtonAsync(message);

                if (interactivityResult.Result == null)
                {
                    // Handle null result if needed becauase if not it dies >:(
                    continue;
                }

                switch (interactivityResult.Result.Id)
                {
                    case "join_raid":
                        // Get the ID of the user who pressed the button
                        ulong userId = interactivityResult.Result.User.Id;

                        if (raidData.Users.Contains(userId))
                        {
                            
                            raidData.Users.Remove(userId);
                            raidData.JoinSlots.Remove(userId);
                            embedBuilder.WithDescription($"**Start Time:** {startTime}\n\nJoin Slots:\n{GetSlotStatus(raidData.JoinSlots, ctx.Member)}\nStandby Slots:\n{GetSlotStatus(raidData.StandbySlots, ctx.Member)}");

                           
                            var responseBuilder = new DiscordInteractionResponseBuilder()
                                .AddEmbed(embedBuilder)
                                .AddComponents(
                                    new DiscordButtonComponent(ButtonStyle.Success, "join_raid", "Join Raid"),
                                    new DiscordButtonComponent(ButtonStyle.Secondary, "join_standby", "Join Standby"),
                                    new DiscordButtonComponent(ButtonStyle.Danger, "delete_raid", "Delete Raid")
                                );

                            
                            await interactivityResult.Result.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, responseBuilder);
                            var messageBuilders = new DiscordFollowupMessageBuilder()
                                .WithContent("You have been removed from the raid slot.")
                                .AsEphemeral(true);

                            await interactivityResult.Result.Interaction.CreateFollowupMessageAsync(messageBuilders);
                            
                        }
                        else if (raidData.JoinSlots.Count >= 6)
                        {
                            embedBuilder.WithDescription($"**Start Time:** {startTime}\n\nJoin Slots:\n{GetSlotStatus(raidData.JoinSlots, ctx.Member)}\nStandby Slots:\n{GetSlotStatus(raidData.StandbySlots, ctx.Member)}");

                            
                            var responseBuilder = new DiscordInteractionResponseBuilder()
                                .AddEmbed(embedBuilder)
                                .AddComponents(
                                    new DiscordButtonComponent(ButtonStyle.Success, "join_raid", "Join Raid"),
                                    new DiscordButtonComponent(ButtonStyle.Secondary, "join_standby", "Join Standby"),
                                    new DiscordButtonComponent(ButtonStyle.Danger, "delete_raid", "Delete Raid")
                                );

                            
                            await interactivityResult.Result.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, responseBuilder);

                            var messageBuilders = new DiscordFollowupMessageBuilder()
                                .WithContent("There are no more join spots!")
                                .AsEphemeral(true);

                            await interactivityResult.Result.Interaction.CreateFollowupMessageAsync(messageBuilders);
                            break;
                        }
                        else
                        {
                           
                            raidData.Users.Add(userId);
                            raidData.JoinSlots[userId] = true;

                            embedBuilder.WithDescription($"**Start Time:** {startTime}\n\nJoin Slots:\n{GetSlotStatus(raidData.JoinSlots, ctx.Member)}\nStandby Slots:\n{GetSlotStatus(raidData.StandbySlots, ctx.Member)}");

                            
                            var responseBuilder = new DiscordInteractionResponseBuilder()
                                .AddEmbed(embedBuilder)
                                .AddComponents(
                                    new DiscordButtonComponent(ButtonStyle.Success, "join_raid", "Join Raid"),
                                    new DiscordButtonComponent(ButtonStyle.Secondary, "join_standby", "Join Standby"),
                                    new DiscordButtonComponent(ButtonStyle.Danger, "delete_raid", "Delete Raid")
                                );

                            
                            await interactivityResult.Result.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, responseBuilder);

                            var messageBuilders = new DiscordFollowupMessageBuilder()
                                .WithContent("You have been added to the raid slot.")
                                .AsEphemeral(true);

                            await interactivityResult.Result.Interaction.CreateFollowupMessageAsync(messageBuilders);

                            //await ctx.create("You have been added to the raid slots.");
                        }
                        break;
                    case "join_standby":
                        // Get the ID of the user who pressed the button
                        ulong standbyUserId = interactivityResult.Result.User.Id;

                        if (raidData.Users.Contains(standbyUserId))
                        {
                           
                            raidData.Users.Remove(standbyUserId);
                            raidData.StandbySlots.Remove(standbyUserId);
                            embedBuilder.WithDescription($"**Start Time:** {startTime}\n\nJoin Slots:\n{GetSlotStatus(raidData.JoinSlots, ctx.Member)}\nStandby Slots:\n{GetSlotStatus(raidData.StandbySlots, ctx.Member)}");

                          
                            var responseBuilder = new DiscordInteractionResponseBuilder()
                                .AddEmbed(embedBuilder)
                                .AddComponents(
                                    new DiscordButtonComponent(ButtonStyle.Success, "join_raid", "Join Raid"),
                                    new DiscordButtonComponent(ButtonStyle.Secondary, "join_standby", "Join Standby"),
                                    new DiscordButtonComponent(ButtonStyle.Danger, "delete_raid", "Delete Raid")
                                );

                            
                            await interactivityResult.Result.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, responseBuilder);

                            var messageBuilders = new DiscordFollowupMessageBuilder()
                                .WithContent("You have been removed from the standby slot.")                                
                                .AsEphemeral(true);

                            await interactivityResult.Result.Interaction.CreateFollowupMessageAsync(messageBuilders);

                            //await ctx.RespondAsync("You have been removed from the standby slots.");
                        }
                        else if (raidData.StandbySlots.Count >= 6)
                        {
                            embedBuilder.WithDescription($"**Start Time:** {startTime}\n\nJoin Slots:\n{GetSlotStatus(raidData.JoinSlots, ctx.Member)}\nStandby Slots:\n{GetSlotStatus(raidData.StandbySlots, ctx.Member)}");

                            
                            var responseBuilder = new DiscordInteractionResponseBuilder()
                                .AddEmbed(embedBuilder)
                                .AddComponents(
                                    new DiscordButtonComponent(ButtonStyle.Success, "join_raid", "Join Raid"),
                                    new DiscordButtonComponent(ButtonStyle.Secondary, "join_standby", "Join Standby"),
                                    new DiscordButtonComponent(ButtonStyle.Danger, "delete_raid", "Delete Raid")
                                );

                            
                            await interactivityResult.Result.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, responseBuilder);

                            var messageBuilders = new DiscordFollowupMessageBuilder()
                                .WithContent("There are no more standby slots!")
                                .AsEphemeral(true);

                            await interactivityResult.Result.Interaction.CreateFollowupMessageAsync(messageBuilders);
                            break;
                        }
                        else
                        {
                            
                            raidData.Users.Add(standbyUserId);
                            raidData.StandbySlots[standbyUserId] = true;

                            embedBuilder.WithDescription($"**Start Time:** {startTime}\n\nJoin Slots:\n{GetSlotStatus(raidData.JoinSlots, ctx.Member)}\nStandby Slots:\n{GetSlotStatus(raidData.StandbySlots, ctx.Member)}");

                            
                            var responseBuilder = new DiscordInteractionResponseBuilder()
                                .AddEmbed(embedBuilder)
                                .AddComponents(
                                    new DiscordButtonComponent(ButtonStyle.Success, "join_raid", "Join Raid"),
                                    new DiscordButtonComponent(ButtonStyle.Secondary, "join_standby", "Join Standby"),
                                    new DiscordButtonComponent(ButtonStyle.Danger, "delete_raid", "Delete Raid")
                                );

                            
                            await interactivityResult.Result.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, responseBuilder);

                            var messageBuilders = new DiscordFollowupMessageBuilder()
                                .WithContent("You have been added to a standby slot.")
                                .AsEphemeral(true);

                            await interactivityResult.Result.Interaction.CreateFollowupMessageAsync(messageBuilders);
                            //await ctx.RespondAsync("You have been added to the standby slots.");
                        }
                        break;
                    case "delete_raid":
                        
                        if (interactivityResult.Result.User is DiscordMember buttonUser && buttonUser.PermissionsIn(ctx.Channel).HasPermission(Permissions.ManageMessages))
                        {
                            RemoveRaid(raidName);
                            await message.DeleteAsync();
                            return;
                        }
                        else
                        {
                            embedBuilder.WithDescription($"**Start Time:** {startTime}\n\nJoin Slots:\n{GetSlotStatus(raidData.JoinSlots, ctx.Member)}\nStandby Slots:\n{GetSlotStatus(raidData.StandbySlots, ctx.Member)}");

                            var responseBuilder = new DiscordInteractionResponseBuilder()
                                .AddEmbed(embedBuilder)
                                .AddComponents(
                                    new DiscordButtonComponent(ButtonStyle.Success, "join_raid", "Join Raid"),
                                    new DiscordButtonComponent(ButtonStyle.Secondary, "join_standby", "Join Standby"),
                                    new DiscordButtonComponent(ButtonStyle.Danger, "delete_raid", "Delete Raid")
                                );

                            
                           


                            var messageBuilders = new DiscordFollowupMessageBuilder()
                                .WithContent("You are not authorized to delete this raid schedule.")
                                .AsEphemeral(true);
                            await interactivityResult.Result.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, responseBuilder);
                            await interactivityResult.Result.Interaction.CreateFollowupMessageAsync(messageBuilders);
                        }
                        break;
                }
            }
        }

        private string GetSlotStatus(Dictionary<ulong, bool> slots, DiscordMember member)
        {
            StringBuilder statusBuilder = new StringBuilder();
            foreach (var slot in slots)
            {
                
                if (slot.Value)
                {
                    
                    if (slot.Key == member.Id)
                    {
                        statusBuilder.AppendLine("-  : " + member.DisplayName); 
                    }
                    else
                    {
                        
                        DiscordMember slotMember = member.Guild.GetMemberAsync(slot.Key).Result;
                        statusBuilder.AppendLine("-  : " + slotMember.DisplayName); 
                    }
                }
                else
                {
                    statusBuilder.AppendLine("-  : "); 
                }
            }
            return statusBuilder.ToString();
        }

        private bool CheckAuthorization(ulong userId)
        {

            List<ulong> authorizedUserIds = new List<ulong>()
    {
        
        653805970610192394, 
        762851502237679637  
    };

            return authorizedUserIds.Contains(userId);
        }

        private void RemoveRaid(string raidName)
        {
            
            if (raidDatabase.TryGetValue(raidName, out RaidData raidData))
            {
                
                raidData.Users.Clear();

                
                raidDatabase.Remove(raidName);
            }
        }

        
    }
    public class JSONStructure
    {
        public string[] BadWords { get; set; }
        public Dictionary<ulong, int> UserPoints { get; set; }
    }

    public class RaidData
    {
        public Dictionary<ulong, bool> JoinSlots { get; set; }
        public Dictionary<ulong, bool> StandbySlots { get; set; }
        public List<ulong> Users { get; set; } = new List<ulong>(); 

        public RaidData()
        {
            JoinSlots = new Dictionary<ulong, bool>();
            StandbySlots = new Dictionary<ulong, bool>();
        }
    }





}






















// copy and paste commands 

// json reader 
/*ulong userId = ctx.User.Id;
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
    // If file doesn't exist, create a new instance
    data = new JSONStructure { UserPoints = new Dictionary<ulong, int>() };
}

// Check if UserPoints is null, then initialize it
if (data.UserPoints == null)
{
    data.UserPoints = new Dictionary<ulong, int>();
}



// Check if user ID exists in the JSON data
if (data != null)
{
    // Check if data is not null before accessing UserPoints dictionary
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
        // If user ID doesn't exist, add it with 500 points
        data.UserPoints[userId] = 500;
        userPoints = 500; // Set userPoints variable to 500

        // Serialize and save the updated data back to the JSON file
        string updatedJsonData = JsonConvert.SerializeObject(data);
        File.WriteAllText(filePath, updatedJsonData);
    }

}*/
//---------------------------------------------------------------------//
// json writer 


/*data.UserPoints[userId] += PointAmount;
userPoints = data.UserPoints[userId]; // Update userPoints variable

// Serialize and save the updated data back to the JSON file
string updatedJsonData1 = JsonConvert.SerializeObject(data);
File.WriteAllText(filePath, updatedJsonData1);*/



//-----------------------------------------------

/*if (user.IsBot)
{
    var embedBuilder = new DiscordEmbedBuilder
    {
        Title = $"ERROR 847",
        Description = $"Bots can't interact with database Error 847...",
        Color = DiscordColor.Red,
       
    };
    embedBuilder.AddField("Error codes", $"Please user !!ErrorCode (error code) for more information", inline: true);
    await ctx.Channel.SendMessageAsync(embed: embedBuilder);

}
*/










