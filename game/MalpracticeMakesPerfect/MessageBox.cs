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
    internal class MessageBox
    {

        public static void DrawItemLabel(SpriteBatch sb, Texture2D texture, SpriteFont font, string text, Vector2 position, Color color)
        {
            Vector2 textSize = font.MeasureString(text);

            sb.Draw(texture, new Rectangle((int)position.X, (int)position.Y, (int)textSize.X, (int)textSize.Y), Color.Black);
            sb.DrawString(font, text, position, color);
        }

        public static void DrawItemLabel(SpriteBatch sb, Texture2D texture, SpriteFont font, string text, MouseState ms, Color color)
        {
            Vector2 textSize = font.MeasureString(text);

            sb.Draw(texture, new Rectangle(ms.X, ms.Y, (int)textSize.X, (int)textSize.Y), Color.Black);
            sb.DrawString(font, text, new Vector2(ms.X, ms.Y), color);
        }

        internal static void DrawItemPreviews(SpriteBatch sb, List<Item> items, MouseState ms, Color color)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Draw(sb, new Rectangle(ms.X + i * 50, ms.Y, 50, 50), color);
            }
        }
    }
}
