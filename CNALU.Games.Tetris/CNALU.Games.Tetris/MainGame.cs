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

        Texture2D gameMenuTexture;

        Color gameMenu1Color;
        Color gameMenu2Color;
        Color gameMenu3Color;
        Rectangle gameMenu1Rectangle;
        Rectangle gameMenu2Rectangle;
        Rectangle gameMenu3Rectangle;

        Texture2D helpTexture;

        // 声音
        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;

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

            gameMenu1Color = Color.Black;
            gameMenu2Color = Color.Black;
            gameMenu3Color = Color.Black;

            gameMenu1Rectangle = new Rectangle((800 - 250) / 2, 150, 250, 80);
            gameMenu2Rectangle = new Rectangle((800 - 250) / 2, 250, 250, 80);
            gameMenu3Rectangle = new Rectangle((800 - 250) / 2, 350, 250, 80);

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

            gameMenuTexture = Content.Load<Texture2D>("Images/game_menu");

            helpTexture = Content.Load<Texture2D>("Images/help");

            audioEngine = new AudioEngine("Content/Audio/gameaudio.xgs");
            waveBank = new WaveBank(audioEngine, "Content/Audio/Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "Content/Audio/Sound Bank.xsb");

            soundBank.PlayCue("bgm");
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

            audioEngine.Update();
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
                        spriteBatch.DrawString(menuFont, "Level", new Vector2(selectArrowPosition.X + 80, selectArrowPosition.Y - 50), Color.Black);
                        // 绘制鼠标
                        spriteBatch.Draw(mouseTexture, mousePosition, Color.White);


                        spriteBatch.End();
                        break;
                    }
                case GameStatus.StartGame:
                    {
                        if (!tetrisGameComponent.Enabled)
                        {
                            tetrisGameComponent.DrawOrder = 1;
                            spriteBatch.Begin();
                            spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.Gray);

                            spriteBatch.Draw(gameMenuTexture, new Vector2((800 - 250) / 2, 150), Color.White);
                            spriteBatch.DrawString(menuFont, "Contine", new Vector2((800 - 250) / 2 + 60, 150 + 25), gameMenu1Color);
                            spriteBatch.Draw(gameMenuTexture, new Vector2((800 - 250) / 2, 250), Color.White);
                            spriteBatch.DrawString(menuFont, "Retry", new Vector2((800 - 250) / 2 + 60, 250 + 25), gameMenu2Color);
                            spriteBatch.Draw(gameMenuTexture, new Vector2((800 - 250) / 2, 350), Color.White);
                            spriteBatch.DrawString(menuFont, "Back to Menu", new Vector2((800 - 250) / 2 + 25, 350 + 25), gameMenu3Color);

                            spriteBatch.Draw(mouseTexture, mousePosition, Color.White);
                            spriteBatch.End();
                        }
                        break;
                    }

                case GameStatus.Help:
                    {
                        spriteBatch.Begin();

                        spriteBatch.Draw(helpTexture, Vector2.Zero, Color.White);

                        spriteBatch.End();
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
                                soundBank.PlayCue("ecptoma");
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
                                soundBank.PlayCue("ecptoma");
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
                                soundBank.PlayCue("ecptoma");
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
                                soundBank.PlayCue("ecptoma");
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
                                soundBank.PlayCue("ecptoma");
                                tetrisGameComponent = new TetrisGameComponent(this);
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

                        if (tetrisGameComponent.IsGameOver)
                        {
                            if (newKeyState.GetPressedKeys().Length > 0 & (!(oldKeyState.GetPressedKeys().Length > 0)))
                            {
                                level = 0;

                                this.Components.Remove(tetrisGameComponent);
                                gameStatus = GameStatus.Start;
                            }
                        }


                        if (!tetrisGameComponent.Enabled)
                        {
                            if (mouseRectangle.Intersects(gameMenu1Rectangle))
                            {
                                gameMenu1Color = Color.Blue;

                                if ((newMouseState.LeftButton == ButtonState.Pressed) & (!(oldMouseState.LeftButton == ButtonState.Pressed)))
                                {
                                    soundBank.PlayCue("ecptoma");
                                    tetrisGameComponent.Enabled = !tetrisGameComponent.Enabled;
                                }
                            }
                            else
                            {
                                gameMenu1Color = Color.Black;
                            }

                            if (mouseRectangle.Intersects(gameMenu2Rectangle))
                            {
                                gameMenu2Color = Color.Blue;

                                if ((newMouseState.LeftButton == ButtonState.Pressed) & (!(oldMouseState.LeftButton == ButtonState.Pressed)))
                                {
                                    soundBank.PlayCue("ecptoma");
                                    this.Components.Remove(tetrisGameComponent);
                                    tetrisGameComponent = new TetrisGameComponent(this);
                                    tetrisGameComponent.Level = level;
                                    this.Components.Add(tetrisGameComponent);
                                }
                            }
                            else
                            {
                                gameMenu2Color = Color.Black;
                            }

                            if (mouseRectangle.Intersects(gameMenu3Rectangle))
                            {
                                gameMenu3Color = Color.Blue;

                                if ((newMouseState.LeftButton == ButtonState.Pressed) & (!(oldMouseState.LeftButton == ButtonState.Pressed)))
                                {
                                    soundBank.PlayCue("ecptoma");
                                    level = 0;
                                    this.Components.Remove(tetrisGameComponent);
                                    gameStatus = GameStatus.Start;
                                }
                            }
                            else
                            {
                                gameMenu3Color = Color.Black;
                            }
                        }

                        break;
                    }
                case GameStatus.Help:
                    {
                        if (newKeyState.IsKeyDown(Keys.Escape) & !oldKeyState.IsKeyDown(Keys.Escape))
                        {
                            gameStatus = GameStatus.Start;
                        }
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
