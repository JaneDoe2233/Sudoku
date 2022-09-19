using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aspose.Cells;

namespace SudokuTest
{
    /// <summary>
    /// 修改日志-2022.08
    /// 1.界面修改：
    ///   (1)父控件从TextBox改成FlowLayoutPanel，
    ///   (2)底下的输入数字格子从9个Label改成9个按钮，
    ///   (3)添加带图标的工具栏，
    ///   (4)重新设置所有按钮的边框颜色和经过时的鼠标样式，
    ///   (5)增加选中格子低亮当前行/列/宫，
    ///   (6)数独格子从55变成50，取消格子间的空隙
    /// 2.从 主页选难度->数独界面 变为 直接显示数独界面
    /// 3.添加导入导出、验证、还原功能
    /// 4.从二维数组改成交错数组：因为交错数组访问效率更高，二维数组生成效率高，本程序频繁访问所以改用交错数组
    /// https://www.cnblogs.com/yongbufangqi1988/archive/2010/06/10/1755863.html
    /// 5.输入最后一个数字后自动验证，成功后格子都不能点击或输入
    /// 6.
    /// </summary>
    public partial class SudokuMain : Form
    {
        // todo
        // 1.梳理一下窗体代码，检查有没有新的需求
        // 2.题目不能有多解

        #region 变量

        /// <summary>
        /// 空格数，默认简单模式
        /// </summary>
        public int NullNum = (int)Difficulty.Easy;

        /// <summary>
        /// 键盘中的数字键（主键盘上方和数字键盘）
        /// </summary>
        private List<Keys> _numKeys = new List<Keys>();

        /// <summary>
        /// 用于存放81个格子
        /// </summary>
        private List<Button> _cells = new List<Button>();

        /// <summary>
        /// 用于存放9个输入数字
        /// </summary>
        private List<Button> _nums = new List<Button>();

        /// <summary>
        /// 用于存放本次题目的解
        /// </summary>
        private int[][] _source = new int[9][];

        /// <summary>
        /// 用于存放本次的题目
        /// </summary>
        private int[][] _sudoku = new int[9][];

        /// <summary>
        /// 存放当前数独题目的值
        /// </summary>
        private int[][] _currSudoku = new int[9][];

        /// <summary>
        /// 记录当前选中的按钮
        /// </summary>
        private Button _currCell = null;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public SudokuMain()
        {
            InitializeComponent();

            //TestCode();

            #region 准备全局变量

            #region 准备键盘数字按键集合 _numKeys

            _numKeys.Add(Keys.D1);
            _numKeys.Add(Keys.D2);
            _numKeys.Add(Keys.D3);
            _numKeys.Add(Keys.D4);
            _numKeys.Add(Keys.D5);
            _numKeys.Add(Keys.D6);
            _numKeys.Add(Keys.D7);
            _numKeys.Add(Keys.D8);
            _numKeys.Add(Keys.D9);
            _numKeys.Add(Keys.NumPad1);
            _numKeys.Add(Keys.NumPad2);
            _numKeys.Add(Keys.NumPad3);
            _numKeys.Add(Keys.NumPad4);
            _numKeys.Add(Keys.NumPad5);
            _numKeys.Add(Keys.NumPad6);
            _numKeys.Add(Keys.NumPad7);
            _numKeys.Add(Keys.NumPad8);
            _numKeys.Add(Keys.NumPad9);

            #endregion

            // 81个格子 添加到全局变量、注册点击事件
            for (int i = 0; i < PanelSudoku.Controls.Count; i++)
            {
                FlowLayoutPanel panel = PanelSudoku.Controls[i] as FlowLayoutPanel;
                foreach (var cell in panel.Controls)
                {
                    // 将每个按钮都添加到全局变量 this._buttons 中
                    Button c = cell as Button;
                    this._cells.Add(c);

                    // 注册按钮点击事件
                    c.Click += Cell_Click;
                }
            }

            // 9个输入数字 添加到全局变量、注册点击事件
            foreach (var num in PanelInput.Controls)
            {
                // 将每个按钮都添加到全局变量 this._inputNums 中
                Button n = num as Button;
                this._nums.Add(n);

                // 注册按钮点击事件
                n.Click += Num_Click;
            }

            #endregion

            #region 注册事件

            // 注册×按钮点击事件
            btnDelete.Click += Delete_Click;

            // 注册键盘按键事件
            this.KeyDown += SudokuMain_KeyDown;

            // 菜单栏按钮点击事件
            tsBtnImport.Click += tsBtnImport_Click;
            tsBtnExport.Click += tsBtnExport_Click;
            tsBtnReset.Click += tsBtnReset_Click;
            tsBtnExit.Click += tsBtnExit_Click;

            // 下拉菜单[生成新数独]相关事件
            tsDropBtnNew.Click += tsDropBtnNew_Click;
            tsDropBtnNew.DropDownItemClicked += tsDropBtnNew_DropDownItemClicked;
            tsDropBtnNew.MouseHover += tsDropBtnNew_MouseHover;

            #endregion

            // 9个输入数字 和 ×按钮 默认可用
            // 设置按键输入可用：该属性设置为true，确保在窗体的控件上按键时，优先触发窗体的按键事件
            this.KeyPreview = true;

            // 生成新数独（ NullNum 默认是简单模式）
            GenerateSudoku(this.NullNum);

            // 将新生成的数独显示到界面上
            SetSudoku();
        }

        #region 事件

        //--------------------输入数字事件-------------------------

        /// <summary>
        /// 81个数独格子的点击事件
        /// </summary>
        private void Cell_Click(object sender, EventArgs e)
        {
            // todo 不会清空值，点击其他格子的时候重新覆盖，会有问题吗？
            // 设置 _currCell 为当前选中的格子
            _currCell = sender as Button;

            // 根据 _currCell 设置格子的背景色：低亮当前行列宫，或高亮相同数字格子背景色
            SetBtnBackColor();
        }

        /// <summary>
        /// 9个输入数字格子的点击事件
        /// </summary>
        private void Num_Click(object sender, EventArgs e)
        {
            // 根据输入数字按钮填写数独
            Button input = sender as Button;
            bool res = int.TryParse(input.Text, out int num);
            if (res)
            {
                SetNum(num);
            }
        }

        /// <summary>
        /// 删除×键
        /// </summary>
        private void Delete_Click(object sender, EventArgs e)
        {
            // 清空背景色
            foreach (Button btn in _cells)
            {
                btn.BackColor = SystemColors.ButtonHighlight;
            }

            // 选中填写的格子，清空已填写的数字
            if (_currCell != null)
            {
                // 当前序号
                string[] indexs = _currCell.Name.Split(new char[] { '_' });
                int row = int.Parse(indexs[1]);
                int col = int.Parse(indexs[2]);

                // 如果是初始题目格子，直接返回
                if (_sudoku[row - 1][col - 1] != 0) { return; }

                // 清空当前格子
                _currCell.Text = "";

                // 更新数独
                _currSudoku[row - 1][col - 1] = 0;

                // 重置边框
                _currCell.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
                _currCell.FlatAppearance.BorderSize = 1;

                // 设置输入数字的可用性
                SetInputButton();
            }

            // 不能在这清空 _currCell，因为清空后如果继续输入，需要再点击一次，会比较奇怪
        }

        /// <summary>
        /// 按键输入数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SudokuMain_KeyDown(object sender, KeyEventArgs e)
        {
            // 退格键、Delete键
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                // 手动调用删除按钮的点击事件
                Delete_Click(null, null);
                return;
            }

            // 如果键盘按键不是数字键，直接返回
            if (!_numKeys.Contains(e.KeyCode)) { return; }

            // 将 KeyCode 转化为数字：KeyCode 类型为枚举类 Keys，例如 Keys.D4 / Keys.NumPad5
            string code = e.KeyCode.ToString();
            string numstr = code.Substring(code.Length - 1);
            bool res = int.TryParse(numstr, out int num);
            if (res)
            {
                // 填写数独
                SetNum(num);
            }
        }

        //--------------------工具栏按钮点击事件-------------------------

        /// <summary>
        /// 导入
        /// </summary>
        private void tsBtnImport_Click(object sender, EventArgs e)
        {
            // 如果当前数独解答了一半，询问用户是否确认放弃并导入
            if (CheckSolveStatus() == 1)
            {
                DialogResult res = MessageBox.Show("当前数独解答了一半，是否放弃当前数独并导入新的数独？", "询问", MessageBoxButtons.YesNoCancel);
                if (res != DialogResult.Yes)
                {
                    return;
                }
            }

            // 从文件导入数据
            OpenFileDialog import = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Filter = "数独初盘文本文件（*.csv）|*.csv"
            };
            if (import.ShowDialog() == DialogResult.OK)
            {
                string data = File.ReadAllText(import.FileName);
                string[] arrays = data.Split(new char[] { '—' }, StringSplitOptions.RemoveEmptyEntries);

                // 导入数独题目
                string sudoku = arrays[0];
                int[][] arr1 = ConvertStringToArray(sudoku);
                if (arr1 != null)
                {
                    // 指向同一个地址
                    _sudoku = arr1;

                    // _currSudoku 更新为导入的题目
                    for (int i = 0; i < _sudoku.Length; i++)
                    {
                        // arr.Clone() Array.Copy() 可以对元素类型为 非引用类型 的数组进行复制
                        _currSudoku[i] = (int[])_sudoku[i].Clone();
                    }


                }

                // 判断是否解答过
                string solved = arrays[1];
                int[][] arr2 = ConvertStringToArray(solved);
                string[] solvedNums = solved.Split(new char[] { '0', ',', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                bool hasSolved = solvedNums.Length > 0;

                // 存在已解答的数字，则将解答的格子添加到 _currSudoku 中
                if (hasSolved && arr2 != null)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            if (arr2[i][j] != 0)
                            {
                                _currSudoku[i][j] = arr2[i][j];
                            }
                        }
                    }
                }

                // 导入数独解
                string source = arrays[2];
                int[][] arr3 = ConvertStringToArray(source);
                if (arr3 != null)
                {
                    _source = arr3;
                }

                // 判断是不是已经解答完毕
                int solveStatus = CheckSolveStatus();
                // 未解答或成功解答完毕，则直接导入题目
                if (solveStatus == 0 || solveStatus == 2)
                {
                    // 解答完毕，则_currSudoku 还原为导入的题目
                    for (int i = 0; i < _sudoku.Length; i++)
                    {
                        _currSudoku[i] = (int[])_sudoku[i].Clone();
                    }

                    //根据 _sudoku 显示数独
                    SetSudoku();
                }
                // 解答一半，则还原解答状态
                else
                {
                    //根据 _sudoku 显示数独
                    SetSudoku();

                    //将之前解答的显示到界面上
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            if (arr2[i][j] != 0)
                            {
                                string btnName = "button_" + (i + 1) + "_" + (j + 1);
                                var btn = this.Controls.Find(btnName, true)[0] as Button;
                                btn.Text = arr2[i][j].ToString();

                                // 赋值时检查当前格子是否正确
                                if (_source[i][j] != arr2[i][j])
                                {
                                    // 边框加粗变红 
                                    btn.FlatAppearance.BorderColor = Color.Red;
                                    btn.FlatAppearance.BorderSize = 2;
                                }
                                else
                                {
                                    // 边框默认灰色
                                    btn.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
                                    btn.FlatAppearance.BorderSize = 1;
                                }
                            }
                        }
                    }

                    // 输入之后更新输入数字按钮的可用性
                    SetInputButton();
                }
            }
        }

        /// <summary>
        /// 导出
        /// </summary>
        private void tsBtnExport_Click(object sender, EventArgs e)
        {
            // 获取当前日期和时间
            string datetime = DateTime.Now.ToString("yyyyMMdd-HHmm");

            // 导出题目和已解答格子
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                // .txt

                //todo "Excel file (*.xls)|*.xlsx"：Excel 文件导出后打不开(手动设置扩展名也不可以)，先用csv
                Filter = "CSV file (*.csv)|*.csv",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                FileName = "数独题目_" + datetime
            };

            // 拼接数独题目
            StringBuilder sudokuSB = new StringBuilder("——————\n"); //"[题目]\n"
            for (int i = 0; i < 9; i++)
            {
                sudokuSB.Append(string.Join(",", _sudoku[i]) + "\n");
            }

            // 拼接已解答格子
            StringBuilder solvedSB = new StringBuilder("——————\n"); //"[已解答]\n"
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (_currSudoku[i][j] != _sudoku[i][j])
                    {
                        solvedSB.Append(_currSudoku[i][j] + ",");
                    }
                    else
                    {
                        solvedSB.Append("0,");
                    }
                }

                solvedSB.Append("\n");
            }

            // 拼接数独解
            StringBuilder sourceSB = new StringBuilder("——————\n"); //"[解]\n"
            for (int i = 0; i < 9; i++)
            {
                sourceSB.Append(string.Join(",", _source[i]) + "\n");
            }

            // 保存到文件
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Workbook workbook = new Workbook();
                //Worksheet worksheet = workbook.Worksheets[0];
                //worksheet.Cells.ImportArray(_sudokuDataSource.Sudoku, 0, 0);

                string data = sudokuSB.ToString() + solvedSB.ToString() + sourceSB.ToString();

                File.WriteAllText(saveFileDialog.FileName, data, Encoding.UTF8); // 因为有中文字符，所以需要传编码规则参数
            }
        }

        /// <summary>
        /// 生成新数独
        /// </summary>
        private void tsDropBtnNew_Click(object sender, EventArgs e)
        {
            // 如果数独解答一半，则询问用户是否确认放弃并生成
            if (CheckSolveStatus() == 1)
            {
                DialogResult result = MessageBox.Show("当前题目开始解答且未解答完，请问是否放弃当前题目且生成新数独？", "询问", MessageBoxButtons.YesNoCancel);
                if (result != DialogResult.Yes)
                {
                    return;
                }
            }

            if (!btnDelete.Enabled)
            {
                // 设置按键事件可用
                this.KeyPreview = true;

                // 设置×按钮可用
                btnDelete.BackColor = SystemColors.Control;
                btnDelete.Enabled = true;
            }

            // 生成数独（NullNum 默认是简单模式）
            GenerateSudoku(this.NullNum);

            // 将新生成的数独显示到界面上，更新 _nums 的可用性
            SetSudoku();
        }

        /// <summary>
        /// 生成新数独 菜单项点击事件
        /// </summary>
        private void tsDropBtnNew_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // 如果数独解答一半，则询问用户是否确认放弃并生成
            if (CheckSolveStatus() == 1)
            {
                DialogResult result = MessageBox.Show("当前题目开始解答且未解答完，请问是否放弃当前题目且生成新数独？", "询问", MessageBoxButtons.YesNoCancel);
                if (result != DialogResult.Yes)
                {
                    return;
                }
            }

            // 点击不同的按钮，设置不同的空格子数
            switch (e.ClickedItem.Text)
            {
                case "简单": { this.NullNum = (int)Difficulty.Easy; break; }
                case "中等": { this.NullNum = (int)Difficulty.Medium; break; }
                case "困难": { this.NullNum = (int)Difficulty.Difficult; break; }
            }

            // 如果还上次解答完成后没有设置可用性，已经设置过不需要重复设置
            if (!btnDelete.Enabled)
            {
                // 设置按键事件可用
                this.KeyPreview = true;

                // 设置×按钮可用
                btnDelete.BackColor = SystemColors.Control;
                btnDelete.Enabled = true;
            }

            // 生成并显示数独
            GenerateSudoku(this.NullNum);

            // 将新生成的数独显示到界面上
            SetSudoku();
        }

        /// <summary>
        /// 生成新数独 鼠标悬停事件
        /// </summary>
        private void tsDropBtnNew_MouseHover(object sender, EventArgs e)
        {
            // 鼠标悬停，展示下拉按钮
            tsDropBtnNew.ShowDropDown();
        }

        /// <summary>
        /// 还原为数独初始题目
        /// </summary>
        private void tsBtnReset_Click(object sender, EventArgs e)
        {
            // 如果从完成后状态还原，设置按钮可编辑，重新注册事件
            if (!btnDelete.Enabled)
            {
                // 注册按键事件
                this.KeyPreview = true;

                // 设置×按钮可用
                btnDelete.BackColor = SystemColors.Control;
                btnDelete.Enabled = true;
            }

            // 根据数独题目 _sudoku 重新设置设置数独，更新 _nums 的可用性
            SetSudoku();
        }

        /// <summary>
        /// 退出程序
        /// </summary>
        private void tsBtnExit_Click(object sender, EventArgs e)
        {
            // 解答一半时，询问用户是否确认退出
            if (CheckSolveStatus() == 1)
            {
                DialogResult res = MessageBox.Show("当前数独解答了一半，是否放弃当前数独并退出程序？", "询问", MessageBoxButtons.YesNoCancel);
                if (res != DialogResult.Yes)
                {
                    return;
                }
            }

            // 关闭窗体
            this.Close();
        }

        #endregion

        #region 方法

        /// <summary>
        /// 根据给定数字填写数独
        /// </summary>
        private void SetNum(int num)
        {
            // 如果没有选中的数独格子，直接返回
            if (_currCell == null) { return; }

            // 如果不是1-9，直接返回
            if (num < 1 || num > 9) { return; }

            // 获取序号
            string[] nos = _currCell.Name.Split(new char[] { '_' });
            int row = int.Parse(nos[1]);
            int col = int.Parse(nos[2]);

            // 如果选中的格子是数独题目，直接返回
            if (_sudoku[row - 1][col - 1] != 0) { return; }

            // 在格子上填写数字
            _currCell.Text = num.ToString();                                                                                                                                                                                                                         
            // 更新全局变量
            _currSudoku[row - 1][col - 1] = num;

            // 输入之后检查当前格子是否正确
            #region //方法一：循环检查当前行/列/宫有没有重复数字

            //// 所在宫的索引( , )
            //int rowNo = (row - 1) / 3;
            //int colNo = (col - 1) / 3;

            //// 检查当前行/列/宫是否有重复数字
            //for (int i = 0; i < 9; i++)
            //{
            //    // 当前行（跳过当前单元格）
            //    if (i != col - 1 && _currSudoku[row - 1, i] == num)
            //    {
            //        return false;
            //    }

            //    // 当前列（跳过当前单元格）
            //    if (i != row - 1 && _currSudoku[i, col - 1] == num)
            //    {
            //        return false;
            //    }

            //    // 当前宫（跳过当前单元格）
            //    int r = rowNo * 3 + (i / 3);
            //    int c = colNo * 3 + i % 3;
            //    if (r != row - 1 && c != col - 1 && _currSudoku[r, c] == num)
            //    {
            //        return false;
            //    }
            //}

            //return true;

            #endregion
            // 方法二：直接根据题解判断
            if (_source[row - 1][col - 1] != num)
            {
                // 边框加粗变红 
                _currCell.FlatAppearance.BorderColor = Color.Red;
                _currCell.FlatAppearance.BorderSize = 2;
            }
            else
            {
                // 边框默认灰色
                _currCell.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
                _currCell.FlatAppearance.BorderSize = 1;
            }

            // 输入之后设置输入数字按钮的可用性（在 判断 isFinish 之前执行，确保成功弹窗前所有数字按钮都不可用）
            SetInputButton();

            // 高亮当前数字，并判断是否解答成功
            bool isFinish = true;
            foreach (Button btn in _cells)
            {
                // 高亮所有当前数字
                bool res = int.TryParse(btn.Text.Trim(), out int btnNum);
                if (res)
                {
                    btn.BackColor = btnNum == num ? SystemColors.GradientInactiveCaption : SystemColors.ButtonHighlight;
                }

                // 格子为空 或 存在错误的格子，isFinish 为false
                if (btn.Text.Trim().Equals("") || btn.FlatAppearance.BorderColor == Color.Red)
                {
                    isFinish = false;
                }
            }

            // 如果填写完成，验证数独
            if (isFinish)
            {
                MessageBox.Show("解答成功，恭喜！", "Congratulation", MessageBoxButtons.OK);

                // 清空格子背景色
                foreach (Button btn in _cells)
                {
                    btn.BackColor = SystemColors.ButtonHighlight;
                }

                // 屏蔽按键事件
                this.KeyPreview = false;

                // 设置×按钮不可用
                btnDelete.BackColor = SystemColors.ControlLight;
                btnDelete.Enabled = false;
            }
        }

        //-----------被调用的子函数----------------

        /// <summary>
        /// 生成新数独，更新全局变量
        /// </summary>
        /// <param name="nullnum"></param>
        private void GenerateSudoku(int nullnum)
        {
            // 生成新的不同的数独题目（解是否相同都无所谓）
            SudokuDataSource sudoku = new SudokuDataSource(nullnum);
            // todo 二维数组和交错数组比较
            while (sudoku.Sudoku.Equals(_sudoku))
            {
                sudoku = new SudokuDataSource(nullnum);
            }

            // 更新空格数
            this.NullNum = nullnum;

            // 更新数独和解变量 
            // 原来的二维数组改成交错数组了
            //this._source = sudokuDataSource.Source; // 解
            //this._sudoku = sudokuDataSource.Sudoku; // 题目
            for (int i = 0; i < 9; i++)
            {
                this._source[i] = new int[9];
                this._sudoku[i] = new int[9];
                this._currSudoku[i] = new int[9];

                for (int j = 0; j < 9; j++)
                {
                    this._source[i][j] = sudoku.Source[i, j]; // 解
                    this._sudoku[i][j] = sudoku.Sudoku[i, j]; // 题目
                    this._currSudoku[i][j] = sudoku.Sudoku[i, j]; // 当前数独
                }
            }

            // 这种方式两个数组指向的同一个地址，对 _currSudoku 数组的修改也会修改 _source
            //this._currSudoku = sudokuDataSource.Sudoku; 
            //Array.Copy(_sudoku, _currSudoku, 81);
            //_currSudoku.Clone()：跟Copy()一样，可以对数组元素非引用类型的数组进行复制；但对数组元素为引用类型的数组，这两种方法都不能复制。
        }

        /// <summary>
        /// 根据数独题目设置设置数独
        /// </summary>
        private void SetSudoku()
        {
            int[] counts = new int[9];
            foreach (var btn in this._cells)
            {
                string[] btnNames = btn.Name.Split(new char[] { '_' }); // buttont_1_1
                int row = int.Parse(btnNames[1]);
                int col = int.Parse(btnNames[2]);
                int num = this._sudoku[row - 1][col - 1];

                // 设置对应格子的数字，无数字则为空格
                btn.Text = num == 0 ? " " : num.ToString();

                // 设置题目为默认黑色，空格子数字颜色为蓝色
                btn.ForeColor = num == 0 ? SystemColors.HotTrack : SystemColors.ControlText;

                // 记录数字的个数
                switch (num)
                {
                    case 1: counts[0]++; break;
                    case 2: counts[1]++; break;
                    case 3: counts[2]++; break;
                    case 4: counts[3]++; break;
                    case 5: counts[4]++; break;
                    case 6: counts[5]++; break;
                    case 7: counts[6]++; break;
                    case 8: counts[7]++; break;
                    case 9: counts[8]++; break;
                    default: break;
                }
            }

            // todo 把函数调用改成直接嵌入，因为调用函数会再循环一遍_buttons
            // 但是导入的时候，先SetSudoku()。设置过数字格子可用性，然后设置颜色，再设置一次数字格子可用性，会重复
            // 已全部填写的数字输入按钮 背景变为白色 且 不可用，否则背景为默认的灰且可用
            foreach (Button input in this._nums)
            {
                int index = int.Parse(input.Text) - 1;
                input.BackColor = counts[index] == 9 ? SystemColors.ControlLight : SystemColors.Control;
                input.Enabled = counts[index] != 9;
            }
        }

        /// <summary>
        /// 检查解答状态。未解答返回0，解答一半返回1，成功解答完返回2，异常返回-1
        /// </summary>
        /// <returns></returns>
        private int CheckSolveStatus()
        {
            // 判断是否未解答
            bool isNew = true;
            for (int i = 0; i < 9; i++)
            {
                int[] arr1 = _currSudoku[i];
                int[] arr2 = _sudoku[i];
                if (!arr1.SequenceEqual(arr2))
                {
                    isNew = false;
                }
            }

            // 判断是否解答成功
            bool isComplete = true;
            for (int i = 0; i < 9; i++)
            {
                int[] arr1 = _currSudoku[i];
                int[] arr2 = _source[i];
                if (!arr1.SequenceEqual(arr2))
                {
                    isComplete = false;
                }
            }

            // 未解答
            if (isNew)
            {
                return 0;
            }
            // 解答一半 或 全部填写但有错误
            else if (!isNew && !isComplete)
            {
                return 1;
            }
            // 成功解答
            else if (isComplete)
            {
                return 2;
            }

            return -1;

            #region //方法二：循环每个格子，可多判断 解答完但有错 的情况

            //int unSolvedCount = 0;
            //bool existErr = false;

            //// 循环每一个格子
            //foreach (var btn in this._buttons)
            //{
            //    if (btn.Text.Trim().Equals(""))
            //    {
            //        unSolvedCount++;
            //    }

            //    if (btn.FlatAppearance.BorderColor == Color.Red)
            //    {
            //        existErr = true;
            //    }
            //}

            //// 未解答
            //if (unSolvedCount == this.NullNum)
            //{
            //    return 0;
            //}
            //// 解答一半
            //else if (unSolvedCount > 0 && unSolvedCount < this.NullNum)
            //{
            //    return 1;
            //}
            //// 全部填写但有错误
            //else if (unSolvedCount == 0 && existErr)
            //{
            //    return 1.5;
            //}
            //// 成功解答
            //else if (unSolvedCount == 0 && !existErr)
            //{
            //    return 2;
            //}


            //return -1;

            #endregion
        }

        /// <summary>
        /// 设置输入数字按钮的背景色和可用性
        /// </summary>
        private void SetInputButton()
        {
            // 循环所有输入数字按钮，找出界面上已有9个格子的数字，并设置背景色和可用性
            int[] count = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            foreach (Button btn in _cells)
            {
                // 若当前格子内容为空，则直接跳过
                if (btn.Text.Trim().Equals("")) { continue; }

                // 记录数字在题目中出现的次数
                int text = int.Parse(btn.Text);
                switch (text)
                {
                    case 1: count[0]++; break;
                    case 2: count[1]++; break;
                    case 3: count[2]++; break;
                    case 4: count[3]++; break;
                    case 5: count[4]++; break;
                    case 6: count[5]++; break;
                    case 7: count[6]++; break;
                    case 8: count[7]++; break;
                    case 9: count[8]++; break;
                    default: break;
                }
            }

            // 已全部填写的数字输入按钮 背景变为白色 且 不可用，否则背景为默认的灰且可用
            foreach (Button input in this._nums)
            {
                int index = int.Parse(input.Text) - 1;
                input.BackColor = count[index] == 9 ? SystemColors.ControlLight : SystemColors.Control;
                input.Enabled = count[index] != 9;
            }
        }

        /// <summary>
        /// 根据 _currCell 设置格子的背景色
        /// </summary>
        private void SetBtnBackColor()
        {
            // 如果未选择格子，则清空所有背景色
            if (_currCell == null)
            {
                foreach (Button btn in _cells)
                {
                    btn.BackColor = SystemColors.ButtonHighlight;
                }
            }
            // 选中未填写格子，设置低亮当前行/列/宫（浅蓝色）
            else if (_currCell.Text.Trim().Equals(""))
            {
                // 低亮当前行/列/宫
                string[] nos = _currCell.Name.Split(new char[] { '_' });
                int row = int.Parse(nos[1]);
                int col = int.Parse(nos[2]);
                int rowNo = (row - 1) / 3; // 当前宫的行索引
                int colNo = (col - 1) / 3; // 当前宫的列索引
                foreach (Button btn in _cells)
                {
                    string[] bNos = btn.Name.Split(new char[] { '_' });
                    int brow = int.Parse(bNos[1]);
                    int bcol = int.Parse(bNos[2]);

                    // 当前行/列
                    if (bNos[1].Equals(nos[1]) || bNos[2].Equals(nos[2]))
                    {
                        btn.BackColor = SystemColors.InactiveBorder;
                    }
                    // 当前宫
                    else if ((brow - 1 < (rowNo + 1) * 3 && brow - 1 >= rowNo * 3) && (bcol - 1 < (colNo + 1) * 3 && bcol - 1 >= colNo * 3))
                    {
                        btn.BackColor = SystemColors.InactiveBorder;
                    }
                    else
                    {
                        btn.BackColor = SystemColors.ButtonHighlight;
                    }
                }

                // 高亮当前格子
                _currCell.BackColor = SystemColors.InactiveCaption;
            }
            // 选中数独题目格子或已填写的格子，高亮相同数字格子（蓝色）
            else if (!_currCell.Text.Trim().Equals(""))
            {
                foreach (Button btn in _cells)
                {
                    if (btn.Text.Trim().Equals(_currCell.Text.Trim()))
                    {
                        if (btn == _currCell)
                        {
                            // 当前按钮颜色是更深的蓝色
                            btn.BackColor = SystemColors.GradientActiveCaption;
                        }
                        else
                        {
                            // 相同数字按钮的颜色是浅一点的蓝色
                            btn.BackColor = SystemColors.GradientInactiveCaption;
                        }
                    }
                    else
                    {
                        // 不是同一数字的按钮背景为白色
                        btn.BackColor = SystemColors.ButtonHighlight;
                    }
                }
            }
        }

        /// <summary>
        /// 将数独字符串转换为交错数组（"\n"分割行 ","分割列）
        /// </summary>
        private int[][] ConvertStringToArray(string array)
        {
            int[][] numArr = new int[9][];
            string[] rows = array.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (rows.Length != 9)
            {
                MessageBox.Show($"数独格式出错：数独行数为[{rows.Length}]，请检查数据后重试！");
                return null;
            }

            for (int i = 0; i < rows.Length; i++)
            {
                string row = rows[i];
                string[] nums = row.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (nums.Length != 9)
                {
                    MessageBox.Show($"数独第[{i}]行数据格式出错：数字个数为[{nums.Length}]，请检查数据后重试！");
                    return null;
                }

                numArr[i] = new int[9];
                for (int j = 0; j < nums.Length; j++)
                {
                    int num = int.Parse(nums[j]);
                    numArr[i][j] = num;
                }
            }

            return numArr;
        }

        #endregion

        // 用于测试的代码
        private void TestCode()
        {
            #region 测试 SequenceEqual()

            // 一维数组判断相等
            int[] arr1 = new int[9] { 1, 2, 1, 1, 1, 1, 1, 1, 1 };
            int[] arr2 = new int[9] { 1, 1, 1, 1, 1, 1, 1, 1, 2 };
            int[] arr3 = new int[9] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            int[] arr4 = new int[9] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            bool isSeEqual1 = arr1.SequenceEqual(arr2); // false
            bool equal1 = arr3.SequenceEqual(arr4); // true
            bool equal2 = arr3.Equals(arr4); // false

            // 交错数组可以用 SequenceEqual() 判断相等
            int[][] jcarr1 = new int[9][];
            int[][] jcarr2 = new int[9][];
            jcarr1.SequenceEqual(jcarr2);

            // 一维引用数组判断相等
            string[] arr11 = new string[5] { "1", "2", "3", "4", "5" };
            string[] arr12 = new string[5] { "1", "2", "3", "5", "4" };
            string[] arr13 = new string[5] { "1", "2", "3", "4", "5" };
            string[] arr14 = arr13;
            bool isSeEqual11 = arr11.SequenceEqual(arr12); // false
            bool isSeEqual12 = arr11.SequenceEqual(arr13); // true
            bool equal11 = arr13.Equals(arr11); // false
            bool equal12 = arr13.Equals(arr14); // true
            bool equal13 = arr11.Equals(arr14); // false

            #endregion

            #region 对 引用类型数组 复制的错误尝试

            //Array.Copy()、
            //arr.Clone()、
            //arr.CopyTo()：这三种方法可以对数组元素为 非引用类型 的数组进行复制；但对数组元素为 引用类型 的数组，这三种都不能复制。

            // 不能保证源数组不变的方法一
            Array.Copy(_sudoku, _currSudoku, 9);
            bool isEqual2 = _currSudoku.Equals(_sudoku); // false
            bool isSeqEqual2 = _currSudoku.SequenceEqual(_sudoku); // true
            _currSudoku[0][0] = 11; // _sudoku 变

            // 不能保证源数组不变的方法二：因为数组元素是引用类型，所以复制出的新数组元素指向的是旧地址
            _currSudoku = (int[][])_sudoku.Clone();
            bool isEqual3 = _currSudoku.Equals(_sudoku); // false
            bool isSeqEqual3 = _currSudoku.SequenceEqual(_sudoku); // true
            _currSudoku[0][0] = 11; // _sudoku 变

            // 不能保证源数组不变的方法三
            _sudoku.CopyTo(_currSudoku, 0);
            bool isEqual4 = _currSudoku.Equals(_sudoku); // false
            bool isSeqEqual4 = _currSudoku.SequenceEqual(_sudoku); // true
            _currSudoku[0][0] = 11; // _sudoku 变

            #endregion
        }
    }
}