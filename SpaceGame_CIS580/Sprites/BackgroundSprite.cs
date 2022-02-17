using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SpaceGame_CIS580
{
    /// <summary>
    /// Class representing the background sprite
    /// </summary>
    public class BackgroundSprite
    {
        Texture2D _backgroundTexture;

        double animationTimer;
        int animationFrame;

        /// <summary>
        /// Loads the image the background sprite is contained in
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

            //fill the background with the animation
            for(int i = 0; i < Constants.GAME_HEIGHT; i += 64)
            {
                for (int j = 0; j < Constants.GAME_WIDTH; j += 64)
                {
                    spriteBatch.Draw(_backgroundTexture, new Rectangle(j, i, 64, 64), source, Color.White);
                }   
            }
        }
    }
}
