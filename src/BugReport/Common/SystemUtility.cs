using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BugReport.Common
{
    public class SystemUtility
    {
        private static string AppRunAtBoot = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        /// <summary>
        /// 设置开机自启
        /// </summary>
        /// <param name="autoRun"></param>
        /// <param name="appName"></param>
        /// <param name="appPath"></param>
        public static void SetAutoRun(bool autoRun, string appName, string appPath)
        {
            RegistryKey key = Registry.LocalMachine;
            RegistryKey software = key.OpenSubKey(AppRunAtBoot, true);
            if (autoRun)
            {
                software.SetValue(appName, appPath);
            }
            else
            {
                if (IsRegistryPropExist(Registry.LocalMachine, AppRunAtBoot, appName))
                {
                    software.DeleteValue(appName);
                }
            }
            key.Close();
        }

        /// <summary>
        /// 注册表键是否已存在
        /// </summary>
        /// <param name="root"></param>
        /// <param name="subkey"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsRegistryPropExist(RegistryKey root, string subkey, string name)
        {
            bool _exit = false;
            string[] subkeyNames;
            RegistryKey myKey = root.OpenSubKey(subkey, true);
            //subkeyNames = myKey.GetSubKeyNames();
            subkeyNames = myKey.GetValueNames();
            foreach (string keyName in subkeyNames)
            {
                if (keyName == name)
                {
                    _exit = true;
                    return _exit;
                }
            }
            return _exit;
        }

        /// <summary>
        /// 关闭进程名含某某的进程
        /// 存在进程则删除返回true，不存在直接删除返回true
        /// </summary>
        /// <param name="processName">进程名</param>
        public static Tuple<bool, string> KillProcess(string processName)
        {
            try
            {
                processName = processName.Replace(".exe", "").Replace(".EXE", "");
                var i = 0;
                Process[] myproc = Process.GetProcesses();
                foreach (Process item in myproc)
                {
                    if (item.ProcessName.Equals(processName))
                    {
                        i++;
                        item.Kill();
                    }
                }
                return new Tuple<bool, string>(true, $"{(i > 0 ? $"当前系统存在进程【{processName}】，并且关闭成功{i}个！" : $"当前系统不存在进程【{processName}】！")}");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, ex.ToString());
            }
        }

        public static bool IsExitProcess(string processName)
        {
            bool flag = false;
            try
            {
                processName = processName.Replace(".exe", "").Replace(".EXE", "");
                var i = 0;
                Process[] myproc = Process.GetProcesses();
                foreach (Process item in myproc)
                {
                    if (item.ProcessName.Equals(processName))
                    {
                        flag = true;
                        return flag;
                    }
                }
                return flag;
            }
            catch (Exception ex)
            {
                return flag;
            }
        }

        /// <summary>
        /// 根据进程名称获取handleid
        /// </summary>
        /// <param name="processName">示例：请填写qq 不要填qq.exe</param>
        /// <returns></returns>
        public static IntPtr GetHandleIdInProcess(string processName)
        {
            try
            {
                processName = processName.Replace(".exe", "").Replace(".EXE", "");
                var i = 0;
                Process[] myproc = Process.GetProcesses();
                foreach (Process item in myproc)
                {
                    if (item.ProcessName.Equals(processName))
                    {
                        return item.Handle;
                    }
                }
                return IntPtr.Zero;
            }
            catch (Exception ex)
            {
                return IntPtr.Zero;
            }
        }

        //强制关闭最近打开的某个进程

        public static void KillRecentProcess(string processName)
        {
            Process[] Proc = Process.GetProcessesByName(processName);
            DateTime startTime = new DateTime();
            int m, killId = 0;
            for (m = 0; m < Proc.Length; m++)
            {
                if (startTime < Proc[m].StartTime)
                {
                    startTime = Proc[m].StartTime;
                    killId = m;
                }
            }
            if (Proc[killId].HasExited == false)
            {
                Proc[killId].Kill();
            }

        }

        /// <summary> 
        /// 显示已运行的程序。 
        /// </summary> 
        //public static void HandleRunningInstance(string formName)
        //{
        //    //MessageBox.Show("ID:"+instance.Id .ToString()+"--句柄"+instance.MainWindowHandle.ToString() + "--正常窗口" + WS_SHOWNORMAL + "--" + ShowWindowAsync(instance.MainWindowHandle, WS_SHOWNORMAL) + "--" + SetForegroundWindow(instance.MainWindowHandle));
        //    var handle = SDK.FindWindow(null, formName);
        //    var bbo = SDK.ShowWindow(handle, 1); //显示窗体
        //    SDK.SwitchToThisWindow(handle, true); //切换到窗体
        //    //MessageBox.Show($"handle={handle},showwindow={bbo}");
        //    //SDK.ShowWindowAsync(instance.MainWindowHandle, 1); //显示，可以注释掉 
        //    //SDK.SetForegroundWindow(handle);            //放到前端 
        //}
    }
}
