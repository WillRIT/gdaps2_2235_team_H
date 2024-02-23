using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace MalpracticeMakesPerfect
{
    public class Item : Draggable
    {
        private string itemName;
        private string itemDesc;
        private double itemCost;
        private bool inInventory;
        private Texture2D itemSprite;
        private Vector2 pos;
        private Vector2 origin;

        public Vector2 Position
        {
            get { return pos; } 
            set {  pos = value; }
        }

        public Rectangle Rectangle => new((int)(Position.X - origin.X), (int)(Position.Y - origin.Y), itemSprite.Width, itemSprite.Height);

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

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(itemSprite, Position, Color.DarkBlue);
        }

    }
}
