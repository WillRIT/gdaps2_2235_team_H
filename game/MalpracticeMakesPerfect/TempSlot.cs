using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalpracticeMakesPerfect
{
    internal class TempSlot : Draggable
    {
        private SpriteFont font;
        private Item item;
        public Item Item
        {
            get { return item; }
            set { item = value; }
        }
        private int amount;
        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        /// <summary>
        /// Create temp (draggable) slot
        /// </summary>
        /// <param name="position">Dimensions/position</param>
        /// <param name="font">Text font</param>
        /// <param name="item">Item in slot</param>
        /// <param name="amount">Amount of item in slot</param>
        public TempSlot(Rectangle position, SpriteFont font, Item item, int amount)
            : base(item.Asset, position)
        {
            this.font = font;
            this.amount = amount;
            this.item = item;
        }

        public override void Draw(SpriteBatch sb)
        {
            item.Draw(sb, new Rectangle((position.X + 5), (position.Y + 5), 40, 40), Color.White);
            sb.DrawString(font, $"{amount}", new Vector2(position.X + (int)(position.Width * (1.0 / 8.0)), position.Y + (int)(position.Height * (3.0 / 5.0))), Color.Black);
        }
    }
}
