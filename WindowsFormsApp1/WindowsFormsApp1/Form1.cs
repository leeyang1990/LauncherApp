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
            using (StreamReader sr = new StreamReader("version.txt", Encoding.UTF8))
            {
                // Read the stream to a string, and write the string to the console.
                String line = sr.ReadToEnd();
                Console.WriteLine(line);


                label2.Text = line;
            }
            label5.Text = "";
            CheckDeltaFile();
        }
        void CheckDeltaFile()
        {
            Console.WriteLine(Path.Combine(Environment.CurrentDirectory, INI.ReadIni("Common", "DELTA") + ".zip"));
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, INI.ReadIni("Common", "DELTA")+ ".zip")))
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
                UnZip(Path.Combine(Environment.CurrentDirectory , "delta.zip"),Path.Combine( dialog.FileName,"delta"));

                {
                    Process process = new Process();
                    process.StartInfo.FileName = Path.Combine(dialog.FileName, "patch.exe");
                    process.Start();
                    process.WaitForExit();
                    label5.Text = "完成";
                    INI.DeleteFile(Path.Combine(dialog.FileName, "patch.exe"));
                    INI.DeleteFolder(Path.Combine(dialog.FileName, "delta"));
                }
            }
                
        }
         void UnZip(string src,string dst) {
            
            using (var zipFile = ZipFile.Read(src))
            {
                if (!Directory.Exists(dst))
                {
                    Directory.CreateDirectory(dst);
                }
                
                zipFile.ExtractProgress += (o, e) =>
                { //Fix this, not showing percentage of extraction.
                    progressBar1.Invoke(new MethodInvoker(delegate
                    {

                        if (e.EntriesTotal > e.EntriesExtracted)
                        {
                            int percentage = Convert.ToInt32(e.EntriesExtracted / (0.01 * e.EntriesTotal));
                            string curEntry = e.CurrentEntry.FileName.Split('/').Last();
                            Console.WriteLine($"Extracting {curEntry} {percentage} %");
                            progressBar1.Maximum = 100;
                            progressBar1.Value = percentage;
                        }else if (e.EventType == ZipProgressEventType.Extracting_AfterExtractAll) {
                            progressBar1.Value = 100;
                        }
                    }));
                };
                zipFile.ExtractAll(dst, ExtractExistingFileAction.OverwriteSilently);
                Console.WriteLine("???:" +"unzip end");
            }
        }


        private void label4_Click(object sender, EventArgs e)
        {
            (new Form2()).ShowDialog();
            CheckDeltaFile();

        }
        private void progressBar1_Click(object sender, EventArgs e)
        {
            

        }
        private void label5_Click(object sender, EventArgs e)
        {


        }
        private void label2_Click(object sender, EventArgs e)
        {


        }
        private void label1_Click_1(object sender, EventArgs e)
        {


        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

    }
}
