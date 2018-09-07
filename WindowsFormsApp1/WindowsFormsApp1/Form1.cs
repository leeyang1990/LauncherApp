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
using Ionic.Zip;
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

            CheckDeltaFile();
        }
        void CheckDeltaFile()
        {
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
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
        void CopyFiles(string src,string dst) {
            using (FileStream fsRead = new FileStream(src, FileMode.OpenOrCreate, FileAccess.Read))
            {
                using (FileStream fsWrite = new FileStream(dst, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    //设置进度条最大值
                    //SaveFileProgressBar.Maximum = (int)fsRead.Length;
                    progressBar1.Maximum = (int)fsRead.Length;
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
                        progressBar1.Invoke(new MethodInvoker(delegate
                        {
                            progressBar1.Value = (int)fsWrite.Length;
                            progressBar1.Update();
                        }));
                    }
                    //MessageBox.Show("保存成功！");
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                //INI.WriteIni("Common", "OLD",dialog.FileName);\
                
                CopyFiles(Environment.CurrentDirectory +"/tools/patch.exe", dialog.FileName+ "/patch.exe");
                UnZip(Environment.CurrentDirectory + "/delta.zip", dialog.FileName+"/delta");
            }
        }
        async void UnZip(string src,string dst) {
            await Task.Run(() =>
            {
                using (var zipFile = ZipFile.Read(src))
                {
                    zipFile.ExtractAll(dst, ExtractExistingFileAction.OverwriteSilently);
                    zipFile.SaveProgress += (o, args) =>
                    { //Fix this, not showing percentage of extraction.
                        var percentage = (int)(1.0d / args.TotalBytesToTransfer * args.BytesTransferred * 100.0d);
                        progressBar1.Invoke(new MethodInvoker(delegate
                        {
                            //progressBar1.Maximum = 100;
                            //progressBar1.Value = (int)((args.BytesTransferred * 100) / args.TotalBytesToTransfer); ;
                            progressBar1.Maximum = args.EntriesTotal;
                            progressBar1.Value = args.EntriesSaved + 1;
                            progressBar1.Update();
                        }));
                    };
                }
            });


        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            (new Form2()).ShowDialog();
            CheckDeltaFile();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Create a process
            Process process = new System.Diagnostics.Process();

            // Set the StartInfo of process
            process.StartInfo.FileName = Environment.CurrentDirectory + "/tools/patch.exe";

            // Start the process
            process.Start();
            process.WaitForExit();

            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
