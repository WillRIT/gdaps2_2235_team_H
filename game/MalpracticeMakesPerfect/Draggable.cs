﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalpracticeMakesPerfect
{
    internal class Draggable : GameObject
    {
        private bool dragging;
        private MouseState mouseState;
        private MouseState mousePrev;
        private bool placing;
        public bool Placing
        {
            get { return placing; }
        }
        private bool snapped;
        public bool Snapped
        {
            get { return snapped; }
        }
        public Rectangle snapLocation;

        public Draggable(Texture2D asset, Rectangle position) : base(asset, position)
        {
            snapped = false;
        }

        public override void Update()
        {
            mouseState = Mouse.GetState();
            placing = false;

            //cursor within object
            if (position.Contains(mouseState.Position))
            {
                //click and release left button
                if (mouseState.LeftButton == ButtonState.Pressed &&  mousePrev.LeftButton == ButtonState.Released)
                {
                    dragging = true;
                }
            }

            if (dragging)
            {
                //set object position to cursor
                position.X = mouseState.X - (position.Width / 2);
                position.Y = mouseState.Y - (position.Height / 2);

                //turn off dragging if released left button
                if (mouseState.LeftButton == ButtonState.Released)
                {
                    dragging = false;
                    placing = true;

                    if (snapped)
                    {
                        position = snapLocation;
                    }
                }
            }

            mousePrev = mouseState;
        }

        public void SnapIntersect(GameObject snap)
        {
            if (snap.Position.Contains(mouseState.Position))
            {
                position = snap.Position;
                snapLocation = position;
                snapped = true;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            if (!dragging)
            {
                sb.Draw(asset, position, Color.White);
            }
            else
            {
                sb.Draw(asset, position, Color.White * 0.5f);
            }
        }
    }
}
