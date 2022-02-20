/**
 * Starting Code from Nathan Bean's GameArchitectureExample project (Screens and State Management folders
 * and ScrrenFactory.cs file), currently the architecture example is not implemented though, but it is still present
 */

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceGame_CIS580.Screens;
using SpaceGame_CIS580.StateManagement;
using SpaceGame_CIS580.Collisions;
//using tainicom.Aether.Physics2D.Dynamics; //was having some trouble with the physics engine, so using pure calculations from physics examples provided by Nathan Bean

namespace SpaceGame_CIS580
{
    /// <summary>
    /// The main game window
    /// </summary>
    public class SpaceGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private readonly ScreenManager _screenManager;

        /// <summary>
        /// Contructor for the game
        /// </summary>
        public SpaceGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = Constants.GAME_WIDTH;
            _graphics.PreferredBackBufferHeight = Constants.GAME_HEIGHT;
            _graphics.ApplyChanges();

            var screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);

            _screenManager = new ScreenManager(this);
            Components.Add(_screenManager);

            AddInitialScreens();
        }

        /// <summary>
        /// For game architecutre, not implemented currently
        /// </summary>
        private void AddInitialScreens()
        {
            _screenManager.AddScreen(new BackgroundScreen(), null);
            //_screenManager.AddScreen(new GamePlayScreen(), null);
            _screenManager.AddScreen(new MainMenuScreen(), null);
        }

        /// <summary>
        /// Initialize the game and its sprites
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Load the game content
        /// </summary>
        protected override void LoadContent() { }
        

        /// <summary>
        /// Updates the game window and its sprites
        /// </summary>
        /// <param name="gameTime">The game time</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game window and its sprites
        /// </summary>
        /// <param name="gameTime">The game time</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
