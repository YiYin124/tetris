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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class GameComponentTetris : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;

        Texture2D blocksTexture;
        Texture2D gameBoxTexture;

        DynamicTexture blockAnimationTexture;
        Block testGameBlock;

        GameBlock gameBlock;

        public GameComponentTetris(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            blocksTexture = Game.Content.Load<Texture2D>(@"Images/block");
            gameBoxTexture = Game.Content.Load<Texture2D>(@"Images/gamebox");

            blockAnimationTexture = new DynamicTexture(Game.Content.Load<Texture2D>(@"Images/block"), new Vector2(3, 2), 30, 30, 3);
            blockAnimationTexture.Play();

            testGameBlock = new Block(blockAnimationTexture, Vector2.Zero);

            gameBlock = new GameBlock(new System.Drawing.Size(10, 15), new Vector2(108.0F, 57.0F), Game.Content.Load<Texture2D>(@"Images/block"));
            gameBlock.Test();

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            gameBlock.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(gameBoxTexture, Vector2.Zero, Color.White);
            spriteBatch.End();

            gameBlock.Draw(spriteBatch, gameTime);

            base.Draw(gameTime);
        }
    }
}
