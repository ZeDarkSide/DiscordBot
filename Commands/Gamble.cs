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
        public async Task RobSomeone(CommandContext ctx, DiscordMember user)
        {

            var customColor = new DiscordColor(255, 71, 59);
            ulong allowedChannelId = 1114778208903122964;
            if (ctx.Channel.Id != allowedChannelId && ctx.Channel.Id != 778062522888224788)
            {
                await ctx.RespondAsync("This command Has been moved to a / command");
                return;
            }
            else
            {
                await ctx.RespondAsync("This command Has been moved to a / command");
                return;
            }

        }

        [Command("leaderboard")]
        public async Task Leaderboard(CommandContext ctx)
        {

            var customColor = new DiscordColor(255, 71, 59);
            ulong allowedChannelId = 1114778208903122964;

            if (ctx.Channel.Id != allowedChannelId && ctx.Channel.Id != 778062522888224788)
            {
                await ctx.RespondAsync("This command Has been moved to a / command");
                return;
            }
            else
            {
                await ctx.RespondAsync("This command Has been moved to a / command");
                return;
            }
         
        }

        [Command("bank")]
        public async Task SeeHowMuchInBank(CommandContext ctx)
        {
            var customColor = new DiscordColor(255, 71, 59);
            ulong allowedChannelId = 1114778208903122964;

            if (ctx.Channel.Id != allowedChannelId && ctx.Channel.Id != 778062522888224788)
            {
                await ctx.RespondAsync("This command Has been moved to a / command");
                return;
            }
            else
            {
                await ctx.RespondAsync("This command Has been moved to a / command");
                return;
            }
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










