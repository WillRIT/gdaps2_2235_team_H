﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework.Content;

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

        public Inventory MakeInventory()
        {
            //TODO: code
            string itemList = File.ReadAllText(jsonFileIn);
            Item itemList2 = JsonSerializer.Deserialize<Item>(itemList);
            items.Add(new Item($"name: {itemList2.ItemName}", $"description: {itemList2.ItemDesc}", itemList2.ItemCost, itemList2.InInventory, itemList2.ItemSprite));
            recipes[0] = JsonSerializer.Deserialize<Recipe>(itemList);
            return null;
        }

        public List<Item> GetItemsAndRecipes(ContentManager Content, out Dictionary<string,Recipe> recipes)
        {
            List<Item> items = new List<Item>();
            Dictionary<string, Item> findItems = new Dictionary<string, Item>();
            Dictionary<string, Recipe> recipeList = new Dictionary<string, Recipe>();

            string itemFilePath = "../../../Items.csv";
            string recipeFilePath = "../../../Recipes.csv";

            try
            {
                // Check if the file exists
                if (File.Exists(itemFilePath))
                {
                    // Read all lines from the file
                    string[] lines = File.ReadAllLines(itemFilePath);

                    // Create Item for Each Line
                    for (int i = 1; i < lines.Length; i++)
                    {
                        string[] itemElements = lines[i].Split('|');
                        string name = itemElements[0];
                        string desc = itemElements[1];
                        double cost = double.Parse(itemElements[2]);
                        bool inInventory = bool.Parse(itemElements[3]);
                        Texture2D asset = Content.Load<Texture2D>(itemElements[4]);

                        items.Add(new Item(asset, new Rectangle(0, 0, 50, 50), name, desc, cost, inInventory));
                        findItems.Add(name, new Item(asset, new Rectangle(0, 0, 50, 50), name, desc, cost, inInventory));
                    }
                }

                // Check if the file exists
                if (File.Exists(recipeFilePath))
                {
                    // Read all lines from the file
                    string[] lines = File.ReadAllLines(recipeFilePath);

                    // Create Recipe for Each Line
                    for (int i = 1; i < lines.Length; i++)
                    {
                        string[] recipeElements = lines[i].Split('|');
                        string[] inputs = recipeElements[0].Split(',');
                        string[] outputs = recipeElements[1].Split(',');

                        Item[] itemsIn = new Item[]
                        {
                            findItems[inputs[0]],
                            findItems[inputs[1]]
                        };

                        List<Item> itemsOut = new List<Item>();

                        foreach (string o in outputs)
                        {
                            itemsOut.Add(findItems[o]);
                        }

                        recipeList.Add($"{itemsIn[0]},{itemsIn[1]}",new Recipe(itemsIn, itemsOut));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            recipes = recipeList;
            return items;
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
