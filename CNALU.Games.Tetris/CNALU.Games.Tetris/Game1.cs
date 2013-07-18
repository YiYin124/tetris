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
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Random random = new Random();

        Texture2D backgroundTexture;
        Texture2D blockTexture;
        Texture2D gameBoxTexture;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
            graphics.IsFullScreen = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            backgroundTexture = Content.Load<Texture2D>(@"Images\bg");
            blockTexture = Content.Load<Texture2D>(@"Images\block");
            gameBoxTexture = Content.Load<Texture2D>(@"Images\gamebox");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundTexture, Vector2.Zero, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(gameBoxTexture, Vector2.Zero, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 15; y++)
                {
                    spriteBatch.Draw(blockTexture, new Vector2(108 + x * 30, 57 + y * 30), new Rectangle(30 * random.Next(3), 30 * random.Next(2), 30, 30), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
                }
            }

            for (int x = 0; x < 6; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    spriteBatch.Draw(blockTexture, new Vector2(508 + x * 30, 57 + y * 30), new Rectangle(30 * random.Next(3), 30 * random.Next(2), 30, 30), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
