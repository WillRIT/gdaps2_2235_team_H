using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalpracticeMakesPerfect
{
    internal class Shop : GameObject
    {
        private List<Item> items; // list of all items
        private List<ShopSlot> slots;
        public List<ShopSlot> Slots
        {
            get { return slots; }
        }
        private SpriteFont font;
        private Texture2D slotAsset;
        private Texture2D buttonAsset;

        private Point slotDims = new Point(100, 150);

        /// <summary>
        /// Create shop
        /// </summary>
        /// <param name="asset">Shop background (not currently implemented</param>
        /// <param name="slotAsset">Asset of shop slots</param>
        /// <param name="buttonAsset">Asset of bottons</param>
        /// <param name="position">Dimensions/position</param>
        /// <param name="font">Text font</param>
        /// <param name="items">List of items that start in the shop</param>
        /// <param name="Purchase">Method for purchasing an item</param>
        public Shop(Texture2D asset, Texture2D slotAsset, Texture2D buttonAsset, Rectangle position, SpriteFont font, List<Item> items, ShopSlot.PurchaseItem Purchase)
            : base(asset, position)
        {
            this.slotAsset = slotAsset;
            this.buttonAsset = buttonAsset;
            this.font = font;
            this.items = items;

            slots = new List<ShopSlot>();

            //make slot of every item
            int row = 0;
            int col = 0;
            //center items
            int slotsPerLine = (int)Math.Floor((double)position.Width / slotDims.X);
            int padding = 0;
            for (int i = 0; i < items.Count; i++)
            {

                //go to next row
                if (slotDims.X * (col + 1) > position.Width)
                {
                    row++;
                    col = 0;
                }

                //centering
                if (col == 0)
                {
                    if (items.Count - i < slotsPerLine)
                    {
                        padding = (position.Width - (slotDims.X * (items.Count - i))) / 2;
                    }
                    else
                    {
                        padding = (position.Width - (slotDims.X * slotsPerLine)) / 2;
                    }
                }

                slots.Add(new ShopSlot(slotAsset, buttonAsset,
                    new Rectangle(position.X + slotDims.X * col + padding, position.Y + slotDims.Y * row, slotDims.X, slotDims.Y),
                    font, items[i]));
                slots[i].Purchase += Purchase;

                col++;
            }
        }

        public override void Update()
        {
            foreach (ShopSlot s in slots)
            {
                s.Update();
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            foreach(ShopSlot s in slots)
            {
                s.Draw(sb);
            }
        }
    }
}
