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

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            label1.Text = INI.ReadIni("Common", "OLD");
            if (!Directory.Exists(INI.ReadIni("Common", "OLD")))
            {
                //label1.Text = "...";
                state1.Text = "!";
                state1.ForeColor = Color.Red;
            }
            else
            {
                state1.Text = "√";
                state1.ForeColor = Color.PaleGreen;
            }
            label3.Text = INI.ReadIni("Common", "NEW");
            if (!Directory.Exists(label3.Text))
            {
                label3.Text = "...";
                state2.Text = "!";
                state2.ForeColor = Color.Red;
            }
            else
            {
                state2.Text = "√";
                state2.ForeColor = Color.PaleGreen;
            }
            CheckDiffButton();
        }

        private void diff_Load(object sender, EventArgs e)
        {
            
        }
        void CheckDiffButton()
        {
            if(state1.Text == "√"&& state2.Text == "√")
            {
                button3.Enabled = true;
            }
            else
            {
                button3.Enabled = false;
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
                label1.Text = dialog.FileName;
                state1.Text = "√";
                state1.ForeColor = Color.PaleGreen;
                CheckDiffButton();
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
                label3.Text = dialog.FileName;
                state2.Text = "√";
                state2.ForeColor = Color.PaleGreen;
                CheckDiffButton();
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
            // Create a process
            System.Diagnostics.Process process = new System.Diagnostics.Process();

            // Set the StartInfo of process
            process.StartInfo.FileName = Environment.CurrentDirectory+ "/delta/diff.exe";

            // Start the process
            process.Start();
            process.WaitForExit();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
