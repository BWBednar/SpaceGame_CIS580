using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SpaceGame_CIS580
{
    public class BackgroundSprite
    {
        private Texture2D _backgroundTexture;

        private double animationTimer;
        private int animationFrame;

        /// <summary>
        /// Loads the image the asteroid sprite is contained in
        /// </summary>
        /// <param name="contentManager">The game's content manager</param>
        public void LoadContent(ContentManager contentManager)
        {
            _backgroundTexture = contentManager.Load<Texture2D>("space_background");
        }

        /// <summary>
        /// Draws the background image.
        /// This background image should have some slight motion, so the timer is needed for this animation
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (animationTimer > 0.2)
            {
                animationTimer -= 0.2;
                animationFrame++;
                if (animationFrame == 4) animationFrame = 0;
            }
            var source = new Rectangle(animationFrame * 64, 0, 64, 64);

            spriteBatch.Draw(_backgroundTexture, new Vector2(100,100), source, Color.White, 0, new Vector2(16, 16), 0, SpriteEffects.None, 1);
            //spriteBatch.Draw(_backgroundTexture, fullscreen,
            //new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
        }
    }
}
