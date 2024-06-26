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

           

                await ctx.RespondAsync("ok");
            
            
        }

        // do later!

    }
}
