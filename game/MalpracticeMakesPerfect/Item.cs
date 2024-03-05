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

        public string ItemName
        {
            get {return itemName;}
            set {itemName = value;}
        }

        public string ItemDesc
        {
            get {return itemDesc;}
            set {itemDesc = value;}
        }

        public double ItemCost
        {
            get{return itemCost;}
            set {itemCost = value;}
        }

        public bool InInventory
        {
            get {return inInventory;}
            set {inInventory = value;}
        }

        public Texture2D ItemSprite
        {
            get { return itemSprite; }
            set { itemSprite = value; }
        }

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
            this.itemName = ItemName;
            this.itemDesc = ItemDesc;
            this.itemCost = ItemCost;
            this.inInventory = InInventory;
            this.itemSprite = ItemSprite;
        }


    }
}
