using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using SpaceGame_CIS580.Collisions;

namespace SpaceGame_CIS580
{
    public class AsteroidSprite
    {
        private Texture2D _texture;
        private Vector2 origin;
        private float radius;
        private float scale;
        private double timer;
        private int rotationCount;

        private BoundingCircle bounds;

        public BoundingCircle Bounds => bounds;

        public bool Destroyed = false;

        public AsteroidSprite()
        {
            this.bounds = new BoundingCircle(Center + new Vector2(-16, -16), 32);
        }

        /// <summary>
        /// Indicates if the asteroid has collided with anything
        /// </summary>
        public bool Colliding { get; set; }

        /// <summary>
        /// Vector for the center of the asteroid
        /// </summary>
        public Vector2 Center { get; set; }

        /// <summary>
        /// Vector for the velocity of the asteroid
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
        /// Loads the image the asteroid sprite is contained in
        /// </summary>
        /// <param name="contentManager">The game's content manager</param>
        public void LoadContent(ContentManager contentManager)
        {
            _texture = contentManager.Load<Texture2D>("CelestialObjects");
        }

        /// <summary>
        /// Updates the movement of the asteroid
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (Destroyed) return;
            Center += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            //Keeps the asteroid on screen, will eventually want it to travel to the opposite end of the screen
            if (Center.X < radius || Center.X > Constants.GAME_WIDTH - radius) Velocity *= -Vector2.UnitX;
            if (Center.Y < radius || Center.Y > Constants.GAME_HEIGHT - radius) Velocity *= -Vector2.UnitY;
            bounds.Center.X = this.Center.X;
            bounds.Center.Y = this.Center.Y;
            Colliding = false;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Destroyed) return;

            timer += gameTime.ElapsedGameTime.TotalSeconds;
            if (timer > 0.1)
            {
                timer -= 0.1;
                rotationCount++;
            }
            Rectangle source = new Rectangle(0, 192, 32, 32);
            double roation = (float)rotationCount * 0.05;
            spriteBatch.Draw(_texture, Center, source, Color.White, (float)roation, origin, scale, SpriteEffects.None, 0);
        }

        public bool CollidesWith(AsteroidSprite other)
        {
            return this.Mass + other.Mass >= Vector2.Distance(Center, other.Center);
        }
    }

}
