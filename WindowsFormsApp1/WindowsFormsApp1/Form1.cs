using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            using (StreamReader sr = new StreamReader("version.txt",Encoding.UTF8))
            {
                // Read the stream to a string, and write the string to the console.
                String line = sr.ReadToEnd();
                Console.WriteLine(line);
                label2.Text = line;
            }

            if (Directory.Exists(INI.ReadIni("Common", "DELTA")))
            {
                label4.Text = "√";
                label4.ForeColor = Color.Green;
            }
            else
            {
                label4.Text = "!";
                label4.ForeColor = Color.Red;
            }
            this.Text = "patch";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
        string pathpath = Environment.CurrentDirectory + "/delta/patch.exe";
        void CopyFiles(string filepath) {
            using (FileStream fsRead = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Read))
            {
                using (FileStream fsWrite = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    //设置进度条最大值
                    //SaveFileProgressBar.Maximum = (int)fsRead.Length;
                    //设置缓冲区大小
                    byte[] buffer = new byte[1024 * 1024 * 3];
                    while (true)
                    {
                        //读
                        int r = fsRead.Read(buffer, 0, buffer.Length);
                        if (r == 0)
                        {
                            break;
                        }
                        //写
                        fsWrite.Write(buffer, 0, r);
                        //进度条进度值与写入数据大小关联
                        
                    }
                    MessageBox.Show("保存成功！");
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();
            if(result == CommonFileDialogResult.Ok)
            {
                INI.WriteIni("Common", "OLD",dialog.FileName);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            (new Form2()).ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Create a process
            System.Diagnostics.Process process = new System.Diagnostics.Process();

            // Set the StartInfo of process
            process.StartInfo.FileName = Environment.CurrentDirectory + "/delta/patch.exe";

            // Start the process
            process.Start();
            process.WaitForExit();
        }
    }
}
