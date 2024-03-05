using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;

namespace MalpracticeMakesPerfect
{
    /// <summary>
    /// File I/O manager for making the database that
    /// players read in game
    /// </summary>
    internal class DatabaseManager
    {
        /// <summary>
        /// Reads a file to get a list of all the items and recipes
        /// </summary>
        /// <returns>An inventory</returns>
        public Inventory GetItemsAndRecipes(string jsonFileIn, List<Item> items, List<Recipe> recipes)
        {
            //TODO: code
            string itemList = File.ReadAllText(jsonFileIn);
            Item itemList2 = JsonSerializer.Deserialize<Item>(itemList);
            items.Add(new Item($"name: {itemList2.ItemName}", $"description: {itemList2.ItemDesc}", itemList2.ItemCost, itemList2.InInventory, itemList2.ItemSprite));
            recipes[0] = JsonSerializer.Deserialize<Recipe>(itemList);
            return null;
        }

        /// <summary>
        /// Reads a file to get all the scenarios
        /// </summary>
        /// <returns>A list of scenario</returns>
        public List<Scenario> GetScenarios()
        {
            //TODO: code
            return null;
        }
    }
}
