using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MalpracticeMakesPerfect
{
    internal class ShopSlot : GameObject
    {
        private Item item;
        private Rectangle button;
        private Texture2D buttonAsset;
        public string ButtonText
        {
            get { return $"{item.ItemName} (${item.Cost})"; }
        }
        private SpriteFont font;
        
        private bool isHovered;
        private MouseState mouseState;
        private MouseState mousePrev;

        public delegate void PurchaseItem(Item bought);
        public event PurchaseItem Purchase;

        public ShopSlot(Texture2D asset, Texture2D buttonAsset, Rectangle position, SpriteFont font, Item item)
            : base(asset, position)
        {
            this.buttonAsset = buttonAsset;
            this.font = font;
            this.item = item;

            item.Position = new Rectangle(position.X + position.Width / 4, position.Y + position.Width / 4, position.Width / 2, position.Width / 2);
            button = new Rectangle((int)(position.X + position.Width * 0.05), (int)(position.Y + position.Height * 0.59), (int)(position.Width * 0.90), (int)(position.Height * 0.35));

            
        }

        private Vector2 TextPos(string words)
        {
            Vector2 textSize = font.MeasureString(words);
            return new Vector2(
                (button.X + button.Width / 2) - textSize.X / 2,
                (button.Y + button.Height / 2) - textSize.Y / 2
            );
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(asset, position, Color.White);

            if (isHovered)
            {
                sb.Draw(buttonAsset, button, Color.Green);
            }
            else
            {
                sb.Draw(buttonAsset, button, Color.Red);
            }

            if (item.Position.Contains(Mouse.GetState().Position))
            {

            }

            item.Draw(sb);

            sb.DrawString(font, item.ItemName, TextPos(item.ItemName) - new Vector2(0, 10), Color.Black);
            sb.DrawString(font, $"${item.Cost:N2}", TextPos($"${item.Cost:N2}") + new Vector2(0, 10), Color.Black);
        }

        public override void Update()
        {
            mouseState = Mouse.GetState();

            isHovered = button.Contains(mouseState.Position);

            if (isHovered && mouseState.LeftButton == ButtonState.Pressed && mousePrev.LeftButton == ButtonState.Released)
            {
                Purchase(item);
            }

            mousePrev = mouseState;
        }
    }
}
