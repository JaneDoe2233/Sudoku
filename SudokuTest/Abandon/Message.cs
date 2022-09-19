using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuTest
{
    /// <summary>
    /// 用于存放程序运行过程中的提示信息
    /// </summary>
    public class Message
    {
        /// <summary>
        /// "提示"
        /// </summary>
        public string Hint = "提示";

        /// <summary>
        /// "错误"
        /// </summary>
        public string Error = "错误";

        /// <summary>
        /// "打开数据库连接失败！"
        /// </summary>
        public string OpenSqlConnFailed = "打开数据库连接失败！";

        /// <summary>
        /// "更新数据库记录的备注失败！"
        /// </summary>
        public string UpdateNoteFailed = "更新数据库记录的备注失败！";

        //枚举不方便
        //public enum MessageContent
        //{
        //    Hint = 1,
        //    Error = 2,
        //    test = 3
        //}
    }
}