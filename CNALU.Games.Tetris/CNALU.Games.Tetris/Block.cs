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
    enum Shape { O, T, S, Z, L, J, I }

    class Block : IBlock
    {
        readonly bool[][,] ShapeData = {
                new bool[,] { { true, true }, { true, true } },
                new bool[,] { { true, true, true }, { false, true, false } },
                new bool[,] { { false, true, true }, { true, true, false } },
                new bool[,] { { true, true, false }, { false, true, true } },
                new bool[,] { { true, false }, { true, false }, { true, true } },
                new bool[,] { { false, true }, { false, true }, { true, true } },
                new bool[,] { { true }, { true }, { true }, { true } },
            };

        public bool[,] Shape { get; private set; }
        public Vector2 Position { get; private set; }

        public Block(Vector2 position, Shape blockShape)
        {
            this.Position = position;
            this.Shape = ShapeData[(int)blockShape];
        }

        public void Rotate()
        {
            bool[,] tmp = new bool[Shape.GetLength(1), Shape.GetLength(0)];

            for (int col = 0; col < Shape.GetLength(1); col++)
            {
                for (int ln = Shape.GetLength(0) - 1; ln >= 0; ln--)
                {
                    tmp[col, Shape.GetLength(0) - 1 - ln] = Shape[ln, col];
                }
            }
            Shape = tmp;
        }


        public bool Up()
        {
            throw new NotImplementedException();
        }

        public bool Down()
        {
            throw new NotImplementedException();
        }

        public bool Left()
        {
            throw new NotImplementedException();
        }

        public bool Right()
        {
            throw new NotImplementedException();
        }
    }
}
