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

    public delegate void OnButtonClick();

    internal class Button : GameObject
    {
        private SpriteFont font;
        private MouseState mouseState;
        private MouseState mousePrev;
        private bool hovering;
        private string text;
        private Vector2 textPos;
        private Color textColor;
        private Color mainColor;
        private Color hoverColor;

        public event OnButtonClick OnLeftButton;
        public event OnButtonClick OnRightButton;

        public Button(Texture2D asset, Rectangle position, SpriteFont font, string text, Color textColor) 
            : base(asset, position)
        {
            this.font = font;
            this.text = text;
            this.textColor = textColor;

            Vector2 textSize = font.MeasureString(text);
            textPos = new Vector2(
                (position.X + position.Width / 2) - textSize.X / 2,
                (position.Y + position.Height / 2) - textSize.Y / 2
            );
        }

        public Button(Texture2D asset, Rectangle position, SpriteFont font, string text, Color textColor, Color mainColor, Color hoverColor)
            :base(asset, position)
        {
            this.font = font;
            this.text = text;
            this.textColor = textColor;
            this.mainColor = mainColor;
            this.hoverColor = hoverColor;

            Vector2 textSize = font.MeasureString(text);
            textPos = new Vector2(
                (position.X + position.Width / 2) - textSize.X / 2,
                (position.Y + position.Height / 2) - textSize.Y / 2
            );
        }

        public override void Update()
        {
            mouseState = Mouse.GetState();

            if (position.Contains(mouseState.Position))
            {
                hovering = true;

                if (mouseState.LeftButton == ButtonState.Pressed && mousePrev.LeftButton == ButtonState.Released)
                {
                    if (OnLeftButton != null)
                    {
                        OnLeftButton();
                    }
                }

                if (mouseState.RightButton == ButtonState.Pressed && mousePrev.RightButton == ButtonState.Released)
                {
                    if (OnRightButton != null)
                    {
                        OnRightButton();
                    }
                }
            }
            else
            {
                hovering = false;
            }
            

            mousePrev = Mouse.GetState();
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(asset, position, (hovering ? hoverColor : mainColor));

            sb.DrawString(font, text, textPos, textColor);
        }
    }
}
