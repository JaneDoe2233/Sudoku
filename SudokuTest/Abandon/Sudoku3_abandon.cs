using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SudokuTest
{
    public partial class Sudoku3_abandon : Form
    {
        public Sudoku3_abandon()
        {
            InitializeComponent();

            /* 注册窗体事件 */
            this.Load += Sudoku3_Load;
        }

        /// <summary>
        /// 加载窗体
        /// </summary>
        private void Sudoku3_Load(object sender, EventArgs e)
        {
            /* 数独结果 */
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

            /* 随机选取数据清空 */
            int nullnum = 40; //40简单，45，50困难
            Random random = new Random();
            for (int i = 0; i < nullnum; i++)
            {
                source[random.Next(0, 9), random.Next(0, 9)] = -1; //random.Next(0, 9); //可以取到最小值，不能取到最大值；索引从0开始；
            }

            #region 准备List<Button> 型变量 buttons 

            List<Button> buttons = new List<Button>();
            buttons.Add(button11);
            buttons.Add(button12);
            buttons.Add(button13);
            buttons.Add(button14);
            buttons.Add(button15);
            buttons.Add(button16);
            buttons.Add(button17);
            buttons.Add(button18);
            buttons.Add(button19);
            buttons.Add(button21);
            buttons.Add(button22);
            buttons.Add(button23);
            buttons.Add(button24);
            buttons.Add(button25);
            buttons.Add(button26);
            buttons.Add(button27);
            buttons.Add(button28);
            buttons.Add(button29);
            buttons.Add(button31);
            buttons.Add(button32);
            buttons.Add(button33);
            buttons.Add(button34);
            buttons.Add(button35);
            buttons.Add(button36);
            buttons.Add(button37);
            buttons.Add(button38);
            buttons.Add(button39);
            buttons.Add(button41);
            buttons.Add(button42);
            buttons.Add(button43);
            buttons.Add(button44);
            buttons.Add(button45);
            buttons.Add(button46);
            buttons.Add(button47);
            buttons.Add(button48);
            buttons.Add(button49);
            buttons.Add(button51);
            buttons.Add(button52);
            buttons.Add(button53);
            buttons.Add(button54);
            buttons.Add(button55);
            buttons.Add(button56);
            buttons.Add(button57);
            buttons.Add(button58);
            buttons.Add(button59);
            buttons.Add(button61);
            buttons.Add(button62);
            buttons.Add(button63);
            buttons.Add(button64);
            buttons.Add(button65);
            buttons.Add(button66);
            buttons.Add(button67);
            buttons.Add(button68);
            buttons.Add(button69);
            buttons.Add(button71);
            buttons.Add(button72);
            buttons.Add(button73);
            buttons.Add(button74);
            buttons.Add(button75);
            buttons.Add(button76);
            buttons.Add(button77);
            buttons.Add(button78);
            buttons.Add(button79);
            buttons.Add(button81);
            buttons.Add(button82);
            buttons.Add(button83);
            buttons.Add(button84);
            buttons.Add(button85);
            buttons.Add(button86);
            buttons.Add(button87);
            buttons.Add(button88);
            buttons.Add(button89);
            buttons.Add(button91);
            buttons.Add(button92);
            buttons.Add(button93);
            buttons.Add(button94);
            buttons.Add(button95);
            buttons.Add(button96);
            buttons.Add(button97);
            buttons.Add(button98);
            buttons.Add(button99);

            #endregion

            foreach (Button btn in buttons)
            {
                /* 从0开始的数组索引 */
                int rowIndex = Convert.ToInt32(btn.Name.Substring(6, 1))-1;
                int columnIndex = Convert.ToInt32(btn.Name.Substring(7, 1))-1;
                btn.Text = source[rowIndex, columnIndex] == -1 ? "" : source[rowIndex, columnIndex].ToString();
            }

            /* Button_Click */
            //如果 btn.Text != "" ，则高亮所有 btn.Text 的 Button ，更改背景颜色；下方的1-9框不可用
            //如果 btn.Text == "" ，则下方的1-9框可用；点击1-9的任意一个，更改 btn.Text ；
            //加一个 提交 按钮，按下的时候检查是否有9个1-9且每行/列无重复
        }
    }
}
