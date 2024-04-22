using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalpracticeMakesPerfect
{
    internal class Log : GameObject
    {
        private SpriteFont font;

        public string Text { get; set; }

        public int Count
        {
            get
            {
                return Text.Split('\n').Length;
            }
        }

        private Button upB;
        private Button downB;

        private int firstLine;
        private int LastLine
        {
            get
            {
                return Math.Min(firstLine + 5, Count - 1);
            }
        }

        private int prevCount;

        /// <summary>
        /// Create log
        /// </summary>
        /// <param name="asset">Log background</param>
        /// <param name="position">Log dimensions/position</param>
        /// <param name="font">Log text font</param>
        public Log(Texture2D asset, Rectangle position, SpriteFont font)
            : base(asset, position)
        {
            this.font = font;
            Text = String.Empty;

            firstLine = 0;

            upB = new Button(asset, new Rectangle(position.X + position.Width - 50, position.Y, 50, 50), font, "▲", Color.Black);
            upB.OnLeftButton += Up;
            downB = new Button(asset, new Rectangle(position.X + position.Width - 50, position.Y + position.Height - 50, 50, 50), font, "▼", Color.Black);
            downB.OnLeftButton += Down;
        }

        /// <summary>
        /// Returns select lines of the log given beginning and end indices
        /// </summary>
        /// <returns>Subsection of the text</returns>
        private string GetTextSection()
        {
            string subSect = string.Empty;
            string[] lines = Text.Split("\n");

            for (int i = firstLine; i < LastLine; i++)
            {
                subSect += lines[i] + "\n";
            }

            return subSect;
        }

        /// <summary>
        /// Go up one line of text
        /// </summary>
        private void Up()
        {
            if (firstLine != 0)
            {
                firstLine--;
            }
        }

        /// <summary>
        /// Go down one line of text
        /// </summary>
        private void Down()
        {
            if (firstLine + 6 < Count)
            {
                firstLine++;
            }
        }

        /// <summary>
        /// Reset log
        /// </summary>
        public void Clear()
        {
            Text = string.Empty;

            firstLine = 0;
        }

        public override void Update()
        {
            //put subsection of the end of the log when the log updates
            if (prevCount < Count && Count > firstLine + 5)
            {
                firstLine = Count - 6;
            }

            upB.Update();
            downB.Update();

            prevCount = Count;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(asset, position, Color.White);

            upB.Draw(sb);
            downB.Draw(sb);

            sb.DrawString(font, GetTextSection(), new Vector2(position.X + 25, position.Y + 5), Color.Black);
        }
    }
}
