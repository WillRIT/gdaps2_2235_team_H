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

        /// <summary>
        /// Create inventory object
        /// </summary>
        /// <param name="asset">Inventory background</param>
        /// <param name="position">Dimensions/position of inventory</param>
        /// <param name="font">Text font</param>
        /// <param name="slotAsset">Asset for item slots</param>
        /// <param name="pickUpItem">Method for picking up item</param>
        /// <param name="putDownItem">Method for placing item</param>
        /// <param name="putSingleItem">Method for right-click placing (single item)</param>
        /// <param name="setHighlighted">Method for setting which slot is highlighted</param>
        public Inventory(Texture2D asset, Rectangle position, SpriteFont font, Texture2D slotAsset, OnLeftPress pickUpItem, OnLeftRelease putDownItem, OnRightPress putSingleItem, OnHover setHighlighted)
            :base(asset, position)
        {
            this.font = font;
            this.slotAsset = slotAsset;

            slotSize = (int)(position.Width - position.Width * 0.15) / hotbar.Length;

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
                    hotbar[i].Position = new Rectangle(position.X + (int)(position.Width * 0.075) + (hotbar[i].Position.Width * i), position.Y + (int)(position.Width * 0.075), slotSize, slotSize);
                }
                else
                {
                    hotbar[i].Position = new Rectangle(position.X + (int)(position.Width * 0.075) + (hotbar[i].Position.Width * i), position.Y + (int)(position.Height - slotSize - position.Width * 0.075), slotSize, slotSize);
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
