using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.CommandsNext.Executors;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Logging;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeDarkSide_Discord_Bot.Commands;
using ZeDarkSide_Discord_Bot.config;

namespace ZeDarkSide_Discord_Bot
{
    internal class main
    {
        public static DiscordClient Client { get; set; }
        public static CommandsNextExtension Commands { get; set; }

        static async Task Main(string[] args)
        {
            var jsonreader = new JSONReader();
            await jsonreader.ReadJson();

            var discordConfig = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
               Token = jsonreader.token,
              
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                ReconnectIndefinitely = true
            };

            Client = new DiscordClient(discordConfig);
            Client.UseInteractivity(new InteractivityConfiguration() { Timeout = TimeSpan.FromMinutes(1) });

            Client.Ready += Client_Ready;

            var commandConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { jsonreader.prefix },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = false,
                CommandExecutor = new AsynchronousCommandExecutor(),
            };

            Commands = Client.UseCommandsNext(commandConfig);
            var slashCommandsConfiguration = Client.UseSlashCommands();

            // Commands event handler 
            Commands.CommandErrored += Commands_CommandErrored;

            // Register commands
            Commands.RegisterCommands<Tools>();
            Commands.RegisterCommands<Gamble>();
            slashCommandsConfiguration.RegisterCommands<ToolsSlash>();


            Client.ComponentInteractionCreated += ComponentInteractionCreated;

            await Client.ConnectAsync(activity: new DiscordActivity("Your Raids", ActivityType.ListeningTo));
            await Task.Delay(-1);
        }

        private static async Task Commands_CommandErrored(CommandsNextExtension sender, CommandErrorEventArgs args)
        {
            if (args.Exception is ChecksFailedException exception)
            {
                string timeLeft = string.Empty;
                foreach (var check in exception.FailedChecks)
                {
                    var coolDown = (CooldownAttribute)check;
                    timeLeft = coolDown.GetRemainingCooldown(args.Context).ToString(@"hh\:mm\:ss");
                }

                var coolDownMessage = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Red,
                    Title = "Please let the bot rest for a second!",
                    Description = $"Time Remaining: {timeLeft}"
                };

                await args.Context.Channel.SendMessageAsync(embed: coolDownMessage);
            }
        }

        private static Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs args)
        {
            return Task.CompletedTask;
        }

        private static async Task ComponentInteractionCreated(DiscordClient sender, ComponentInteractionCreateEventArgs e)
        {
            var interaction = e.Interaction;

            if (interaction.Type == InteractionType.Component)
            {
                var customId = interaction.Data.CustomId;

                switch (customId)
                {
                    case string joinRaidId when joinRaidId.StartsWith("join_raid_"):
                        Console.WriteLine("Join Raid button pressed");
                        await HandleJoinRaidButton(interaction);
                        break;
                    case string joinStandbyId when joinStandbyId.StartsWith("join_standby_"):
                        Console.WriteLine("Join Standby button pressed");
                        await HandleJoinStandbyButton(interaction);
                        break;
                    case string deleteRaidId when deleteRaidId.StartsWith("delete_raid_"):
                        Console.WriteLine("Delete Raid button pressed");
                        await HandleDeleteRaidButton(interaction);
                        break;
                }
            }
        }
        public static async Task HandleJoinRaidButton(DiscordInteraction interaction)
        {
            try
            {
                ulong messageId;
                if (!ulong.TryParse(interaction.Data.CustomId.Split('_').LastOrDefault(), out messageId))
                {
                    Console.WriteLine($"Failed to parse message ID from custom ID: {interaction.Data.CustomId}");
                    return;
                }

                var originalMessage = await interaction.Channel.GetMessageAsync(messageId);
                if (originalMessage == null)
                {
                    Console.WriteLine($"Failed to retrieve original message with ID: {messageId}");
                    return;
                }

                var embed = originalMessage.Embeds.First();
                var joinField = embed.Fields.FirstOrDefault(f => f.Name == "Join Slots");
                if (joinField == null)
                {
                    Console.WriteLine("Join Slots field not found in the embed.");
                    return;
                }

                var currentJoinSlots = joinField.Value.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var user = interaction.User;

                if (currentJoinSlots.Contains(user.Mention))
                {
                    // User is already in the join slots, remove them
                    currentJoinSlots.Remove(user.Mention);
                    await interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"You have been removed from the join slots.").AsEphemeral());
                }
                else
                {
                    // Check if there are available slots for joining
                    if (currentJoinSlots.Count <= 6)
                    {
                        // Add the user to the join slots
                        currentJoinSlots.Add(user.Mention);
                        await interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"You have been added to the join slots.").AsEphemeral());

                    }
                    else
                    {
                        // Notify the user that join slots are full
                        // Notify the user that join slots are full
                        await interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("There are no more join slots available!").AsEphemeral());




                        return;
                    }
                }

                // Update the join slots field in the embed
                var updatedJoinField = currentJoinSlots.Any() ? string.Join("\n", currentJoinSlots) : "*"; // Set to "Empty" if there are no users
                embed.Fields.First(f => f.Name == "Join Slots").Value = updatedJoinField;

                // Create a new message with the updated embed
                var messageBuilder = new DiscordMessageBuilder()
                    .WithContent(originalMessage.Content)
                    .WithEmbed(embed)
                    .AddComponents(
                        new DiscordButtonComponent(ButtonStyle.Success, $"join_raid_{messageId}", "Join Raid"),
                        new DiscordButtonComponent(ButtonStyle.Secondary, $"join_standby_{messageId}", "Join Standby"),
                        new DiscordButtonComponent(ButtonStyle.Danger, $"delete_raid_{messageId}", "Delete Raid")
                    );

                // Replace the original message with the updated message
                var newMessage = await originalMessage.ModifyAsync(messageBuilder);

                Console.WriteLine($"Join action completed. New message ID: {newMessage.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while handling the join raid button: {ex.Message}");
            }
        }


        public static async Task HandleJoinStandbyButton(DiscordInteraction interaction)
        {
            try
            {
                ulong messageId;
                if (!ulong.TryParse(interaction.Data.CustomId.Split('_').LastOrDefault(), out messageId))
                {
                    Console.WriteLine($"Failed to parse message ID from custom ID: {interaction.Data.CustomId}");
                    return;
                }

                var originalMessage = await interaction.Channel.GetMessageAsync(messageId);
                if (originalMessage == null)
                {
                    Console.WriteLine($"Failed to retrieve original message with ID: {messageId}");
                    return;
                }

                var embed = originalMessage.Embeds.First();
                var standbyField = embed.Fields.FirstOrDefault(f => f.Name == "Standby Slots");
                if (standbyField == null)
                {
                    Console.WriteLine("Standby Slots field not found in the embed.");
                    return;
                }

                var currentStandbySlots = standbyField.Value.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var user = interaction.User;

                if (currentStandbySlots.Contains(user.Mention))
                {
                    // User is already in the standby slots, remove them
                    currentStandbySlots.Remove(user.Mention);
                    await interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"You have been removed from the standby slots.").AsEphemeral());
                }
                else
                {
                    // Check if there are available slots for joining
                    if (currentStandbySlots.Count <= 6)
                    {
                        // Add the user to the standby slots
                        currentStandbySlots.Add(user.Mention);
                        await interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"You have been added to the standby slots.").AsEphemeral());
                    }
                    else
                    {
                        // Notify the user that standby slots are full
                        await interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("There are no more standby slots available!").AsEphemeral());
                        return;
                    }
                }

                // Update the standby slots field in the embed
                var updatedStandbyField = currentStandbySlots.Any() ? string.Join("\n", currentStandbySlots) : "*"; // Set to "*" if there are no users
                embed.Fields.First(f => f.Name == "Standby Slots").Value = updatedStandbyField;

                // Create a new message with the updated embed
                var messageBuilder = new DiscordMessageBuilder()
                    .WithContent(originalMessage.Content)
                    .WithEmbed(embed)
                    .AddComponents(
                        new DiscordButtonComponent(ButtonStyle.Success, $"join_raid_{messageId}", "Join Raid"),
                        new DiscordButtonComponent(ButtonStyle.Secondary, $"join_standby_{messageId}", "Join Standby"),
                        new DiscordButtonComponent(ButtonStyle.Danger, $"delete_raid_{messageId}", "Delete Raid")
                    );

                // Replace the original message with the updated message
                var newMessage = await originalMessage.ModifyAsync(messageBuilder);

                Console.WriteLine($"Join action completed. New message ID: {newMessage.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while handling the join standby button: {ex.Message}");
            }
        }


        public static async Task HandleDeleteRaidButton(DiscordInteraction interaction)
        {
            try
            {
                // Retrieve the original message ID and author ID from the interaction data
                ulong messageId;
                ulong authorId;
                var customIdParts = interaction.Data.CustomId.Split('_');
                if (customIdParts.Length != 4 ||
                    customIdParts[0] != "delete" ||
                    !ulong.TryParse(customIdParts[2], out messageId) ||
                    !ulong.TryParse(customIdParts[3], out authorId))
                {
                    Console.WriteLine($"Failed to parse message ID or author ID from custom ID: {interaction.Data.CustomId}");
                    return;
                }

                // Get the original message
                var originalMessage = await interaction.Channel.GetMessageAsync(messageId);
                if (originalMessage == null)
                {
                    Console.WriteLine($"Failed to retrieve original message with ID: {messageId}");
                    return;
                }

                // Check if the user is either the message author or has permission to manage messages
                var member = await interaction.Guild.GetMemberAsync(interaction.User.Id);
                if (member != null && (member.PermissionsIn(interaction.Channel).HasPermission(Permissions.ManageMessages) || authorId == interaction.User.Id))
                {
                    // If the user has permission, delete the original message
                    await originalMessage.DeleteAsync();
                    await interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Raid schedule deleted successfully.").AsEphemeral());
                }
                else
                {
                    // If the user does not have permission, send a response indicating authorization failure
                    await interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("You are not authorized to delete this raid schedule.").AsEphemeral());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while handling the delete raid button: {ex.Message}");
            }
        }









    }
}
