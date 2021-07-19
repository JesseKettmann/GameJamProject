using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameJamProject
{
    public static class Input
    {
        // The keyboard state
        public static KeyboardState KeyboardState;
        public static KeyboardState KeyboardStatePrevious;

        // The mouse state
        public static MouseState MouseState;
        public static MouseState MouseStatePrevious;

        // Get the input state
        public static void Update()
        {
            if (Game1.gameInstance.IsActive)
            {
                // Update the keyboard state
                KeyboardStatePrevious = KeyboardState;
                KeyboardState = Keyboard.GetState();

                // Update the mouse state
                MouseStatePrevious = MouseState;
                MouseState = Mouse.GetState();

                // Disable mouse if it's outside of the window
                if (!MouseIsInWindow())
                {
                    MouseStatePrevious = new MouseState();
                    MouseState = new MouseState();
                }
            }
            else
            {
                // Empty the keyboard state
                KeyboardStatePrevious = new KeyboardState();
                KeyboardState = new KeyboardState();

                // Empty the mouse state
                MouseStatePrevious = new MouseState();
                MouseState = new MouseState();
            }
        }


        #region keyboard functions

        // If a key is pressed this frame
        public static bool KeyPressed(Keys key)
        {
            return !KeyboardStatePrevious.IsKeyDown(key) && KeyboardState.IsKeyDown(key);
        }

        // If a key is being held down
        public static bool KeyDown(Keys key)
        {
            return KeyboardState.IsKeyDown(key);
        }

        /// If a key is released this frame
        public static bool KeyReleased(Keys key)
        {
            return KeyboardStatePrevious.IsKeyDown(key) && !KeyboardState.IsKeyDown(key);
        }

        #endregion

        #region mouse functions

        #region left mouse

        public static bool MouseLeftClick()
        {
            return MouseStatePrevious.LeftButton == ButtonState.Released && MouseState.LeftButton == ButtonState.Pressed;
        }
        public static bool MouseLeftDown()
        {
            return MouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool MouseLeftReleased()
        {
            return MouseStatePrevious.LeftButton == ButtonState.Pressed && MouseState.LeftButton == ButtonState.Released;
        }

        #endregion

        #region right mouse

        public static bool MouseRightClick()
        {
            return MouseStatePrevious.RightButton == ButtonState.Released && MouseState.RightButton == ButtonState.Pressed;
        }

        public static bool MouseRightDown()
        {
            return MouseState.RightButton == ButtonState.Pressed;
        }

        public static bool MouseRightReleased()
        {
            return MouseStatePrevious.RightButton == ButtonState.Pressed && MouseState.RightButton == ButtonState.Released;
        }

        #endregion

        #region middle mouse

        public static bool MouseMiddleClick()
        {
            return MouseStatePrevious.MiddleButton == ButtonState.Released && MouseState.MiddleButton == ButtonState.Pressed;
        }

        public static int ScrollWheelValue()
        {
            return MouseState.ScrollWheelValue - MouseStatePrevious.ScrollWheelValue;
        }

        #endregion

        public static bool MouseIsInWindow()
        {
            Rectangle bounds = new Rectangle(0, 0, Game1.gameInstance.portSize.X, Game1.gameInstance.portSize.Y);
            return (bounds.Contains(Mouse.GetState().Position));
        }

        #endregion
    }
}
