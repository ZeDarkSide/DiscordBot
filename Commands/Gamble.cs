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
using System.Text.RegularExpressions;

namespace ZeDarkSide_Discord_Bot.Commands
{
    public class Gamble : BaseCommandModule
    {





        [Command("rob")]
        [Cooldown(1, 7200, CooldownBucketType.User)]
        public async Task RobSomeone(CommandContext ctx, DiscordMember user)
        {

            var customColor = new DiscordColor(255, 71, 59);
            ulong allowedChannelId = 1114778208903122964;
            if (ctx.Channel.Id == allowedChannelId && ctx.Channel.Id == 778062522888224788)
            {
                await ctx.RespondAsync("This command Has been moved to a / command");
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
            else
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
                        Description = $"But made too many sounds and was about to be caught.",
                        Color = customColor,
                    };
                    await ctx.Channel.SendMessageAsync(embed: embedbuilder);
                }
            }




        }

        [Command("leaderboard")]
        public async Task Leaderboard(CommandContext ctx)
        {

            var customColor = new DiscordColor(255, 71, 59);
            ulong allowedChannelId = 1114778208903122964;

            if (ctx.Channel.Id == allowedChannelId && ctx.Channel.Id == 778062522888224788)
            {
                await ctx.RespondAsync("This command Has been moved to a / command");
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
                Color = customColor
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
        [Command("bank")]
        [Cooldown(1, 5, CooldownBucketType.User)]
        public async Task SeeHowMuchInBank(CommandContext ctx)
        {
            var customColor = new DiscordColor(255, 71, 59);
            ulong allowedChannelId = 1114778208903122964;

            if (ctx.Channel.Id == allowedChannelId && ctx.Channel.Id == 778062522888224788)
            {
                await ctx.RespondAsync("This command Has been moved to a / command");
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

            await ctx.Channel.SendMessageAsync(embed: embedBuilder);
        }












    }
    public class JSONStructure
    {
        public Dictionary<ulong, int> UserPoints { get; set; }
        public Dictionary<ulong, List<string>> UserInventory { get; set; }
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










