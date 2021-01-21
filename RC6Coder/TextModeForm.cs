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
using System.Windows.Forms.VisualStyles;

namespace RC6Coder
{
    public partial class TextModeForm : Form
    {
        
        public TextModeForm()
        {
            InitializeComponent();
        }

        private void checkBoxShow_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShow.Checked)
            {
                textBox2.PasswordChar = '\0';
            }
            else
            {
                textBox2.PasswordChar = '*';
            }
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            if (textBoxLengthOfKeyword.Text.Trim()=="")
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
            else if (listBox1.SelectedIndex==-1)
            {
                MessageBox.Show(new Form() { TopMost = true },
                               "Выберите режим работы!",
                               "Ошибка!",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            } else
            {
                if (listBox1.SelectedIndex == 0)
                    textBox2.Text=SomeTools.GenerateRandomKey(Int32.Parse(textBoxLengthOfKeyword.Text), false);
                else
                    textBox2.Text = SomeTools.GenerateRandomKey(Int32.Parse(textBoxLengthOfKeyword.Text), true);
            }
        }

      

        private void buttonEncodeToFile_Click(object sender, EventArgs e)
        {

            if (textBox2.Text.Trim() == "")
            {
                MessageBox.Show(new Form() { TopMost = true },
                                "Введите парольную фразу для шифрования!",
                                "Ошибка!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }else           
            if (textBox1.Text.Trim()=="")
            {
                MessageBox.Show(new Form() { TopMost = true },
                                "Введите текст для кодирования!",
                                "Ошибка!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            else
            {
                int longtoEncode;

                if (radioButton1.Checked)
                { longtoEncode = 128;  }

                else
                    if (radioButton2.Checked)
                { longtoEncode = 192;  }
                else
                { longtoEncode = 256; }

                //String userKeyword = textBox2.Text.Length < maxLength ? textBox2.Text+ new string(' ', maxLength-textBox2.Text.Length): textBox2.Text;

                byte[] userKeyBytes = Encoding.Unicode.GetBytes(textBox2.Text);
                PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(userKeyBytes, null);
                byte[] passInByte = passwordDeriveBytes.GetBytes(longtoEncode);

                RC6 rc6 = new RC6(longtoEncode, passInByte);

                byte[] codedText = rc6.EncodeRc6(textBox1.Text);

                //textBox2.Text=Encoding.UTF8.GetString(rc6.DecodeRc6(codedText));


                #region codeToFile
                 SaveFileDialog s = new SaveFileDialog();
                 s.CheckPathExists = true;
                 s.SupportMultiDottedExtensions = true;
                 s.Title = "Выберите файл для сохранения кодированного текста";
                 if (s.ShowDialog(this) != DialogResult.OK) return;
              
                 File.WriteAllBytes(s.FileName, codedText);
                #endregion
            }
        }

        private void buttonDecodeFromFile_Click(object sender, EventArgs e)
        {



            if (textBox2.Text.Trim() == "")
            {
                MessageBox.Show(new Form() { TopMost = true },
                                "Введите парольную фразу для расшифрования!",
                                "Ошибка!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            else
            if (!radioButton1.Checked && !radioButton2.Checked && !radioButton3.Checked)
            {
                MessageBox.Show(new Form() { TopMost = true },
                                "Выберите длину ключа!",
                                "Ошибка!",
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

                #region FileToCode

                OpenFileDialog o = new OpenFileDialog();
                o.CheckFileExists = true;
                o.CheckPathExists = true;
                o.Multiselect = false;
                o.SupportMultiDottedExtensions = true;
                o.Title = "Выберите файл с текстом для расшифрования";
                if (o.ShowDialog(this) != DialogResult.OK) return;

                //String userKeyword = textBox2.Text.Length < maxLength ? textBox2.Text + new string(' ', maxLength - textBox2.Text.Length) : textBox2.Text;

                byte[] userKeyBytes = Encoding.Unicode.GetBytes(textBox2.Text);
                PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(userKeyBytes, null);
                byte[] passInByte = passwordDeriveBytes.GetBytes(longtoEncode);

                try
                {
                    RC6 rc6 = new RC6(longtoEncode, passInByte);


                    byte[] fromFile = File.ReadAllBytes(o.FileName);

                    textBox1.Text = Encoding.UTF8.GetString(rc6.DecodeRc6(fromFile));
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
