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
        private Slot[] hotbar = new Slot[10];
        public Slot[] Hotbar
        {
            get { return hotbar; }
        }
        private List<Slot> shopItems;
        private SpriteFont font;
        private Texture2D slotAsset;

        private int slotSize;


        public Inventory(Texture2D asset, Rectangle position, SpriteFont font, Texture2D slotAsset, OnLeftPress pickUpItem, OnLeftRelease putDownItem, OnRightPress putSingleItem, OnHover setHighlighted)
            :base(asset, position)
        {
            this.font = font;
            this.slotAsset = slotAsset;

            slotSize = position.Width / hotbar.Length;

            //initialize hotbar as empty slots
            for (int i = 0; i < hotbar.Length - 1; i++)
            {
                hotbar[i] = new Slot(slotAsset, new Rectangle(0, 0, slotSize, slotSize), font);
            }

            //create trash
            hotbar[hotbar.Length - 1] = new Slot(slotAsset, new Rectangle(0, 0, slotSize, slotSize), font, true);

            foreach (Slot s in hotbar)
            {
                s.PickUpItem += pickUpItem;
                s.PutDownItem += putDownItem;
                s.PutSingleItem += putSingleItem;
                s.SetHighlighted += setHighlighted;
            }
        }

        public Inventory(Texture2D asset, Rectangle position, SpriteFont font, Texture2D slotAsset, List<Slot> hotbarItems)
            : base(asset, position)
        {
            this.font = font;
            this.slotAsset = slotAsset;

            //initialize hotbar as empty slots
            for (int i = 0; i < hotbar.Length - 1; i++)
            {
                hotbar[i] = new Slot(slotAsset, new Rectangle(0, 0, slotSize, slotSize), font);
            }

            for (int i = 0; i < Math.Min(hotbarItems.Count, hotbar.Length); i++)
            {
                hotbar[i] = hotbarItems[i];
            }

            //create trash
            hotbar[hotbar.Length - 1].IsTrash = true;
        }
        
        /// <summary>
        /// Clears inventory
        /// </summary>
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
        /// <param name="slotIndex">Slot for item to be added</param>
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
                    hotbar[i].Position = new Rectangle(position.X + (hotbar[i].Position.Width * i), position.Y, slotSize, slotSize);
                }
                else
                {
                    hotbar[i].Position = new Rectangle(position.X + (hotbar[i].Position.Width * i), position.Y + (position.Height - slotSize), slotSize, slotSize);
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
