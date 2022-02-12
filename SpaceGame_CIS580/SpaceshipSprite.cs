/**
 * Code for this file adapated from the PhsycisExampleB exercise created by Nathan Bean
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
//using tainicom.Aether.Physics2D.Dynamics;
//using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace SpaceGame_CIS580
{
    public class SpaceshipSprite
    {
        const float LINEAR_ACCELERATION = 50;
        const float ANGULAR_ACCELERATION = 5;

        Texture2D _texture;
        float scale;
        float radius;
        Vector2 origin;

        Game game;
        Vector2 position = new Vector2(Constants.GAME_WIDTH / 2, Constants.GAME_HEIGHT / 2);
        Vector2 velocity;
        Vector2 direction;

        float angle;
        float angularVelocity;

        public SpaceshipSprite(Game game)
        {
            this.game = game;
            this.direction = -Vector2.UnitY;
        }

        /// <summary>
        /// Indicates if the space ship has collided with anything
        /// </summary>
        public bool Colliding { get; set; }

        /// <summary>
        /// Vector for the center of the space ship
        /// </summary>
        public Vector2 Center { get; set; }

        /// <summary>
        /// Vector for the velocity of the space ship
        /// </summary>
        public Vector2 Velocity { get; set; }

        public float Mass
        {
            get => radius;
            set
            {
                radius = value;
                scale = radius / 16;
                origin = new Vector2(16, 16); // asteroid sprite is 32 by 32 pixels
            }
        }

        /// <summary>
        /// Load the space ship texture
        /// </summary>
        /// <param name="contentManager">The content manager to use</param>
        public void LoadContent(ContentManager contentManager)
        {
            _texture = contentManager.Load<Texture2D>("shuttle2");
        }

        /// <summary>
        /// Updates the sprite
        /// </summary>
        /// <param name="gameTime">The game's play time</param>
        public void Update(GameTime gameTime)
        {
            Colliding = false;

            KeyboardState keyboardState = Keyboard.GetState();
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 acceleration = new Vector2(0, 0);
            float angularAcceleration = 0;
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                acceleration += direction * LINEAR_ACCELERATION;
                angularAcceleration += ANGULAR_ACCELERATION;
            }
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                acceleration += direction * LINEAR_ACCELERATION;
                angularAcceleration -= ANGULAR_ACCELERATION;
            }
            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            {
                acceleration += direction * LINEAR_ACCELERATION;
            }
            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                acceleration += -direction * LINEAR_ACCELERATION;
            }

            angularVelocity += angularAcceleration * t;
            angle += angularAcceleration * t;
            direction.X = (float)Math.Sin(angle);
            direction.Y = (float)-Math.Cos(angle);

            velocity += acceleration * t;
            position += velocity * t;

            // Wrap the ship to keep it on-screen
            var viewport = game.GraphicsDevice.Viewport;
            if (position.Y < 0) position.Y = viewport.Height;
            if (position.Y > viewport.Height) position.Y = 0;
            if (position.X < 0) position.X = viewport.Width;
            if (position.X > viewport.Width) position.X = 0;
        }

        /// <summary>
        /// Draws the space ship sprite
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, position, null, Color.White, angle, new Vector2(26, 26), 1.0f, SpriteEffects.None, 0);
        }

    }
}
