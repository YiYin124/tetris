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

    public class TetrisGameComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        Texture2D gameBoxTexture;
        Texture2D blockTexture;

        IBlock[,] gamePanel;
        Vector2 gamePanelPosition;

        IBlock[,] previewPanel;
        Vector2 previewPanelPosition;

        KeyboardState oldState;

        ComboBlock userControlComboBlock;
        ComboBlock previewComboBlock;

        Random random;
        int randomDirection;
        int randomShape;

        int randomLnImageNum;
        int randomColImageNum;

        int lastAutoFallTime;
        int autoFallSpanTime;

        // 积分项
        public int Lines { get; private set; }
        public int Level { get; private set; }
        public int Score { get; private set; }

        // 声音
        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;
        Cue bgmTrackCue;

        // 下落按键延时
        int lastKeyTime;
        const int KeyDownSpanTime = 50;

        // 删除满行延时
        int lastDeleteLineTime;
        const int DeleteLineTime = 50;
        IBlock[] deleteLineBuffer;

        int twinleCount;
        const int TwinleNumber = 10;

        enum GameState
        {
            Stop,
            Fall,
            Arrive,
            GameOver
        }

        GameState gameState;

        int nowDeleteLineID;
        int deleteLineCount;

        // 显示字体
        SpriteFont gameFont;
        Vector2 gameTextPosition;

        public TetrisGameComponent(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        public override void Initialize()
        {
            gamePanel = new Block[15, 10];
            gamePanelPosition = new Vector2(108.0F, 57.0F);

            previewPanel = new Block[6, 6];
            previewPanelPosition = new Vector2(508.0F, 57.0F);

            gameTextPosition = new Vector2(508.0F, 325.0F);

            random = new Random();

            randomDirection = random.Next(4);
            randomShape = random.Next(7);
            randomLnImageNum = random.Next(3);
            randomColImageNum = random.Next(4);

            Lines = 0;
            Score = 0;
            Level = 0;
            autoFallSpanTime = 1000 - Level * 100;

            deleteLineBuffer = new IBlock[gamePanel.GetLength(1)];

            gameState = GameState.Fall;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            gameBoxTexture = Game.Content.Load<Texture2D>("Images/gamebox");
            blockTexture = Game.Content.Load<Texture2D>("Images/block_4");

            audioEngine = new AudioEngine("Content/Audio/gameaudio.xgs");
            waveBank = new WaveBank(audioEngine, "Content/Audio/Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "Content/Audio/Sound Bank.xsb");

            gameFont = Game.Content.Load<SpriteFont>("Fonts/game");

            bgmTrackCue = this.soundBank.GetCue("bgm");

            bgmTrackCue.Play();

            SpawnComboBlock();

            
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            switch (gameState)
            {
                case GameState.Stop:
                    {

                        break;
                    }
                case GameState.Fall:
                    {
                        UpdateInput(gameTime);
                        UpdateAutoFall(gameTime);
                        break;
                    }
                case GameState.Arrive:
                    {
                        UpdateDeleteLines(gameTime);
                        break;
                    }
                case GameState.GameOver:
                    {
                        soundBank.PlayCue("game_over");
                        gameState = GameState.Stop;
                        break;
                    }
                default:
                    break;

            }

            audioEngine.Update();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // 绘制背景
            spriteBatch.Begin();
            spriteBatch.Draw(gameBoxTexture, Vector2.Zero, Color.White);
            spriteBatch.End();

            // 绘制游戏面板
            DrawPanel(gameTime, spriteBatch, gamePanel, gamePanelPosition);

            // 绘制预览面板
            DrawPanel(gameTime, spriteBatch, previewPanel, previewPanelPosition);

            DrawGameText(spriteBatch, gameTextPosition);

            base.Draw(gameTime);
        }

        void DrawPanel(GameTime gameTime, SpriteBatch spriteBatch, IBlock[,] panel, Vector2 position)
        {
            for (int ln = 0; ln < panel.GetLength(0); ln++)
            {
                for (int col = 0; col < panel.GetLength(1); col++)
                {
                    if (panel[ln, col] != null)
                    {
                        panel[ln, col].Position = new Vector2(
                            position.X + col * panel[ln, col].SourceRectangle.Value.Width,
                            position.Y + ln * panel[ln, col].SourceRectangle.Value.Height
                            );
                        panel[ln, col].Draw(gameTime, spriteBatch);
                    }
                }
            }
        }

        void SpawnComboBlock()
        {
            userControlComboBlock = new ComboBlock(new Block(blockTexture, new Rectangle(30 * randomColImageNum, 30 * randomLnImageNum, 30, 30)), (ComboBlockShape)randomShape, new Point(gamePanel.GetLength(1) / 2 - 1, 0), gamePanel, randomDirection);

            randomDirection = random.Next(4);
            randomShape = random.Next(7);
            randomLnImageNum = random.Next(3);
            randomColImageNum = random.Next(4);

            if (previewComboBlock != null)
                previewComboBlock.Dispose();

            previewComboBlock = new ComboBlock(new Block(blockTexture, new Rectangle(30 * randomColImageNum, 30 * randomLnImageNum, 30, 30)), (ComboBlockShape)randomShape, new Point(2, 2), previewPanel, randomDirection);
        }

        void UpdateInput(GameTime gameTime)
        {
            lastKeyTime += gameTime.ElapsedGameTime.Milliseconds;
            KeyboardState newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.Up) & !oldState.IsKeyDown(Keys.Up))
            {
                if (userControlComboBlock.Rotate())
                    soundBank.PlayCue("blip");
            }

            if (newState.IsKeyDown(Keys.Left) & !oldState.IsKeyDown(Keys.Left))
            {
                if (userControlComboBlock.SetPosition(new Point(userControlComboBlock.Position.X - 1, userControlComboBlock.Position.Y)))
                    soundBank.PlayCue("blip");
            }

            if (newState.IsKeyDown(Keys.Right) & !oldState.IsKeyDown(Keys.Right))
            {
                if (userControlComboBlock.SetPosition(new Point(userControlComboBlock.Position.X + 1, userControlComboBlock.Position.Y)))
                    soundBank.PlayCue("blip");
            }

            if (lastKeyTime >= KeyDownSpanTime)
            {
                lastKeyTime = 0;

                if (newState.IsKeyDown(Keys.Down))
                {
                    Fall();
                }
            }

            oldState = newState;
        }

        void UpdateAutoFall(GameTime gameTime)
        {
            lastAutoFallTime += gameTime.ElapsedGameTime.Milliseconds;

            if (lastAutoFallTime >= autoFallSpanTime)
            {
                Fall();
            }
        }

        void Fall()
        {
            lastAutoFallTime = 0;

            if (!userControlComboBlock.SetPosition(new Point(userControlComboBlock.Position.X, userControlComboBlock.Position.Y + 1)))
            {
                soundBank.PlayCue("ecptoma");

                gameState = GameState.Arrive;
                nowDeleteLineID = GetDeleteLineID();
                Score += 10;
            }
        }

        void UpdateDeleteLines(GameTime gameTime)
        {
            lastDeleteLineTime += gameTime.ElapsedGameTime.Milliseconds;

            if (lastDeleteLineTime >= DeleteLineTime)
            {
                lastDeleteLineTime = 0;


                if (nowDeleteLineID != -1)
                {
                    if (twinleCount == TwinleNumber)
                    {
                        twinleCount = 0;
                        DeleteLine(nowDeleteLineID);
                        nowDeleteLineID = GetDeleteLineID();
                        deleteLineCount++;
                        Lines++;
                    }
                    else
                    {
                        if (deleteLineBuffer[0] == null)
                        {
                            for (int col = 0; col < gamePanel.GetLength(1); col++)
                            {
                                deleteLineBuffer[col] = gamePanel[nowDeleteLineID, col];
                                gamePanel[nowDeleteLineID, col] = null;
                            }
                        }
                        else
                        {
                            for (int col = 0; col < gamePanel.GetLength(1); col++)
                            {
                                gamePanel[nowDeleteLineID, col] = deleteLineBuffer[col];
                                deleteLineBuffer[col] = null;
                            }
                        }

                        twinleCount++;
                    }
                }
                else
                {
                    Score += deleteLineCount * 100;
                    deleteLineCount = 0;

                    try
                    {
                        SpawnComboBlock();
                        gameState = GameState.Fall;
                    }
                    catch
                    {
                        gameState = GameState.GameOver;
                    }
                }
            }
        }

        int GetDeleteLineID()
        {
            for (int ln = 0; ln < gamePanel.GetLength(0); ln++)
            {
                bool full = true;

                for (int col = 0; col < gamePanel.GetLength(1); col++)
                {
                    if (gamePanel[ln, col] == null)
                    {
                        full = false;
                        break;
                    }
                }

                if (full)
                    return ln;
            }
            return -1;
        }

        void DeleteLine(int ln)
        {
            // 删除行
            for (int col = 0; col < gamePanel.GetLength(1); col++)
            {
                gamePanel[ln, col] = null;
            }

            // 重排
            for (int ln1 = ln; ln1 > 0; ln1--)
            {
                for (int col = 0; col < gamePanel.GetLength(1); col++)
                {
                    gamePanel[ln1, col] = gamePanel[ln1 - 1, col];
                }
            }

            soundBank.PlayCue("delete_line");
        }

        void DrawGameText(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Begin();

            if (Score >= 99999999)
                Score = 99999999;
            if (Lines >= 99999999)
                Lines = 99999999;

            spriteBatch.DrawString(gameFont, "Score :" + Score, new Vector2(position.X - 15, position.Y), Color.Black);
            spriteBatch.DrawString(gameFont, "Level  :" + (Level + 1), new Vector2(position.X - 10, position.Y + 60 * 1), Color.Black);
            spriteBatch.DrawString(gameFont, "Lines :" + Lines, new Vector2(position.X - 10, position.Y + 60 * 2), Color.Black);

            spriteBatch.End();
        }
    }
}
