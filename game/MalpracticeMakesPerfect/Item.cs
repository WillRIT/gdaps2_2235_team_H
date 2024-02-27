using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalpracticeMakesPerfect
{
    internal class Item
    {
        private string itemName;
        private string itemDesc;
        private double itemCost;
        private bool inInventory;
        private Texture2D itemSprite;

        private enum itemAspects { };

        /// <summary>
        /// Create item
        /// </summary>
        /// <param name="itemName">Name of the item</param>
        /// <param name="itemDesc">Description of the item</param>
        /// <param name="itemCost">Cost of the item</param>
        /// <param name="inInventory">Whether or not the item should be placed in the inventory</param>
        /// <param name="itemSprite">The sprite of the item</param>
        public Item(string itemName, string itemDesc, double itemCost, bool inInventory, Texture2D itemSprite)
        {
            this.itemName = itemName;
            this.itemDesc = itemDesc;
            this.itemCost = itemCost;
            this.inInventory = inInventory;
            this.itemSprite = itemSprite;
        }


    }
}
