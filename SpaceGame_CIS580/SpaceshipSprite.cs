using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace SpaceGame_CIS580
{
    public class SpaceshipSprite
    {
        Texture2D _texture;
        float scale;
        float radius;
        Vector2 origin;

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
            _texture = contentManager.Load<Texture2D>("ship");
        }

        /// <summary>
        /// Updates the sprite
        /// </summary>
        /// <param name="gameTime">The game's play time</param>
        public void Update(GameTime gameTime)
        {
            Colliding = false;
        }

        /// <summary>
        /// Draws the space ship sprite
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Vector2(Constants.GAME_HEIGHT / 2, Constants.GAME_WIDTH / 2), null, Color.White, 0, new Vector2(64, 64), 1.0f, SpriteEffects.None, 0);
        }

        bool CollisionHandler(Fixture fixture, Fixture other, Contact contact)
        {
            Colliding = true;
            return true;
        }
    }
}
