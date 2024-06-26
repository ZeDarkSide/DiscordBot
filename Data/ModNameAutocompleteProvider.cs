using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

public class ModNameAutocompleteProvider : IAutocompleteProvider
{
    private readonly List<string> modNames = new List<string>
    {
        "Helmet Mods",
        "Siphon Mods",
        "Targeting Mods",
        "Ammo Finder Mods",
        "Ammo Scout Mods",
        "Ashes to Assets Mod",
        "Hands On Mod",
        "Dynamo Mod" ,
        "Power Preservation Mod",
        "Font of Wisdom Mod",
        "Resistance Mods",
        
    };

    public async Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
    {
        var query = ctx.OptionValue?.ToString()?.ToLower() ?? "";

        var results = modNames
            .Where(mod => mod.ToLower().Contains(query))
            .Select(mod => new DiscordAutoCompleteChoice(mod, mod))
            .Take(35); 

        return await Task.FromResult(results);
    }
}
