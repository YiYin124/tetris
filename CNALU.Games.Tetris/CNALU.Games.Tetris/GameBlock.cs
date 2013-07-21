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
    class GameBlock
    {
        #region SHAPES
        static readonly bool[][][,] Shapes = {
                                    new bool[][,] {     // O
                                        new bool[,] {
                                            {true, true},
                                            {true, true}
                                        }
                                    },
                                    new bool[][,] {     // T
                                        new bool[,] {
                                            {true, true, true},
                                            {false, true, false}
                                        },
                                        new bool[,] {
                                            {false, true},
                                            {true, true},
                                            {false, true}
                                        },
                                        new bool[,] {
                                            {false, true, false},
                                            {true, true, true}
                                        },
                                        new bool[,] {
                                            {true, false},
                                            {true, true},
                                            {true, false}
                                        }
                                    },
                                    new bool[][,] {     // S
                                        new bool[,] {
                                            {false, true, true},
                                            {true, true, false}
                                        },
                                        new bool[,] {
                                            {true, false},
                                            {true, true},
                                            {false, true}
                                        }
                                    },
                                    new bool[][,] {     // Z
                                        new bool[,] {
                                            {true, true, false},
                                            {false, true, true}
                                        },
                                        new bool[,] {
                                            {false, true},
                                            {true, true},
                                            {true, false}
                                        }
                                    },
                                    new bool[][,] {     // L
                                        new bool[,] {
                                            {true, false},
                                            {true, false},
                                            {true, true}
                                        },
                                        new bool[,] {
                                            {true, true, true},
                                            {true, false, false}
                                        },
                                        new bool[,] {
                                            {true, true},
                                            {false, true},
                                            {false, true}
                                        },
                                        new bool[,] {
                                            {false, false, true},
                                            {true, true, true}
                                        }
                                    },
                                    new bool[][,] {     // J
                                        new bool[,] {
                                            {false, true},
                                            {false, true},
                                            {true, true}
                                        },
                                        new bool[,] {
                                            {true, false, false},
                                            {true, true, true}
                                        },
                                        new bool[,] {
                                            {true, true},
                                            {true, false},
                                            {true, false}
                                        },
                                        new bool[,] {
                                            {true, true, true},
                                            {false, false, true}
                                        }
                                    },
                                    new bool[][,] {     // I
                                        new bool[,] {
                                            {true},
                                            {true},
                                            {true},
                                            {true}
                                        },
                                        new bool[,] {
                                            {true, true, true, true}
                                        }
                                    }
        };
        #endregion

        bool[,] panelGame;

        System.Drawing.Size size;
        Vector2 position;
        Texture2D texture;
        Random random;

        public GameBlock(System.Drawing.Size size, Vector2 position, Texture2D texture)
        {
            this.size = size;
            this.position = position;
            this.texture = texture;
            panelGame = new bool[size.Width, size.Height];
            random = new Random();
        }

        public void Test()
        {
            PutGameBlock(new Vector2(8, 0), 1, 0);
        }

        int PutGameBlock(Vector2 position, int type, int index)
        {
            // 检测是否可以落下
            // 检测是否会越界
            if (((int)position.X + Shapes[type][index].GetLength(0) > panelGame.GetLength(0)) || ((int)position.Y + Shapes[type][index].GetLength(1) > panelGame.GetLength(1)))
            {
                return -1;
            }

            // 检测是否可以填充
            for (int x = 0; x < Shapes[type][index].GetLength(0); x++)
            {
                for (int y = 0; y < Shapes[type][index].GetLength(1); y++)
                {
                    if (Shapes[type][index][x, y] & panelGame[(int)position.X + x, (int)position.Y + y])
                    {
                        return -1;
                    }
                }
            }

            // 进行填充
            for (int x = 0; x < Shapes[type][index].GetLength(0); x++)
                for (int y = 0; y < Shapes[type][index].GetLength(1); y++)
                    panelGame[(int)position.X + x, (int)position.Y + y] |= Shapes[type][index][x, y];

            return 0;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin();
            for (int x = 0; x < panelGame.GetLength(0); x++)
            {
                for (int y = 0; y < panelGame.GetLength(1); y++)
                {
                    if (panelGame[x, y])
                    {
                        spriteBatch.Draw(texture, new Vector2(position.X + x * 30, position.Y + y * 30), new Rectangle(0, 0, 30, 30), Color.White);
                    }
                }
            }
            spriteBatch.End();
        }
    }
}
