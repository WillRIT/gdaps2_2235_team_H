using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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
        /// Get list of items and recipes from file
        /// </summary>
        /// <param name="Content">Content manager for textures</param>
        /// <param name="recipes">List of recipes to be output</param>
        /// <param name="itemDict">Dictionary of items for item lookup</param>
        /// <returns>Complete list of items</returns>
        public static List<Item> GetItemsAndRecipes(ContentManager Content, out Dictionary<string,Recipe> recipes, out Dictionary<string, Item> itemDict)
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
                        Texture2D asset = Content.Load<Texture2D>("items/" + itemElements[4]);

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
            itemDict = findItems;
            return items;
        }

        /// <summary>
        /// Gets every scenario from a file
        /// </summary>
        /// <param name="Content">Content manager for textures</param>
        /// <param name="items">A list of every item</param>
        /// <param name="slotAsset">Asset for the slot to be used by the scenario</param>
        /// <param name="font">Font to be used by the scenario</param>
        /// <param name="buttonAsset">Asset for scenario button</param>
        /// <param name="pickUpItem">Method for picking up and item</param>
        /// <param name="putDownItem">Method for placing an item</param>
        /// <param name="setHighlight">Method for setting the highlighted slot</param>
        /// <param name="updateStats">Method for updating money/reputation</param>
        /// <returns>Complete list of scenarios</returns>
        public static List<Scenario> GetScenarios(ContentManager Content, List<Item> items, Texture2D slotAsset, SpriteFont font, Texture2D buttonAsset, OnLeftPress pickUpItem, OnLeftRelease putDownItem, OnHover setHighlight, UpdateStats updateStats)
        {
            List<Scenario> list = new List<Scenario>();
            string path = "../../../scenarios.csv";

            try
            {
                // Check if the file exists
                if (File.Exists(path))
                {
                    // Read all lines from the file
                    string[] lines = File.ReadAllLines(path);

                    for (int i = 1; i < lines.Length; i++) // i = 1 because data starts on second line
                    {
                        string[] elements = lines[i].Split('|');

                        string name = elements[0];
                        string message = elements[1];
                        string godMessage = elements[2];
                        Texture2D personSprite = Content.Load<Texture2D>("people/" + elements[3]);
                        double money = double.Parse(elements[4]);

                        Dictionary<string, string[]> cures = new Dictionary<string, string[]>();
                        for (int j = 5; j < elements.Length; j++)
                        {
                            string[] strings = elements[j].Split(";");

                            cures.Add(strings[0], new string[] { strings[1], strings[2], strings[3] });
                        }

                        list.Add(new Scenario(items, slotAsset, font, buttonAsset, pickUpItem, putDownItem, setHighlight, name, message, godMessage, personSprite, money, cures));
                        list[i-1].UpdateStats += updateStats; //-1 because of index
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return list;
        }
        
        /// <summary>
        /// Gets scenario specific hints from a file
        /// </summary>
        /// <param name="hints"></param>
        public static void GetHintList(Dictionary<string, string> hints)
        {
            string hintPath = "../../../Hints.txt";

            try
            {
                //Checks if the file exists
                if (File.Exists(hintPath))
                {
                    string[] lines = File.ReadAllLines(hintPath);

                    //Adds each hint to the dictionary
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string[] elements = lines[i].Split('|');
                        hints.Add(elements[0], elements[1]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
