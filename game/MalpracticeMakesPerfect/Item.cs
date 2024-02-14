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
        protected string itemName;
        protected string itemDesc;
        protected double itemCost;
        protected bool inInventory;
        protected Texture2D itemSprite;

        protected enum itemAspects { };

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
