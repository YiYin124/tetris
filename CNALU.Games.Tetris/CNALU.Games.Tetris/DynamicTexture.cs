using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CNALU.Games.Tetris
{
    class DynamicTexture
    {
        Texture2D texture;
        Vector2 nowPelPosition;
        int lastGameTime;
        Vector2 position;
        Vector2 scale;

        public bool IsPlaying { get; private set; }
        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                this.Bounds = new Rectangle((int)position.X, (int)position.Y, (int)(Scale.X * (float)Width), (int)(Scale.Y * (float)Height));
            }
        }
        public int FramePerSecond { get; set; }
        public float Rotation { get; set; } // *
        public Vector2 Origin { get; set; } // *
        public Vector2 Scale
        {
            get { return scale; }
            set
            {
                scale = value;
                this.Bounds = new Rectangle((int)Position.X, (int)Position.Y, (int)(scale.X * (float)Width), (int)(scale.Y * (float)Height));
            }
        }
        public Rectangle Bounds { get; private set; }

        public readonly Vector2 PelPosition;
        public readonly int Width;
        public readonly int Height;
        public readonly int MillisecondPerFrame;

        public DynamicTexture(Texture2D texture, Vector2 pelPosition, int width, int height, int framePerSecond)
            : this(texture, Vector2.Zero, pelPosition, width, height, framePerSecond, 0.0F, Vector2.Zero, new Vector2(1.0F, 1.0F)) { }

        public DynamicTexture(Texture2D texture, Vector2 position, Vector2 pelPosition, int width, int height, int framePerSecond)
            : this(texture, position, pelPosition, width, height, framePerSecond, 0.0F, Vector2.Zero, new Vector2(1.0F, 1.0F)) { }

        public DynamicTexture(Texture2D texture, Vector2 position, Vector2 pelPosition, int width, int height, int framePerSecond, float rotation, Vector2 origin, Vector2 scale)
        {
            this.texture = texture;
            this.Position = position;
            this.PelPosition = pelPosition;
            this.Width = width;
            this.Height = height;
            this.FramePerSecond = framePerSecond;
            MillisecondPerFrame = 1000 / framePerSecond;
            nowPelPosition = Vector2.Zero;
            IsPlaying = false;
            this.Rotation = rotation;
            this.Origin = origin;
            this.Scale = scale;

            this.Bounds = new Rectangle((int)Position.X, (int)Position.Y, (int)(Scale.X * (float)Width), (int)(Scale.Y * (float)Height));
        }

        public virtual void Play()
        {
            if (!IsPlaying)
                IsPlaying = true;
        }

        public virtual void Pause()
        {
            if (IsPlaying)
                IsPlaying = false;
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (IsPlaying)
            {
                lastGameTime += gameTime.ElapsedGameTime.Milliseconds;

                if (lastGameTime >= MillisecondPerFrame)
                {
                    lastGameTime = 0;
                    if (nowPelPosition.X < PelPosition.X)
                        nowPelPosition.X++;
                    else
                    {
                        nowPelPosition.X = 0;
                        if (nowPelPosition.Y < PelPosition.Y)
                            nowPelPosition.Y++;
                        else
                            nowPelPosition.Y = 0;
                    }
                }
            }

            spriteBatch.Begin();
            spriteBatch.Draw(texture, Position, new Rectangle(Width * (int)nowPelPosition.X, Height * (int)nowPelPosition.Y, Width, Height), Color.White, Rotation, Origin, Scale, SpriteEffects.None, 0.0F);
            spriteBatch.End();
        }
    }
}
