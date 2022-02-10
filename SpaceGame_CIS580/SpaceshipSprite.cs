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
        Vector2 origin;
        Body body;

        /// <summary>
        /// A boolean indicating if the spaceship sprite has collided with another sprite
        /// </summary>
        public bool Colliding { get; protected set; }

        public SpaceshipSprite(float body)
        {

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
