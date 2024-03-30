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
        private Slot[] hotbar = new Slot[10];
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
            for (int i = 0; i < hotbar.Length - 1; i++)
            {
                hotbar[i] = new Slot(slotAsset, new Rectangle(0, 0, 50, 50), font);
            }

            //create trash
            hotbar[hotbar.Length - 1] = new Slot(slotAsset, new Rectangle(0, 0, 50, 50), font, true);
        }

        public Inventory(Texture2D asset, Rectangle position, SpriteFont font, Texture2D slotAsset, List<Slot> hotbarItems)
            : base(asset, position)
        {
            this.font = font;
            this.slotAsset = slotAsset;

            //initialize hotbar as empty slots
            for (int i = 0; i < hotbar.Length - 1; i++)
            {
                hotbar[i] = new Slot(slotAsset, new Rectangle(0, 0, 50, 50), font);
            }

            for (int i = 0; i < Math.Min(hotbarItems.Count, hotbar.Length); i++)
            {
                hotbar[i] = hotbarItems[i];
            }

            //create trash
            hotbar[hotbar.Length - 1].IsTrash = true;
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

        public void Clear()
        {
            foreach (Slot s in hotbar)
            {
                s.Item = null;
            }
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
                if (!hotbar[i].IsTrash)
                {
                    hotbar[i].Position = new Rectangle(position.X + (hotbar[i].Position.Width * i), position.Y, 50, 50);
                }
                else
                {
                    hotbar[i].Position = new Rectangle(position.X + (hotbar[i].Position.Width * i), position.Y + (position.Height - 50), 50, 50);
                }
                
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
