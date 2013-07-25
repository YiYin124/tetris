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

    class ComboBlock : IDisposable
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
        Point lastPosition;

        IBlock baseBlock;

        public Point Position
        {
            get
            {
                return lastPosition;
            }
        }

        public ComboBlock(IBlock baseBlock, ComboBlockShape shape, Point position, IBlock[,] panel)
        {
            this.baseBlock = baseBlock;
            this.panel = panel;
            comboBlock = Build(shape, baseBlock);

            if (PutCheck(position))
                Put(position);
            else
                throw new Exception("此坐标不能创建, 已被占用或越界");
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

        bool Outbounds(Point position)
        {
            // 坐标越界检测
            if (position.X < 0 || position.Y < 0 || position.X + comboBlock.GetLength(1) > panel.GetLength(1) || position.Y + comboBlock.GetLength(0) > panel.GetLength(0))
                return true;

            return false;
        }

        bool PutCheck(Point position)
        {
            // 越界检测
            if (Outbounds(position))
                return false;

            // 摆放检测
            for (int ln = 0; ln < comboBlock.GetLength(0); ln++)
            {
                for (int col = 0; col < comboBlock.GetLength(1); col++)
                {
                    if (comboBlock[ln, col] != null && panel[ln + position.Y, col + position.X] != null)
                        // 区域摆放
                        return false;
                }
            }

            return true;
        }

        bool Erasure(Point position)
        {
            // 越界检测
            if (Outbounds(position))
                return false;

            // 擦除
            for (int ln = 0; ln < comboBlock.GetLength(0); ln++)
            {
                for (int col = 0; col < comboBlock.GetLength(1); col++)
                {
                    if (comboBlock[ln, col] != null)
                    {
                        panel[ln + position.Y, col + position.X] = null;
                    }
                }
            }

            return true;
        }

        bool Put(Point position)
        {
            // 越界检测
            if (Outbounds(position))
                return false;

            // 摆放
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

            lastPosition = position;
            return true;
        }

        public bool SetPosition(Point position)
        {
            // 越界检测
            if (Outbounds(position))
                return false;

            Erasure(lastPosition);

            if (PutCheck(position))
            {
                Put(position);
                return true;
            }

            Put(lastPosition);

            return false;
        }

        public bool Rotate()
        {
            IBlock[,] tmp = new IBlock[comboBlock.GetLength(1), comboBlock.GetLength(0)];

            for (int col = 0; col < comboBlock.GetLength(1); col++)
            {
                for (int ln = comboBlock.GetLength(0) - 1; ln >= 0; ln--)
                {
                    tmp[col, comboBlock.GetLength(0) - 1 - ln] = comboBlock[ln, col];
                }
            }

            Erasure(lastPosition);

            IBlock[,] comboBlockTmp = comboBlock;
            comboBlock = tmp;

            if (PutCheck(lastPosition))
            {
                Put(lastPosition);
                return true;
            }

            comboBlock = comboBlockTmp;
            Put(lastPosition);
            return false;
        }

        public void Dispose()
        {
            Erasure(lastPosition);
        }
    }
}
