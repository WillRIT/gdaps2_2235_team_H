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
        public bool IsEmpty
        {
            get { return item == null; }
        }
        public string ItemName
        {
            get 
            { 
                if (IsEmpty)
                {
                    return "Empty";
                }
                else
                {
                    return item.ItemName;
                }
            }
        }
        private SpriteFont font;
        private MouseState mouseState;
        private bool hovered;
        public bool Hovered 
        { 
            get { return hovered; } 
        }


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

        public override void Update()
        {
            mouseState = Mouse.GetState();

            hovered = position.Contains(mouseState.Position);

            if (!IsEmpty)
            {
                item.Update();
            }
            else
            {
                amount = 0;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            if (hovered)
            {
                sb.Draw(asset, position, Color.Red);
            }
            else
            {
                sb.Draw(asset, position, Color.White);
            }

            if (!IsEmpty)
            {
                item.Draw(sb, new Rectangle((position.X + 5), (position.Y + 5), 40, 40), Color.White);
                sb.DrawString(font, $"{amount}", new Vector2(position.X + (int)(position.Width * (1.0/8.0)), position.Y + (int)(position.Height * (3.0/5.0))), Color.Black);
            }
        }

        public override string ToString()
        {
            if (IsEmpty)
            {
                return ItemName;
            }
            else
            {
                return $"{ItemName} ({amount})";
            }
        }

    }
}
