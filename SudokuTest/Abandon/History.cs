using System.Windows.Forms;

namespace SudokuTest
{
    public partial class History : Form
    {
        public History()
        {
            InitializeComponent();

            /* 注册窗体事件 */
            this.Load += History_Load;
        }

        /// <summary>
        /// 加载事件
        /// </summary>
        private void History_Load(object sender, System.EventArgs e)
        {
            
        }
    }
}
