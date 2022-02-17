using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SpaceGame_CIS580.Sprites
{
    class LoadingSprite
    {
        Texture2D _texture;
        double animationTimer;
        int rotationCount;
        Vector2 center;

        public LoadingSprite(Vector2 center)
        {
            this.center = center;
        }

        public Vector2 Center
        {
            get { return center; }
            set { this.center = value; }
        }

        /// <summary>
        /// Loads the image the sprite is contained in
        /// </summary>
        /// <param name="contentManager">The game's content manager</param>
        public void LoadContent(ContentManager contentManager)
        {
            _texture = contentManager.Load<Texture2D>("CelestialObjects");
        }

        /// <summary>
        /// Draws the sprite, rotates sprite in a circle slightly
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The sprite batch</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (animationTimer > 0.1)
            {
                animationTimer -= 0.1;
                rotationCount++;
            }
            Rectangle source = new Rectangle(129, 197, 32, 32);
            double roation = (float)rotationCount * -0.05;
            spriteBatch.Draw(_texture, center, source, Color.White, (float)roation, new Vector2(16, 16), 2.0f, SpriteEffects.None, 0);
        }
    }
}
