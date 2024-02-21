﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalpracticeMakesPerfect
{
    internal class Inventory
    {
        private List<Item> items;
        private List<Recipe> recipes;
        private Slot[] hotbar = new Slot[6];
        private List<Slot> shopItems;
        private Rectangle position;
        private Texture2D asset;

        /// <summary>
        /// Creates inventory
        /// </summary>
        /// <param name="items">Every item</param>
        /// <param name="recipes">All possible combinations of items</param>
        public Inventory(List<Item> items,List<Recipe> recipes, Rectangle position, Texture2D asset)
        {
            this.items = items;
            this.recipes = recipes;
            this.position = position;
            this.asset = asset;
        }

        /// <summary>
        /// Check if two items combine to make an item.
        /// </summary>
        /// <param name="item1">The first item checked</param>
        /// <param name="item2">The second item</param>
        /// <returns>If the items match up with any recipe, the resulting item is returned.</returns>
        public Item CheckRecipe(Item item1, Item item2)
        {
            //TODO: code the method
            return null;
        }

        /// <summary>
        /// Adds an item to the inventory if it doesn't already exist there
        /// </summary>
        /// <param name="item">Item to be added</param>
        public void AddItem(Item item, int slotIndex)
        {

        }

        public void DrawScene(SpriteBatch sb)
        {
            sb.Draw(asset, position, Color.White);
        }
    }
}
