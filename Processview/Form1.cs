
 
// A simple Processviewer written in C# by Andreas Leinz (c)2011 
// purpose: list,save or print all running processes on the system




using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Drawing.Printing;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Taskbar;
using Microsoft.WindowsAPICodePack.Shell;


namespace Processview
{
    // a glassform is a new win7 feature
    public partial class Form1 : GlassForm
    {
        

        public Form1()
        {
           
            InitializeComponent();
            
            button1.Font = new Font(new FontFamily("Courier new"), 10f, FontStyle.Regular);
            button2.Font = new Font(new FontFamily("Courier new"), 10f, FontStyle.Regular);
            button3.Font = new Font(new FontFamily("Courier new"), 10f, FontStyle.Regular);
            textBox1.Font = new Font(new FontFamily("Courier new"), 20f, FontStyle.Regular);
            button1.ForeColor = Color.White;
            button1.BackColor = Color.LightSkyBlue;

            button2.ForeColor = Color.White;
            button2.BackColor = Color.LightSkyBlue;

            button3.ForeColor = Color.White;
            button3.BackColor = Color.LightSkyBlue;

            textBox1.BackColor = Color.MidnightBlue;
            textBox1.ForeColor = Color.White;
        }













// list processes in textbox1 with progress in the taskbar, new win7 feature
        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            TaskbarManager tm = TaskbarManager.Instance;
            tm.SetProgressState(TaskbarProgressBarState.Normal);
            
            Process[] pp = Process.GetProcesses();
            var sorted = from p in pp orderby  p.Id select p;
            int i = 0;
            int anzahl=0;
             foreach (Process p in sorted)
                 anzahl++;

            foreach (Process p in sorted)
            {
                string format = string.Format("{0:0000}      {1}", p.Id, p.ProcessName);
                textBox1.AppendText(format+"\n");
                tm.SetProgressValue(i, anzahl);
                i++;
            }
            tm.SetProgressValue(0, anzahl);
            
        }

 //save processes from textBox1 to chosen textfile , now i use a commonsavedialog (win7)
        private void button2_Click(object sender, EventArgs e)
        {
            button1_Click(button2, e);
            String strFileName = "";
            CommonSaveFileDialog dialog = new CommonSaveFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("Textdateien",
               "txt files (*.txt)|*.txt|All files (*.*)|*.*"));
            dialog.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);

            dialog.Title = "Save a text file";
            dialog.DefaultExtension = "txt";
            if (dialog.ShowDialog() == CommonFileDialogResult.OK)
                strFileName = dialog.FileName;
            if (strFileName == String.Empty)
            {
                TaskDialog.Show("YOU MUST SUPPLY A FILENAME !");
                return;
            }

            StreamWriter writer = new StreamWriter(strFileName,true);
            Process[] pp = Process.GetProcesses();
            var sorted = from p in pp orderby p.Id select p;
            foreach (Process p in sorted)
            {
                string format = string.Format("{0:0000}      {1}", p.Id, p.ProcessName);
                writer.WriteLine(format+"\n");
            

            }
            writer.Close();
        }

// printer event handler routine
        private void printDoc_Printpage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            
           
            float leftMargin = e.MarginBounds.Left;
            float topMargin = e.MarginBounds.Top;
            float yPos = 0;
            Font printFont = new Font("Arial", 12);
            yPos = topMargin + printFont.GetHeight(e.Graphics);
            e.HasMorePages = false;
             Process[] pp = Process.GetProcesses();
            var sorted = from p in pp orderby p.Id select p;
            foreach (Process p in sorted)
            {
              string format=string.Format("{0:0000}      {1}",p.Id,p.ProcessName);
             e.Graphics.DrawString(format, printFont, Brushes.Black,
            leftMargin, yPos = yPos + printFont.GetHeight(e.Graphics), new StringFormat());
            

            }

      
            
           
        }

// do the print job

        private void button3_Click(object sender, EventArgs e)
        {
            button1_Click(button3, e);

            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += new PrintPageEventHandler(printDoc_Printpage);
            PrintDialog pd = new PrintDialog();
            pd.Document = printDoc;
            if (pd.ShowDialog() == DialogResult.OK)
            {
                
                printDoc.Print();
            }
        }


       





    }
}
