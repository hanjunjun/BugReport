using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DotNetTest
{
    class Program
    {
        [DllImport("BugReportLib.dll", EntryPoint = "SetGlobalExceptionFilter", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public extern static void SetGlobalExceptionFilter(int timeTick, string productName);
        static void Main(string[] args)
        {
            SetGlobalExceptionFilter(15, "C#测试程序");
            Console.WriteLine("加载BugReportLib.dll成功！");
            Console.ReadLine();
        }
    }
}
