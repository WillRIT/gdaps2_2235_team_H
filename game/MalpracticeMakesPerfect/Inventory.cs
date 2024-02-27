using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalpracticeMakesPerfect
{
    internal class Inventory : GameObject
    {
        private List<Item> items;
        private List<Recipe> recipes;
        private Slot[] hotbar = new Slot[6];
        public Slot[] Hotbar
        {
            get { return hotbar; }
        }
        private List<Slot> shopItems;
        private SpriteFont font;
        private Texture2D slotAsset;


        public Inventory(Texture2D asset, Rectangle position, SpriteFont font, Texture2D slotAsset)
            :base(asset, position)
        {
            this.font = font;
            this.slotAsset = slotAsset;

            //initialize hotbar as empty slots
            hotbar[0] = new Slot(slotAsset, position, font, new Item(asset, new Rectangle(0, 0, 50, 50), "funee haha", "not funny", 80.68, false), 3);
            hotbar[1] = new Slot(slotAsset, position, font, new Item(slotAsset, new Rectangle(0, 0, 50, 50), "funee haha", "not funny", 80.68, false), 3);
            for (int i = 2; i < hotbar.Length; i++)
            {
                hotbar[i] = new Slot(slotAsset, new Rectangle(0, 0, 50, 50), font);
            }
        }

        /// <summary>
        /// Creates inventory
        /// </summary>
        /// <param name="items">Every item</param>
        /// <param name="recipes">All possible combinations of items</param>
        public Inventory(Texture2D asset, Rectangle position, SpriteFont font, Texture2D slotAsset, List<Item> items, List<Recipe> recipes)
            :base(asset,position)
        {
            this.font = font;
            this.slotAsset = slotAsset;
            this.items = items;
            this.recipes = recipes;
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

        /// <summary>
        /// Draw inventory
        /// </summary>
        /// <param name="sb"></param>
        public void DrawScene(SpriteBatch sb)
        {
            //background
            sb.Draw(asset, position, Color.White);

            //draw hotbar
            for (int i = 0; i < hotbar.Length; i++)
            {
                hotbar[i].Position = new Rectangle(position.X + (hotbar[i].Position.Width * i), position.Y, 50, 50);
                hotbar[i].Draw(sb);
            }
        }

        public override void Update()
        {
            if (items != null)
            {
                foreach (Item i in items)
                {
                    i.Update();
                }
            }
            

            foreach (Slot s in hotbar)
            {
                s.Update();
            }

            if (shopItems != null)
            {
                foreach (Slot s in shopItems)
                {
                    s.Update();
                }
            }
            
        }
    }
}
