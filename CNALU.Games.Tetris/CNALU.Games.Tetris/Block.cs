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
    class Block : IBlock
    {
        Texture2D texture;
        public Rectangle? SourceRectangle { get; private set; }
        public Vector2 Position { get; set; }

        public Block(Texture2D texture, Rectangle? sourceRectangle) 
        {
            this.texture = texture;
            this.Position = Vector2.Zero;
            this.SourceRectangle = sourceRectangle;
        }

        public Block(Texture2D texture, Vector2 position, Rectangle? sourceRectangle)
            : this(texture, sourceRectangle)
        {
            this.Position = position;
        }

        public void Update(GameTime gameTime) { }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, Position, SourceRectangle, Color.White);
            spriteBatch.End();
        }
    }
}
