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
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net.Http;



namespace ZeDarkSide_Discord_Bot.Commands
{
    public class BungieApi : BaseCommandModule
    {



        private const string BUNGIE_API_KEY = "";
        private const string BUNGIE_API_URL = "https://www.bungie.net/Platform";
        private const string API_URL = "https://example.com/api/weapons";
       

        [Command("weekly")]
        [Cooldown(1,20,CooldownBucketType.Global)]
        public async Task Weekly(CommandContext ctx)
        {

            var loadingMessage = await ctx.RespondAsync("Loading data...");
            string RaidInfo = "";
            string imageUrl = "";
            string LostSectorName = "";
            string NightfallName = "";
            string WeeklyExoticMission = "";
            string WeeklyRaid = "";
            string WeeklyDungeon = "";

            var client = new RestClient(BUNGIE_API_URL);
            var request = new RestRequest("/Destiny2/Milestones/", Method.Get);
            request.AddHeader("X-API-Key", BUNGIE_API_KEY);

            var response = await client.ExecuteAsync(request);

            var jsonResponse = JObject.Parse(response.Content);
            using (var httpClient = new HttpClient())
            {
                var htmlContent = await httpClient.GetStringAsync("https://todayindestiny.com/");
                var lostSectorName = GetLostSectorName(htmlContent);
                var nightfallName = GetNightfallName(htmlContent);
                var weeklyExoticMission = GetWeeklyExoticMission(htmlContent);
                var weeklyraid = GetWeeklyRaid(htmlContent);
                var weeklydungeon = GetWeeklyDungeon(htmlContent);

                if (!string.IsNullOrEmpty(lostSectorName))
                {
                    LostSectorName = lostSectorName;
                }
                else
                {
                    LostSectorName = "Unknown";
                }

                if (!string.IsNullOrEmpty(nightfallName))
                {
                    NightfallName = nightfallName;
                }
                else
                {
                    NightfallName = "Unknown";
                }

                if (!string.IsNullOrEmpty(weeklyExoticMission))
                {
                    WeeklyExoticMission = weeklyExoticMission;
                }
                else
                {
                    WeeklyExoticMission = "Unknown";
                }
                if (!string.IsNullOrEmpty(weeklyraid))
                {
                    WeeklyRaid = weeklyraid;
                }
                else
                {
                    WeeklyRaid = "Unknown";
                }
                if (!string.IsNullOrEmpty(weeklydungeon))
                {
                    WeeklyDungeon = weeklydungeon;
                }
                else
                {
                    WeeklyDungeon = "Unknown";
                }
            }


/*            if (jsonResponse["Response"] != null && jsonResponse["Response"][541780856.ToString()] != null)
            {
                var activityHash = jsonResponse["Response"][541780856.ToString()]["activities"][0]["activityHash"].ToString();

                long activityHashLong; 
                
                if (long.TryParse(activityHash, out activityHashLong)) 
                {
                    if (activityHashLong == 910380154)
                    {
                        RaidInfo = "Deep Stone Crypt";
                        imageUrl = "https://www.bungie.net/img/destiny_content/pgcr/europa-raid-deep-stone-crypt.jpg";
                    }
                    else if (activityHashLong == 4179289725)
                    {
                        RaidInfo = "Crota's End";
                        imageUrl = "https://www.bungie.net/img/destiny_content/pgcr/raid_Crotas_end.jpg";
                    }
                    else if (activityHashLong == 2381413764)
                    {
                        RaidInfo = "Root of Nightmares";
                        imageUrl = "https://www.bungie.net/img/destiny_content/pgcr/raid_root_of_nightmares.jpg";
                    }
                    else if (activityHashLong == 1374392663)
                    {
                        RaidInfo = "King's Fall";
                        imageUrl = "https://www.bungie.net/img/destiny_content/pgcr/raid_nemesis.jpg";
                    }
                    else if (activityHashLong == 1441982566)
                    {
                        RaidInfo = "Vow of the Disciple";
                        imageUrl = "https://www.blueberries.gg/wp-content/uploads/2022/03/Vow-of-the-Disciple-Destiny-2-900p-1-1536x864.jpg";
                    }
                    else if (activityHashLong == 3458480158)
                    {
                        RaidInfo = "Garden of Salvation";
                        imageUrl = "https://www.bungie.net/img/destiny_content/pgcr/raid_garden_of_salvation.jpg";
                    }
                    else if (activityHashLong == 2122313384)
                    {
                        RaidInfo = "Last Wish";
                        imageUrl = "https://www.bungie.net/img/destiny_content/pgcr/raid_beanstalk.jpg";
                    }
                    else if (activityHashLong == 3881495763)
                    {
                        RaidInfo = "Vault of Glass";
                        imageUrl = "https://www.bungie.net/img/destiny_content/pgcr/vault_of_glass.jpg";
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
            }*/

            if (WeeklyRaid == "Deep Stone Crypt")
            {
                RaidInfo = "Deep Stone Crypt";
                imageUrl = "https://www.bungie.net/img/destiny_content/pgcr/europa-raid-deep-stone-crypt.jpg";
            }
            else if (WeeklyRaid == "Crota's End")
            {
                RaidInfo = "Crota's End";
                imageUrl = "https://www.bungie.net/img/destiny_content/pgcr/raid_Crotas_end.jpg";
            }
            else if (WeeklyRaid == "Root of Nightmares")
            {
                RaidInfo = "Root of Nightmares";
                imageUrl = "https://www.bungie.net/img/destiny_content/pgcr/raid_root_of_nightmares.jpg";
            }
            else if (WeeklyRaid == "Kings Fall")
            {
                RaidInfo = "King's Fall";
                imageUrl = "https://www.bungie.net/img/destiny_content/pgcr/raid_nemesis.jpg";
            }
            else if (WeeklyRaid == "Vow of the Disciple")
            {
                RaidInfo = "Vow of the Disciple";
                imageUrl = "https://www.blueberries.gg/wp-content/uploads/2022/03/Vow-of-the-Disciple-Destiny-2-900p-1-1536x864.jpg";
            }
            else if (WeeklyRaid == "Garden of Salvation")
            {
                RaidInfo = "Garden of Salvation";
                imageUrl = "https://www.bungie.net/img/destiny_content/pgcr/raid_garden_of_salvation.jpg";
            }
            else if (WeeklyRaid == "Last Wish")
            {
                RaidInfo = "Last Wish";
                imageUrl = "https://www.bungie.net/img/destiny_content/pgcr/raid_beanstalk.jpg";
            }
            else if (WeeklyRaid == "Vault of Glass")
            {
                RaidInfo = "Vault of Glass";
                imageUrl = "https://www.bungie.net/img/destiny_content/pgcr/vault_of_glass.jpg";
            }



            await loadingMessage.DeleteAsync();
            var embedBuilder = new DiscordEmbedBuilder()
                .WithTitle("Weekly Rotation")
                .AddField("Raid", $"**{WeeklyRaid}\nCrota's End**", inline: true)
                .AddField("Dungeon", $"**{WeeklyDungeon}\nWarlord's Ruin**", inline: true)
                .AddField("Updates Daily \nLost Sector", $"**{LostSectorName}**", inline: false)
                .AddField("NightFall", $"**{NightfallName}**", inline: true)
                .AddField("Weekly Exotic Mission", $"**{WeeklyExoticMission}**", inline: true)
                .WithImageUrl(imageUrl)
                .WithColor(new DiscordColor("#ff473b"));
            await ctx.Message.DeleteAsync();
            await ctx.RespondAsync(embed: embedBuilder.Build());


        }

        
        public string GetLostSectorName(string htmlContent)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            var lostSectorHeader = doc.DocumentNode.SelectSingleNode("//p[@class='eventCardHeaderSet' and contains(text(), 'Lost Sector')]");

            if (lostSectorHeader != null)
            {
                var lostSectorNameNode = lostSectorHeader.NextSibling;
                if (lostSectorNameNode != null && lostSectorNameNode.Name == "p" && lostSectorNameNode.GetAttributeValue("class", "") == "eventCardHeaderName")
                {
                    string lostSectorName = lostSectorNameNode.InnerText.Trim();
                    return lostSectorName;
                }
            }

            return null;
        }
        public string GetNightfallName(string htmlContent)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            var nightfallHeader = doc.DocumentNode.SelectSingleNode("//p[@class='eventCardHeaderSet' and contains(text(), 'Nightfall')]");

            if (nightfallHeader != null)
            {
                var nightfallNameNode = nightfallHeader.NextSibling;
                if (nightfallNameNode != null && nightfallNameNode.Name == "p" && nightfallNameNode.GetAttributeValue("class", "") == "eventCardHeaderName")
                {
                    string nightfallName = nightfallNameNode.InnerText.Trim();
                    return nightfallName;
                }
            }

            return null;
        }
        public string GetWeeklyExoticMission(string htmlContent)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            var weeklyExoticMissionHeader = doc.DocumentNode.SelectSingleNode("//p[@class='eventCardHeaderSet' and contains(text(), 'Weekly Exotic Mission')]");

            if (weeklyExoticMissionHeader != null)
            {
                var weeklyExoticMissionNameNode = weeklyExoticMissionHeader.NextSibling;
                if (weeklyExoticMissionNameNode != null && weeklyExoticMissionNameNode.Name == "p" && weeklyExoticMissionNameNode.GetAttributeValue("class", "") == "eventCardHeaderName")
                {
                    string weeklyExoticMissionName = weeklyExoticMissionNameNode.InnerText.Trim();
                    return weeklyExoticMissionName;
                }
            }

            return null;
        }
        public string GetWeeklyRaid(string htmlContent)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            var weeklyExoticRaidHeader = doc.DocumentNode.SelectSingleNode("//p[@class='eventCardHeaderSet' and contains(text(), 'Weekly Raid')]");

            if (weeklyExoticRaidHeader != null)
            {
                var weeklyExoticRaidNameNode = weeklyExoticRaidHeader.NextSibling;
                if (weeklyExoticRaidNameNode != null && weeklyExoticRaidNameNode.Name == "p" && weeklyExoticRaidNameNode.GetAttributeValue("class", "") == "eventCardHeaderName")
                {
                    string weeklyExoticRaidName = weeklyExoticRaidNameNode.InnerText.Trim();
                    return weeklyExoticRaidName;
                }
            }

            return null;
        }
        public string GetWeeklyDungeon(string htmlContent)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            var weeklyExoticDungeonHeader = doc.DocumentNode.SelectSingleNode("//p[@class='eventCardHeaderSet' and contains(text(), 'Weekly Dungeon')]");

            if (weeklyExoticDungeonHeader != null)
            {
                var weeklyExoticDungeonNameNode = weeklyExoticDungeonHeader.NextSibling;
                if (weeklyExoticDungeonNameNode != null && weeklyExoticDungeonNameNode.Name == "p" && weeklyExoticDungeonNameNode.GetAttributeValue("class", "") == "eventCardHeaderName")
                {
                    string weeklyExoticDungeonName = weeklyExoticDungeonNameNode.InnerText.Trim();
                    return weeklyExoticDungeonName;
                }
            }

            return null;
        }

    }



}
