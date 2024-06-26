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
    public class DestinyTwo : BaseCommandModule
    {

        private static bool isRaidCommandEnabled = true;

        #region Destiny Raid system
        [Command("raid")]
        [RequireBotPermissions(Permissions.ManageMessages)]
        public async Task Raid(CommandContext ctx, string raidAlias, string startTime = "undefined", [RemainingText] string Desc = "")
        {
            if (!isRaidCommandEnabled)
            {
                await ctx.RespondAsync("The raid command is currently disabled due to a fatal error or up coming maintenance.");
                return;
            }

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

            if (string.IsNullOrEmpty(startTime))
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
        { "vow", "Vow of the Disciple" },
        { "dsc", "Deep Stone Crypt" },
        { "se", "Salvation’s Edge" }
    };

            if (!raidNameAliases.TryGetValue(raidAlias.ToLower(), out string raidName))
            {
                await ctx.RespondAsync("Invalid raid name alias. Please use a valid raid name alias.");
                return;
            }

            var embedBuilder = new DiscordEmbedBuilder()
                .WithTitle($"**{raidName}**")
                .WithDescription($"**Start Time:** {startTime}\n\n{Desc}")
                .WithColor(new DiscordColor("#ff473b"))
                .WithFooter($"Raid ID: {ctx.Message.Id} | Created by: {ctx.User.Id}")
                .AddField("Join Slots", "*", inline: true)
                .AddField("Standby Slots", "*", inline: true);

            string imageUrl = raidAlias.ToLower() switch
            {
                "crota" => "https://www.blueberries.gg/wp-content/uploads/2023/08/Crotas-End-Raid-gameplay-Destiny-2-1536x864.jpg",
                "ron" => "https://www.blueberries.gg/wp-content/uploads/2023/03/Root-of-Nightmares-Raid-Featured-Destiny-2-1536x864.jpg",
                "vog" => "https://www.blueberries.gg/wp-content/uploads/2022/03/Vault-of-Glass-Destiny-2-900p.jpg",
                "kf" => "https://www.blueberries.gg/wp-content/uploads/2022/08/Kings-Fall-Destiny-2-900p-1536x864.jpg",
                "lw" => "https://www.blueberries.gg/wp-content/uploads/2022/01/Last-Wish-raid-Destiny-2-900p-1536x864.jpeg",
                "dsc" => "https://www.blueberries.gg/wp-content/uploads/2022/03/Deep-Stone-Crypt-Destiny-2-900p.jpg",
                "gos" => "https://www.blueberries.gg/wp-content/uploads/2022/01/Garden-of-Salvation-Destiny-2-900p-1536x864.jpeg",
                "vow" => "https://www.blueberries.gg/wp-content/uploads/2022/03/Vow-of-the-Disciple-Destiny-2-900p-1-1536x864.jpg",
                "se" => "https://www.bungie.net/img/destiny_content/pgcr/raid_splinter.jpg",
                _ => ""
            };

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
            embedBuilder.WithFooter($"Raid ID: {messageId} | Created by: {ctx.User.Id}");

            messageBuilder = new DiscordMessageBuilder()
                .WithEmbed(embedBuilder.WithImageUrl(imageUrl).Build())
                .AddComponents(
                    new DiscordButtonComponent(ButtonStyle.Success, $"join_raid_{messageId}_{ctx.User.Id}", "Join Raid"),
                    new DiscordButtonComponent(ButtonStyle.Secondary, $"join_standby_{messageId}_{ctx.User.Id}", "Join Standby"),
                    new DiscordButtonComponent(ButtonStyle.Danger, $"delete_raid_{messageId}_{ctx.User.Id}", "Delete Raid")
                );

            await message.ModifyAsync(messageBuilder);
        }


        [Command("exc")]
        [RequireBotPermissions(Permissions.ManageMessages)]
        public async Task GmMaker(CommandContext ctx,  string startTime = "undefined", [RemainingText] string Desc = "")
        {




            var embedBuilder = new DiscordEmbedBuilder()
                 .WithTitle($"**Excision Grandmaster**")
                 .WithDescription($"**Start Time:** {startTime}\n\n{Desc}")
                 .WithColor(new DiscordColor("#ff473b"))
                 .WithFooter("Excision GM Schedule", "https://images-ext-1.discordapp.net/external/OF0r3zkCIyqBD9kLTZDqZ0Sv-2sblM0-ZpdRwNRPbxs/https/img.icons8.com/cotton/64/000000/info.png")
                 .AddField("Join Slots", "*", inline: true)
                 .AddField("Standby Slots", "*", inline: true);
            string imageurl1 = "https://i.imgur.com/gUroiT6.jpeg";
            ulong userID = ctx.User.Id;
            var messageBuilder = new DiscordMessageBuilder()
                .WithEmbed(embedBuilder.Build())
                .AddComponents(
                    new DiscordButtonComponent(ButtonStyle.Success, $"join_GM_{ctx.Message.Id}", "Join GM"),
                    new DiscordButtonComponent(ButtonStyle.Secondary, $"join_standbyGM_{ctx.Message.Id}", "Join Standby"),
                    new DiscordButtonComponent(ButtonStyle.Danger, $"delete_GM_{ctx.Message.Id}", "Delete GM")
                );
            await ctx.Message.DeleteAsync();
            var message = await ctx.RespondAsync(messageBuilder);

            var messageId = message.Id;
            messageBuilder = new DiscordMessageBuilder()
                .WithEmbed(embedBuilder.WithImageUrl(imageurl1).Build())
                .AddComponents(
                    new DiscordButtonComponent(ButtonStyle.Success, $"join_GM_{messageId}_{userID}", "Join Excision"),
                    new DiscordButtonComponent(ButtonStyle.Secondary, $"join_standbyGM_{messageId}_{userID}", "Join Standby"),
                    new DiscordButtonComponent(ButtonStyle.Danger, $"delete_GM_{messageId}_{userID}", "Delete Excision")
                );

            await message.ModifyAsync(messageBuilder);
        }




        [Command("raidping")]
        [RequireBotPermissions(Permissions.MentionEveryone)]
        public async Task RaidPing(CommandContext ctx, ulong messageId, [RemainingText] string Desc = "")
        {
            try
            {
                // Retrieve the original message
                var originalMessage = await ctx.Channel.GetMessageAsync(messageId);
                if (originalMessage == null)
                {
                    await ctx.RespondAsync("Message not found.");
                    return;
                }
                // Check if the message contains an embed
                var embed = originalMessage.Embeds.FirstOrDefault();
                if (embed == null)
                {
                    await ctx.RespondAsync("No embed found in the message.");
                    return;
                }

                // Get the "Join Slots" field
                var joinField = embed.Fields.FirstOrDefault(f => f.Name == "Join Slots");
                if (joinField == null || joinField.Value == "*")
                {
                    await ctx.RespondAsync("No one has joined this raid yet.");
                    return;
                }

                // Extract the mentions from the "Join Slots" field
                var mentions = joinField.Value.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                // Create a ping message
                await ctx.Message.DeleteAsync();
                var pingMessage = string.Join(" ", mentions);
                await ctx.RespondAsync($"{pingMessage} {Desc}");
            }
            catch (Exception ex)
            {
                await ctx.RespondAsync($"An error occurred: {ex.Message}");
            }
        }


        [Command("changeraidtime")]
        [RequireBotPermissions(Permissions.ManageMessages)]
        public async Task ChangeRaidTime(CommandContext ctx, ulong messageId, string newStartTime)
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

            var message = await ctx.Channel.GetMessageAsync(messageId);
            if (message == null)
            {
                await ctx.RespondAsync("The specified message ID does not exist.");
                return;
            }

            var embed = message.Embeds.FirstOrDefault();
            if (embed == null)
            {
                await ctx.RespondAsync("The specified message does not contain an embed.");
                return;
            }

            var footerText = embed.Footer?.Text;
            if (footerText == null || !footerText.Contains("Created by:"))
            {
                await ctx.RespondAsync("The specified embed does not contain the creator's information.");
                return;
            }

            var creatorIdString = footerText.Split("Created by:").Last().Trim();
            if (!ulong.TryParse(creatorIdString, out ulong creatorId))
            {
                await ctx.RespondAsync("The specified embed contains invalid creator information.");
                return;
            }

            if (creatorId != ctx.User.Id)
            {
                await ctx.RespondAsync("You do not have permission to change the start time of this raid post.");
                return;
            }

            var descriptionParts = embed.Description.Split(new string[] { "\n\n" }, StringSplitOptions.None);
            if (descriptionParts.Length < 2)
            {
                await ctx.RespondAsync("The specified embed does not contain a start time.");
                return;
            }

            var newDescription = $"**Start Time:** {newStartTime}\n\n{descriptionParts[1]}";
            var embedBuilder = new DiscordEmbedBuilder(embed)
                .WithDescription(newDescription);

            var messageBuilder = new DiscordMessageBuilder()
                .WithEmbed(embedBuilder.Build())
                .AddComponents(
                    new DiscordButtonComponent(ButtonStyle.Success, $"join_raid_{messageId}_{ctx.User.Id}", "Join Raid"),
                    new DiscordButtonComponent(ButtonStyle.Secondary, $"join_standby_{messageId}_{ctx.User.Id}", "Join Standby"),
                    new DiscordButtonComponent(ButtonStyle.Danger, $"delete_raid_{messageId}_{ctx.User.Id}", "Delete Raid")
                );

            await message.ModifyAsync(messageBuilder);
            await ctx.RespondAsync("The raid start time has been updated.");
        }

        [Command("raidpower")]
        [RequireOwner]
        public async Task RaidPower(CommandContext ctx, string status)
        {
            if (ctx.User.Id != 653805970610192394) // Replace with your Discord user ID
            {
                await ctx.RespondAsync("You do not have permission to use this command.");
                return;
            }

            switch (status.ToLower())
            {
                case "on":
                    isRaidCommandEnabled = true;
                    await ctx.RespondAsync("The raid command is now enabled.");
                    break;
                case "off":
                    isRaidCommandEnabled = false;
                    await ctx.RespondAsync("The raid command is now disabled.");
                    break;
                default:
                    await ctx.RespondAsync("Invalid status. Use 'on' or 'off'.");
                    break;
            }
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
