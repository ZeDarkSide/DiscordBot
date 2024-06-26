using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeDarkSide_Discord_Bot.Data
{
    public static class DictionaryDataBase
    {
        public static readonly Dictionary<string, (string Title, string Description, string Slot, string ImageUrl, string CommunityResearch)> ModDetails = new()
        {
            ["Helmet Mods"] = ("Helmet Mods", "PlaceHolder", "PlaceHolder", "PlaceHolder", "PlaceHolder"),

            ["Siphon Mods"] = ("Siphon Mods", "Rapid weapon final blows matching the element create an **Orb of Power**. \nHarmonic = matching Subclass", "Helmet", "https://i.imgur.com/tglmPPo.png", null),

            ["Targeting Mod"] = ("Targeting Mods", "Improves the Target acquisition accuracy and ADS time on Weapon matching the element. \nHarmonic = matching Subclass", "Helmet", "https://www.bungie.net/common/destiny2_content/icons/ebac07dcd16d7a0d079c934c85a16c47.png", "Grants the following to Arc Weapons 1 second after readying them\n• 5|8|10 Aim Assist with 1|2|3 Mods equipped\n• 0.9x|0.85x|0.8x Accuracy Cone Size\n• 0.85x|0.75x|0.7x ADS Animation Duration Multiplier"),

            ["Ammo Finder Mods"] = ("Ammo Finder Mods", "Enhances your chances getting the Ammo type.", "Helmet", "https://www.bungie.net/common/destiny2_content/icons/51d838fa6df423306dee2bfd1c290e16.png", "Spawns a Special Ammo Brick upon scoring a Weapon Kill after reaching 100% Counter Progress. Counter is only progressed by Weapon Kills.\n\nAmmo Finder Bricks grant 27.5%|60%|100% of a Regular Ammo Brick's Pickup Amount with 1|2|3 Mods equipped. Pickup Amount is determined on Brick Spawn. \n\nCounter Progress on Weapon Kills:\r\n1.66% to 1.8%. Average of 57.25 Kills."),

            ["Ammo Scout Mods"] = ("Ammo Scout Mods", "Drops Ammo for your Team-mates if Ammo Finder triggers.", "Helmet", "https://www.bungie.net/common/destiny2_content/icons/dce8a1f70e98285dd6535fee3c39cbf7.png", "Ammo Bricks spawned this way grant 27.5%|60% of a Regular Ammo Brick's Pickup Amount with 1|2 Mods equipped."),

            ["Ashes to Assets Mod"] = ("Ashes to Assets Mod", "Bonus Super Energy on grenade kills.", "Helmet", "https://www.bungie.net/common/destiny2_content/icons/e30b0976cfaba9e604288e4874a0a690.png", "• Tier 1 (T1) Minors: 1%|?|? \r\n• T2 Minors, T1 Elites: ?|?|? \r\n• T3 Minors, T2 Elites: ?|?|? \r\n• T4 Minors, T3-T4 Elites, and higher: 5%|?|? \r\n• Players: ?|?|?"),

            ["Hands On Mod"] = ("Hands On Mod", "Bonus Super Energy on melee kills.", "Helmet", "https://www.bungie.net/common/destiny2_content/icons/f6f3983578ffe594eb4cc7d2a804ad60.png", "N/A"),

            ["Dynamo Mod"] = ("Dynamo Mod", "Reduces Super cooldown when using your class ability near targets.", "Helmet", "https://www.bungie.net/common/destiny2_content/icons/a52c0055430bd947e6b7fd1a8f8515ce.png", "Class Ability Usage while within 15 metres of an enemy grants Super Ability Energy.\r\nSuper Energy Gains with 1|2+ Mods:\r\n2.5% [PVP: 1.25%] | 4% [PVP: 2.4%] \r\nEnergy Multipliers based on Class:\r\nHunters: x1 | Titans: x1.2 | Warlocks: x2"),


            ["Power Preservation Mod"] = ("Power Preservation", "Your Super final blows create extra Orbs of Power for your allies.", "Helmet", "https://www.bungie.net/common/destiny2_content/icons/63f5150a5b9ed5962880c0bb5b6467bf.png", "Spawned Orbs of Power grant 2.5%|3.75%|4.4% Super Energy with 1|2|3 Mods equipped. \r\n1.5? second cooldown between Orb spawns."),

            ["Font of Wisdom Mod"] = ("Font of Wisdom", "Collecting an Orb of Power causes you to gain 1 temporary Armor Charge.", "Helmet", "https://www.bungie.net/common/destiny2_content/icons/22c3d89c0f5c1a09042a4043bf5c25d0.png", "Grants 30|50|60 Intellect with 1|2|3 Mods equipped while you have active Armor Charges. \r\nArmor Charge now decays over time at the rate of 1 every 10 seconds by default.")

        };
    }
}
