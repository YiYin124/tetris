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
    enum GameBlockShape { O, T, S, Z, L, J, I }

    static class GameBlock<T>// where T : IBlock
    {
        static readonly bool[][,] shapeBase = {
                new bool[,] { 
                    { true, true }, 
                    { true, true } 
                },
                new bool[,] {
                    { true, true, true }, 
                    { false, true, false } 
                },
                new bool[,] { 
                    { false, true, true }, 
                    { true, true, false } 
                },
                new bool[,] { 
                    { true, true, false }, 
                    { false, true, true } 
                },
                new bool[,] { 
                    { true, false }, 
                    { true, false }, 
                    { true, true } 
                },
                new bool[,] { 
                    { false, true }, 
                    { false, true }, 
                    { true, true } 
                },
                new bool[,] { 
                    { true }, 
                    { true }, 
                    { true }, 
                    { true } 
                },
            };

        public static T[,] Build(GameBlockShape shape, T block)
        {
            T[,] tmp = new T[shapeBase[(int)shape].GetLength(0), shapeBase[(int)shape].GetLength(1)];

            for (int ln = 0; ln < tmp.GetLength(0); ln++)
            {
                for (int col = 0; col < tmp.GetLength(1); col++)
                {
                    if (shapeBase[(int)shape][ln, col])
                        tmp[ln, col] = block;
                }
            }
            return tmp;
        }

        public static void Rotate(ref T[,] gameBlock)
        {
            T[,] tmp = new T[gameBlock.GetLength(1), gameBlock.GetLength(0)];

            for (int col = 0; col < gameBlock.GetLength(1); col++)
            {
                for (int ln = gameBlock.GetLength(0) - 1; ln >= 0; ln--)
                {
                    tmp[col, gameBlock.GetLength(0) - 1 - ln] = gameBlock[ln, col];
                }
            }
            gameBlock = tmp;
        }
    }
}
