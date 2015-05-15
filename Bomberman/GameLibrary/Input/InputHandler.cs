using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameLibrary.Input
{
    public class InputHandler : GameComponent
    {
        private static KeyboardState keyboardState;
        private static KeyboardState lastKeyboardState;

        private static GamePadState[] gamePadStates;
        private static GamePadState[] lastGamePadStates;

        public static KeyboardState KeyboardState
        {
            get { return keyboardState; }
        }

        public static KeyboardState LastKeyboardState
        {
            get { return lastKeyboardState; }
        }

        public static GamePadState[] GamePadStates
        {
            get { return gamePadStates; }
        }

        public static GamePadState[] LastGamePadStates
        {
            get { return lastGamePadStates; }
        }

        public InputHandler(Game game)
            : base(game)
        {
            keyboardState = Keyboard.GetState();

            gamePadStates = new GamePadState[Enum.GetValues(typeof (PlayerIndex)).Length];

            foreach (PlayerIndex index in Enum.GetValues(typeof (PlayerIndex)))
                gamePadStates[(int) index] = GamePad.GetState(index);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            lastGamePadStates = (GamePadState[]) gamePadStates.Clone();
            foreach (PlayerIndex index in Enum.GetValues(typeof (PlayerIndex)))
                gamePadStates[(int) index] = GamePad.GetState(index);

            base.Update(gameTime);
        }

        public static Keys[] GetPressedKeys()
        {
            return keyboardState.GetPressedKeys();
        }

        public static IList<Keys> GetSinglePressedKeys()
        {
            var keys = Enum.GetValues(typeof (Keys)) as Keys[];
            IList<Keys> result = new List<Keys>();
            foreach (var key in keys)
            {
                if (keyboardState.IsKeyDown(key) && !lastKeyboardState.IsKeyDown(key))
                    result.Add(key);
            }

            return result;
        }

        public static void Flush()
        {
            lastKeyboardState = keyboardState;  
        }

        public static bool KeyReleased(Keys key)
        {
            return keyboardState.IsKeyUp(key) &&
                   lastKeyboardState.IsKeyDown(key);
        }

        public static bool KeyPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key) &&
                   lastKeyboardState.IsKeyUp(key);
        }

        public static bool KeyDown(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        public static bool ButtonReleased(Buttons button, PlayerIndex index)
        {
            return gamePadStates[(int) index].IsButtonUp(button) &&
                   lastGamePadStates[(int) index].IsButtonDown(button);
        }

        public static bool ButtonPressed(Buttons button, PlayerIndex index)
        {
            return gamePadStates[(int) index].IsButtonDown(button) &&
                   lastGamePadStates[(int) index].IsButtonUp(button);
        }

        public static bool ButtonDown(Buttons button, PlayerIndex index)
        {
            return gamePadStates[(int) index].IsButtonDown(button);
        }
    }
}