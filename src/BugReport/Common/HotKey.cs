using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BugReport.Common
{
    public class HotKey : IMessageFilter
    {
        public HotKey(IntPtr hWnd)
        {
            this.hWnd = hWnd;
            Application.AddMessageFilter(this);
        }
        public delegate void HotkeyEventHandler(int HotKeyID);
        Hashtable keyIDs = new Hashtable();
        IntPtr hWnd;
        public event HotkeyEventHandler OnHotkey;
        public enum KeyFlags
        {
            MOD_ALT = 0x1,
            MOD_CONTROL = 0x2,
            MOD_SHIFT = 0x4,
            MOD_WIN = 0x8
        }
        [DllImport("user32.dll")]
        public static extern uint RegisterHotKey(IntPtr hWnd, uint id, uint fsModifiers, uint vk);
        [DllImport("user32.dll")]
        public static extern uint UnregisterHotKey(IntPtr hWnd, uint id);
        [DllImport("kernel32.dll")]
        public static extern uint GlobalAddAtom(string lpString);
        [DllImport("kernel32.dll")]
        public static extern uint GlobalDeleteAtom(uint nAtom);

        public int RegisterHotkey(Keys Key, KeyFlags keyflags)
        {
            uint hotkeyid = GlobalAddAtom(Guid.NewGuid().ToString());
            RegisterHotKey(hWnd, hotkeyid, (uint)keyflags, (uint)Key);
            keyIDs.Add(hotkeyid, hotkeyid);
            return (int)hotkeyid;
        }
        public void UnregisterHotkeys()
        {
            Application.RemoveMessageFilter(this);
            foreach (uint key in keyIDs.Values)
            {
                UnregisterHotKey(hWnd, key);
                GlobalDeleteAtom(key);
            }
        }
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == 0x312)
            {
                if (OnHotkey != null)
                {
                    foreach (uint key in keyIDs.Values)
                    {
                        if ((uint)m.WParam == key)
                        {
                            OnHotkey((int)m.WParam);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
