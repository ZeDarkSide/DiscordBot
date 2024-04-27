using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Logging;
using System;
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
            new DiscordConfiguration()
            {
                MinimumLogLevel = LogLevel.Debug
            };
            Client = new DiscordClient(discordConfig);
            

            Client.UseInteractivity(new InteractivityConfiguration()
            {
                Timeout = TimeSpan.FromMinutes(1)
            });

            Client.Ready += Client_Ready;

            var commandConfig = new CommandsNextConfiguration()
            {

                StringPrefixes = new string[] { jsonreader.prefix },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = false,
            };

            Commands = Client.UseCommandsNext(commandConfig);
            var slashCommandsConfiguration = Client.UseSlashCommands();

            //Commands event handler 
            Commands.CommandErrored += Commands_CommandErrored;


            // commands files 
            Commands.RegisterCommands<Tools>();
            Commands.RegisterCommands<Gamble>();
            slashCommandsConfiguration.RegisterCommands<ToolsSlash>();

            //await Client.ConnectAsync();
            await Client.ConnectAsync(activity: new DiscordActivity("Your Raids", ActivityType.ListeningTo) );
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




    }




}
