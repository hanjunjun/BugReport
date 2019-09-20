using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using System.Threading;

namespace BugReport.Common
{
    public class Logger
    {
        private static readonly object _lock = new object();
        //队列元素
        static ConcurrentQueue<Tuple<string, string>> logQueue = new ConcurrentQueue<Tuple<string, string>>();

        static Task writeTask = default;
        //开始是无信号的
        static ManualResetEvent pause = new ManualResetEvent(false);

        static Logger()
        {
            //开一个长时间运行的task
            writeTask = new Task((obj) =>
            {
                while (true)
                {
                    pause.WaitOne();//等待信号到来
                    pause.Reset();//设置无信号
                    List<string[]> temp = new List<string[]>();
                    foreach (var logItem in logQueue)
                    {
                        string logPath = logItem.Item1;
                        string logMergeContent = string.Concat(logItem.Item2, Environment.NewLine);//, Environment.NewLine, ""
                        string[] logArr = temp.FirstOrDefault(d => d[0].Equals(logPath));//取出路径相同的记录
                        if (logArr != null)
                        {
                            //如果找到相同路径的记录，就在写入内容后面加上。
                            logArr[1] = string.Concat(logArr[1], logMergeContent);
                        }
                        else
                        {
                            //如果没找到相同路径的记录，加一个新的list
                            logArr = new string[] { logPath, logMergeContent };
                            temp.Add(logArr);
                        }
                        Tuple<string, string> val = default;
                        logQueue.TryDequeue(out val);//删除队列头的元素
                    }
                    foreach (string[] item in temp)//写入文件
                    {
                        WriteText(item[0], item[1]);
                    }

                }
            }
            , null
            , TaskCreationOptions.LongRunning);//意味着该任务将长时间运行，因此他不是在线程池中执行。
            writeTask.Start();
        }

        public static void WriteProfilerLog(Tuple<string,string> pathLevel, string logContent, bool isError = false, string errorTitle = "Errors：")
        {
            if (isError)
            {
                logContent = $"|Error|{pathLevel.Item2}|{logContent}";
            }
            else
            {
                logContent = $"|Debug|{pathLevel.Item2}|{logContent}";
            }

            WriteLog(pathLevel, logContent);
        }

        public static void WriteLog(Tuple<string,string> customDirectory, string infoData, string EventID = "")
        {
            string logPath = GetLogPath(customDirectory, string.Empty);
            string logContent = string.Concat(DateTime.Now, EventID == "" ? "" : $@" 【事件ID：{EventID}】 ", infoData);
            logQueue.Enqueue(new Tuple<string, string>(logPath, logContent));
            pause.Set();//设置有信号激活线程
        }

        /// <summary>
        /// 获取绝对路径
        /// </summary>
        /// <param name="customDirectory"></param>
        /// <param name="preFile"></param>
        /// <returns></returns>
        private static string GetLogPath(Tuple<string,string> customDirectory, string preFile)
        {
            string newFilePath = string.Empty;
            //string logDir = customDirectory == null || string.IsNullOrEmpty(customDirectory.Item1) || string.IsNullOrEmpty(customDirectory.Item2) ?
            //    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Log\\其他\\{customDirectory.Item1}\\{customDirectory.Item2}")
            //    : $"{customDirectory.Item1}\\Log\\{customDirectory.Item2}\\{customDirectory.Item3}";
            var logDir = $"{AppDomain.CurrentDomain.BaseDirectory}Log\\{customDirectory.Item1}";
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }
            string extension = ".log";
            string fileNameNotExt = string.Concat(preFile, DateTime.Now.ToString("yyyyMMdd"));
            string fileName = string.Concat(fileNameNotExt, extension);
            string fileNamePattern = string.Concat(fileNameNotExt, "(*)", extension);
            List<string> filePaths = Directory.GetFiles(logDir, fileNamePattern, SearchOption.TopDirectoryOnly).ToList();

            if (filePaths.Count > 0)
            {
                int fileMaxLen = filePaths.Max(d => d.Length);
                string lastFilePath = filePaths.Where(d => d.Length == fileMaxLen).OrderByDescending(d => d).FirstOrDefault();
                if (new FileInfo(lastFilePath).Length > 1 * 1024 * 1024 * 1024)
                {
                    string no = new Regex(@"(?is)(?<=\()(.*)(?=\))").Match(Path.GetFileName(lastFilePath)).Value;
                    int tempno = 0;
                    bool parse = int.TryParse(no, out tempno);
                    string formatno = string.Format("({0})", parse ? tempno + 1 : tempno);
                    string newFileName = string.Concat(fileNameNotExt, formatno, extension);
                    newFilePath = Path.Combine(logDir, newFileName);
                }
                else
                {
                    newFilePath = lastFilePath;
                }
            }
            else
            {
                string newFileName = string.Concat(fileNameNotExt, string.Format("({0})", 0), extension);
                newFilePath = Path.Combine(logDir, newFileName);
            }
            return newFilePath;
        }

        private static void WriteText(string logPath, string logContent)
        {
            lock (_lock)
            {
                try
                {
                    if (!Directory.Exists(Path.GetDirectoryName(logPath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(logPath));
                    }
                    if (!File.Exists(logPath))
                    {
                        File.CreateText(logPath).Close();
                    }
                    StreamWriter sw = File.AppendText(logPath);
                    sw.Write(logContent);
                    sw.Close();
                }
                catch (Exception ex)
                {

                }
                finally
                {

                }
            }
        }
    }
}
