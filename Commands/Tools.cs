using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace ZeDarkSide_Discord_Bot.Commands
{
    public class Tools : BaseCommandModule
    {


        #region Bug testing 

        [Command("bug")]
        public async Task Bug(CommandContext ctx, params string[] args)
        {
            if (args.Length < 4)
            {
                await ctx.RespondAsync("Please provide all required parameters: bug name, bug details, reproduction steps, and reporter name.\nexample:  !!bug \"bug name\" \"bug details\" \"Reproduction Steps\" \"user sending report\" ");
                return;
            }


            string bugName = args[0];
            string bugDetails = args[1];
            string reproductionSteps = args[2];
            string reporterName = args[3];


            var embed = new DiscordEmbedBuilder()
                .WithTitle(bugName)
                .WithDescription($"Bug Details:\n {bugDetails}")
                .AddField("Reproduction Steps", reproductionSteps)
                .WithFooter($"Reported by {reporterName}")
                .WithColor(DiscordColor.Red);


            var owner = await ctx.Guild.GetMemberAsync(653805970610192394) as DiscordMember;
            await owner.SendMessageAsync($"Bug Report by {ctx.User.Username}");
            await owner.SendMessageAsync(embed: embed);


            await ctx.Message.DeleteAsync();
            await ctx.RespondAsync($"Bug report for '{bugName}' has been submitted successfully. Thank you! {ctx.User.Username}");
        }





        #endregion

        #region Updates

/*        [Command("patch")]
        public async Task PatchNotes(CommandContext ctx)
        {

            var EmbedMaker = new DiscordEmbedBuilder()
            {
                Title = $"Patch Notes",
                ImageUrl = "PatchNotes.png"
            };


        }*/


        #endregion




    }




}
