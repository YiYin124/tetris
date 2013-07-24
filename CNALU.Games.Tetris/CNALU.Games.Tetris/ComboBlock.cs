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
    enum ComboBlockShape { O, T, S, Z, L, J, I }

    class ComboBlock
    {
        #region SHAPE BASE DATA
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
        #endregion

        IBlock[,] comboBlock;
        IBlock[,] panel;
        Point nowPosition;
        Point lastPosition;

        public ComboBlock(IBlock block, ComboBlockShape shape, Point position, IBlock[,] panel)
        {
            comboBlock = Build(shape, block);

            this.panel = panel;
            this.lastPosition = position;
            this.nowPosition = position;
            Put(position);
        }

        IBlock[,] Build(ComboBlockShape shape, IBlock block)
        {
            IBlock[,] tmp = new IBlock[shapeBase[(int)shape].GetLength(0), shapeBase[(int)shape].GetLength(1)];

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

        public bool Put(Point position)
        {
            // 摆放坐标越界检测
            if (position.X < 0 || position.Y < 0 || position.X + comboBlock.GetLength(1) > panel.GetLength(1) || position.Y + comboBlock.GetLength(0) > panel.GetLength(0))
                return false;

            // 区域摆放检测
            for (int ln = 0; ln < comboBlock.GetLength(0); ln++)
            {
                for (int col = 0; col < comboBlock.GetLength(1); col++)
                {
                    if (comboBlock[ln, col] != null && panel[ln + position.Y, col + position.X] != null)
                        return false;
                }
            }

            // 擦除上一次摆放区域
            for (int ln = 0; ln < comboBlock.GetLength(0); ln++)
            {
                for (int col = 0; col < comboBlock.GetLength(1); col++)
                {
                    if (comboBlock[ln, col] != null)
                    {
                        panel[ln + lastPosition.Y, col + lastPosition.X] = null;
                    }
                }
            }

            // 区域摆放
            for (int ln = 0; ln < comboBlock.GetLength(0); ln++)
            {
                for (int col = 0; col < comboBlock.GetLength(1); col++)
                {
                    if (comboBlock[ln, col] != null)
                    {
                        panel[ln + position.Y, col + position.X] = comboBlock[ln, col];
                    }
                }
            }

            lastPosition = nowPosition;
            nowPosition = position;
            return true;
        }
    }
}
