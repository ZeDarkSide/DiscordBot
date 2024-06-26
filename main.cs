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
using Sentry;
using Sentry.Profiling;

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
                Intents = DiscordIntents.DirectMessageReactions
                | DiscordIntents.DirectMessages
                | DiscordIntents.GuildEmojis
                | DiscordIntents.GuildInvites
                | DiscordIntents.GuildMembers
                | DiscordIntents.GuildMessages
                | DiscordIntents.MessageContents
                | DiscordIntents.GuildIntegrations
                | DiscordIntents.Guilds
                | DiscordIntents.GuildWebhooks,
                //Token = jsonreader.token,
                
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
            Commands.RegisterCommands<Economy>();
            Commands.RegisterCommands<DestinyTwo>();
            Commands.RegisterCommands<BungieApi>();
            slashCommandsConfiguration.RegisterCommands<ToolsSlash>();


            Client.ComponentInteractionCreated += ComponentInteractionCreated;

            await Client.ConnectAsync(activity: new DiscordActivity("Your Raids", ActivityType.ListeningTo));
         //   SentrySdk.CaptureMessage("Something went wrong");
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
                    Title = "You are being rate limited let the api cooldown!",
                    Description = $"Time Remaining: {timeLeft}"
                };

                await args.Context.Channel.SendMessageAsync(embed: coolDownMessage);
            }
        }

        private static Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs args)
        {
            return Task.CompletedTask;
        }
        private static Task OnSocketError(DiscordClient sender, SocketErrorEventArgs e)
        {
            if (e.Exception.Message.Contains("connection is zombie"))
            {
               
                
                Environment.Exit(1); // Exit the program with an error code
            }
            else
            {
               
                //Environment.Exit(1);
            }

            // Log other socket errors

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

                        //gms shit
                    case string joinRaidGMId when joinRaidGMId.StartsWith("join_GM_"):
                        Console.WriteLine("Join GM button pressed");
                        await HandleJoinRaidGMButton(interaction);
                        break;
                    case string deleteRaiGMdId when deleteRaiGMdId.StartsWith("delete_GM_"):
                        Console.WriteLine("Delete Raid button pressed");
                        await HandleDeleteRaidGMButton(interaction);
                        break;
                    case string joinStandbyGMId when joinStandbyGMId.StartsWith("join_standbyGM_"):
                        Console.WriteLine("Join Standby button pressed");
                        await HandleJoinStandbyGMButton(interaction);
                        break;
                }
            }
        }
        //raid buttons
        public static async Task HandleJoinRaidButton(DiscordInteraction interaction)
        {
            try
            {
                string[] customIdParts = interaction.Data.CustomId.Split('_');
                if (customIdParts.Length < 4)
                {
                    Console.WriteLine($"Invalid custom ID format: {interaction.Data.CustomId}");
                    return;
                }

                ulong messageId;
                if (!ulong.TryParse(customIdParts[2], out messageId))
                {
                    Console.WriteLine($"Failed to parse message ID from custom ID: {interaction.Data.CustomId}");
                    return;
                }

                ulong userId;
                if (!ulong.TryParse(customIdParts[3], out userId))
                {
                    Console.WriteLine($"Failed to parse user ID from custom ID: {interaction.Data.CustomId}");
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
                    currentJoinSlots.Remove(user.Mention);
                    await interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"You have been removed from the join slots.").AsEphemeral());
                }
                else
                {
                    if (currentJoinSlots.Count <= 6)
                    {
                        currentJoinSlots.Add(user.Mention);
                        await interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"You have been added to the join slots.").AsEphemeral());
                    }
                    else
                    {
                        await interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("There are no more join slots available!").AsEphemeral());
                        return;
                    }
                }

                var updatedJoinField = currentJoinSlots.Any() ? string.Join("\n", currentJoinSlots) : "*";
                embed.Fields.First(f => f.Name == "Join Slots").Value = updatedJoinField;

                var messageBuilder = new DiscordMessageBuilder()
                    .WithContent(originalMessage.Content)
                    .WithEmbed(embed)
                    .AddComponents(
                        new DiscordButtonComponent(ButtonStyle.Success, $"join_raid_{messageId}_{userId}", "Join Raid"),
                        new DiscordButtonComponent(ButtonStyle.Secondary, $"join_standby_{messageId}_{userId}", "Join Standby"),
                        new DiscordButtonComponent(ButtonStyle.Danger, $"delete_raid_{messageId}_{userId}", "Delete Raid")
                    );

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
                string[] customIdParts = interaction.Data.CustomId.Split('_');
                if (customIdParts.Length < 4)
                {
                    Console.WriteLine($"Invalid custom ID format: {interaction.Data.CustomId}");
                    return;
                }

                ulong messageId;
                if (!ulong.TryParse(customIdParts[2], out messageId))
                {
                    Console.WriteLine($"Failed to parse message ID from custom ID: {interaction.Data.CustomId}");
                    return;
                }

                ulong userId;
                if (!ulong.TryParse(customIdParts[3], out userId))
                {
                    Console.WriteLine($"Failed to parse user ID from custom ID: {interaction.Data.CustomId}");
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
                    currentStandbySlots.Remove(user.Mention);
                    await interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"You have been removed from the standby slots.").AsEphemeral());
                }
                else
                {
                    if (currentStandbySlots.Count <= 6)
                    {
                        currentStandbySlots.Add(user.Mention);
                        await interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"You have been added to the standby slots.").AsEphemeral());
                    }
                    else
                    {
                        await interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("There are no more standby slots available!").AsEphemeral());
                        return;
                    }
                }

                var updatedStandbyField = currentStandbySlots.Any() ? string.Join("\n", currentStandbySlots) : "*";
                embed.Fields.First(f => f.Name == "Standby Slots").Value = updatedStandbyField;

                var messageBuilder = new DiscordMessageBuilder()
                    .WithContent(originalMessage.Content)
                    .WithEmbed(embed)
                    .AddComponents(
                        new DiscordButtonComponent(ButtonStyle.Success, $"join_raid_{messageId}_{userId}", "Join Raid"),
                        new DiscordButtonComponent(ButtonStyle.Secondary, $"join_standby_{messageId}_{userId}", "Join Standby"),
                        new DiscordButtonComponent(ButtonStyle.Danger, $"delete_raid_{messageId}_{userId}", "Delete Raid")
                    );

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

             
                var originalMessage = await interaction.Channel.GetMessageAsync(messageId);
                if (originalMessage == null)
                {
                    Console.WriteLine($"Failed to retrieve original message with ID: {messageId}");
                    return;
                }

               
                var member = await interaction.Guild.GetMemberAsync(interaction.User.Id);
                if (member != null && (member.PermissionsIn(interaction.Channel).HasPermission(Permissions.ManageMessages) || authorId == interaction.User.Id))
                {
                    
                    await originalMessage.DeleteAsync();
                    await interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Raid schedule deleted successfully.").AsEphemeral());
                }
                else
                {
                   
                    await interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("You are not authorized to delete this raid schedule.").AsEphemeral());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while handling the delete raid button: {ex.Message}");
            }
        }

        //excision gm 
        public static async Task HandleJoinRaidGMButton(DiscordInteraction interaction)
        {
            try
            {
                string[] customIdParts = interaction.Data.CustomId.Split('_');
                if (customIdParts.Length < 4)
                {
                    Console.WriteLine($"Invalid custom ID format: {interaction.Data.CustomId}");
                    return;
                }

                ulong messageId;
                if (!ulong.TryParse(customIdParts[2], out messageId))
                {
                    Console.WriteLine($"Failed to parse message ID from custom ID: {interaction.Data.CustomId}");
                    return;
                }

                ulong userId;
                if (!ulong.TryParse(customIdParts[3], out userId))
                {
                    Console.WriteLine($"Failed to parse user ID from custom ID: {interaction.Data.CustomId}");
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
                    currentJoinSlots.Remove(user.Mention);
                    await interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"You have been removed from the join slots.").AsEphemeral());
                }
                else
                {
                    if (currentJoinSlots.Count <= 12)
                    {
                        currentJoinSlots.Add(user.Mention);
                        await interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"You have been added to the join slots.").AsEphemeral());
                    }
                    else
                    {
                        await interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("There are no more join slots available!").AsEphemeral());
                        return;
                    }
                }

                var updatedJoinField = currentJoinSlots.Any() ? string.Join("\n", currentJoinSlots) : "*";
                embed.Fields.First(f => f.Name == "Join Slots").Value = updatedJoinField;

                var messageBuilder = new DiscordMessageBuilder()
                    .WithContent(originalMessage.Content)
                    .WithEmbed(embed)
                    .AddComponents(
                        new DiscordButtonComponent(ButtonStyle.Success, $"join_GM_{messageId}_{userId}", "Join Excision"),
                        new DiscordButtonComponent(ButtonStyle.Secondary, $"join_standbyGM_{messageId}_{userId}", "Join Standby"),
                        new DiscordButtonComponent(ButtonStyle.Danger, $"delete_GM_{messageId}_{userId}", "Delete Excision")
                    );

                var newMessage = await originalMessage.ModifyAsync(messageBuilder);

                Console.WriteLine($"Join action completed. New message ID: {newMessage.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while handling the join raid button: {ex.Message}");
            }
        }

        public static async Task HandleJoinStandbyGMButton(DiscordInteraction interaction)
        {
            try
            {
                string[] customIdParts = interaction.Data.CustomId.Split('_');
                if (customIdParts.Length < 4)
                {
                    Console.WriteLine($"Invalid custom ID format: {interaction.Data.CustomId}");
                    return;
                }

                ulong messageId;
                if (!ulong.TryParse(customIdParts[2], out messageId))
                {
                    Console.WriteLine($"Failed to parse message ID from custom ID: {interaction.Data.CustomId}");
                    return;
                }

                ulong userId;
                if (!ulong.TryParse(customIdParts[3], out userId))
                {
                    Console.WriteLine($"Failed to parse user ID from custom ID: {interaction.Data.CustomId}");
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
                    currentStandbySlots.Remove(user.Mention);
                    await interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"You have been removed from the standby slots.").AsEphemeral());
                }
                else
                {
                    if (currentStandbySlots.Count <= 12)
                    {
                        currentStandbySlots.Add(user.Mention);
                        await interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"You have been added to the standby slots.").AsEphemeral());
                    }
                    else
                    {
                        await interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("There are no more standby slots available!").AsEphemeral());
                        return;
                    }
                }

                var updatedStandbyField = currentStandbySlots.Any() ? string.Join("\n", currentStandbySlots) : "*";
                embed.Fields.First(f => f.Name == "Standby Slots").Value = updatedStandbyField;

                var messageBuilder = new DiscordMessageBuilder()
                    .WithContent(originalMessage.Content)
                    .WithEmbed(embed)
                    .AddComponents(
                        new DiscordButtonComponent(ButtonStyle.Success, $"join_GM_{messageId}_{userId}", "Join Excision"),
                        new DiscordButtonComponent(ButtonStyle.Secondary, $"join_standbyGM_{messageId}_{userId}", "Join Standby"),
                        new DiscordButtonComponent(ButtonStyle.Danger, $"delete_GM_{messageId}_{userId}", "Delete Excision")
                    );

                var newMessage = await originalMessage.ModifyAsync(messageBuilder);

                Console.WriteLine($"Join action completed. New message ID: {newMessage.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while handling the join standby button: {ex.Message}");
            }
        }

        public static async Task HandleDeleteRaidGMButton(DiscordInteraction interaction)
        {
            try
            {

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


                var originalMessage = await interaction.Channel.GetMessageAsync(messageId);
                if (originalMessage == null)
                {
                    Console.WriteLine($"Failed to retrieve original message with ID: {messageId}");
                    return;
                }


                var member = await interaction.Guild.GetMemberAsync(interaction.User.Id);
                if (member != null && (member.PermissionsIn(interaction.Channel).HasPermission(Permissions.ManageMessages) || authorId == interaction.User.Id))
                {

                    await originalMessage.DeleteAsync();
                    await interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Raid schedule deleted successfully.").AsEphemeral());
                }
                else
                {

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
