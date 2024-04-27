using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json; // Added using directive for Newtonsoft.Json
using System;
using System.Collections.Generic;
using System.IO;

using System.Threading.Tasks;
using System.Xml.Linq;
using ZeDarkSide_Discord_Bot.config;
namespace ZeDarkSide_Discord_Bot.Commands
{
    internal class emergency : BaseCommandModule
    {




        [Command("PauseBot")]
        public async Task EMERGENCY(CommandContext ctx)
        {

            if (ctx.User.Id == 653805970610192394)
            {
                ulong userId = ctx.User.Id;
                int userPoints = 0;

                string filePath = "Pause.json";
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
                        data.UserPoints[userId] = 1;
                        userPoints = 1; // Set userPoints variable to 500

                        // Serialize and save the updated data back to the JSON file
                        string updatedJsonData = JsonConvert.SerializeObject(data);
                        File.WriteAllText(filePath, updatedJsonData);
                    }

                }



                if (userPoints == 0)
                {
                    data.UserPoints[userId] = 1;
                    userPoints = 1; // Set userPoints variable to 500

                    // Serialize and save the updated data back to the JSON file
                    string updatedJsonData = JsonConvert.SerializeObject(data);
                    File.WriteAllText(filePath, updatedJsonData);
                }else if (userPoints == 1)
                {
                    data.UserPoints[userId] = 0;
                    userPoints = 0; // Set userPoints variable to 500

                    // Serialize and save the updated data back to the JSON file
                    string updatedJsonData = JsonConvert.SerializeObject(data);
                    File.WriteAllText(filePath, updatedJsonData);
                }


            }
            
        }

        // do later!

    }
}
