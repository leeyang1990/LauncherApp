using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using Ionic.Zip;

namespace updatetool
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            textBox1.Text = INI.ReadIni("Common", "OLD");
            if (!Directory.Exists(INI.ReadIni("Common", "OLD")))
            {
                textBox1.Text = "...";
                state1.Text = "!";
                state1.ForeColor = Color.Red;
            }
            else
            {
                state1.Text = "√";
                state1.ForeColor = Color.PaleGreen;
            }
            textBox2.Text = INI.ReadIni("Common", "NEW");
            if (!Directory.Exists(textBox2.Text))
            {
                textBox2.Text = "...";
                state2.Text = "!";
                state2.ForeColor = Color.Red;
            }
            else
            {
                state2.Text = "√";
                state2.ForeColor = Color.PaleGreen;
            }
        }

        bool CheckDiffButton()
        {
            if(state1.Text == "√"&& state2.Text == "√")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<string> excludeFiles = new List<string>()
        {
            "NativeBridge.log",
            "patch.log",
        };
        public List<string> excludeDics = new List<string>()
        {
            "cache",
        };
        private void button1_Click(object sender, EventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                INI.WriteIni("Common", "OLD", dialog.FileName);
                textBox1.Text = dialog.FileName;
                state1.Text = "√";
                state1.ForeColor = Color.PaleGreen;
                
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                INI.WriteIni("Common", "NEW", dialog.FileName);
                textBox2.Text = dialog.FileName;
                state2.Text = "√";
                state2.ForeColor = Color.PaleGreen;
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (!CheckDiffButton())
            {
                MessageBox.Show("Please select or check your folder.","Message",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }

            //Delete exclude 
            List<string> rootpaths = new List<string>() { textBox1.Text, textBox2.Text };
            foreach (var item in rootpaths)
            {
                foreach (var ee in excludeFiles)
                {
                    INI.DeleteFile(Path.Combine(item, ee));
                }

                foreach (var ee in excludeDics)
                {
                    INI.DeleteFolder(Path.Combine(item, ee));
                }
            }


            INI.DeleteFolder(Path.Combine(Environment.CurrentDirectory, INI.ReadIni("Common", "DELTA")));
            // Create a process
            System.Diagnostics.Process process = new System.Diagnostics.Process();

            // Set the StartInfo of process
            process.StartInfo.FileName = Environment.CurrentDirectory + "/tools/diff.exe";

            // Start the process
            process.Start();
            process.WaitForExit();

            string startPath = Environment.CurrentDirectory + @"/delta";
            string zipPath = Environment.CurrentDirectory + @"/delta.zip";

            ZipFile(startPath,zipPath);


        }
        void ZipFile(string src,string dst)
        {
            Thread thread =  new Thread(() =>
            {
                using (var zipFile = new ZipFile())
                {
                    // add content to zip here 
                    zipFile.AddDirectory(src);
                    zipFile.SaveProgress +=
                        (o, e) =>
                        {

                            if (e.EventType == ZipProgressEventType.Saving_BeforeWriteEntry)
                            {
                                progressBar1.Invoke(new MethodInvoker(delegate {
                                    progressBar1.Maximum = e.EntriesTotal;
                                    progressBar1.Value = e.EntriesSaved + 1;
                                }));
                                
                            }
                            else if (e.EventType == ZipProgressEventType.Saving_EntryBytesRead)
                            {
                                //progressBar1.Invoke(new MethodInvoker(delegate {
                                //    progressBar1.Value = (int)((e.BytesTransferred * 100) / e.TotalBytesToTransfer);
                                //}));
                            }
                            else if (e.EventType == ZipProgressEventType.Saving_Completed)
                            {
                                MessageBox.Show("Done: " + e.ArchiveName);
                                INI.DeleteFolder(src);

                            }

                        };
                    zipFile.Save(dst);
                }
            });
            thread.Start();
        }
        private void label2_Click(object sender, EventArgs e)
        {


        }
        private void label4_Click(object sender, EventArgs e)
        {


        }
        private void progressBar1_Click(object sender, EventArgs e)
        {


        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {


        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {


        }
        private void label1_Click_1(object sender, EventArgs e)
        {


        }
        private void label2_Click_1(object sender, EventArgs e)
        {


        }
        private void diff_Load(object sender, EventArgs e)
        {


        }
    }
}
