using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceGame_CIS580
{
    public class ExplosionSprite
    {
        private Vector2 position;
        private int animationFrame;
        private double animationTimer;
        private Texture2D _texture;

        private bool animationComplete;
        public bool AnimationComplete { get { return animationComplete; } }

        public ExplosionSprite(Vector2 position)
        {
            this.position = position;
        }

        /// <summary>
        /// Loads the image the explosion sprite is contained in
        /// </summary>
        /// <param name="contentManager">The game's content manager</param>
        public void LoadContent(ContentManager contentManager)
        {
            _texture = contentManager.Load<Texture2D>("explosionBig");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (animationComplete) return;
            if(animationFrame == 9)
            {
                animationComplete = true;
                return;
            }
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (animationTimer > 0.1)
            {
                animationTimer -= 0.1;
                animationFrame++;
            }
            Rectangle source = new Rectangle(animationFrame * 111, 0, 111, 111);
            spriteBatch.Draw(_texture, position, source, Color.White, 0, new  Vector2 (55, 55) , 0.70f, SpriteEffects.None, 0);
        }
    }
}
