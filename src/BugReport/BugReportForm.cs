using BugReport.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BugReport
{
    public partial class BugReportForm : Form
    {
        #region 成员和ctor
        private int _sendReport;//发送错误报告s
        private int _restart;//重启快捷键r
        private int _ok;//确定按钮o
        //注意1和2空格在C++代码中已经处理过
        private readonly string[] args = null;//1.当前程序运行目录，2.错误模块路径，3.错误偏移量，4.倒计时退出bugreport最大99超过99自动变99，5.产品名称
        private int tick = 0;//倒计时
        private string _productName = "BugReport";//产品名称
        private readonly string _eventId = DateTime.Now.ToString("yyyyMMddHHmmssfff");

        public BugReportForm()
        {
            InitializeComponent();
            this.textBox1.WaterText = "这个问题是如何出现的？";
        }

        public BugReportForm(string[] args)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            SDK.SetFormTop(this.Handle);
            try
            {
                //Thread.Sleep(15000);//延迟加载方便vs附加进程调试
                Logger.WriteProfilerLog(new Tuple<string, string>("CrashLog", _eventId), $"");
                Logger.WriteProfilerLog(new Tuple<string,string>("CrashLog", _eventId), $"args={string.Join(",", args)}");
                this.args = args;
                InitializeComponent();
                textBox1.WaterText = "这个问题是如何出现的？";
                errorModule.Text = args[1];
                excpAddress.Text = args[2];
                _productName = args[4];
                labelTitle1.Text = $"{_productName}遇到错误，给您带来不便，我们深表歉意。";
                labelInfo.Text = $"我们已经产生了相关错误报告（报告中不包含您的任何隐私信息），希望您发送此报告给我们以帮助改善{_productName}的质量。";
                int value = 0;
                if (!int.TryParse(args[3], out value))
                {
                    value = 20;
                }
                tick = value;
                var timeTickTask = new Task((obj) =>
                {
                    while (true)
                    {
                        try
                        {
                            if (tick < 0)
                            {
                                SetLabelText(timeTick, $"");
                                //点击确定
                                Button1_Click(null, null);
                                return;
                            }
                            SetLabelText(timeTick, $"{tick}S");
                        }
                        catch
                        {

                        }
                        finally
                        {
                            Thread.Sleep(998);
                            tick--;
                        }
                    }
                }
            , null
            , TaskCreationOptions.LongRunning);//意味着该任务将长时间运行，因此他不是在线程池中执行。
                timeTickTask.Start();
            }
            catch
            {
                var list = args?.ToList();
                if (list != null)
                {
                    var msg = string.Join(", ", list);
                    MessageBox.Show(this, $"BugReport初始化异常，入参（{list.Count}个）：{msg}！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    MessageBox.Show(this, $"BugReport初始化异常，入参缺失！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                return;
            }
        }
        #endregion

        #region 窗体加载事件、注册快捷键、提交按钮、窗体正在关闭事件
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            HotKey hotkey = new HotKey(this.Handle);
            _sendReport = hotkey.RegisterHotkey(System.Windows.Forms.Keys.S, HotKey.KeyFlags.MOD_ALT);
            _restart = hotkey.RegisterHotkey(System.Windows.Forms.Keys.R, HotKey.KeyFlags.MOD_ALT);
            _ok = hotkey.RegisterHotkey(System.Windows.Forms.Keys.O, HotKey.KeyFlags.MOD_ALT);
            hotkey.OnHotkey += new HotKey.HotkeyEventHandler(Hotkey);
        }

        /// <summary>
        /// 注册快捷键
        /// </summary>
        /// <param name="hotkeyId"></param>
        public void Hotkey(int hotkeyId)
        {
            if (hotkeyId == _sendReport)
            {
                if (checkBox1.Checked)
                {
                    checkBox1.Checked = false;
                }
                else
                {
                    checkBox1.Checked = true;
                }
            }
            else if (hotkeyId == _restart)
            {
                if (checkBox2.Checked)
                {
                    checkBox2.Checked = false;
                }
                else
                {
                    checkBox2.Checked = true;
                }
            }
            else if (hotkeyId == _ok)
            {
                button1.PerformClick();
            }
        }

        /// <summary>
        /// 提交按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                SetButtonDisabled(button1, false);
                //程序崩溃生成转储文件
                if (args == null|| args.Length==0)
                {
                    MessageBox.Show(this, $"args=null不允许操作！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                var mainPath = args[0];
                if (!File.Exists(mainPath))
                {
                    MessageBox.Show(this, $"{mainPath} 文件不存在！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                var programeName = Path.GetFileNameWithoutExtension(mainPath);
                var path = Application.StartupPath + $"\\CrashDumps\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path += $"{DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss")}{programeName}.dmp";
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.Write))
                {

                    MiniDump.Write(fs.SafeFileHandle, MiniDump.Option.WithFullMemory, programeName);
                }
                Logger.WriteProfilerLog(new Tuple<string, string>("CrashLog", _eventId), $"创建Crash文件成功！{path}");
                //发送报告，并上传dmp文件到服务器
                if (checkBox1.Checked)
                {
                    Logger.WriteProfilerLog(new Tuple<string, string>("CrashLog", _eventId), $"上传Crash文件到服务器...");
                }
                //重启宿主
                if (checkBox2.Checked)
                {
                    Logger.WriteProfilerLog(new Tuple<string, string>("CrashLog", _eventId), $"杀进程：{programeName},重启宿主：{mainPath}");
                    //杀掉进程
                    var rtKill = SystemUtility.KillProcess(programeName);
                    Process pr = new Process();
                    pr.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    pr.StartInfo.FileName = mainPath;//重启宿主
                    pr.Start();
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"提交异常：{ex.Message}", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                SetButtonDisabled(button1, true);
            }
        }

        /// <summary>
        /// 窗体正在关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BugReportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
        #endregion

        #region 多线程操作UI封装-设置按钮文本、设置按钮启用状态、设置label文本
        /// <summary>
        /// 设置按钮文本
        /// </summary>
        /// <param name="bt"></param>
        /// <param name="text"></param>
        private void SetButtonText(Button bt, string text)
        {
            if (bt.InvokeRequired)
            {
                bt.Invoke(new EventHandler(delegate
                {
                    bt.Text = text;
                }), null);
            }
            else
            {
                bt.Text = text;
            }
        }

        /// <summary>
        /// 设置按钮启用状态
        /// </summary>
        /// <param name="bt"></param>
        /// <param name="status"></param>
        private void SetButtonDisabled(Button bt, bool status)
        {
            if (bt.InvokeRequired)
            {
                bt.Invoke(new EventHandler(delegate
                {
                    bt.Enabled = status;
                }), null);
            }
            else
            {
                bt.Enabled = status;
            }
        }

        /// <summary>
        /// 设置label文本
        /// </summary>
        /// <param name="bt"></param>
        /// <param name="text"></param>
        private void SetLabelText(Label lb, string text)
        {
            if (lb.InvokeRequired)
            {
                lb.Invoke(new EventHandler(delegate
                {
                    lb.Text = text;
                }), null);
            }
            else
            {
                lb.Text = text;
            }
        }
        #endregion
    }
}
