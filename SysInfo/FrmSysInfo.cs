using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SysInfo
{
    public partial class FrmSysInfo : Form
    {
        public FrmSysInfo()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(() => RunCommand("systeminfo"))).Start();
        }

        public void RunCommand(string cmd)
        {
            var strInput = cmd;
            var p = new Process();
            //设置要启动的应用程序
            p.StartInfo.FileName = "cmd.exe";
            //是否使用操作系统shell启动
            p.StartInfo.UseShellExecute = false;
            // 接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardInput = true;
            //输出信息
            p.StartInfo.RedirectStandardOutput = true;
            // 输出错误
            p.StartInfo.RedirectStandardError = true;
            //不显示程序窗口
            p.StartInfo.CreateNoWindow = true;
            //启动程序
            p.Start();

            //向cmd窗口发送输入信息
            p.StandardInput.WriteLine(strInput + "&exit");

            p.StandardInput.AutoFlush = true;

            //获取输出信息
            string strOuput = p.StandardOutput.ReadToEnd();
            var __ = strOuput.Split(new [] { Environment.NewLine }, StringSplitOptions.None).ToList();
            __.RemoveRange(0, 5);
            var sb = new StringBuilder();
            foreach(var s in __) { sb.AppendLine(s); }
            strOuput = sb.ToString();
            BeginInvoke(new Action(() => txtSysInfo.Text = strOuput));
            
            //File.AppendAllText(@"c:\install-log.txt", $"[{DateTime.Now.ToString("yyyyMMdd hh:mm:ss fff")}]" + strOuput);

            //等待程序执行完退出进程
            p.WaitForExit();
            p.Close();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtSysInfo.Text, TextDataFormat.Text);
        }
    }
}
