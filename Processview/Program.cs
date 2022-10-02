// A simple Processviewer written in C# by Andreas Leinz (c)2011 
// purpose: list,save or print all running processes on the system




using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack;
using Microsoft.WindowsAPICodePack.Taskbar;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;

namespace Processview
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!TaskbarManager.IsPlatformSupported)
            {
                MessageBox.Show("This Program requires to be run on Windows 7", "Program needs Windows 7", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Environment.Exit(0);
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
