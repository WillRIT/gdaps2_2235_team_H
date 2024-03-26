using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalpracticeMakesPerfect
{
    internal class ShopSlot : GameObject
    {
        private Item item;
        public string ButtonText
        {
            get { return $"{item.ItemName} (${item.Cost})"; }
        }
        private SpriteFont font;
        private Texture2D assetHovered;
        private bool isHovered;
        private MouseState mouseState;
        private MouseState mousePrev;

        public delegate void PurchaseItem(Item bought);
        public event PurchaseItem Purchase;

        public ShopSlot(Texture2D asset, Texture2D assetHovered, Rectangle position, SpriteFont font, Item item)
            : base(asset, position)
        {
            this.assetHovered = assetHovered;
            this.font = font;
            this.item = item;

            item.Position = new Rectangle(position.X + 25, position.Y + 25, 50, 50);
        }

        public override void Draw(SpriteBatch sb)
        {
            if (isHovered)
            {
                sb.Draw(assetHovered, position, Color.White);
            }
            else
            {
                sb.Draw(asset, position, Color.White);
            }

            item.Draw(sb);
        }

        public override void Update()
        {
            mouseState = Mouse.GetState();

            isHovered = position.Contains(mouseState.Position);

            if (isHovered && mouseState.LeftButton == ButtonState.Pressed && mousePrev.LeftButton == ButtonState.Released)
            {
                Purchase(item);
            }

            mousePrev = mouseState;
        }
    }
}
