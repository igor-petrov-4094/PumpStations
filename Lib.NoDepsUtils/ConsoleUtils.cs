using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Lib.NoDepsUtils
{
    public class ConsoleUtils
    {
        const int MF_BYCOMMAND = 0x00000000;
        const int SC_CLOSE = 0xF060;

        Action cancelAction;

        [DllImport("user32.dll")]
        static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(System.IntPtr hWnd, int cmdShow);

        public static void disableCloseButtonOnConsoleWindow()
        {
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_CLOSE, MF_BYCOMMAND);
        }

        public void setCancelEvent(Action _cancelAction)
        {
            cancelAction = _cancelAction;
            Console.CancelKeyPress += console_CancelKeyPress;
        }

        private void console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            cancelAction.Invoke();
        }

        public static void maximizeWindow()
        {
            Process p = Process.GetCurrentProcess();
            ShowWindow(p.MainWindowHandle, 3); //SW_MAXIMIZE = 3
        }
    }
}
