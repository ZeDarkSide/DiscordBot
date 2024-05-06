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
    public class DestinyTwo : BaseCommandModule
    {

        #region Destiny Raid system
        [Command("raid")]
        [RequireBotPermissions(Permissions.ManageMessages)]
        public async Task Raid(CommandContext ctx, string raidAlias, string startTime = "undefined")
        {

            var requiredRole = ctx.Guild.Roles.Values.FirstOrDefault(role => role.Name.ToLower() == "clan member");

            if (requiredRole == null)
            {
                await ctx.RespondAsync("The 'clan member' role does not exist.");
                return;
            }
            var member = await ctx.Guild.GetMemberAsync(ctx.User.Id);
            if (!member.Roles.Contains(requiredRole))
            {
                await ctx.RespondAsync("You do not have permission to use this command.");
                return;
            }
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

            string raidName;
            if (!raidNameAliases.TryGetValue(raidAlias.ToLower(), out raidName))
            {
                await ctx.RespondAsync("Invalid raid name alias. Please use a valid raid name alias.");
                return;
            }

            var embedBuilder = new DiscordEmbedBuilder()
                 .WithTitle($"**{raidName}**")
                 .WithDescription($"**Start Time:** {startTime}")
                 .WithColor(new DiscordColor("#ff473b"))
                 .WithFooter("Raid Schedule")
                 .AddField("Join Slots", "*", inline: true)
                 .AddField("Standby Slots", "*", inline: true);

            string imageUrl = "";
            switch (raidAlias.ToLower())
            {
                case "crota":
                    imageUrl = "https://www.blueberries.gg/wp-content/uploads/2023/08/Crotas-End-Raid-gameplay-Destiny-2-1536x864.jpg";
                    break;
                case "ron":
                    imageUrl = "https://www.blueberries.gg/wp-content/uploads/2023/03/Root-of-Nightmares-Raid-Featured-Destiny-2-1536x864.jpg";
                    break;
                case "vog":
                    imageUrl = "https://www.blueberries.gg/wp-content/uploads/2022/03/Vault-of-Glass-Destiny-2-900p.jpg";
                    break;
                case "kf":
                    imageUrl = "https://www.blueberries.gg/wp-content/uploads/2022/08/Kings-Fall-Destiny-2-900p-1536x864.jpg";
                    break;
                case "lw":
                    imageUrl = "https://www.blueberries.gg/wp-content/uploads/2022/01/Last-Wish-raid-Destiny-2-900p-1536x864.jpeg";
                    break;
                case "dsc":
                    imageUrl = "https://www.blueberries.gg/wp-content/uploads/2022/03/Deep-Stone-Crypt-Destiny-2-900p.jpg";
                    break;
                case "gos":
                    imageUrl = "https://www.blueberries.gg/wp-content/uploads/2022/01/Garden-of-Salvation-Destiny-2-900p-1536x864.jpeg";
                    break;
                case "vow":
                    imageUrl = "https://www.blueberries.gg/wp-content/uploads/2022/03/Vow-of-the-Disciple-Destiny-2-900p-1-1536x864.jpg";
                    break;
            }

            var messageBuilder = new DiscordMessageBuilder()
                .WithEmbed(embedBuilder.WithImageUrl(imageUrl).Build())
                .AddComponents(
                    new DiscordButtonComponent(ButtonStyle.Success, $"join_raid_{ctx.Message.Id}", "Join Raid"),
                    new DiscordButtonComponent(ButtonStyle.Secondary, $"join_standby_{ctx.Message.Id}", "Join Standby"),
                    new DiscordButtonComponent(ButtonStyle.Danger, $"delete_raid_{ctx.Message.Id}", "Delete Raid")
                );
            await ctx.Message.DeleteAsync();
            var message = await ctx.RespondAsync(messageBuilder);

            var messageId = message.Id;
            messageBuilder = new DiscordMessageBuilder()
                .WithEmbed(embedBuilder.WithImageUrl(imageUrl).Build())
                .AddComponents(
                    new DiscordButtonComponent(ButtonStyle.Success, $"join_raid_{messageId}", "Join Raid"),
                    new DiscordButtonComponent(ButtonStyle.Secondary, $"join_standby_{messageId}", "Join Standby"),
                    new DiscordButtonComponent(ButtonStyle.Danger, $"delete_raid_{messageId}_{ctx.User.Id}", "Delete Raid")
                );

            await message.ModifyAsync(messageBuilder);
        }


        public class ButtonClickedEventArgs : EventArgs
        {
            public DiscordInteraction Interaction { get; }

            public ButtonClickedEventArgs(DiscordInteraction interaction)
            {
                Interaction = interaction;
            }
        }

        public delegate Task ButtonClickedEventHandler(object sender, ButtonClickedEventArgs e);

        public static event ButtonClickedEventHandler ButtonClicked;
        #endregion











    }
}
