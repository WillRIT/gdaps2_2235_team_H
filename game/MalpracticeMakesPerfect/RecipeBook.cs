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

        private MouseState mState;
        private MouseState mPrev;

        private SpriteFont font;

        public RecipeBook(Texture2D asset, Rectangle position, SpriteFont font,List<Item> allItems, Dictionary<string, Recipe> recipes)
            :base(asset, position)
        {
            this.font = font;

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
            //int slotsPerLine = (int)Math.Floor((double)position.Width / slotDims.X);
            int padding = 0;
            for (int i = 0; i < craftableItems.Count; i++)
            {

                //go to next row
                if (slotDims.X * (col + 1) + 100 > position.Width - 100)
                {
                    row++;
                    col = 0;
                }

                //centering
                /*if (col == 0)
                {
                    if (craftableItems.Count - i < slotsPerLine)
                    {
                        padding = (position.Width - (slotDims.X * (craftableItems.Count - i))) / 2;
                    }
                    else
                    {
                        padding = (position.Width - (slotDims.X * slotsPerLine)) / 2;
                    }
                }*/

                slots.Add(new RecipeBookSlot(new Rectangle(position.X + 125 + (slotDims.X * col) + padding, position.Y + 75 + (slotDims.Y * row), slotDims.X, slotDims.Y), craftableItems[i]));

                col++;

                //add recipe
                foreach (Recipe rec in recipes.Values)
                {
                    foreach (Item item in rec.Outputs)
                    {
                        if (craftableItems[i].ItemName == item.ItemName)
                        {
                            slots[i].Recipes.Add(rec, false);
                        }
                    }
                }
            }

            IsShown = false;
        }

        public void NewRecipe(Recipe recipe)
        {
            foreach (RecipeBookSlot slot in slots)
            {
                bool isItem = false;

                foreach (Item i in recipe.Outputs)
                {
                    isItem = i.ItemName == slot.Item.ItemName;
                }

                if (isItem)
                {
                    slot.Unlocked = true;

                    foreach (Recipe r in slot.Recipes.Keys)
                    {
                        slot.Recipes[r] = r.ToString == recipe.ToString;
                    }
                }
            }
        }

        public void Show()
        {
            IsShown = !IsShown;
        }

        public override void Update()
        {
            mState = Mouse.GetState();

            //do not show when clicked
            if (IsShown && position.Contains(mState.Position) && mState.LeftButton == ButtonState.Pressed && mPrev.LeftButton == ButtonState.Released)
            {
                Show();
            }

            mPrev = mState;
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

                //item preview (draw over other slots)
                foreach (RecipeBookSlot slot in slots)
                {
                    //draw preview if hover
                    if (slot.Position.Contains(Mouse.GetState().Position))
                    {
                        if (slot.Unlocked)
                        {
                            //count row
                            int recipeCount = 0;
                            MouseState ms = Mouse.GetState();

                            foreach (Recipe rec in slot.Recipes.Keys)
                            {
                                if (slot.Recipes[rec])
                                {
                                    MessageBox.DrawItemPreviews(sb, rec.Inputs.ToList(), new Vector2(ms.X, ms.Y + recipeCount * 50), Color.White);

                                    recipeCount++;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.DrawItemLabel(sb, asset, font, "Item not unlocked!", new Vector2(mState.X + 10, mState.Y + 10), Color.Red);
                        }
                    }
                }
            }
        }
    }
}