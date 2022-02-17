using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceGame_CIS580.Collisions;

namespace SpaceGame_CIS580
{
    /// <summary>
    /// Class representing a blaster fire sprite
    /// </summary>
    public class BlasterFireSprite
    {
        const float LINEAR_ACCELERATION = 500;
        Texture2D _texture;
        float angle;
        Vector2 position;
        Vector2 velocity;
        Vector2 direction;
        float angularVelocity;

        /// <summary>
        /// Vector for the center of the blaster fire
        /// </summary>
        public Vector2 Center { get; set; }

        /// <summary>
        /// If the blaster fire is still on the game screen
        /// </summary>
        public bool OnScreen { get; set; } = true;


        private BoundingRectangle bounds;
        /// <summary>
        /// The collision bounds of the blaster fire
        /// </summary>
        public BoundingRectangle Bounds => bounds;

        /// <summary>
        /// Constructor for the blaster fire sprite
        /// </summary>
        /// <param name="angle">The angle of the blaster fire movement</param>
        /// <param name="startingPosition">Where the sprite is starting</param>
        /// <param name="bounds">The bounding rectangle</param>
        public BlasterFireSprite(float angle, Vector2 startingPosition, BoundingRectangle bounds)
        {
            this.angle = angle;
            this.position = startingPosition + new Vector2(0, 0);
            this.bounds = bounds;
        }

        /// <summary>
        /// Loads the image the blaster fire sprite is contained in
        /// </summary>
        /// <param name="contentManager">The game's content manager</param>
        public void LoadContent(ContentManager contentManager)
        {
            _texture = contentManager.Load<Texture2D>("blasterbolt");
        }

        /// <summary>
        /// Updates the sprite
        /// </summary>
        /// <param name="gameTime">The game's play time</param>
        public void Update(GameTime gameTime)
        {
            if (!OnScreen) return;
            //If it is no longer on screen, don't show it
            if (this.position.X > Constants.GAME_WIDTH || this.position.X < 0
                || this.position.Y > Constants.GAME_HEIGHT || this.position.Y < 0)
            {
                OnScreen = false;
                return;
            }


            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 acceleration = new Vector2(0, 0);
            float angularAcceleration = 0;

            acceleration += direction * LINEAR_ACCELERATION;
            angularVelocity += angularAcceleration * t;
            angle += angularAcceleration * t;
            direction.X = (float)Math.Sin(angle);
            direction.Y = (float)-Math.Cos(angle);

            velocity += acceleration * t;
            position += velocity * t;

            bounds.X = this.position.X;
            bounds.Y = this.position.Y;
        }

        /// <summary>
        /// Draws the blaster sprite
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The sprite batch</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!OnScreen) return; //Do not draw if the sprite is not on screen
            Rectangle source = new Rectangle(0, 0, 32, 32);
            spriteBatch.Draw(_texture, position, source, Color.White, angle, new Vector2(16, 16), 1.0f, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Detects a collision with an asteroid sprite's bounding circle
        /// </summary>
        /// <param name="asteroid">The asteroid sprite's bounding circle</param>
        /// <returns>If there was a collision</returns>
        public bool CollidesWith(BoundingCircle asteroid)
        {
            if (this.bounds.CollidesWith(asteroid))
            {
                this.OnScreen = false;
                return true;
            }
            return false;
        }
    }
}
