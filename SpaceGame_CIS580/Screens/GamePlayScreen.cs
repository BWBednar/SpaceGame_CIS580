using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceGame_CIS580.StateManagement;

namespace SpaceGame_CIS580.Screens
{
    /// <summary>
    /// Implements the main game logic
    /// </summary>
    public class GamePlayScreen : GameScreen
    {
        private ContentManager _content;
        //private SpriteFont _gameFont;

        private Vector2 _playerPosition = new Vector2(100, 100);
        //private SpaceshipSprite _ship = new SpaceshipSprite();

        public GamePlayScreen()
        {

        }

        public override void Activate()
        {
            if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");
            //_ship.LoadContent(_content);
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            _content.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            if (IsActive)
            {
                //put gameplay here
            }
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));


            // Look up inputs for the active player profile.
            int playerIndex = 1;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];

            //  move the player position.
            var movement = Vector2.Zero;

            if (keyboardState.IsKeyDown(Keys.Left))
                movement.X--;

            if (keyboardState.IsKeyDown(Keys.Right))
                movement.X++;

            if (keyboardState.IsKeyDown(Keys.Up))
                movement.Y--;

            if (keyboardState.IsKeyDown(Keys.Down))
                movement.Y++;

            if (movement.Length() > 1)
                movement.Normalize();

            _playerPosition += movement * 8f;
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;

            //spriteBatch.Begin();
            //_ship.Draw(gameTime, spriteBatch);
            //spriteBatch.DrawString(_gameFont, "// TODO", _playerPosition, Color.Green);

            //spriteBatch.End();

        }
    }
}
