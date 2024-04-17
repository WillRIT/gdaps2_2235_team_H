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
        /// <summary>
        /// Draw text with black background
        /// </summary>
        /// <param name="sb">SpriteBatch</param>
        /// <param name="texture">Texture to pass in for text background</param>
        /// <param name="font">Font of text</param>
        /// <param name="text">Text to be displayed</param>
        /// <param name="position">Position of label</param>
        /// <param name="color">Color of the text</param>
        public static void DrawItemLabel(SpriteBatch sb, Texture2D texture, SpriteFont font, string text, Vector2 position, Color color)
        {
            Vector2 textSize = font.MeasureString(text);

            sb.Draw(texture, new Rectangle((int)position.X, (int)position.Y, (int)textSize.X, (int)textSize.Y), Color.Black);
            sb.DrawString(font, text, position, color);
        }

        /// <summary>
        /// Draw text with black background, with opacity
        /// </summary>
        /// <param name="sb">SpriteBatch</param>
        /// <param name="texture">Texture to pass in for text background</param>
        /// <param name="font">Font of text</param>
        /// <param name="text">Text to be displayed</param>
        /// <param name="position">Position of label</param>
        /// <param name="color">Color of the text</param>
        /// <param name="opacity">Opacity of label</param>
        public static void DrawItemLabel(SpriteBatch sb, Texture2D texture, SpriteFont font, string text, Vector2 position, Color color, float opacity)
        {
            Vector2 textSize = font.MeasureString(text);

            sb.Draw(texture, new Rectangle((int)position.X, (int)position.Y, (int)textSize.X, (int)textSize.Y), Color.Black * opacity);
            sb.DrawString(font, text, position, color * opacity);
        }

        /// <summary>
        /// Draw text with black background, given mousestate
        /// </summary>
        /// <param name="sb">SpriteBatch</param>
        /// <param name="texture">Texture to pass in for text background</param>
        /// <param name="font">Font of text</param>
        /// <param name="text">Text to be displayed</param>
        /// <param name="ms">Mouse state</param>
        /// <param name="color">Color of text</param>
        public static void DrawItemLabel(SpriteBatch sb, Texture2D texture, SpriteFont font, string text, MouseState ms, Color color)
        {
            Vector2 textSize = font.MeasureString(text);

            sb.Draw(texture, new Rectangle(ms.X, ms.Y, (int)textSize.X, (int)textSize.Y), Color.Black);
            sb.DrawString(font, text, new Vector2(ms.X, ms.Y), color);
        }

        /// <summary>
        /// Draw a row of items
        /// </summary>
        /// <param name="sb">SpriteBatch</param>
        /// <param name="items">List of items to be drawn</param>
        /// <param name="ms">Mouse state for position</param>
        /// <param name="color">Color to tint items</param>
        internal static void DrawItemPreviews(SpriteBatch sb, List<Item> items, MouseState ms, Color color)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Draw(sb, new Rectangle(ms.X + i * 50, ms.Y, 50, 50), color);
            }
        }

        /// <summary>
        /// Draw a row of items
        /// </summary>
        /// <param name="sb">SpriteBatch</param>
        /// <param name="items">List of items to be drawn</param>
        /// <param name="pos">Position of draw</param>
        /// <param name="color">Color to tint items</param>
        internal static void DrawItemPreviews(SpriteBatch sb, List<Item> items, Vector2 pos, Color color)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Draw(sb, new Rectangle((int)pos.X + i * 50, (int)pos.Y, 50, 50), color);
            }
        }

        /// <summary>
        /// Draw a row of items
        /// </summary>
        /// <param name="sb">SpriteBatch</param>
        /// <param name="items">List of items to be drawn</param>
        /// <param name="pos">Position of draw</param>
        /// <param name="itemSize">Size to draw items</param>
        /// <param name="color">Color to tint items</param>
        internal static void DrawItemPreviews(SpriteBatch sb, List<Item> items, Vector2 pos, int itemSize, Color color)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Draw(sb, new Rectangle((int)pos.X + i * itemSize, (int)pos.Y, itemSize, itemSize), color);
            }
        }
    }
}
