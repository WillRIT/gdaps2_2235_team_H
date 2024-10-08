﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalpracticeMakesPerfect
{

    internal delegate void OnLeftPress(Slot mySlot);
    internal delegate void OnLeftRelease(Slot mySlot);
    internal delegate void OnRightPress(Slot mySlot);
    internal delegate void OnHover(Slot mySlot);

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
                if (isTrash)
                {
                    if (!IsEmpty)
                    {
                        return $"Trash ({item.ItemName})";
                    }
                    return "Trash";
                }
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
        private MouseState mousePrev;
        private bool hovered;
        public bool Hovered
        {
            get { return hovered; }
        }
        private bool isTrash;
        public bool IsTrash
        {
            get { return isTrash; }
            set { isTrash = value; }
        }

        public event OnLeftPress PickUpItem;
        public event OnLeftRelease PutDownItem;
        public event OnRightPress PutSingleItem;
        public event OnHover SetHighlighted;

        /// <summary>
        /// Create empty slot
        /// </summary>
        /// <param name="asset">Background asset</param>
        /// <param name="position">Dimensions/position</param>
        /// <param name="font">Text font</param>
        public Slot(Texture2D asset, Rectangle position, SpriteFont font)
            : base(asset, position)
        {
            this.font = font;
            item = null;
            amount = 0;
        }

        /// <summary>
        /// Create slot with item
        /// </summary>
        /// <param name="asset">Background asset</param>
        /// <param name="position">Dimensions/position</param>
        /// <param name="font">Text font</param>
        /// <param name="item">Item in slot</param>
        /// <param name="amount">Amount of item in slot</param>
        public Slot(Texture2D asset, Rectangle position, SpriteFont font, Item item, int amount)
            : base(asset, position)
        {
            this.font = font;
            this.item = item;
            this.amount = amount;
        }

        /// <summary>
        /// Create trash slot
        /// </summary>
        /// <param name="asset">Background asset</param>
        /// <param name="position">Dimensions/position</param>
        /// <param name="font">Text font</param>
        /// <param name="isTrash">Override for creating trash slot</param>
        public Slot(Texture2D asset, Rectangle position, SpriteFont font, bool isTrash)
            : base(asset, position)
        {
            this.font = font;
            item = null;
            amount = 0;
            this.isTrash = isTrash;
        }

        /// <summary>
        /// Adds item to slot if slot is empty or if item type is the name
        /// </summary>
        /// <param name="item">Type of item to be added</param>
        /// <param name="amount">Amount of item to be added</param>
        /// <returns>Whether or not successful</returns>
        public bool AddItem(Item item, int amount)
        {
            //overwrites previous item if trash
            if (isTrash)
            {
                this.item = item;
                this.amount = amount;
                return true;
            }
            else
            {
                if (this.item == null)
                {
                    this.item = item;
                    this.amount = amount;
                    return true;
                }
                else if (ItemName == item.ItemName)
                {
                    this.amount += amount;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Clears the slot
        /// </summary>
        public void Clear()
        {
            item = null;
            amount = 0;
        }

        public override void Update()
        {
            mouseState = Mouse.GetState();

            hovered = position.Contains(mouseState.Position);

            if (amount == 0)
            {
                item = null;
            }

            if (!IsEmpty)
            {
                item.Update();
            }
            else
            {
                amount = 0;
            }

            if (hovered)
            {
                if (SetHighlighted != null)
                {
                    SetHighlighted(this);
                }

                if (mouseState.LeftButton == ButtonState.Pressed && mousePrev.LeftButton == ButtonState.Released && !IsEmpty && PickUpItem != null)
                {
                    PickUpItem(this);
                }
                if (mouseState.LeftButton == ButtonState.Released && mousePrev.LeftButton == ButtonState.Pressed && PutDownItem != null)
                {
                    PutDownItem(this);
                }
                if (mouseState.RightButton == ButtonState.Pressed && mousePrev.RightButton == ButtonState.Released && PutSingleItem != null)
                {
                    PutSingleItem(this);
                }
            }

            mousePrev = mouseState;
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

            if (!IsEmpty && amount > 0)
            {
                item.Draw(sb, new Rectangle((position.X + (int)(position.Width * 0.1)), (position.Y + (int)(position.Width * 0.1)), (int)(position.Width * 0.8), (int)(position.Width * 0.8)), Color.White);
                sb.DrawString(font, $"{amount}", new Vector2(position.X + (int)(position.Width * (1.0 / 8.0)), position.Y + (int)(position.Height * (3.0 / 5.0))), Color.Black);
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
