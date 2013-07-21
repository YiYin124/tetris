using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CNALU.Games.Tetris
{
    class Block
    {
        protected DynamicTexture texture;

        public Vector2 Position
        {
            get { return texture.Position; }
            set
            {
                texture.Position = value;
            }
        }

        public Rectangle Bounts
        {
            get { return texture.Bounds; }
        }

        public Block(DynamicTexture texture, Vector2 position)
        {
            this.texture = texture;
            this.Position = position;
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            texture.Draw(spriteBatch, gameTime);
        }
    }
}
