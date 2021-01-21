using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RC6Coder
{
    public partial class FileModeForm : Form
    {
        public FileModeForm()
        {
            InitializeComponent();
        }

        private void buttonBrowse1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Multiselect = false;
            openFileDialog.SupportMultiDottedExtensions = true;
            openFileDialog.Title = "Выберите файл для шифрования";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                if (textBoxOutput.Text != openFileDialog.FileName|| textBoxOutput.Text == "") 
                    textBoxInput.Text = openFileDialog.FileName;
                else 
                    MessageBox.Show(this, "Входной и выходной файлы должны быть разными!", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (textBoxInput.Text == "" || textBoxOutput.Text == "")
                buttonEncode.Enabled = buttonDecode.Enabled = false;
            else
                buttonEncode.Enabled = buttonDecode.Enabled = true;
        }

       

        private void buttonOutputBrowse_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.SupportMultiDottedExtensions = true;
            saveFileDialog.Title = "Выберите файл для расшифрования";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                if (textBoxInput.Text != saveFileDialog.FileName) 
                    textBoxOutput.Text = saveFileDialog.FileName;
                else
                    MessageBox.Show(this, "Входной и выходной файлы должны быть разными!",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (textBoxInput.Text == "" || textBoxOutput.Text == "")
                buttonEncode.Enabled = buttonDecode.Enabled = false;
            else
                buttonEncode.Enabled = buttonDecode.Enabled = true;


        }

        private void checkBoxShow_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShow.Checked)
            {
                textBox1.PasswordChar = '\0';
            }
            else
            {
                textBox1.PasswordChar = '*';
            }
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            if (textBoxLengthOfKeyword.Text.Trim() == "")
            {
                MessageBox.Show(new Form() { TopMost = true },
                                "Введите длину генерируемого ключа!",
                                "Ошибка!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            else if (!SomeTools.IsNumber(textBoxLengthOfKeyword.Text))
            {
                MessageBox.Show(new Form() { TopMost = true },
                               "Длина ключа должна состоять из десятичных цифр!",
                               "Ошибка!",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
            else if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show(new Form() { TopMost = true },
                               "Выберите режим работы!",
                               "Ошибка!",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
            else
            {
                if (listBox1.SelectedIndex == 0)
                    textBox1.Text = SomeTools.GenerateRandomKey(Int32.Parse(textBoxLengthOfKeyword.Text), false);
                else
                    textBox1.Text = SomeTools.GenerateRandomKey(Int32.Parse(textBoxLengthOfKeyword.Text), true);
            }

        }

        
        private void buttonEncode_Click(object sender, EventArgs e)
        {
            string inputPath = textBoxInput.Text;
            string outputPath = textBoxOutput.Text;
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show(new Form() { TopMost = true },
                                "Введите парольную фразу для шифрования!",
                                "Ошибка!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            else
            if (!File.Exists(inputPath))
            {
                MessageBox.Show(new Form() { TopMost = true },
                    "Данного входного файла не существует!",
                    "Ошибка!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            if (outputPath == "")
            {
                MessageBox.Show(new Form() { TopMost = true },
                    "Отсутствует файл для вывода!",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                int longtoEncode;

                if (radioButton1.Checked)
                { longtoEncode = 128; }

                else
                    if (radioButton2.Checked)
                { longtoEncode = 192; }
                else
                { longtoEncode = 256; }

                byte[] userKeyBytes = Encoding.Unicode.GetBytes(textBox1.Text);
                PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(userKeyBytes, null);
                byte[] passInByte = passwordDeriveBytes.GetBytes(longtoEncode);
                
                
                #region endcodeFile
                RC6 rc6 = new RC6(longtoEncode, passInByte);

                byte[] inputBytes = File.ReadAllBytes(inputPath);
                byte[] codedText = rc6.EncodeRc6(inputBytes);

                File.WriteAllBytes(outputPath, codedText);
                #endregion
            }

           


        }
        private void buttonDecode_Click(object sender, EventArgs e)
        {
            string inputPath = textBoxInput.Text;
            string outputPath = textBoxOutput.Text;
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show(new Form() { TopMost = true },
                                "Введите парольную фразу для шифрования!",
                                "Ошибка!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            else
            if (!File.Exists(inputPath))
            {
                MessageBox.Show(new Form() { TopMost = true },
                    "Данного входного файла не существует!",
                    "Ошибка!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            if (outputPath == "")
            {
                MessageBox.Show(new Form() { TopMost = true },
                    "Отсутствует файл для вывода!",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                int longtoEncode;

                if (radioButton1.Checked)
                { longtoEncode = 128; }

                else
                    if (radioButton2.Checked)
                { longtoEncode = 192; }
                else
                { longtoEncode = 256; }

                byte[] userKeyBytes = Encoding.Unicode.GetBytes(textBox1.Text);
                PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(userKeyBytes, null);
                byte[] passInByte = passwordDeriveBytes.GetBytes(longtoEncode);

                #region decodeFile

                try { 
                RC6 rc6 = new RC6(longtoEncode, passInByte);

                byte[] inputBytes = File.ReadAllBytes(inputPath);
                byte[] decodedText = rc6.DecodeRc6(inputBytes);

                File.WriteAllBytes(outputPath, decodedText);
                }
                catch (Exception)
                {
                    MessageBox.Show(new Form() { TopMost = true },
                    "Был выбран некорректный файл для расшифровки!",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                }


                #endregion
            }


        }
    }
}
