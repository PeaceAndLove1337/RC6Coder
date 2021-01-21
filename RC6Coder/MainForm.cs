using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RC6Coder
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FileModeForm fileModeForm = new FileModeForm();
            fileModeForm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TextModeForm textModeForm = new TextModeForm();
            textModeForm.ShowDialog();
        }
    }
}
