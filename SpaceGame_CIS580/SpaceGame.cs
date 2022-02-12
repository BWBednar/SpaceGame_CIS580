/**
 * Starting Code from Nathan Bean's GameArchitectureExample project
 */

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceGame_CIS580.Screens;
using SpaceGame_CIS580.StateManagement;
//using tainicom.Aether.Physics2D.Dynamics; Was having trouble with physics engine, so following example for the example C for game physics for now

namespace SpaceGame_CIS580
{
    // Sample showing how to manage different game states, with transitions
    // between menu screens, a loading screen, the game itself, and a pause
    // menu. This main game class is extremely simple: all the interesting
    // stuff happens in the ScreenManager component.
    public class SpaceGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        //private readonly ScreenManager _screenManager;
        //private World world;
        List<AsteroidSprite> asteroids;
        BackgroundSprite background;
        

        public SpaceGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = Constants.GAME_WIDTH;
            _graphics.PreferredBackBufferHeight = Constants.GAME_HEIGHT;
            _graphics.ApplyChanges();


            //Going to implement actual game architecture later, going to go simple version for functionality first

            //var screenFactory = new ScreenFactory();
            //Services.AddService(typeof(IScreenFactory), screenFactory);

            //_screenManager = new ScreenManager(this);
            //Components.Add(_screenManager);

            //AddInitialScreens();
        }

        private void AddInitialScreens()
        {
            //_screenManager.AddScreen(new BackgroundScreen(), null);
            //_screenManager.AddScreen(new GamePlayScreen(), null);
            //_screenManager.AddScreen(new BackgroundScreen(), null);
            //_screenManager.AddScreen(new MainMenuScreen(), null);
            //_screenManager.AddScreen(new SplashScreen(), null);
        }

        protected override void Initialize()
        {
            System.Random random = new System.Random();
            asteroids = new List<AsteroidSprite>();
            int xMidPoint = Constants.GAME_WIDTH / 2;
            int yMidPoint = Constants.GAME_HEIGHT / 2;
            for (int i = 0; i < 20; i++)
            {
                int xLocation = random.Next(50, Constants.GAME_WIDTH - 50);
                int yLocation = random.Next(50, Constants.GAME_HEIGHT - 50);
                while ((xLocation >= xMidPoint + 150 || xLocation <= xMidPoint - 150) ||
                    ((yLocation >= yMidPoint + 150 || yLocation <= yMidPoint - 150)))
                {
                    asteroids.Add(new AsteroidSprite()
                    {
                        Center = new Vector2(xLocation, yLocation),
                        Velocity = new Vector2(50 - (float)random.NextDouble() * 100, 50 - (float)random.NextDouble() * 100),
                        Mass = 25
                    });
                    xLocation = random.Next(50, Constants.GAME_WIDTH - 50);
                    yLocation = random.Next(50, Constants.GAME_HEIGHT - 50);
                }
            }

            background = new BackgroundSprite();

            base.Initialize();
            //asteroids = new List

            /*
            base.Initialize();

            
            world = new World();
            world.Gravity = Vector2.Zero;
            var top = 0;
            var bottom = Constants.GAME_HEIGHT;
            var left = 0;
            var right = Constants.GAME_WIDTH;
            var edges = new Body[]
            {
                world.CreateEdge(new Vector2(left, top), new Vector2(right, top)),
                world.CreateEdge(new Vector2(left, top), new Vector2(left, bottom)),
                world.CreateEdge(new Vector2(left, bottom), new Vector2(right, bottom)),
                world.CreateEdge(new Vector2(right, top), new Vector2(right, bottom))
            };
            foreach (var edge in edges)
            {
                edge.BodyType = BodyType.Static;
                edge.SetRestitution(1.0f);
            }
            */

        }

        protected override void LoadContent() 
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            foreach (var asteroid in asteroids) asteroid.LoadContent(Content);
            background.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            //Move the asteroid across the screen
            foreach (var asteroid in asteroids) asteroid.Update(gameTime);
            //Detect collisions of asteroids
            for (int i = 0; i < asteroids.Count; i++)
            {
                for (int j = i + 1; j < asteroids.Count; j++)
                {
                    if (asteroids[i].CollidesWith(asteroids[j]))
                    {
                        asteroids[i].Colliding = true;
                        asteroids[j].Colliding = true;

                        Vector2 collisionAxis = asteroids[i].Center - asteroids[j].Center;
                        collisionAxis.Normalize();
                        float angle = (float)System.Math.Acos(Vector2.Dot(collisionAxis, Vector2.UnitX));
                        float m0 = asteroids[i].Mass;
                        float m1 = asteroids[j].Mass;

                        Vector2 u0 = Vector2.Transform(asteroids[i].Velocity, Matrix.CreateRotationZ(angle));
                        Vector2 u1 = Vector2.Transform(asteroids[j].Velocity, Matrix.CreateRotationZ(angle));

                        Vector2 v0;
                        Vector2 v1;

                        v0.X = ((m0 - m1) / (m0 + m1)) * u0.X + ((2 * m1) / (m0 + m1)) * u1.X;
                        v1.X = ((2 * m0) / (m0 + m1)) * u0.X + ((m1 - m0) / (m0 + m1)) * u1.X;
                        v0.Y = u0.Y;
                        v1.Y = u1.Y;

                        asteroids[i].Velocity = Vector2.Transform(v0, Matrix.CreateRotationZ(angle));
                        asteroids[i].Velocity = Vector2.Transform(v1, Matrix.CreateRotationZ(angle));
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //base.Draw(gameTime);    // The real drawing happens inside the ScreenManager component, not implemented yet
            _spriteBatch.Begin();
            foreach (var asteroid in asteroids) asteroid.Draw(gameTime, _spriteBatch);
            background.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);

        }
    }
}
