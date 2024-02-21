using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalpracticeMakesPerfect
{
    internal class Item : GameObject
    {
        private string itemName;
        public string ItemName
        {
            get { return itemName; }
        }
        private string itemDesc;
        private double itemCost;
        private bool inInventory;

        private enum itemAspects { };

        /// <summary>
        /// Create item
        /// </summary>
        /// <param name="itemName">Name of the item</param>
        /// <param name="itemDesc">Description of the item</param>
        /// <param name="itemCost">Cost of the item</param>
        /// <param name="inInventory">Whether or not the item should be placed in the inventory</param>
        public Item(Texture2D asset, Rectangle position,string itemName, string itemDesc, double itemCost, bool inInventory)
            :base (asset, position)
        {
            this.itemName = itemName;
            this.itemDesc = itemDesc;
            this.itemCost = itemCost;
            this.inInventory = inInventory;
        }

        public void Draw(SpriteBatch sb, Rectangle position, Color color)
        {
            sb.Draw(asset, position, color);
        }
    }
}
