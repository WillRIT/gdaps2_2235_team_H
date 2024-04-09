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
    internal class RecipeBook : GameObject
    {
        private List<Item> craftableItems;
        public bool IsShown { get; private set; }
        private List<RecipeBookSlot> slots;

        private Point slotDims = new Point(100, 100);

        public RecipeBook(Texture2D asset, Rectangle position, List<Item> allItems, Dictionary<string, Recipe> recipes)
            :base(asset, position)
        {
            craftableItems = new List<Item>();
            foreach (Item i in allItems)
            {
                if (!i.InShop)
                {
                    craftableItems.Add(i);
                }
            }

            slots = new List<RecipeBookSlot>();

            //make slot of every item
            int row = 0;
            int col = 0;
            //center items
            int slotsPerLine = (int)Math.Floor((double)position.Width / slotDims.X);
            int padding = 0;
            for (int i = 0; i < craftableItems.Count; i++)
            {

                //go to next row
                if (slotDims.X * (col + 1) > position.Width)
                {
                    row++;
                    col = 0;
                }

                //centering
                if (col == 0)
                {
                    if (craftableItems.Count - i < slotsPerLine)
                    {
                        padding = (position.Width - (slotDims.X * (craftableItems.Count - i))) / 2;
                    }
                    else
                    {
                        padding = (position.Width - (slotDims.X * slotsPerLine)) / 2;
                    }
                }

                slots.Add(new RecipeBookSlot(new Rectangle(position.X + slotDims.X * col + padding, position.Y + slotDims.Y * row, slotDims.X, slotDims.Y), allItems[i]));

                col++;
            }

            IsShown = false;
        }

        public override void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                IsShown = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.T))
            {
                IsShown = false;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            if (IsShown)
            {
                sb.Draw(asset, position, Color.White);

                foreach (RecipeBookSlot slot in slots)
                {
                    slot.Draw(sb);
                }
            }
        }
    }
}
