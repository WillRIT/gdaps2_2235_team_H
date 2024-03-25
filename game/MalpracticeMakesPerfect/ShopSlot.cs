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
        private Rectangle button;
        public string ButtonText
        {
            get { return $"{item.ItemName} (${item.Cost})"; }
        }
        private SpriteFont font;
        private Texture2D assetHovered;
        private bool isHovered;

        public ShopSlot(Texture2D asset, Rectangle position, SpriteFont font, Item item)
            : base(asset, position)
        {
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
                sb.Draw(assetHovered, position, Color.White);
            }

            item.Draw(sb);
        }

        public override void Update()
        {
            if (position.Contains(Mouse.GetState().Position))
            {
                //hi
            }
        }
    }
}
