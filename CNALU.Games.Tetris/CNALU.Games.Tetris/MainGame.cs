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
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D backgroundTexture;

        TetrisGameComponent tetrisGameComponent;

        KeyboardState oldKeyState;
        MouseState oldMouseState;

        Texture2D mouseTexture;
        Vector2 mousePosition;
        Rectangle mouseRectangle;

        Texture2D startMenuTexture;

        SpriteFont startFont;
        SpriteFont menuFont;

        Vector2 startFontPosition;
        Vector2 menuFontPosition;

        Rectangle menu1Rectangle;
        Rectangle menu2Rectangle;

        Color menu1Color;
        Color menu2Color;

        Texture2D arrow_1Texture;
        Texture2D arrow_2Texture;

        Texture2D selectArrowLeft;
        Texture2D selectArrowRight;

        Vector2 selectArrowPosition;
        int level;
        SpriteFont setFont;

        Rectangle selectArrowLeftRectangle;
        Rectangle selectArrowRightRectangle;

        Texture2D startButton;
        Vector2 startButtonPosition;
        Rectangle startButtonRectangle;
        Color startButtonColor;

        enum GameStatus
        {
            Start,
            Set,
            StartGame,
            Help,
        }

        GameStatus gameStatus;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
            graphics.IsFullScreen = false;

            tetrisGameComponent = new TetrisGameComponent(this);
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
            startFontPosition = new Vector2(170, 100);

            menuFontPosition = new Vector2(300, 380);

            menu1Rectangle = new Rectangle(300, 380, 20 * 10, 20);

            menu2Rectangle = new Rectangle(300, 380 + 100, 20 * 4, 20);

            menu1Color = Color.Black;
            menu2Color = Color.Black;

            selectArrowPosition = new Vector2((800 - 260) / 2, 200);
            level = 0;

            selectArrowLeftRectangle = new Rectangle((int)selectArrowPosition.X, (int)selectArrowPosition.Y, 60, 60);
            selectArrowRightRectangle = new Rectangle((int)selectArrowPosition.X + 200, (int)selectArrowPosition.Y, 60, 60);

            startButtonPosition = new Vector2(300, 400);

            startButtonRectangle = new Rectangle((int)startButtonPosition.X, (int)startButtonPosition.Y, 200, 100);
            startButtonColor = Color.Black;

            gameStatus = GameStatus.Start;
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
            backgroundTexture = Content.Load<Texture2D>("Images/bg");
            mouseTexture = Content.Load<Texture2D>("Images/mouse");
            startMenuTexture = Content.Load<Texture2D>("Images/start_menu");

            startFont = Content.Load<SpriteFont>("Fonts/start");
            menuFont = Content.Load<SpriteFont>("Fonts/menu");

            arrow_1Texture = Content.Load<Texture2D>("Images/arrow_1");
            arrow_2Texture = Content.Load<Texture2D>("Images/arrow_2");

            selectArrowLeft = arrow_1Texture;
            selectArrowRight = arrow_1Texture;

            startButton = Content.Load<Texture2D>("Images/start_button");

            setFont = Content.Load<SpriteFont>("Fonts/set");
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
            UpdateInput(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            // 绘制背景
            spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);

            spriteBatch.End();

            switch (gameStatus)
            {
                case GameStatus.Start:
                    {
                        // TODO: Add your drawing code here
                        spriteBatch.Begin();

                        // 绘制启动菜单
                        spriteBatch.Draw(startMenuTexture, Vector2.Zero, Color.White);

                        // 绘制标题
                        spriteBatch.DrawString(startFont, "Tetris", startFontPosition, Color.Black);

                        // 绘制菜单内容
                        spriteBatch.DrawString(menuFont, "Start Game", menuFontPosition, menu1Color);
                        spriteBatch.DrawString(menuFont, "Help", new Vector2(menuFontPosition.X, menuFontPosition.Y + 100), menu2Color);

                        // 绘制鼠标
                        spriteBatch.Draw(mouseTexture, mousePosition, Color.White);

                        spriteBatch.End();
                        break;
                    }
                case GameStatus.Set:
                    {
                        spriteBatch.Begin();

                        spriteBatch.Draw(selectArrowLeft, selectArrowPosition, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                        spriteBatch.DrawString(setFont, "" + (level + 1), new Vector2(selectArrowPosition.X + 100, selectArrowPosition.Y - 20), Color.Black);
                        spriteBatch.Draw(selectArrowRight, new Vector2(selectArrowPosition.X + 200, selectArrowPosition.Y), Color.White);

                        spriteBatch.Draw(startButton, startButtonPosition, Color.White);
                        spriteBatch.DrawString(menuFont, "Start", new Vector2(startButtonPosition.X + 60, startButtonPosition.Y + 25), startButtonColor);

                        // 绘制鼠标
                        spriteBatch.Draw(mouseTexture, mousePosition, Color.White);


                        spriteBatch.End();
                        break;
                    }
                case GameStatus.StartGame:
                    {
                        break;
                    }

                case GameStatus.Help:
                    {
                        break;
                    }
                default:
                    {

                    }
                    break;
            }

            base.Draw(gameTime);
        }

        void UpdateInput(GameTime gameTime)
        {
            KeyboardState newKeyState = Keyboard.GetState();
            MouseState newMouseState = Mouse.GetState();

            mousePosition = new Vector2(newMouseState.X, newMouseState.Y);
            mouseRectangle = new Rectangle(newMouseState.X, newMouseState.Y, 30, 30);

            switch (gameStatus)
            {
                case GameStatus.Start:
                    {
                        if (newKeyState.IsKeyDown(Keys.Escape) & !oldKeyState.IsKeyDown(Keys.Escape))
                        {
                            this.Exit();
                        }

                        if (mouseRectangle.Intersects(menu1Rectangle))
                        {
                            menu1Color = Color.Gray;

                            if ((newMouseState.LeftButton == ButtonState.Pressed) & (!(oldMouseState.LeftButton == ButtonState.Pressed)))
                            {
                                gameStatus = GameStatus.Set;
                            }

                        }
                        else
                        {
                            menu1Color = Color.Black;
                        }

                        if (mouseRectangle.Intersects(menu2Rectangle))
                        {
                            menu2Color = Color.Gray;

                            if ((newMouseState.LeftButton == ButtonState.Pressed) & (!(oldMouseState.LeftButton == ButtonState.Pressed)))
                            {
                                gameStatus = GameStatus.Help;
                            }
                        }
                        else
                        {
                            menu2Color = Color.Black;
                        }

                        break;
                    }
                case GameStatus.Set:
                    {
                        if (mouseRectangle.Intersects(selectArrowLeftRectangle))
                        {
                            selectArrowLeft = arrow_2Texture;

                            if ((newMouseState.LeftButton == ButtonState.Pressed) & (!(oldMouseState.LeftButton == ButtonState.Pressed)))
                            {
                                if (level > 0)
                                    level--;
                            }
                        }
                        else
                        {
                            selectArrowLeft = arrow_1Texture;
                        }

                        if (mouseRectangle.Intersects(selectArrowRightRectangle))
                        {
                            selectArrowRight = arrow_2Texture;

                            if ((newMouseState.LeftButton == ButtonState.Pressed) & (!(oldMouseState.LeftButton == ButtonState.Pressed)))
                            {
                                if (level < 9)
                                    level++;
                            }
                        }
                        else
                        {
                            selectArrowRight = arrow_1Texture;
                        }

                        if (mouseRectangle.Intersects(startButtonRectangle))
                        {
                            startButtonColor = Color.Gray;

                            if ((newMouseState.LeftButton == ButtonState.Pressed) & (!(oldMouseState.LeftButton == ButtonState.Pressed)))
                            {
                                tetrisGameComponent.Level = level;

                                this.Components.Add(tetrisGameComponent);

                                gameStatus = GameStatus.StartGame;
                            }
                        }
                        else
                        {
                            startButtonColor = Color.Black;
                        }

                        if (newKeyState.IsKeyDown(Keys.Escape) & !oldKeyState.IsKeyDown(Keys.Escape))
                        {
                            level = 0;
                            gameStatus = GameStatus.Start;
                        }

                        break;
                    }
                case GameStatus.StartGame:
                    {
                        if (newKeyState.IsKeyDown(Keys.Escape) & !oldKeyState.IsKeyDown(Keys.Escape))
                        {
                            tetrisGameComponent.Enabled = !tetrisGameComponent.Enabled;
                        }
                        break;
                    }
                case GameStatus.Help:
                    {

                        break;
                    }
                default:
                    {

                        break;
                    }
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (newKeyState.IsKeyDown(Keys.F11) & !oldKeyState.IsKeyDown(Keys.F11))
            {
                graphics.ToggleFullScreen();
            }

            oldKeyState = newKeyState;
            oldMouseState = newMouseState;
        }
    }
}
