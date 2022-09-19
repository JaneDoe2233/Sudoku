using System;
using System.Collections;

namespace SudokuTest
{
    /// <summary>
    /// 处理数独题目数据源
    /// </summary>
    public class SudokuDataSource
    {
        //一个标准数独题目如果提示数（已知数）少于17个，一定不唯一解（电脑证明吧……）

        // 修改日志2022.08
        // 1.清空格子之后验证是否有唯一解：只需要判断是不是多解，因为是从完整数独清空得来的，不可能无解
        // How?
        // 一个标准数独题目如果提示数（已知数）少于17个，一定不唯一解（电脑证明吧……）
        // 提示数只有1~9这9个数字的其中7个甚至更少的，一定不唯一解（反用唯一环结构的利用点）；
        // 任何一个并排三宫里出现两行/列是全空的，一定不唯一解。  
        // 连续的三个空行、三个空列、三个空宫排列在一起，题目一定不唯一解（枚举出来的）；
        // 

        // todo
        // 1.生成解之后，存起来，共有______种解；只记录解的序号
        // 2.生成数独之后，判断相同解的序号下，是否有相同的题目（二维数组，linq判断？）
        // 3.只有成功解答的数独才能收藏？否。添加解答状态


        #region 变量

        /// <summary>
        /// 用于存放填的格子数
        /// </summary>
        public int NullNum;

        /// <summary>
        /// 存放有空格的数独题目
        /// </summary>
        public int[,] Sudoku = new int[9, 9];

        /// <summary>
        /// 存放生成的完整数独
        /// </summary>
        public int[,] Source = new int[9, 9];

        #endregion

        #region 构造函数

        /// <summary>
        /// 无参构造函数，默认简单模式
        /// </summary>
        public SudokuDataSource()
        {
            /* 数独题解 */
            this.Source = SetSource();

            /* 设置题目 */
            this.Sudoku = SetSudoku(Source, (int)Difficulty.Easy);
        }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        public SudokuDataSource(int nullnum)
        {
            this.NullNum = nullnum;

            /* 数独题解 */
            this.Source = SetSource();

            /* 设置题目 */
            this.Sudoku = SetSudoku(this.Source, this.NullNum);
        }

        #endregion

        #region 内部方法

        /// <summary>
        /// 设置数独题解数据源
        /// </summary>
        public int[,] SetSource()
        {
            /* 数独题解 */
            int[,] source = new int[9, 9]
            {
                { 1,2,3,4,5,6,7,8,9 },
                { 4,5,6,7,8,9,1,2,3 },
                { 7,8,9,1,2,3,4,5,6 },
                { 2,3,4,5,6,7,8,9,1 },
                { 5,6,7,8,9,1,2,3,4 },
                { 8,9,1,2,3,4,5,6,7 },
                { 3,4,5,6,7,8,9,1,2 },
                { 6,7,8,9,1,2,3,4,5 },
                { 9,1,2,3,4,5,6,7,8 }
            };


            //todo 算法b 先生成1-9的所有可能的排列（A9_9=9!=3628806），从这些中随机选出9个排列（A9_3628806）
            //todo 算法a 先生成一个，再把数字1-9分别替换为乱序的1-9，比如1替换成7,2替换成8，以此类推
            //todo 行交换随机次数次，列交换随机次数次；宫格(3行/3列)随机交换多少次
            //先只做行列交换，要实现交换后source可以不变，
            /* 行交换 */
            Swap(source, 0, new int[] { 0, 1, 2 });
            Swap(source, 0, new int[] { 3, 4, 5 });
            Swap(source, 0, new int[] { 6, 7, 8 });
            /* 列交换 */
            Swap(source, 1, new int[] { 0, 1, 2 });
            Swap(source, 1, new int[] { 3, 4, 5 });
            Swap(source, 1, new int[] { 6, 7, 8 });

            return source;
        }

        /// <summary>
        /// 交换两行或两列
        /// </summary>
        private void Swap(int[,] array, int dimension, int[] indexs)
        {
            /* 要交换的索引从小到大排序 */
            Array.Sort(indexs);

            for (int i = 0; i < indexs.Length; i++)
            {
                Random random = new Random();
                int curr = indexs[i];
                /* 左闭右开区间 */
                int orther = random.Next(indexs[0], indexs[indexs.Length - 1] + 1);
                if (curr == orther)
                {
                    continue;
                }

                /* 交换行 */
                if (dimension == 0)
                {
                    /* 取出列数 */
                    int length = array.GetLength(1);
                    int[] temp = new int[length];
                    for (int k = 0; k < length; k++)
                    {
                        /* 交换行的每个元素 */
                        temp[k] = array[curr, k];
                        array[curr, k] = array[orther, k];
                        array[orther, k] = temp[k];
                    }
                }
                else if (dimension == 1)
                {
                    /* 取出行数 */
                    int length = array.GetLength(0);
                    int[] temp = new int[length];
                    for (int k = 0; k < length; k++)
                    {
                        /* 交换列的每个元素 */
                        temp[k] = array[k, curr];
                        array[k, curr] = array[k, orther];
                        array[k, orther] = temp[k];
                    }
                }
            }
        }

        /// <summary>
        /// 设置数独题目
        /// </summary>
        public int[,] SetSudoku(int[,] source, int nullnum)
        {
            int[,] sourceQuestion = (int[,])source.Clone();
            Random random = new Random();
            Hashtable htIndex = new Hashtable();

            // 随机选取 nullnum 个格子清空
            for (int i = 0; i < nullnum; i++)
            {
                /* 随机选取 sourceQuestion 的数据清空 */
                int rowIndex = random.Next(0, 9); //可以取到最小值，不能取到最大值；索引从0开始；
                int columnIndex = random.Next(0, 9);

                string index = "(" + rowIndex + "," + columnIndex + ")";
                while (htIndex.ContainsKey(index))
                {
                    /* 如果之前已经设置为-1，重新选取 */
                    rowIndex = random.Next(0, 9);
                    columnIndex = random.Next(0, 9);
                    index = "(" + rowIndex + "," + columnIndex + ")";
                }
                htIndex[index] = index;

                sourceQuestion[rowIndex, columnIndex] = 0;
            }

            // 移除格子之后，判断题目是否有唯一解



            return sourceQuestion;
        }

        #endregion
    }
}