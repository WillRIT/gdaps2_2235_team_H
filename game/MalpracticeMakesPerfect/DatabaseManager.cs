using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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
            return null;
        }

        public List<Item> GetItemsAndRecipes(ContentManager Content, out List<Recipe> recipes)
        {
            string filePath = "../../../Items.csv";

            try
            {
                // Check if the file exists
                if (File.Exists(filePath))
                {
                    // Read all lines from the file
                    string[] lines = File.ReadAllLines(filePath);

                    // Display each line
                    for (int i = 1; i < lines.Length; i++)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            recipes = new List<Recipe>();
            return new List<Item>();
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
