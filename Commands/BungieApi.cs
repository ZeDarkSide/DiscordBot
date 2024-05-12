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
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Drawing;

namespace ZeDarkSide_Discord_Bot.Commands
{
    public class BungieApi : BaseCommandModule
    {



        private const string BUNGIE_API_KEY = "blank";
        private const string BUNGIE_API_URL = "https://www.bungie.net/Platform";
        private const string API_URL = "https://example.com/api/weapons";
       

        [Command("weekly")]
        public async Task Weekly(CommandContext ctx)
        {
            string RaidInfo = "";
            string imageUrl = "";
            var client = new RestClient(BUNGIE_API_URL);
            var request = new RestRequest("/Destiny2/Milestones/", Method.Get);
            request.AddHeader("X-API-Key", BUNGIE_API_KEY);

            var response = await client.ExecuteAsync(request);

            var jsonResponse = JObject.Parse(response.Content);

            if (jsonResponse["Response"] != null && jsonResponse["Response"][541780856.ToString()] != null)
            {
                var activityHash = jsonResponse["Response"][541780856.ToString()]["activities"][0]["activityHash"].ToString();

                long activityHashLong; 
                
                if (long.TryParse(activityHash, out activityHashLong)) 
                {
                    if (activityHashLong == 910380154)
                    {
                        RaidInfo = "Deep Stone Crypt";
                        imageUrl = "https://www.blueberries.gg/wp-content/uploads/2022/03/Deep-Stone-Crypt-Destiny-2-900p.jpg";
                    }
                    else if (activityHashLong == 4179289725)
                    {
                        RaidInfo = "Crota's End";
                        imageUrl = "https://www.blueberries.gg/wp-content/uploads/2023/08/Crotas-End-Raid-gameplay-Destiny-2-1536x864.jpg";
                    }
                    else if (activityHashLong == 2381413764)
                    {
                        RaidInfo = "Root of Nightmares";
                        imageUrl = "https://www.blueberries.gg/wp-content/uploads/2023/03/Root-of-Nightmares-Raid-Featured-Destiny-2-1536x864.jpg";
                    }
                    else if (activityHashLong == 1374392663)
                    {
                        RaidInfo = "King's Fall";
                        imageUrl = "https://www.blueberries.gg/wp-content/uploads/2022/08/Kings-Fall-Destiny-2-900p-1536x864.jpg";
                    }
                    else if (activityHashLong == 1441982566)
                    {
                        RaidInfo = "Vow of the Disciple";
                        imageUrl = "https://www.blueberries.gg/wp-content/uploads/2022/03/Vow-of-the-Disciple-Destiny-2-900p-1-1536x864.jpg";
                    }
                    else if (activityHashLong == 3458480158)
                    {
                        RaidInfo = "Garden of Salvation";
                        imageUrl = "https://www.blueberries.gg/wp-content/uploads/2022/01/Garden-of-Salvation-Destiny-2-900p-1536x864.jpeg";
                    }
                    else if (activityHashLong == 2122313384)
                    {
                        RaidInfo = "Last Wish";
                        imageUrl = "https://www.blueberries.gg/wp-content/uploads/2022/01/Last-Wish-raid-Destiny-2-900p-1536x864.jpeg";
                    }
                    else if (activityHashLong == 3881495763)
                    {
                        RaidInfo = "Vault of Glass";
                        imageUrl = "https://www.blueberries.gg/wp-content/uploads/2022/03/Vault-of-Glass-Destiny-2-900p.jpg";
                    }
                    else
                    {
                        await ctx.RespondAsync("Unknow");
                    }
                }
                else
                {
                    await ctx.RespondAsync("Error: please report to dev!");
                }
            }
            else
            {
                await ctx.RespondAsync("Error: Request not found if you think this is a issue report it to the developer!");
            }
            string DungeonInfo = "";
            if (jsonResponse["Response"] != null && jsonResponse["Response"][422102671.ToString()] != null)
            {
                var activityHash = jsonResponse["Response"][422102671.ToString()]["activities"][0]["activityHash"].ToString();

                long activityHashLong;

                if (long.TryParse(activityHash, out activityHashLong))
                {
                    if (activityHashLong == 2582501063)
                    {
                        DungeonInfo = "Pit of Heresy";
                    }
                    else if (activityHashLong == 2032534090)
                    {
                        DungeonInfo = "Shattered Throne";
                    }
                    else if (activityHashLong == 1077850348)
                    {
                        DungeonInfo = "Prophecy";
                    }
                    else if (activityHashLong == 1262462921)
                    {
                        DungeonInfo = "Spire of the Watcher";
                    }
                    else if (activityHashLong == 4078656646)
                    {
                        DungeonInfo = "Grasp of Avarice";
                    }
                    else if (activityHashLong == 2823159265)
                    {
                        DungeonInfo = "Duality";
                    }
                    else if (activityHashLong == 313828469)
                    {
                        RaidInfo = "Ghosts of the Deep";
                    }
                    else
                    {
                        await ctx.RespondAsync("Unknow");
                    }
                }
                else
                {
                    await ctx.RespondAsync("Error: please report to dev!");
                }
            }
            else
            {
                await ctx.RespondAsync("Error: Request not found if you think this is a issue report it to the developer!");
            }


            var embedBuilder = new DiscordEmbedBuilder()
                .WithTitle("Weekly Rotation")
                .AddField("Raid", $"**{RaidInfo}**", inline: true)
                .AddField("Dungeon", $"**{DungeonInfo}**", inline: true)
                .WithImageUrl(imageUrl)
                .WithColor(new DiscordColor("#ff473b"));

            await ctx.RespondAsync(embed: embedBuilder);

        }




    }
}
