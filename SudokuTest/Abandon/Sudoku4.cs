using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace SudokuTest
{
    //todo
    //1.边框
    //2.不用数据库连接，直接放当前文件夹下
    //3.数字背景一进去就变白？
    //4.

    /// <summary>
    /// 数独窗体
    /// </summary>
    public partial class Sudoku4 : Form
    {
        #region 变量

        /// <summary>
        /// 自定义类型，用来存放提示信息
        /// </summary>
        public Message msg = new Message();

        /// <summary>
        /// 用于存放81个按钮对象
        /// </summary>
        private List<Button> _buttons = new List<Button>();

        /// <summary>
        /// 用于存放9个标签对象
        /// </summary>
        private List<Label> _labels = new List<Label>();

        /// <summary>
        /// 用于存放10个TextBox对象
        /// </summary>
        private List<TextBox> _textBoxes = new List<TextBox>();

        /// <summary>
        /// 用于存放触发label_Click事件的按钮
        /// </summary>
        private Button _currBtn = new Button();

        /// <summary>
        /// 用于存放触发label_Click事件的标签
        /// </summary>
        private Label _currLabel = new Label();

        /// <summary>
        /// 用于存放已编辑的按钮
        /// </summary>
        private List<Button> _editBtns = new List<Button>();

        /// <summary>
        /// 用于存放填的格子数
        /// </summary>
        private int _nullnum;

        /// <summary>
        /// 用于存放本次的题目
        /// </summary>
        private int[,] _sourceQuestion = new int[9, 9];

        /// <summary>
        /// 用于存放本次题目的解
        /// </summary>
        private int[,] _source = new int[9, 9];

        /// <summary>
        /// 用于存放暂停题目的解答状态
        /// </summary>
        private Hashtable _htSolution = new Hashtable();

        //TODO：连接不同客户端的本地数据库or连接网络数据库or？
        /// <summary>
        /// 用于存放数据库连接
        /// </summary>
        private SqlConnection _sqlConn = new SqlConnection("server = . ; database = SudokuTest; uid = sa; pwd = lt123$");

        /// <summary>
        /// 用于存放暂停题目的备注
        /// </summary>
        private string _note = "";

        /// <summary>
        /// 标志备注是否改变
        /// </summary>
        private bool _isNoteChanged = false;

        #endregion

        #region 窗体方法

        /// <summary>
        /// 初始化类，方便带参构造函数调用
        /// </summary>
        private void InitializeSudoku()
        {
            InitializeComponent();

            /* 该属性设置为true，确保在窗体的控件上按键时，优先触发窗体的按键事件 */
            this.KeyPreview = true;

            /* 注册窗体事件 */
            this.Load += Sudoku4_Load;
            this.KeyDown += Sudoku4_KeyDown;
            this.FormClosing += Sudoku4_FormClosing;

            #region 准备控件集合变量  _labels 、_textBoxes 和 _buttons

            /* 将1~9的标签存入变量 */
            _labels.Add(label1);
            _labels.Add(label2);
            _labels.Add(label3);
            _labels.Add(label4);
            _labels.Add(label5);
            _labels.Add(label6);
            _labels.Add(label7);
            _labels.Add(label8);
            _labels.Add(label9);
            //_labels.Add(labelNote);
            //_labels.Add(labelToolBar);

            /* 将需要设置为不可用的textBox存入变量 */
            _textBoxes.Add(textBoxLabel);
            _textBoxes.Add(textBox11);
            _textBoxes.Add(textBox12);
            _textBoxes.Add(textBox13);
            _textBoxes.Add(textBox21);
            _textBoxes.Add(textBox22);
            _textBoxes.Add(textBox23);
            _textBoxes.Add(textBox31);
            _textBoxes.Add(textBox32);
            _textBoxes.Add(textBox33);
            _textBoxes.Add(textBoxToolBar);
            _textBoxes.Add(textBoxToolBarText);
            //_textBoxes.Add(textBoxNote);

            /* 将放置题目的81个格子存入变量 */
            _buttons.Add(button11);
            _buttons.Add(button12);
            _buttons.Add(button13);
            _buttons.Add(button14);
            _buttons.Add(button15);
            _buttons.Add(button16);
            _buttons.Add(button17);
            _buttons.Add(button18);
            _buttons.Add(button19);
            _buttons.Add(button21);
            _buttons.Add(button22);
            _buttons.Add(button23);
            _buttons.Add(button24);
            _buttons.Add(button25);
            _buttons.Add(button26);
            _buttons.Add(button27);
            _buttons.Add(button28);
            _buttons.Add(button29);
            _buttons.Add(button31);
            _buttons.Add(button32);
            _buttons.Add(button33);
            _buttons.Add(button34);
            _buttons.Add(button35);
            _buttons.Add(button36);
            _buttons.Add(button37);
            _buttons.Add(button38);
            _buttons.Add(button39);
            _buttons.Add(button41);
            _buttons.Add(button42);
            _buttons.Add(button43);
            _buttons.Add(button44);
            _buttons.Add(button45);
            _buttons.Add(button46);
            _buttons.Add(button47);
            _buttons.Add(button48);
            _buttons.Add(button49);
            _buttons.Add(button51);
            _buttons.Add(button52);
            _buttons.Add(button53);
            _buttons.Add(button54);
            _buttons.Add(button55);
            _buttons.Add(button56);
            _buttons.Add(button57);
            _buttons.Add(button58);
            _buttons.Add(button59);
            _buttons.Add(button61);
            _buttons.Add(button62);
            _buttons.Add(button63);
            _buttons.Add(button64);
            _buttons.Add(button65);
            _buttons.Add(button66);
            _buttons.Add(button67);
            _buttons.Add(button68);
            _buttons.Add(button69);
            _buttons.Add(button71);
            _buttons.Add(button72);
            _buttons.Add(button73);
            _buttons.Add(button74);
            _buttons.Add(button75);
            _buttons.Add(button76);
            _buttons.Add(button77);
            _buttons.Add(button78);
            _buttons.Add(button79);
            _buttons.Add(button81);
            _buttons.Add(button82);
            _buttons.Add(button83);
            _buttons.Add(button84);
            _buttons.Add(button85);
            _buttons.Add(button86);
            _buttons.Add(button87);
            _buttons.Add(button88);
            _buttons.Add(button89);
            _buttons.Add(button91);
            _buttons.Add(button92);
            _buttons.Add(button93);
            _buttons.Add(button94);
            _buttons.Add(button95);
            _buttons.Add(button96);
            _buttons.Add(button97);
            _buttons.Add(button98);
            _buttons.Add(button99);

            #endregion

            /* 设置作为背景的 TextBox 不可用 */
            foreach (TextBox textBox in _textBoxes)
            {
                textBox.Enabled = false;
                textBox.BackColor = SystemColors.ButtonHighlight;
            }
        }

        /// <summary>
        /// 构造函数：传入 nullparam 
        /// </summary>
        public Sudoku4(int nullparam)
        {
            /* 初始化类 */
            InitializeSudoku();

            /* 设置题解 */
            SudokuDataSource sudokuDataSource = new SudokuDataSource(nullparam);
            this._source = sudokuDataSource.Source;
            this._sourceQuestion = sudokuDataSource.Sudoku;

            /* 为全局变量赋值 */
            _nullnum = nullparam;
        }

        /// <summary>
        /// 构造函数：将暂停的题目还原
        /// </summary>
        public Sudoku4(int[,] source, int[,] sourceQuestion, Hashtable htSolution, int nullnum, string note)
        {
            /* 初始化类 */
            InitializeSudoku();

            /* 设置题目和题解 */
            this._source = source;
            this._sourceQuestion = sourceQuestion;
            this._htSolution = htSolution;

            /* 为全局变量赋值 */
            this._nullnum = nullnum;
            this._note = note;
        }

        /// <summary>
        /// 加载窗体
        /// </summary>
        private void Sudoku4_Load(object sender, EventArgs e)
        {
            /* 注册按钮事件 */
            buttonClear.Click += ButtonClear_Click;
            buttonCollect.Click += ButtonCollect_Click;
            buttonQuit.Click += ButtonQuit_Click;
            buttonPause.Click += ButtonPause_Click;
            buttonSubmit.Click += ButtonSubmit_Click;
            buttonTryAgain.Click += ButtonTryAgain_Click;
            buttonExit.Click += ButtonExit_Click;
            textBoxNote.TextChanged += TextBoxNote_TextChanged;

            /* 设置题目，并注册81个按钮的事件 */
            foreach (Button btn in _buttons)
            {
                /* 设置题目 */
                int rowIndex = Convert.ToInt32(btn.Name.Substring(6, 1)) - 1;
                int columnIndex = Convert.ToInt32(btn.Name.Substring(7, 1)) - 1;
                btn.Text = _sourceQuestion[rowIndex, columnIndex] == -1 ? "" : _sourceQuestion[rowIndex, columnIndex].ToString();

                /* 如果解答状态变量有值 */
                if (_htSolution.Count > 0)
                {
                    /* 还原上次已经解答的格子 */
                    foreach (string key in _htSolution.Keys)
                    {
                        /* 获取已填写格子的序号 */
                        string[] orders = new string[2];
                        int inx = 0;
                        for (int i = 0; i < key.Length; i++)
                        {
                            if (char.IsNumber(key[i]))
                            {
                                orders[inx] = key[i].ToString();
                                inx++;
                            }
                        }

                        /* 找到对应的格子，填写之前的解答数据 */
                        foreach (Button btnSol in _buttons)
                        {
                            string btnRow = btnSol.Name.Substring(btnSol.Name.Length - 2, 1);
                            string btnCol = btnSol.Name.Substring(btnSol.Name.Length - 1, 1);
                            if (orders[0].Equals(btnRow) && orders[1].Equals(btnCol))
                            {
                                btnSol.Text = _htSolution[key].ToString();
                                btnSol.ForeColor = Color.FromArgb(0, 128, 0);
                                btnSol.Font = new Font("宋体", 14.25f, _currLabel.Font.Style | FontStyle.Bold);
                            }
                        }
                    }

                    /* 设置备注 */
                    textBoxNote.Text = _note;
                }

                /* 注册81个按钮的点击事件 */
                btn.Click += Buttn_Click;
                btn.TextChanged += Button_TextChanged;
            }

            /* 设置题目后，手动触发按钮文本改变事件 */
            Button_TextChanged(null, null);

            foreach (Label label in _labels)
            {
                /* 注册9个标签的点击事件 */
                label.Click += Label_Click;
            }
        }

        /// <summary>
        /// 按键输入
        /// </summary>
        private void Sudoku4_KeyDown(object sender, KeyEventArgs e)
        {
            #region 准备数字按键集合 nums

            List<Keys> nums = new List<Keys>();
            nums.Add(Keys.D1);
            nums.Add(Keys.D2);
            nums.Add(Keys.D3);
            nums.Add(Keys.D4);
            nums.Add(Keys.D5);
            nums.Add(Keys.D6);
            nums.Add(Keys.D7);
            nums.Add(Keys.D8);
            nums.Add(Keys.D9);
            nums.Add(Keys.NumPad1);
            nums.Add(Keys.NumPad2);
            nums.Add(Keys.NumPad3);
            nums.Add(Keys.NumPad4);
            nums.Add(Keys.NumPad5);
            nums.Add(Keys.NumPad6);
            nums.Add(Keys.NumPad7);
            nums.Add(Keys.NumPad8);
            nums.Add(Keys.NumPad9);

            #endregion

            /* 将Keys的Code转化为数字字符串 */
            string code = e.KeyCode.ToString();
            string numstr = code.Substring(code.Length - 1);

            /* 如果焦点在备注上，直接返回 */
            if (textBoxNote.Focused) { return; }

            /* 如果不是在要填的格子上，按数字键显示所有数字相同的格子 */
            if (_currBtn.Text != "" && _currBtn.ForeColor != Color.FromArgb(0, 128, 0))
            {
                foreach (Button btn in _buttons)
                {
                    /* 相同数字按钮的颜色是的蓝色，否则是白色 */
                    btn.BackColor = btn.Text.Equals(numstr) ? SystemColors.GradientInactiveCaption : SystemColors.ButtonHighlight;
                }
            }
            else
            {
                /* 在要填的格子上按数字键解答题目 */
                if (nums.Contains(e.KeyCode))
                {
                    _currBtn.Text = numstr;
                }
                else if (e.KeyCode == Keys.Back)
                {
                    /* 按退格键，清空填写的数字 */
                    _currBtn.Text = "";
                }
            }
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        private void Sudoku4_FormClosing(object sender, FormClosingEventArgs e)
        {
            //关闭窗体显示主页的代码在HomePage类的按钮事件中，本事件用来阻止关闭窗体；什么情况下不关闭窗体？

        }

        #endregion

        #region 控件方法

        /// <summary>
        /// 响应81个按钮的点击事件
        /// </summary>
        private void Buttn_Click(object sender, EventArgs e)
        {
            /* 获取当前按钮 */
            _currBtn = sender as Button;

            if (_currBtn.Text == "")
            {
                /* 对于未填写的格子：设置 label 控件可用 */
                foreach (Label label in _labels)
                {
                    label.Enabled = true;
                }
            }
            else if (_currBtn.ForeColor == Color.FromArgb(0, 128, 0))
            {
                /* 对于已填写的格子：设置 label 控件可用，并设置背景色 */
                foreach (Label label in _labels)
                {
                    label.Enabled = true;
                }
            }
            else
            {
                /* 对于题目已知的格子：设置 label 控件不可用*/
                foreach (Label label in _labels)
                {
                    label.Enabled = false;
                }
            }

            /* 设置背景色：所有的同Text按钮背景色设为蓝色 */
            SetBtnBackColor();

            /* 设置标签背景色 */
            SetLabelBackColor();
        }

        /// <summary>
        /// 响应9个标签的点击事件
        /// </summary>
        private void Label_Click(object sender, EventArgs e)
        {
            _currLabel = sender as Label;
            if (_currLabel.Enabled)
            {
                /* 点击后，将标签上的数字填写到当前格子上 */
                _currBtn.Text = _currLabel.Text;
            }
        }

        /// <summary>
        /// 当按钮文本变化后触发
        /// </summary>
        private void Button_TextChanged(object sender, EventArgs e)
        {
            /* 在Button_Click时间和KeyDown事件中，设置题目已经填写的按钮文本不允许编辑 */
            if (!_currBtn.Text.Equals(""))
            {
                /* 设置按钮文本前景色为绿色 */
                _currBtn.ForeColor = Color.FromArgb(0, 128, 0);

                /* 设置按钮文本加粗 */
                _currBtn.Font = new Font("宋体", 14.25f, _currLabel.Font.Style | FontStyle.Bold);
            }
            else
            {
                /* 设置前景色为默认值 */
                _currBtn.ForeColor = SystemColors.ControlText;
            }

            /* 设置按钮的背景 */
            SetBtnBackColor();

            /* 设置 label 标签的背景色：如果已经有9个格子，背景变为白色 */
            SetLabelBackColor();
        }

        /// <summary>
        /// 清空
        /// </summary>
        private void ButtonClear_Click(object sender, EventArgs e)
        {
            foreach (Button btn in _buttons)
            {
                /* 清空已填写的内容，设置ForeColor属性为默认值， */
                if (btn.ForeColor != SystemColors.ControlText)
                {
                    btn.Text = "";
                    btn.ForeColor = SystemColors.ControlText;
                }
            }

            /* 设置背景色按钮 */
            SetBtnBackColor();
            SetLabelBackColor();
        }

        /// <summary>
        /// 收藏题目
        /// </summary>
        private void ButtonCollect_Click(object sender, EventArgs e)
        {
            //历史记录直接显示解答完的题目，收藏的显示题目，点击 题解 按钮才显示结果
            /* 收藏题目 */
            Collect();
        }

        /// <summary>
        /// 放弃解答本题目
        /// </summary>
        private void ButtonQuit_Click(object sender, EventArgs e)
        {
            /* 询问是否放弃 */
            DialogResult result = MessageBox.Show("确认要放弃解答本次题目吗？\n【提示】：已放弃的题目不会加入到历史记录中，如果想再次寻找到本题目可选择收藏。", "提示", MessageBoxButtons.YesNoCancel);
            if (result != DialogResult.Yes) { return; }

            /* 关闭窗体 */
            this.Close();
        }

        /// <summary>
        /// 暂停
        /// </summary>
        private void ButtonPause_Click(object sender, EventArgs e)
        {
            if (buttonPause.Text.Equals("暂停"))
            {
                /* 判断是否有已填写的格子，若有，存起来；否则，直接退出 */
                SetEditBtns();

                if (_editBtns.Count <= 0)
                {
                    DialogResult quitResult = MessageBox.Show("当前题目没有解答，是否直接放弃", "提示", MessageBoxButtons.YesNoCancel);

                    /* 如果确定放弃，就直接关闭；否则什么也不做 */
                    if (quitResult == DialogResult.Yes)
                    {
                        this.Close();
                    }

                    return;
                }

                DialogResult result = MessageBox.Show("是否暂停本题目，下次再继续？", "提示", MessageBoxButtons.YesNoCancel);

                /* 只有选择是，才执行暂停操作 */
                if (result != DialogResult.Yes) { return; }

                /* 执行SQL语句将题目暂停 */
                Pause();
            }
            else if (buttonPause.Text.Equals("继续"))
            {
                /* 执行SQL语句将题目从暂停表中删除 */
                Renew();
            }
        }

        /// <summary>
        /// 提交本题目
        /// </summary>
        private void ButtonSubmit_Click(object sender, EventArgs e)
        {
            /* 如果已提交，且备注没改变，直接返回 */
            if (buttonSubmit.Text.Equals("已提交"))
            {
                if (_isNoteChanged)
                {
                    /* 更新数据库表 */
                    Submit();
                }
                else
                {
                    return;
                }
            }

            /* 检查正确性*/
            /* 标志更新历史记录表是否成功 */
            bool isSuccess = false;

            /* 判断题目是否正确解答 */
            foreach (Button btn in _buttons)
            {
                /* 检查是否存在格子为空 */
                if (btn.Text == "")
                {
                    MessageBox.Show("存在未填写的格子，请重新确认后提交！", "提示");
                    return;
                }

                /* 检查是否解答正确 */
                int rowIndex = Convert.ToInt32(btn.Name.Substring(btn.Name.Length - 2, 1)) - 1;
                int columnIndex = Convert.ToInt32(btn.Name.Substring(btn.Name.Length - 1, 1)) - 1;
                if (!_source[rowIndex, columnIndex].Equals(int.Parse(btn.Text)))
                {
                    btn.ForeColor = Color.Red;
                    isSuccess = false;
                }
            }

            if (!isSuccess)
            {
                MessageBox.Show("题目解答错误，请重新确认后提交！", "提示");
                return;
            }
            else
            {
                MessageBox.Show("解答成功，棒棒哒~(｡≧3≦)ﾉ⌒☆", "提示");

                /* 将记录更新到数据库表，更新失败提示并返回 */
                if (!Submit())
                {
                    MessageBox.Show("将记录更新到数据库表失败！", "提示");
                    return;
                }

                /* 禁用“暂停”和“放弃”按钮 */
                buttonPause.Enabled = buttonQuit.Enabled = false;
            }
        }

        /// <summary>
        /// 再来一局：打开主页
        /// </summary>
        private void ButtonTryAgain_Click(object sender, EventArgs e)
        {
            /* 新打开一个相同难度的窗体 */
            Sudoku4 newSudoku = new Sudoku4(this._nullnum);
            newSudoku.Shown += (sender1, e1) => { this.Hide(); }; //如果是Close()，则数独界面关闭时，主界面已释放，不能再show()出来；
            newSudoku.FormClosed += (sender1, e1) => { this.Close(); }; //再来一局的窗体关闭时，这个窗体也关闭，显示原来的主页，直接Close()
            newSudoku.ShowDialog();
        }

        /// <summary>
        /// 退出程序
        /// </summary>
        private void ButtonExit_Click(object sender, EventArgs e)
        {
            /* 退出程序 */
            Application.Exit();
        }

        private void TextBoxNote_TextChanged(object sender, EventArgs e)
        {
            /* 标志 */
            _isNoteChanged = true;
            _note = textBoxNote.Text;
        }

        #endregion

        #region 内部方法

        /// <summary>
        /// 设置按钮的背景色
        /// </summary>
        private void SetBtnBackColor()
        {
            /* 如果是初始未填写的格子，则清空所有背景色 */
            if (_currBtn.Text.Equals(""))
            {
                foreach (Button btn in _buttons)
                {
                    btn.BackColor = SystemColors.ButtonHighlight;
                }
            }
            else
            {
                foreach (Button btn in _buttons)
                {
                    if (btn.Text.Equals(_currBtn.Text))
                    {
                        if (btn == _currBtn)
                        {
                            /* 当前按钮颜色是更深的蓝色 */
                            btn.BackColor = SystemColors.GradientActiveCaption;
                        }
                        else
                        {
                            /* 相同数字按钮的颜色是浅一点的蓝色 */
                            btn.BackColor = SystemColors.GradientInactiveCaption;
                        }
                    }
                    else
                    {
                        /* 不是同一数字的按钮背景为白色 */
                        btn.BackColor = SystemColors.ButtonHighlight;
                    }
                }
            }
        }

        /// <summary>
        /// 设置 label 标签的背景色
        /// </summary>
        private void SetLabelBackColor()
        {
            /* 循环所有按钮，找出界面上已有9个格子的数字，设置 label 标签为白色 */
            int[] count = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            foreach (Button btn in _buttons)
            {
                /* 若当前格子内容为空，不执行后续代码 */
                if (btn.Text.Equals("")) { continue; }

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

            /* 设置数字已全部填写的标签背景变为白色，否则背景为默认的灰 */
            foreach (Label label in _labels)
            {
                int index = int.Parse(label.Text) - 1;
                label.BackColor = count[index] == 9 ? SystemColors.ButtonHighlight : SystemColors.ControlLight;
            }

            //TODO：如果已经有9个，除非再删除一个数字，label标签不可用
        }

        /// <summary>
        /// 连接数据库
        /// </summary>
        //private bool ConnectSQL()
        //{
        //    bool isSuccess = false;
        //    List<string> question = new List<string>();
        //    List<string> result = new List<string>();
        //    try
        //    {
        //        _sqlConn.Open();
        //        if (_sqlConn.State == ConnectionState.Open)
        //        {
        //            SqlCommand sql = new SqlCommand();
        //            sql.Connection = _sqlConn;
        //            sql.CommandText = "select * from History";
        //            sql.CommandType = CommandType.Text;
        //            SqlDataReader dataReader = sql.ExecuteReader();
        //            if (!dataReader.HasRows)
        //            {
        //                //当数据库表没有数据时
        //                //MessageBox.Show("没有历史记录。", "提示");
        //            }
        //            while (dataReader.Read())
        //            {
        //                question.Add(dataReader["SudokuQuestion"].ToString());
        //                result.Add(dataReader["SudokuResult"].ToString());
        //            }
        //            isSuccess = true;
        //        }
        //        else
        //        {
        //            MessageBox.Show("数据库连接失败", "提示");
        //            isSuccess = false;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show("数据库连接取数失败：" + e.Message);
        //        isSuccess = false;
        //    }
        //    finally
        //    {
        //        _sqlConn.Close();
        //    }
        //    return isSuccess;
        //}

        /// <summary>
        /// 将解答成功的记录提交到历史记录
        /// </summary>
        //TODO：修改一下提交按钮和收藏按钮的前后逻辑/依赖，修改提交按钮的Click事件
        private bool Submit()
        {
            /* 标志更新历史记录表是否成功 */
            bool isSuccess = false;

            /* 判断是否已提交，如果已提交 */
            if (buttonSubmit.Text.Equals("已提交"))
            {
                if (_isNoteChanged)
                {
                    /* 更新数据库记录的备注 */
                    try
                    {
                        _sqlConn.Open();
                        //update History set Note = '{0}' where SudokuQuestion = '{1}'
                        if (_sqlConn.State == ConnectionState.Open)
                        {
                            /* 获取History表 */
                            SqlCommand sql = new SqlCommand();
                            sql.Connection = _sqlConn;
                            sql.CommandType = CommandType.Text;
                            sql.CommandText = "select * from History order by CreateTime desc";

                            DataTable dtHistory = new DataTable();

                            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql);
                            int rows = sqlDataAdapter.Fill(dtHistory);

                            if (rows <= 0)
                            {
                                MessageBox.Show("获取历史记录表数据失败！", msg.Hint);
                            }
                            //TODO：0915 9:13 写到这了


                            //SqlCommand sql = new SqlCommand();
                            //sql.Connection = _sqlConn;
                            //sql.CommandType = CommandType.Text;
                            sql.CommandText = string.Format("update History set Note = '{0}' where SudokuQuestion = '{1}'", textBoxNote.Text);
                            sql.ExecuteNonQuery();
                        }
                        else
                        {
                            MessageBox.Show(msg.OpenSqlConnFailed, msg.Error); //Content_CN：打开数据库连接失败！
                            isSuccess = false;
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(msg.UpdateNoteFailed + "：" + e.Message, msg.Error); //Content_CN：更新数据库记录的备注失败！
                        isSuccess = false;
                    }
                    finally
                    {
                        _sqlConn.Close();
                    }

                    return isSuccess;
                }
                else
                {
                    return true;
                }
            }

            /* 如果没有填写备注，备注字符串不加引号；否则，加引号 */
            string note = string.IsNullOrEmpty(textBoxNote.Text) ? "null" : "'" + textBoxNote.Text + "'";



            /* 判断是否已收藏 */
            int isCollect = buttonCollect.BackColor == SystemColors.ButtonHighlight ? 0 : 1;

            #region 旧方法：设置题解和题目字符串，以字符串形式存入到SQL表中

            //StringBuilder sourceQuestion = new StringBuilder();
            //StringBuilder sourceResult = new StringBuilder();
            //for (int i = 0; i < _sourceQuestion.GetLength(0); i++)
            //{
            //    sourceQuestion.Append("[ ");
            //    sourceResult.Append("[ ");
            //    for (int j = 0; j < _sourceQuestion.GetLength(1); j++)
            //    {
            //        sourceQuestion.Append(_sourceQuestion[i, j] + ", ");
            //        sourceResult.Append(_source[i, j] + ", ");
            //    }

            //    /* 移除每一行末尾的", " */
            //    sourceQuestion.Remove(sourceQuestion.Length - 2, 2);
            //    sourceResult.Remove(sourceResult.Length - 2, 2);

            //    sourceQuestion.Append(" ], ");
            //    sourceResult.Append(" ], ");
            //}

            ///* 移除字符串末尾的", " */
            //sourceQuestion.Remove(sourceQuestion.Length - 2, 2);
            //sourceResult.Remove(sourceResult.Length - 2, 2);

            #endregion

            /* 将二维数组转为json字符串，以json字符串形式存入SQL表中 */
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string sourceresult = javaScriptSerializer.Serialize(_source); //序列化
            string sourcequestion = javaScriptSerializer.Serialize(_sourceQuestion); //序列化
            //转成json字符串后，格式为1,2,3,...，从json只能转成int[]
            //int[] rejson = js.Deserialize<int[]>(jsonData); //反序列化

            /* 插入到SQL表中 */
            try
            {
                _sqlConn.Open();
                if (_sqlConn.State == ConnectionState.Open)
                {
                    SqlCommand sql = new SqlCommand();
                    sql.Connection = _sqlConn;
                    //sql.CommandText = string.Format("insert into History(id ,SudokuQuestion, SudokuResult, NullParam, IsCollected, CreateTime, Note) values(NEWID(),'{0}', '{1}', {2}, {3}, GETDATE(), {4})", sourceQuestion.ToString(), sourceResult.ToString(), _nullnum, isCollect, note);
                    sql.CommandText = string.Format("insert into History(id ,SudokuQuestion, SudokuResult, NullParam, IsCollected, CreateTime, Note) values(NEWID(),'{0}', '{1}', {2}, {3}, GETDATE(), {4})", sourcequestion, sourceresult, _nullnum, isCollect, note);
                    sql.CommandType = CommandType.Text;
                    int rows = sql.ExecuteNonQuery();
                    if (rows != 1)
                    {
                        MessageBox.Show("受影响行数不为1！", "提示");
                        isSuccess = false;
                    }
                    else
                    {
                        isSuccess = true;
                    }
                }
                else
                {
                    MessageBox.Show("数据库连接失败", "提示");
                    isSuccess = false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("插入表 History 出错：" + e.Message);
                isSuccess = false;
            }
            finally
            {
                _sqlConn.Close();
            }

            /* 更新按钮状态 */
            buttonSubmit.Text = "已提交";
            buttonSubmit.BackColor = SystemColors.ButtonShadow;

            return isSuccess;
        }

        /// <summary>
        /// 收藏题目
        /// </summary>
        private bool Collect()
        {
            /* 设置收藏是否成功 */
            bool isSuccess = false;

            /* 如果没填写备注，询问是否填写备注 */
            if (string.IsNullOrEmpty(textBoxNote.Text))
            {
                DialogResult result = MessageBox.Show("本题目当前没有填写备注，是否确认收藏不填写备注的题目？", "提示", MessageBoxButtons.YesNoCancel);
                /* 如果点击否或取消，先去填写备注；如果选择是，继续执行 */
                if (result != DialogResult.Yes)
                {
                    return false;
                }
            }

            /* 如果没有填写备注，备注字符串不加引号；否则，加引号 */
            string note = string.IsNullOrEmpty(textBoxNote.Text) ? "null" : "'" + textBoxNote.Text + "'";

            /* 将题目和题解二维数组转为json字符串，以json字符串形式存入SQL表中 */
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string sourceresult = javaScriptSerializer.Serialize(_source);
            string sourcequestion = javaScriptSerializer.Serialize(_sourceQuestion);

            /* 设置解答状态 */
            SetEditBtns();
            string sourcesolution = "";
            //收藏/关闭时，如果已提交，更新SudokuSolution字符串："Success!"
            if (buttonSubmit.Text.Equals("已提交")) { sourcesolution = "Success!"; }
            else
            {
                /* 设置解答状态哈希表，并转化为json字符串 */
                Hashtable htSolution = new Hashtable();
                foreach (Button editBtn in _editBtns)
                {
                    string row = editBtn.Name.Substring(editBtn.Name.Length - 2, 1);
                    string column = editBtn.Name.Substring(editBtn.Name.Length - 1, 1);
                    string key = "(" + row + "," + column + ")";
                    htSolution[key] = editBtn.Text;
                }
                sourcesolution = javaScriptSerializer.Serialize(htSolution);
            }

            /* 插入到 Collect 表中 */
            try
            {
                _sqlConn.Open();
                if (_sqlConn.State == ConnectionState.Open)
                {
                    SqlCommand sql = new SqlCommand();
                    sql.Connection = _sqlConn;
                    sql.CommandText = string.Format("insert into Collection(id ,SudokuQuestion, SudokuResult, NullParam, SudokuSolution, CollectTime, Note) values(NEWID(),'{0}', '{1}', {2}, '{3}', GETDATE(), {4})", sourcequestion, sourceresult, _nullnum, sourcesolution, note);
                    sql.CommandType = CommandType.Text;
                    int rows = sql.ExecuteNonQuery();
                    if (rows != 1)
                    {
                        MessageBox.Show("受影响行数不为1！受影响行数为" + rows + "！", "提示");
                        isSuccess = false;
                    }
                    else
                    {
                        isSuccess = true;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("插入表 Collect 出错：" + e.Message);
                isSuccess = false;
            }
            finally
            {
                _sqlConn.Close();
            }

            ///* 插入或更新 History 表 */
            //try
            //{
            //    _sqlConn.Open();
            //    if (_sqlConn.State == ConnectionState.Open)
            //    {
            //        /* 成功连接数据库后 */
            //        SqlCommand sql = new SqlCommand();
            //        sql.Connection = _sqlConn;

            //        /* 如果本题目没提交，则提交后再更新是否收藏字段，如果不能提交则不能收藏？ */
            //        bool isSubmit = buttonSubmit.BackColor == SystemColors.ButtonHighlight ? false : true;
            //        if (!isSubmit)
            //        {
            //            //本来的insert语句，IsCollected是1，不是变量
            //            ButtonSubmit_Click(null, null);
            //        }
            //        sql.CommandText = string.Format("update History set IsCollected = 1, Note = {0} where SudokuQuestion = '{1}'", note, sourcequestion);
            //        sql.CommandType = CommandType.Text;
            //        int rows = sql.ExecuteNonQuery();
            //        if (rows != 1)
            //        {
            //            MessageBox.Show("受影响行数不为1！受影响行数为" + rows + "！", "提示");
            //            isSuccess = false;
            //        }
            //        else
            //        {
            //            isSuccess = true;

            //            /* 更新收藏和提交按钮状态 */
            //            buttonCollect.Text = "已收藏";
            //            buttonCollect.BackColor = Color.Salmon;
            //            buttonSubmit.Text = "已提交";
            //            buttonSubmit.BackColor = SystemColors.ButtonShadow;
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("数据库连接失败", "提示");
            //        isSuccess = false;
            //    }
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show("操作表 Collect 或表 History 出错：" + e.Message);
            //    isSuccess = false;
            //}
            //finally
            //{
            //    _sqlConn.Close();
            //}

            /* 更新收藏按钮状态 */
            buttonCollect.Text = "已收藏";
            buttonCollect.BackColor = Color.Salmon;

            return isSuccess;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        private bool Pause()
        {
            /* 设置暂停是否成功 */
            bool isSuccess = false;

            /* 如果没有填写备注，备注字符串不加引号；否则，加引号 */
            string note = string.IsNullOrEmpty(textBoxNote.Text) ? "null" : "'" + textBoxNote.Text + "'";

            /* 将数独题目和数独题解数组转为json字符串，以json字符串形式存入SQL表中 */
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string sourceresult = javaScriptSerializer.Serialize(_source);
            string sourcequestion = javaScriptSerializer.Serialize(_sourceQuestion);

            /* 将解答状态哈希表转为json字符串 */
            Hashtable htSolution = new Hashtable();
            foreach (Button editBtn in _editBtns)
            {
                string row = editBtn.Name.Substring(editBtn.Name.Length - 2, 1);
                string column = editBtn.Name.Substring(editBtn.Name.Length - 1, 1);
                string key = "(" + row + "," + column + ")";
                htSolution[key] = editBtn.Text;
            }
            string sourcesolution = javaScriptSerializer.Serialize(htSolution);

            try
            {
                _sqlConn.Open();
                if (_sqlConn.State == ConnectionState.Open)
                {
                    /* 成功连接数据库后 */
                    SqlCommand sql = new SqlCommand();
                    sql.CommandText = string.Format("insert into Pause(id ,SudokuQuestion, SudokuResult, NullParam, SudokuSolution, PauseTime, Note) values(NEWID(),'{0}', '{1}', {2}, '{3}', GETDATE(), {4})", sourcequestion, sourceresult, _nullnum, sourcesolution, note);
                    sql.CommandType = CommandType.Text;
                    sql.Connection = _sqlConn;
                    int rows = sql.ExecuteNonQuery();
                    if (rows != 1)
                    {
                        MessageBox.Show("受影响行数不为1！受影响行数为" + rows + "！", "提示");
                        isSuccess = false;
                    }
                    else
                    {
                        isSuccess = true;

                        /* 更新暂停按钮状态 */
                        buttonPause.Text = "继续";
                        buttonPause.BackColor = Color.RoyalBlue;
                    }
                }
                else
                {
                    MessageBox.Show("数据库连接失败", "提示");
                    isSuccess = false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("插入表 Pause 出错：" + e.Message);
                isSuccess = false;
            }
            finally
            {
                _sqlConn.Close();
            }

            return isSuccess;
        }

        /// <summary>
        /// 继续
        /// </summary>
        private bool Renew()
        {
            /* 解答过程中暂停，继续；删除Pause表数据，更新按钮 */
            /* 设置暂停是否成功 */
            bool isSuccess = false;

            /* 设置题解字符串 */
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string sourceQuestion = javaScriptSerializer.Serialize(_sourceQuestion);

            try
            {
                _sqlConn.Open();
                if (_sqlConn.State == ConnectionState.Open)
                {
                    /* 成功连接数据库后 */
                    SqlCommand sql = new SqlCommand();
                    sql.Connection = _sqlConn;
                    sql.CommandType = CommandType.Text;
                    sql.CommandText = string.Format("delete from Pause where SudokuQuestion = '{0}'", sourceQuestion.ToString());
                    int rows = sql.ExecuteNonQuery();
                    if (rows != 1)
                    {
                        MessageBox.Show("受影响行数为" + rows + "，不等于1！", "提示");
                        isSuccess = false;
                    }
                    else
                    {
                        isSuccess = true;

                        /* 更新暂停按钮状态 */
                        buttonPause.Text = "暂停";
                        buttonPause.BackColor = SystemColors.ButtonHighlight;
                    }
                }
                else
                {
                    MessageBox.Show("数据库连接失败", "提示");
                    isSuccess = false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("删除表 Pause 出错：" + e.Message);
                isSuccess = false;
            }
            finally
            {
                _sqlConn.Close();
            }

            return isSuccess;
        }

        /// <summary>
        /// 取出已编辑的格子
        /// </summary>
        private void SetEditBtns()
        {
            /* 清空原来存起来的格子们 */
            _editBtns.Clear();

            /* 获取当前已编辑的格子 */
            foreach (Button btn in _buttons)
            {
                if (btn.ForeColor == Color.FromArgb(0, 128, 0))
                {
                    _editBtns.Add(btn);
                }
            }
        }

        #endregion
    }
}