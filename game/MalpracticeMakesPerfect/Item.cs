using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace MalpracticeMakesPerfect
{
    internal class Item : GameObject
    {
        private string assetPath;
        public string AssetPath
        {
            get { return assetPath; }
        }
        public Texture2D Asset
        {
            get { return asset; }
            set { asset = value; }
        }
        private string name;
        public string ItemName
        {
            get { return name; }
        }
        private string description;
        private double cost;
        private bool inInventory;

        private ContentManager Content;

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
            this.name = itemName;
            this.description = itemDesc;
            this.cost = itemCost;
            this.inInventory = inInventory;
        }

        public void Draw(SpriteBatch sb, Rectangle position, Color color)
        {
            sb.Draw(asset, position, color);
        }

        public override void Update()
        {

        }
    }
}
