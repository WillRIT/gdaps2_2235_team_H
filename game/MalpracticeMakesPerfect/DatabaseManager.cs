using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<Item> GetItemsAndRecipes(out List<Recipe> recipes)
        {
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
