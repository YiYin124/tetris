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

        int lastAutoDownTime;
        int autoDownSpanTime;

        // 积分项
        public int Lines { get; private set; }
        public int Level { get; private set; }
        public int Score { get; private set; }

        // 声音
        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;

        int lastKeyTime;
        int keyDownSpanTime = 50;

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

            random = new Random();

            randomDirection = random.Next(4);
            randomShape = random.Next(7);
            randomLnImageNum = random.Next(3);
            randomColImageNum = random.Next(4);

            Lines = 0;
            Score = 0;
            Level = 0;
            autoDownSpanTime = 1000;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            gameBoxTexture = Game.Content.Load<Texture2D>("Images/gamebox");
            blockTexture = Game.Content.Load<Texture2D>("Images/block_4");

            SpawnComboBlock();

            audioEngine = new AudioEngine("Content/Audio/gameaudio.xgs");
            waveBank = new WaveBank(audioEngine, "Content/Audio/Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "Content/Audio/Sound Bank.xsb");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {

            UpdateInput(gameTime);

            AutoDownUpdate(gameTime);

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

        void UpdateInput(GameTime gameTime)
        {
            lastKeyTime += gameTime.ElapsedGameTime.Milliseconds;
            KeyboardState newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.Left) & !oldState.IsKeyDown(Keys.Left))
            {
                userControlComboBlock.SetPosition(new Point(userControlComboBlock.Position.X - 1, userControlComboBlock.Position.Y));
                soundBank.PlayCue("blip");
            }

            if (newState.IsKeyDown(Keys.Right) & !oldState.IsKeyDown(Keys.Right))
            {
                userControlComboBlock.SetPosition(new Point(userControlComboBlock.Position.X + 1, userControlComboBlock.Position.Y));
                soundBank.PlayCue("blip");
            }

            if (newState.IsKeyDown(Keys.Up) & !oldState.IsKeyDown(Keys.Up))
            {
                userControlComboBlock.Rotate();
                soundBank.PlayCue("blip");
            }

            if (lastKeyTime >= keyDownSpanTime)
            {
                lastKeyTime = 0;

                if (newState.IsKeyDown(Keys.Down))// & !oldState.IsKeyDown(Keys.Down))
                {
                    Down();
                }
            }

            oldState = newState;
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

        void AutoDownUpdate(GameTime gameTime)
        {
            lastAutoDownTime += gameTime.ElapsedGameTime.Milliseconds;

            if (lastAutoDownTime >= autoDownSpanTime)
            {
                Down();
            }
        }

        void Down()
        {
            lastAutoDownTime = 0;
            userControlComboBlock.SetPosition(new Point(userControlComboBlock.Position.X, userControlComboBlock.Position.Y + 1));

            if (!userControlComboBlock.SetPosition(new Point(userControlComboBlock.Position.X, userControlComboBlock.Position.Y + 1)))
            {
                soundBank.PlayCue("ecptoma");

                DeleteFullLine();
                try
                {
                    SpawnComboBlock();
                }
                catch
                {
                    // Game Over
                    //Initialize();
                    soundBank.PlayCue("game_over");
                }
            }
            else
            {
                userControlComboBlock.SetPosition(new Point(userControlComboBlock.Position.X, userControlComboBlock.Position.Y - 1));
            }
        }

        void DeleteFullLine()
        {
            int lines = 0;
            bool full;
            for (int ln = 0; ln < gamePanel.GetLength(0); ln++)
            {
                full = true;
                for (int col = 0; col < gamePanel.GetLength(1); col++)
                {
                    if (gamePanel[ln, col] == null)
                    {
                        full = false;
                        break;
                    }
                }

                if (full)
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

                    lines++;
                }
            }
            this.Lines += lines;

            this.Score += lines * 100;

            this.Level += lines / 100;

            this.autoDownSpanTime = 1000 - Level * 100;

            if (lines != 0)
                soundBank.PlayCue("delete_line");
        }
    }
}
