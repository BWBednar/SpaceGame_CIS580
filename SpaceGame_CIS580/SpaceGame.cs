/**
 * Starting Code from Nathan Bean's GameArchitectureExample project
 */

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceGame_CIS580.Screens;
using SpaceGame_CIS580.StateManagement;
using SpaceGame_CIS580.Collisions;
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
        SpaceshipSprite ship;
        List<BlasterFireSprite> blasterFire;
        List<ExplosionSprite> explosions;
        

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
            for (int i = 0; i < 10; i++)
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
            ship = new SpaceshipSprite(this, new BoundingRectangle(xMidPoint - 26, yMidPoint - 26, 52, 52));
            blasterFire = new List<BlasterFireSprite>();
            explosions = new List<ExplosionSprite>();

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
            ship.LoadContent(Content);
        }

        KeyboardState lastInput;
        KeyboardState currentInput;
        protected override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
            lastInput = currentInput;
            currentInput = Keyboard.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
            if (currentInput.IsKeyUp(Keys.Space) && lastInput.IsKeyDown(Keys.Space)) CreateBlasterFire();

            //Move the asteroid across the screen
            foreach (var asteroid in asteroids) asteroid.Update(gameTime);
            
            //Detect collisions of asteroids
            AsteroidDetectionAndUpdates();

            //update any blaster fire that is present
            foreach (var fire in blasterFire) fire.Update(gameTime);

            //update the ship based on user input
            ship.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //base.Draw(gameTime);    // The real drawing happens inside the ScreenManager component, not implemented yet
            _spriteBatch.Begin();
            background.Draw(gameTime, _spriteBatch);
            foreach (var asteroid in asteroids) if(!asteroid.Destroyed) asteroid.Draw(gameTime, _spriteBatch);
            foreach (var fire in blasterFire) if (fire.OnScreen) fire.Draw(gameTime, _spriteBatch);
            if(!ship.Destroyed) ship.Draw(gameTime, _spriteBatch);
            foreach (var explosion in explosions) if (!explosion.AnimationComplete) explosion.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);

        }

        private void AsteroidDetectionAndUpdates()
        {
            int toRemoveAsteroid = -1;
            int toRemoveBlaster = -1;
            for (int i = 0; i < asteroids.Count; i++)
            {
                //If the space ship has hit a rocket, the ship will be destroyed
                if (ship.CollidesWith(asteroids[i].Bounds) && !asteroids[i].Destroyed)
                {
                    ship.Destroyed = true;
                    CreateExplosion(ship.Position);
                }
                foreach (var fire in blasterFire)
                {
                    if (fire.CollidesWith(asteroids[i].Bounds) && !asteroids[i].Destroyed)
                    {
                        asteroids[i].Destroyed = true;
                        CreateExplosion(asteroids[i].Bounds.Center);
                        toRemoveAsteroid = asteroids.IndexOf(asteroids[i]);
                        toRemoveBlaster = blasterFire.IndexOf(fire);
                    }
                }
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
            if (toRemoveAsteroid != -1 && toRemoveBlaster != -1)
            {
                asteroids[toRemoveAsteroid] = null;
                asteroids.RemoveAt(toRemoveAsteroid);
                blasterFire[toRemoveBlaster] = null;
                blasterFire.RemoveAt(toRemoveBlaster);
            }
        }

        private void CreateBlasterFire()
        {
            BlasterFireSprite newFire = new BlasterFireSprite(ship.Angle, ship.Position,
                new BoundingRectangle(ship.Position.X - 16, ship.Position.Y - 16, 16, 16));
            newFire.LoadContent(Content);
            blasterFire.Add(newFire);
        }

        private void CreateExplosion(Vector2 position)
        {
            ExplosionSprite newExplosion = new ExplosionSprite(position);
            newExplosion.LoadContent(Content);
            explosions.Add(newExplosion);
        }
    }
}
