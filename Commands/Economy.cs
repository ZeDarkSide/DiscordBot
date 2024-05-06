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
    public class Economy : BaseCommandModule
    {


        #region Owner Commands
        [Command("addpoints")]
        public async Task Addpoints(CommandContext ctx, int PointAmount, DiscordMember user)
        {
            if (ctx.User.Id == 1114778208903122964 || ctx.User.Id == 653805970610192394)
            {
                if (user.Id == null || user.Id == 0)
                {
                    var noUser1 = new DiscordEmbedBuilder
                    {
                        Title = $"You did not pick a user! please try again and use a member to add points to.",
                        Color = DiscordColor.Red,
                    };
                    await ctx.Channel.SendMessageAsync(embed: noUser1);
                }
                if (PointAmount <= 0)
                {
                    PointAmount = 100;
                }
                ulong userId = user.Id;
                int userPoints = 0;
                string filePath = "Bank.json";
                JSONStructure data;
                if (File.Exists(filePath))
                {
                    string jsonData = File.ReadAllText(filePath);
                    data = JsonConvert.DeserializeObject<JSONStructure>(jsonData);
                }
                else
                {
                    data = new JSONStructure { UserPoints = new Dictionary<ulong, int>() };
                }
                if (data.UserPoints == null)
                {
                    data.UserPoints = new Dictionary<ulong, int>();
                }
                if (data != null)
                {
                    if (data.UserPoints == null)
                    {
                        data.UserPoints = new Dictionary<ulong, int>();
                    }
                    if (data.UserPoints.ContainsKey(userId))
                    {
                        userPoints = data.UserPoints[userId];
                    }
                    else
                    {
                        data.UserPoints[userId] = 500;
                        userPoints = 500;
                        string updatedJsonData = JsonConvert.SerializeObject(data);
                        File.WriteAllText(filePath, updatedJsonData);
                    }
                }
                data.UserPoints[userId] += PointAmount;
                userPoints = data.UserPoints[userId];
                string updatedJsonData1 = JsonConvert.SerializeObject(data);
                File.WriteAllText(filePath, updatedJsonData1);
                var embedBuilder = new DiscordEmbedBuilder
                {
                    Title = $"Added ${PointAmount} to {user.Username}'s Bank account!",
                    Color = DiscordColor.Red,

                };
                await ctx.Channel.SendMessageAsync(embed: embedBuilder);
            }
            else
            {
                var embedBuilder = new DiscordEmbedBuilder
                {
                    Title = $"You can't use this command only The Owner can use this command",
                    Color = DiscordColor.Red,

                };
                await ctx.Channel.SendMessageAsync(embed: embedBuilder);
            }
        }

        [Command("removepoints")]
        public async Task Removepoints(CommandContext ctx, int PointAmount, DiscordMember user)
        {
            if (ctx.User.Id == 1114778208903122964 || ctx.User.Id == 653805970610192394)
            {
                if (user.Id == null || user.Id == 0)
                {
                    var noUser1 = new DiscordEmbedBuilder
                    {
                        Title = $"You did not pick a user! please try again and use a member to add points to.",
                        Color = DiscordColor.Red,
                    };
                    await ctx.Channel.SendMessageAsync(embed: noUser1);
                }
                if (PointAmount <= 0)
                {
                    PointAmount = 100;
                }
                ulong userId = user.Id;
                int userPoints = 0;
                string filePath = "Bank.json";
                JSONStructure data;
                if (File.Exists(filePath))
                {
                    string jsonData = File.ReadAllText(filePath);
                    data = JsonConvert.DeserializeObject<JSONStructure>(jsonData);
                }
                else
                {
                    data = new JSONStructure { UserPoints = new Dictionary<ulong, int>() };
                }
                if (data.UserPoints == null)
                {
                    data.UserPoints = new Dictionary<ulong, int>();
                }
                if (data != null)
                {
                    if (data.UserPoints == null)
                    {
                        data.UserPoints = new Dictionary<ulong, int>();
                    }
                    if (data.UserPoints.ContainsKey(userId))
                    {
                        userPoints = data.UserPoints[userId];
                    }
                    else
                    {
                        data.UserPoints[userId] = 500;
                        userPoints = 500;
                        string updatedJsonData = JsonConvert.SerializeObject(data);
                        File.WriteAllText(filePath, updatedJsonData);
                    }
                }
                data.UserPoints[userId] -= PointAmount;
                userPoints = data.UserPoints[userId];
                string updatedJsonData1 = JsonConvert.SerializeObject(data);
                File.WriteAllText(filePath, updatedJsonData1);
                var embedBuilder = new DiscordEmbedBuilder
                {
                    Title = $"Added ${PointAmount} to {user.Username}'s Bank account!",
                    Color = DiscordColor.Red,

                };
                await ctx.Channel.SendMessageAsync(embed: embedBuilder);
            }
            else
            {
                var embedBuilder = new DiscordEmbedBuilder
                {
                    Title = $"You can't use this command only The Owner can use this command",
                    Color = DiscordColor.Red,

                };
                await ctx.Channel.SendMessageAsync(embed: embedBuilder);
            }
        }

        #endregion

        #region Store commands
        public Dictionary<string, int> shopItems = new Dictionary<string, int>
        {

        };

        [Command("shop")]
        public async Task Shop(CommandContext ctx)
        {
            var customColor = new DiscordColor(255, 71, 59);
            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = "Shop",
                Color = customColor
            };

            if (shopItems.Count == 0)
            {
                embedBuilder.Description = "Shop is empty right now. Come back later.";
            }
            else
            {
                foreach (var item in shopItems)
                {
                    embedBuilder.AddField($"{item.Key}", $"Cost: {item.Value} points");

                }
                embedBuilder.WithFooter("Please copy and paste the item name into !!buy [name] till I fix the naming!");
            }

            await ctx.RespondAsync(embed: embedBuilder);
        }

        [Command("buy")]
        public async Task Buy(CommandContext ctx, params string[] itemArgs)
        {
            try
            {
                // Join the item arguments into a single string representing the item name and convert to lowercase
                string item = string.Join(" ", itemArgs).ToLower();
                Console.WriteLine($"User {ctx.User.Username} wants to buy {item}.");

                // Check if the item is available in the shop (case-insensitive)
                KeyValuePair<string, int> matchingItem = shopItems.FirstOrDefault(x => x.Key.ToLower() == item);
                if (matchingItem.Key == null)
                {
                    Console.WriteLine($"Item {item} is not available in the shop.");
                    await ctx.RespondAsync("Item not available in the shop.");
                    return;
                }

                Console.WriteLine($"Item {item} found in the shop with price {matchingItem.Value} points.");

                ulong userId = ctx.User.Id;
                int userPoints = 0;

                // Load user data from UserData JSON file
                string userDataFilePath = "UserData.json";
                var userData = new Dictionary<ulong, List<string>>();

                if (File.Exists(userDataFilePath))
                {
                    string jsonData = File.ReadAllText(userDataFilePath);
                    userData = JsonConvert.DeserializeObject<Dictionary<ulong, List<string>>>(jsonData) ?? new Dictionary<ulong, List<string>>();
                }

                // Check if the user already owns the item
                if (userData.ContainsKey(userId) && userData[userId].Any(x => x.Equals(item, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine($"User {ctx.User.Username} already owns {item}.");
                    await ctx.RespondAsync($"You already own {item}.");
                    return;
                }

                // Load user points from Bank JSON file
                string bankFilePath = "Bank.json";
                var bankData = new JSONStructure();

                if (File.Exists(bankFilePath))
                {
                    string jsonData = File.ReadAllText(bankFilePath);
                    bankData = JsonConvert.DeserializeObject<JSONStructure>(jsonData) ?? new JSONStructure { UserPoints = new Dictionary<ulong, int>() };
                }

                if (bankData.UserPoints.ContainsKey(userId))
                {
                    userPoints = bankData.UserPoints[userId];
                }

                // Get the price of the item
                int itemPrice = matchingItem.Value;

                // Check if the user has enough points to buy the item
                if (userPoints < itemPrice)
                {
                    Console.WriteLine($"User {ctx.User.Username} does not have enough points to buy {item}.");
                    await ctx.RespondAsync($"You don't have enough points to buy this item. It costs {itemPrice} points.");
                    return;
                }

                // Deduct points from user
                userPoints -= itemPrice;

                // Add item to user's inventory
                if (!userData.ContainsKey(userId))
                {
                    userData[userId] = new List<string>();
                }

                userData[userId].Add(matchingItem.Key);

                Console.WriteLine($"User {ctx.User.Username} has successfully bought {item}.");

                // Save updated user data to UserData JSON file
                string updatedUserData = JsonConvert.SerializeObject(userData);
                File.WriteAllText(userDataFilePath, updatedUserData);

                // Update user points in Bank JSON file
                bankData.UserPoints[userId] = userPoints;
                string updatedBankJsonData = JsonConvert.SerializeObject(bankData);
                File.WriteAllText(bankFilePath, updatedBankJsonData);

                await ctx.RespondAsync($"You have successfully bought {matchingItem.Key} for {itemPrice} points.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while processing the buy command: {ex.Message}");
            }
        }

        [Command("modshop")]
        public async Task ModifyShop(CommandContext ctx, params string[] itemsAndPrices)
        {
            ulong permittedUserId = 653805970610192394; // Replace with your user ID

            // Check if the command invoker is the permitted user
            if (ctx.User.Id != permittedUserId)
            {
                await ctx.RespondAsync("You do not have permission to use this command.");
                return;
            }

            // Clear the existing shop items
            shopItems.Clear();

            // Parse the new items and prices
            foreach (var itemAndPrice in itemsAndPrices)
            {
                // Split the item and price by ":"
                string[] parts = itemAndPrice.Split(':');
                if (parts.Length != 2)
                {
                    await ctx.RespondAsync($"Invalid format for item: {itemAndPrice}. Use 'item:price' format.");
                    return;
                }

                string itemName = parts[0].Trim(); // Trim leading and trailing whitespace
                string priceStr = parts[1];

                // Validate item name
                if (string.IsNullOrWhiteSpace(itemName))
                {
                    await ctx.RespondAsync("Item name cannot be empty.");
                    return;
                }

                // Replace hyphens with spaces
                string item = itemName.Replace("-", " ");

                // Validate price
                if (!int.TryParse(priceStr, out int price))
                {
                    await ctx.RespondAsync($"Invalid price for item: {item}. Price must be a number.");
                    return;
                }

                // Add the item to the shop
                shopItems[item] = price;
            }

            await ctx.RespondAsync("Shop items updated successfully.");
        }

        #endregion






    }
}
