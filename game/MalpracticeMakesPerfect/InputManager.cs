using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MalpracticeMakesPerfect
{
    public static class InputManager
    {
        private static MouseState lastMouseState;

        public static Vector2 MousePosition = Mouse.GetState().Position.ToVector2();

        public static bool MouseClicked { get; private set; }
        public static bool MouseReleased { get; private set; }

        public static void Update()
        {
            MouseClicked = (Mouse.GetState().LeftButton == ButtonState.Pressed) &&
                           (lastMouseState.LeftButton == ButtonState.Released);

            MouseClicked = (Mouse.GetState().LeftButton == ButtonState.Released) &&
                           (lastMouseState.LeftButton == ButtonState.Pressed);

            lastMouseState = Mouse.GetState();

        }

    }
}
