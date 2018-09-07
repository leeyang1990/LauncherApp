using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Ionic.Zip;

namespace WindowsFormsApp1
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

        private void diff_Load(object sender, EventArgs e)
        {
            
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (!CheckDiffButton())
            {
                MessageBox.Show("Please select or check your folder.","Message",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
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
        async void ZipFile(string src,string dst)
        {
            await Task.Run(() =>
            {
                using (var zipFile = new ZipFile())
                {
                    // add content to zip here 
                    zipFile.AddDirectory(src);
                    zipFile.SaveProgress +=
                        (o, args) =>
                        {
                            //if (args.EventType == ZipProgressEventType.Saving_AfterWriteEntry)
                            //{
                            //    progressBar1.Value = args.EntriesSaved * 100 / args.EntriesTotal;
                            //}
                            if (args.EventType == ZipProgressEventType.Saving_BeforeWriteEntry)
                            {
                                progressBar1.Invoke(new MethodInvoker(delegate
                                {
                                    //progressBar1.Maximum = 100;
                                    //progressBar1.Value = (int)((args.BytesTransferred * 100) / args.TotalBytesToTransfer); ;
                                    progressBar1.Maximum = args.EntriesTotal;
                                    progressBar1.Value = args.EntriesSaved + 1;
                                    progressBar1.Update();
                                }));
                            }
                            

                        };
                    zipFile.Save(dst);
                }
            });
            //this.Close();
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
