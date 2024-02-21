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
    internal class Slot : GameObject
    {
        private Item item;
        private int amount;
        public bool IsEmpty
        {
            get { return item == null; }
        }
        public string ItemName
        {
            get { return item.ItemName; }
        }
        SpriteFont font;



        public Slot(Texture2D asset, Rectangle position, SpriteFont font)
            :base (asset, position)
        {
            this.font = font;
            item = null;
            amount = 0;
        }

        public Slot(Texture2D asset, Rectangle position, SpriteFont font, Item item, int amount)
            :base (asset, position)
        {
            this.font = font;
            this.item = item;
            this.amount = amount;
        }

        public void AddItem(Item item, int amount)
        {
            if (this.item != null)
            {
                this.item = item;
                this.amount = amount;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(asset, position, Color.White);

            if (!IsEmpty)
            {
                item.Draw(sb, new Rectangle((position.X + 5), (position.Y + 5), 40, 40), Color.White);
                //sb.DrawString(font, $"{amount}", new Vector2(position.X, position.Y), Color.Black);
            }
        }

        public void Draw (SpriteBatch sb, Rectangle position)
        {
            sb.Draw(asset, position, Color.White);

            if (!IsEmpty)
            {
                item.Draw(sb, new Rectangle((position.X + 5), (position.Y + 5), 40, 40), Color.White);
                //sb.DrawString(font, $"{amount}", new Vector2(position.X, position.Y), Color.Black);
            }
        }
    }
}
