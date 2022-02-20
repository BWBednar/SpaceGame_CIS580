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
using SpaceGame_CIS580.Collisions;
//using tainicom.Aether.Physics2D.Dynamics;
//using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace SpaceGame_CIS580
{
    public class SpaceshipSprite
    {

        Texture2D _texture;
        Game game;
        Vector2 position = new Vector2(Constants.GAME_WIDTH / 2, Constants.GAME_HEIGHT / 2);
        Vector2 direction;

        /// <summary>
        /// The position of the space ship sprite
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        private BoundingRectangle bounds;

        /// <summary>
        /// The collision bounds of the sprite
        /// </summary>
        public BoundingRectangle Bounds => bounds;

        /// <summary>
        /// Constructor for the space ship sprite
        /// </summary>
        /// <param name="game">The game being played</param>
        /// <param name="bounds">The collision bounds of the ship</param>
        public SpaceshipSprite(Game game, BoundingRectangle bounds)
        {
            this.game = game;
            this.direction = -Vector2.UnitY;
            this.bounds = bounds;
        }

        /// <summary>
        /// Indicates if the space ship has collided with anything
        /// </summary>
        public bool Colliding { get; set; }

        /// <summary>
        /// Indicates if the ship has been destroyed
        /// </summary>
        public bool Destroyed { get; set; }

        /// <summary>
        /// Vector for the center of the space ship
        /// </summary>
        public Vector2 Center { get; set; }

        public Vector2 Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        /// <summary>
        /// Vector for the velocity of the space ship
        /// </summary>
        public Vector2 Velocity { get; set; }

        public float Angle { get; set; }

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

            // Wrap the ship to keep it on-screen
            var viewport = game.GraphicsDevice.Viewport;
            if (position.Y < 0) position.Y = viewport.Height;
            if (position.Y > viewport.Height) position.Y = 0;
            if (position.X < 0) position.X = viewport.Width;
            if (position.X > viewport.Width) position.X = 0;

            bounds.X = position.X;
            bounds.Y = position.Y;
        }

        /// <summary>
        /// Draws the space ship sprite
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The sprite batch</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, position, null, Color.White, Angle, new Vector2(26, 26), 1.0f, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Detects if there has been a collision, particularly with an asteroid sprite
        /// </summary>
        /// <param name="circle">The bounding circle being detected</param>
        /// <returns>If the ship has collided with the circle</returns>
        public bool CollidesWith(BoundingCircle circle)
        {
            return this.bounds.CollidesWith(circle);
        }

    }
}
