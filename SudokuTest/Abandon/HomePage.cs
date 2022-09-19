using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace SudokuTest
{
    /// <summary>
    /// 首页
    /// </summary>
    public partial class HomePage : Form
    {
        // todo：等到实现收藏、历史、暂停、设置（、美化界面）等功能后，再改为先选难度再显示数独；现在先直接生成数独

        #region 变量

        /// <summary>
        /// 用于存放填的格子数
        /// </summary>
        //private int _nullnum;

        //TODO：如果是移植到其他设备上，询问权限，获取数据库信息，新建数独数据库
        /// <summary>
        /// 用于存放数据库连接
        /// </summary>
        private SqlConnection _sqlConn = new SqlConnection("server = . ; database = SudokuTest; uid = sa; pwd = lt123$");

        /// <summary>
        /// 用于存放暂停数据
        /// </summary>
        private DataTable _dtPause = new DataTable();

        /// <summary>
        /// 数独题目
        /// </summary>
        private string[,] Sudoku = new string[9, 9];

        #endregion

        #region 窗体方法

        /// <summary>
        /// 构造函数
        /// </summary>
        public HomePage()
        {
            InitializeComponent();

            /* 注册窗体事件 */
            this.Load += HomePage_Load;
        }

        /// <summary>
        /// 加载窗体
        /// </summary>
        private void HomePage_Load(object sender, EventArgs e)
        {
            /* 设置作为按钮和标签父容器的 TextBox 不可用 */
            textBoxMain.Enabled = false;
            textBoxMain.BackColor = SystemColors.ButtonHighlight;

            /* 注册控件事件 */
            buttonEasy.Click += ButtonLevel_Click;
            buttonMiddle.Click += ButtonLevel_Click;
            buttonDifficult.Click += ButtonLevel_Click;
            buttonHistory.Click += ButtonHistory_Click;
            buttonSetting.Click += ButtonSetting_Click;
            buttonCollect.Click += ButtonCollect_Click;
            buttonRenew.Click += ButtonRenew_Click;

            /* 根据是否有已暂停的数独，显示或隐藏“继续游戏”按钮 */
            try
            {
                _sqlConn.Open();
                if (_sqlConn.State == ConnectionState.Open)
                {
                    SqlCommand sql = new SqlCommand();
                    sql.Connection = _sqlConn;
                    sql.CommandType = CommandType.Text;
                    sql.CommandText = "select * from Pause order by PauseTime desc";
                    #region 注释
                    //ExecuteNonQuery()执行select、insert、update语句时，返回受影响的行数；
                    //如果是select语句，不论查询结果有没有值都返回-1；所以不用这种方法
                    //int rows = sql.ExecuteNonQuery();
                    #endregion

                    /* 将暂停表的数据存到DataTable中，并返回select查询结果的行数 */
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql);
                    int rows = sqlDataAdapter.Fill(_dtPause);
                    if (rows > 0)
                    {
                        buttonRenew.Visible = true;
                    }
                    else
                    {
                        buttonRenew.Visible = false;
                    }
                }
                else
                {
                    MessageBox.Show("数据库连接失败", "提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取表 Pause 数据出错：" + ex.Message);
            }
            finally
            {
                _sqlConn.Close();
            }
        }

        #endregion

        #region 控件方法

        /// <summary>
        /// 选难度的事件
        /// </summary>
        private void ButtonLevel_Click(object sender, EventArgs e)
        {
            Button btnLevel = sender as Button;

            /* 设置_nullnum的值 */
            int nullNum = 0;
            if (btnLevel == buttonEasy)
            {
                nullNum = (int)Difficulty.Easy;
            }
            else if (btnLevel == buttonMiddle)
            {
                nullNum = (int)Difficulty.Medium;
            }
            else if (btnLevel == buttonDifficult)
            {
                nullNum = (int)Difficulty.Difficult;
            }

            /* 打开新的窗口，并关闭当前窗口 */
            SudokuMain sudoku = new SudokuMain();
            sudoku.NullNum = nullNum;
            sudoku.Shown += (sender1, e1) => { this.Hide(); }; //如果是Close()，则数独界面关闭时，主界面已释放，不能再show()出来
            sudoku.FormClosed += (sender1, e1) => { this.Show(); };
            sudoku.ShowDialog();
        }

        //TODO：显示题解
        /// <summary>
        /// 显示已成功解答出的题目合集
        /// </summary>
        private void ButtonHistory_Click(object sender, EventArgs e)
        {
            History history = new History();
            history.Show();
        }

        //TODO：显示题目
        /// <summary>
        /// 显示已收藏的题目合集
        /// </summary>
        private void ButtonCollect_Click(object sender, EventArgs e)
        {

        }

        //TODO ：背景图片、前景色；修改难度（根据Difficulty类）；声音
        /// <summary>
        /// 设置按钮
        /// </summary>
        private void ButtonSetting_Click(object sender, EventArgs e)
        {
            //
        }

        /// <summary>
        /// 继续游戏
        /// </summary>
        private void ButtonRenew_Click(object sender, EventArgs e)
        {
            string sudokuQuestion = "";
            string sudokuResult = "";
            string sudokuSolution = "";
            string pauseTime = "";
            string note = "";
            int nullParam = 0;

            /* 获取最近暂停的一条数独记录，并从暂停表中删除本记录 */
            try
            {
                _sqlConn.Open();
                if (_sqlConn.State != ConnectionState.Open)
                {
                    MessageBox.Show("数据库连接失败", "提示");
                }

                #region 注释

                ////SqlCommand sql = new SqlCommand();
                ////sql.Connection = _sqlConn;
                ////sql.CommandType = CommandType.Text;
                ////sql.CommandText = "select top 1 * from Pause order by PauseTime desc";
                ////等价于
                //SqlCommand sql = new SqlCommand("select top 1 * from Pause order by PauseTime desc");
                //sql.Connection = _sqlConn;

                ///* 1.SqlDataReader 读取过程中必须一直保持跟数据库连接，只能一行一行读取，只读访问；高效；适合数据量小；
                //   2.SqlDataAdapter 可一次性读取一个表，然后填充到DataSet中，然后就可以断开跟数据库的连接，在本地做了更改后，再连上数据库进行更新；要求资源更多，功能更强大；适合数据量大； */
                ////SqlDataReader sqlDataReader = sql.ExecuteReader();
                ////if (sqlDataReader.HasRows)
                ////{
                ////    //每次只读一行，不好写；还是用 SqlDataAdapter 吧
                ////}
                ////else
                ////{
                ////    MessageBox.Show("没有获取到 Pause 表中的数据", "提示");
                ////    return;
                ////}
                //DataTable dtPause = new DataTable();
                //SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql);

                #endregion

                /* 获取最近暂停的一条数独记录 */
                SqlCommand sql = new SqlCommand("select top 1 * from Pause order by PauseTime desc");
                sql.Connection = _sqlConn;
                DataTable dtPause = new DataTable();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql);
                //int rows= sqlDataAdapter.Fill(dsPause); //可以添加到DataSet中，也可以添加到DataTable中
                int rows = sqlDataAdapter.Fill(dtPause); //返回已添加到DataSet表中的行

                if (rows <= 0)
                {
                    MessageBox.Show("没有将数据填充到DataTable中!", "提示");
                    _sqlConn.Close();
                    return;
                }
                else if (rows > 1)
                {
                    MessageBox.Show("获取的数据大于一行!", "提示");
                    _sqlConn.Close();
                    return;
                }
                else if (rows == 1)
                {
                    sudokuQuestion = dtPause.Rows[0]["SudokuQuestion"].ToString();
                    sudokuResult = dtPause.Rows[0]["SudokuResult"].ToString();
                    sudokuSolution = dtPause.Rows[0]["SudokuSolution"].ToString();
                    pauseTime = dtPause.Rows[0]["PauseTime"].ToString();
                    note = dtPause.Rows[0]["Note"].ToString();
                    nullParam = Convert.ToInt32(dtPause.Rows[0]["NullParam"]);

                    /* 从暂停表中删除本条数据 */
                    try
                    {
                        SqlCommand sqlDelete = new SqlCommand(string.Format("delete from Pause where SudokuQuestion = '{0}'", sudokuQuestion));
                        sqlDelete.Connection = _sqlConn;
                        int rowsDelete = sqlDelete.ExecuteNonQuery();
                        if (rowsDelete != 1)
                        {
                            MessageBox.Show("受影响行数为" + rowsDelete + "，不等于1！", "提示");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("删除已暂停的 Pause 表记录出错：" + ex.Message);
                    }

                    finally
                    {
                        _sqlConn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取表 Pause 数据出错：" + ex.Message, "提示");
            }
            finally
            {
                _sqlConn.Close();
            }

            #region 将json字符串转换为int[,]和Hashtable

            /* 如果字符串为空，提示后返回 */
            if (string.IsNullOrEmpty(sudokuQuestion))
            {
                MessageBox.Show("没有获取到 Pause 表的题目： SudokuQuestion 字段", "提示");
                return;
            }
            if (string.IsNullOrEmpty(sudokuResult))
            {
                MessageBox.Show("没有获取到 Pause 表的题解： SudokuResult 字段", "提示");
                return;
            }
            if (string.IsNullOrEmpty(sudokuSolution))
            {
                MessageBox.Show("没有获取到 Pause 表的解答情况： SudokuSolution 字段", "提示");
                return;
            }

            /* 数独题目数独题解解答状态 json 字符串反序列化为一维数组和 Hastable */
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            int[] reQuestion = javaScriptSerializer.Deserialize<int[]>(sudokuQuestion);
            int[] reResult = javaScriptSerializer.Deserialize<int[]>(sudokuResult);
            Hashtable htSolution = javaScriptSerializer.Deserialize<Hashtable>(sudokuSolution);

            /* 一维数组转二维数组 */
            int[,] source = new int[9, 9];
            int[,] sourceQuestion = new int[9, 9];
            for (int i = 0; i < source.GetLength(0); i++)
            {
                for (int j = 0; j < source.GetLength(1); j++)
                {
                    source[i, j] = reResult[i + j];
                    sourceQuestion[i, j] = reQuestion[i * 9 + j];
                }
            }

            #endregion

            Sudoku4 sd = new Sudoku4(source, sourceQuestion, htSolution, nullParam, note);
            sd.Show();
        }

        #endregion
    }
}