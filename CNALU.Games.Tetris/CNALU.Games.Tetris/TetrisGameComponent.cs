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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            gameBoxTexture = Game.Content.Load<Texture2D>("Images/gamebox");
            blockTexture = Game.Content.Load<Texture2D>("Images/block");

            gamePanel[0, 0] = new Block(blockTexture, new Rectangle(0, 0, 30, 30));
            gamePanel[0, 1] = new Block(blockTexture, new Rectangle(0, 0, 30, 30));

            previewPanel[0, 0] = new Block(blockTexture, new Rectangle(0, 0, 30, 30));
            previewPanel[1, 0] = new Block(blockTexture, new Rectangle(30, 0, 30, 30));
            previewPanel[0, 1] = new Block(blockTexture, new Rectangle(30, 30, 30, 30));

            gamePanel = GameBlock<IBlock>.Build(GameBlockShape.I, new Block(blockTexture, new Rectangle(0, 30, 30, 30)));
            GameBlock<IBlock>.Rotate(ref gamePanel);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // ªÊ÷∆±≥æ∞
            spriteBatch.Begin();
            spriteBatch.Draw(gameBoxTexture, Vector2.Zero, Color.White);
            spriteBatch.End();

            // ªÊ÷∆”Œœ∑√Ê∞Â
            DrawPanel(gameTime, spriteBatch, gamePanel, gamePanelPosition);

            // ªÊ÷∆‘§¿¿√Ê∞Â
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
    }
}
