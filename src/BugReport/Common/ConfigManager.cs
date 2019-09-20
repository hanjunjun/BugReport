using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BugReport.Common
{
    public class ConfigManager
    {
        Configuration config = null;
        public ConfigManager()
        {
            config = ConfigurationManager.OpenExeConfiguration(
            ConfigurationUserLevel.None);
        }

        /// <summary>
        /// //添加键值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddAppSetting(string key, string value)
        {
            config.AppSettings.Settings.Add(key, value);
            config.Save();
        }

        /// <summary>
        /// //修改键值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SaveAppSetting(string key, string value)
        {
            try
            {
                config.AppSettings.Settings.Remove(key);
                config.AppSettings.Settings.Add(key, value);

                config.Save();
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
            }
        }

        /// <summary>
        /// //获得键值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetAppSetting(string key)
        {
            return config.AppSettings.Settings[key].Value.Trim();
        }

        /// <summary>
        /// //移除键值
        /// </summary>
        /// <param name="key"></param>
        public void DelAppSetting(string key)
        {
            config.AppSettings.Settings.Remove(key);
            config.Save();
        }

        public ArrayList GetXmlElements(string strElem)
        {
            ArrayList list = new ArrayList();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(System.Windows.Forms.Application.ExecutablePath + ".config");
            XmlNodeList listNode = xmlDoc.SelectNodes(strElem);
            foreach (XmlElement el in listNode)
            {
                list.Add(el.InnerText);
            }
            return list;
        }
    }
}
