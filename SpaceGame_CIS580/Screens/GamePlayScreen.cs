using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using SpaceGame_CIS580.StateManagement;
using SpaceGame_CIS580.Sprites;
using SpaceGame_CIS580.Collisions;

namespace SpaceGame_CIS580.Screens
{
    /// <summary>
    /// Implements the main game logic
    /// </summary>
    public class GamePlayScreen : GameScreen
    {
        private ContentManager _content;
        private SpriteFont _gameFont;

        private SpaceshipSprite ship;
        private List<AsteroidSprite> asteroids;
        private List<BlasterFireSprite> blasterFire;
        private List<ExplosionSprite> explosions;

        private double timer;
        private bool gameStart = true;
        private bool gameLose = false;
        private bool gameVictory = false;
        private float _pauseAlpha;
        private readonly InputAction _pauseAction;
        private readonly InputAction _fireAction;
        private KeyboardState lastInput;
        private KeyboardState currentInput;
        private SoundEffect blasterFireSound;
        private SoundEffect shipExplosionSound;
        private SoundEffect asteroidExplosionSound;

        public GamePlayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);

            _fireAction = new InputAction(
                new[] { Buttons.X },
                new[] { Keys.Space }, true);
        }

        public override void Activate()
        {

            //Get 10 asteroids in random positions around the ship sprite
            System.Random random = new System.Random();
            asteroids = new List<AsteroidSprite>();
            int xMidPoint = Constants.GAME_WIDTH / 2;
            int yMidPoint = Constants.GAME_HEIGHT / 2;
            for (int i = 0; i < 10; i++)
            {
                int xLocation = random.Next(50, Constants.GAME_WIDTH - 50);
                int yLocation = random.Next(50, Constants.GAME_HEIGHT - 50);
                while ((xLocation <= xMidPoint + 150 && xLocation >= xMidPoint - 150) &&
                    ((yLocation <= yMidPoint + 150 && yLocation >= yMidPoint - 150)))
                {

                    xLocation = random.Next(50, Constants.GAME_WIDTH - 50);
                    yLocation = random.Next(50, Constants.GAME_HEIGHT - 50);
                }
                asteroids.Add(new AsteroidSprite()
                {
                    Center = new Vector2(xLocation, yLocation),
                    Velocity = new Vector2(50 - (float)random.NextDouble() * 50, 50 - (float)random.NextDouble() * 50),
                    Mass = 25
                });
            }
            //Establish the other needed sprites
            ship = new SpaceshipSprite(ScreenManager.Game, new BoundingRectangle(xMidPoint - 26, yMidPoint - 26, 52, 52));
            blasterFire = new List<BlasterFireSprite>();
            explosions = new List<ExplosionSprite>();

            if (_content == null) 
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _gameFont = _content.Load<SpriteFont>("PressStart2P");
            foreach (var asteroid in asteroids) asteroid.LoadContent(_content);
            ship.LoadContent(_content);
            blasterFireSound = _content.Load<SoundEffect>("Laser_Sound");
            shipExplosionSound = _content.Load<SoundEffect>("Ship_Explosion");
            asteroidExplosionSound = _content.Load<SoundEffect>("Asteroid_Explosion");

            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
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

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                lastInput = currentInput;
                currentInput = Keyboard.GetState();

                //Account for some user input for blaster fire and exit
                if (currentInput.IsKeyUp(Keys.Space) && lastInput.IsKeyDown(Keys.Space)) CreateBlasterFire();

                //Move the asteroid across the screen
                foreach (var asteroid in asteroids) asteroid.Update(gameTime);

                //Detect collisions of asteroids
                AsteroidDetectionAndUpdates();

                //update any blaster fire that is present
                foreach (var fire in blasterFire) fire.Update(gameTime);

                //update the ship based on user input
                ship.Update(gameTime);
            }
        }

        //NEEDS CHANGES
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));


            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];

            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player))
            {
                //Implement pause screen later
                //ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            if (_fireAction.Occurred(input, ControllingPlayer, out player))
            {
                CreateBlasterFire();
            }

            Vector2 direction;
            float angularVelocity = 0;
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 acceleration = new Vector2(0, 0);
            float angularAcceleration = 0;
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                acceleration += ship.Direction * Constants.LINEAR_ACCELERATION;
                angularAcceleration += Constants.ANGULAR_ACCELERATION;
            }
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                acceleration += ship.Direction * Constants.LINEAR_ACCELERATION;
                angularAcceleration -= Constants.ANGULAR_ACCELERATION;
            }
            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            {
                acceleration += ship.Direction * Constants.LINEAR_ACCELERATION;
            }
            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                acceleration += -ship.Direction * Constants.LINEAR_ACCELERATION;
            }

            angularVelocity += angularAcceleration * t;
            ship.Angle += angularAcceleration * t;
            direction.X = (float)Math.Sin(ship.Angle);
            direction.Y = (float)-Math.Cos(ship.Angle);

            ship.Velocity += acceleration * t;
            ship.Position += ship.Velocity * t;
            
        }

        public override void Draw(GameTime gameTime)
        {
            var _spriteBatch = ScreenManager.SpriteBatch;
            _spriteBatch.Begin();
            foreach (var asteroid in asteroids) if (!asteroid.Destroyed) asteroid.Draw(gameTime, _spriteBatch);
            foreach (var fire in blasterFire) if (fire.OnScreen) fire.Draw(gameTime, _spriteBatch);
            if (!ship.Destroyed) ship.Draw(gameTime, _spriteBatch);
            foreach (var explosion in explosions) if (!explosion.AnimationComplete) explosion.Draw(gameTime, _spriteBatch);
            //Display a message at the start of the game
            if (gameStart)
            {
                _spriteBatch.DrawString(ScreenManager.Font, "Destory", new Vector2(75, 35), Color.Gold);
                _spriteBatch.DrawString(ScreenManager.Font, "Asteroids", new Vector2(115, 85), Color.Gold);
                timer += gameTime.ElapsedGameTime.TotalSeconds;
                if (timer >= 3.0)
                {
                    gameStart = false;
                }
            }
            //Display a message if the player loses the game
            if (gameLose)
            {
                _spriteBatch.DrawString(ScreenManager.Font, "Restart Game", new Vector2(75, 35), Color.Gold);
                _spriteBatch.DrawString(ScreenManager.Font, "To Try Again", new Vector2(115, 85), Color.Gold);
            }
            //Display a message if the player wins the game
            if (gameVictory && !gameLose)
            {
                _spriteBatch.DrawString(ScreenManager.Font, "You Win!", new Vector2(75, 35), Color.Gold);
                _spriteBatch.DrawString(ScreenManager.Font, "Well Done!", new Vector2(115, 85), Color.Gold);
            }
            _spriteBatch.End();

        }

        /// <summary>
        /// Method that handles the collision interactions of the sprites and any actions that result from those collisions
        /// </summary>
        private void AsteroidDetectionAndUpdates()
        {
            //variables for tracking blaster fires and asteroids that need to be removed
            int toRemoveAsteroid = -1;
            int toRemoveBlaster = -1;
            for (int i = 0; i < asteroids.Count; i++)
            {
                //If the space ship has hit an asteroid, the ship will be destroyed
                if (ship.CollidesWith(asteroids[i].Bounds) && !ship.Destroyed)
                {
                    ship.Destroyed = true;
                    shipExplosionSound.Play();
                    CreateExplosion(new Vector2(ship.Bounds.X, ship.Bounds.Y));
                    gameLose = true;
                }
                foreach (var fire in blasterFire)
                {
                    //If a blaster fire has hit an asteroid, that fire will be gone along with the asteroid
                    if (fire.CollidesWith(asteroids[i].Bounds) && !asteroids[i].Destroyed)
                    {
                        asteroids[i].Destroyed = true;
                        asteroidExplosionSound.Play();
                        CreateExplosion(new Vector2(asteroids[i].Bounds.Center.X, asteroids[i].Bounds.Center.Y));
                        toRemoveAsteroid = asteroids.IndexOf(asteroids[i]);
                        toRemoveBlaster = blasterFire.IndexOf(fire);
                    }
                }
                for (int j = i + 1; j < asteroids.Count; j++)
                {
                    //Detects collision between asteroids, taken from PhysicsExampleC assignment by Nathan Bean
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
            //Removes the blaster fire and asteroid sprites if they collided
            if (toRemoveAsteroid != -1 && toRemoveBlaster != -1)
            {
                asteroids[toRemoveAsteroid] = null;
                asteroids.RemoveAt(toRemoveAsteroid);
                blasterFire[toRemoveBlaster] = null;
                blasterFire.RemoveAt(toRemoveBlaster);
            }
            //The player wins if there are no more asteroids
            if (asteroids.Count == 0) gameVictory = true;
        }

        /// <summary>
        /// Helper method for creating new blaster fire sprites
        /// </summary>
        private void CreateBlasterFire()
        {
            BlasterFireSprite newFire = new BlasterFireSprite(ship.Angle, ship.Position,
                new BoundingRectangle(ship.Position.X - 16, ship.Position.Y - 16, 16, 16));
            blasterFireSound.Play();
            newFire.LoadContent(_content);
            blasterFire.Add(newFire);
        }

        /// <summary>
        /// Helper method for creating new explosion sprites
        /// </summary>
        /// <param name="position">The position of the explosion</param>
        private void CreateExplosion(Vector2 position)
        {
            ExplosionSprite newExplosion = new ExplosionSprite(position);
            newExplosion.LoadContent(_content);
            explosions.Add(newExplosion);
        }
    }
}
