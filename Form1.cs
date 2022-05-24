using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Xml.Serialization;

namespace CaesarEncryptor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int length;
        public bool RichTextBoxChanged = false;
        public bool IsSaved = false;
        public string FileName = String.Empty;
        public DataTable frequenceTableOriginal = new DataTable();
        public DataTable frequenceTableDecrypted = new DataTable();
        public DataTable frequenceTableUkr = new DataTable();
        public DataTable frequenceTableEng = new DataTable();
        public DataTable attackTable = new DataTable();
        public string public_key;
        public string private_key;
        public string RSAkey;

        RSACryptoServiceProvider rSA = new RSACryptoServiceProvider();
        public byte[] encryptedRSA;

        private void ClearRichTextBox()
        {
            OriginalText.Text = "";
            DecryptedText.Text = "";
        }

        private void Open(string OpenFileName)
        {

            if (OpenFileName == "")
            {
                return;
            }
            else
            {
                StreamReader readFile = new StreamReader(OpenFileName);
                while (!readFile.EndOfStream)
                {
                    OriginalText.Text = readFile.ReadToEnd();
                }
                readFile.Close();
                FileName = OpenFileName;
            }
        }

        private void Save(string SaveFileName)
        {
            if (SaveFileName == "")
            {
                return;
            }
            else
            {
                File.WriteAllText(SaveFileName, String.Empty);
                StreamWriter sw = new StreamWriter(SaveFileName);
                sw.WriteLine("Original text ->\n");
                for (int i = 0; i < OriginalText.Lines.Length; i++)
                {
                    sw.WriteLine(OriginalText.Lines[i]);
                }
                sw.WriteLine("\n Decrypted text ->\n");
                for (int i = 0; i < DecryptedText.Lines.Length; i++)
                {
                    sw.WriteLine(DecryptedText.Lines[i]);
                }
                sw.Flush();
                sw.Close();
                FileName = SaveFileName;
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            About form = new About();
            form.Show();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RichTextBoxChanged == true)
            {
                DialogResult dialogResult = MessageBox.Show("Save current content?", "Question", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    saveToolStripMenuItem.PerformClick();
                    this.Text = "RSA";
                }
                else if (dialogResult == DialogResult.No)
                {
                    ClearRichTextBox();
                    this.Text = "RSA";
                }
            }
            else
            {
                ClearRichTextBox();
                this.Text = "RSA";
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RichTextBoxChanged == true)
            {
                if (DecryptedText.Text != String.Empty)
                {
                    DialogResult dialogResult = MessageBox.Show("Save current content?", "", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        saveToolStripMenuItem.PerformClick();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        DialogResult res;
                        res = openFileDialog1.ShowDialog();
                        if (res == DialogResult.OK)
                        {
                            Open(openFileDialog1.FileName);
                            RichTextBoxChanged = false;
                        }
                        FileName = openFileDialog1.FileName;
                        this.Text = Path.GetFileNameWithoutExtension(FileName);
                    }
                }
                else
                {
                    DialogResult res;
                    res = openFileDialog1.ShowDialog();
                    if (res == DialogResult.OK)
                    {
                        Open(openFileDialog1.FileName);
                        RichTextBoxChanged = false;
                    }
                    FileName = openFileDialog1.FileName;
                    this.Text = Path.GetFileNameWithoutExtension(FileName);
                }
            }
            else
            {
                DialogResult res;
                res = openFileDialog1.ShowDialog();
                if (res == DialogResult.OK)
                {
                    Open(openFileDialog1.FileName);
                }
                FileName = openFileDialog1.FileName;
                this.Text = Path.GetFileNameWithoutExtension(FileName);
            }
            saveToolStripMenuItem.Enabled = true;
            IsSaved = true;
            RichTextBoxChanged = false;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FileName))
            {
                saveAsToolStripMenuItem.PerformClick();
            }
            else
            {
                Save(FileName);
                RichTextBoxChanged = false;
                IsSaved = true;
                saveToolStripMenuItem.Enabled = true;
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult res;
            saveFileDialog1.Filter = "Text files (*.txt)|*.txt|Word Doucment (*.doc)|*.doc;*.docx|All files (*.*)|*.*";
            res = saveFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                Save(saveFileDialog1.FileName);
                RichTextBoxChanged = false;
                IsSaved = true;
            }
            FileName = saveFileDialog1.FileName;
            saveToolStripMenuItem.Enabled = true;
            this.Text = Path.GetFileNameWithoutExtension(FileName);
        }

        private void encryptButton_Click(object sender, EventArgs e)
        {
            string text = OriginalText.Text;
            if (string.IsNullOrEmpty(private_key) && string.IsNullOrEmpty(public_key))
            {
                MessageBox.Show("Firstly generate RSA key pair");
            }
            else
            {
                byte[] toEncryptData = Encoding.UTF8.GetBytes(text);
                encryptedRSA = rSA.Encrypt(toEncryptData, false);
                DecryptedText.Text = Encoding.Default.GetString(encryptedRSA);
            }
        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            bool valid = true;
            if (string.IsNullOrEmpty(private_key) && string.IsNullOrEmpty(public_key))
            {
                MessageBox.Show("Firstly generate RSA key pair");
            }
            else
            {
                try
                {
                    rSA.FromXmlString(private_key);
                }
                catch
                {
                    MessageBox.Show("Invalid key pair");
                    valid = false;
                }
                
                if(valid)
                {
                    byte[] decryptedRSA = rSA.Decrypt(encryptedRSA, false);
                    DecryptedText.Text = Encoding.Default.GetString(decryptedRSA);
                }
            }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printDialog1.Document = printDocument1;
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
            else
            {
                MessageBox.Show("Print Cancelled");
            }
        }

        private void printDocument1_PrintPage_1(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Font myFont = new Font("Arial", 14, FontStyle.Regular, GraphicsUnit.Pixel);
            float leftMargin = e.MarginBounds.Left;
            if (!string.IsNullOrEmpty(DecryptedText.Text))
            {
                e.Graphics.DrawString("Original text:\n" + OriginalText.Text + "\n\nDecrytped text:\n" + DecryptedText.Text + "\n\nRSA pub key:\n" + public_key + "\n\nRSA private key:\n" + private_key, myFont, Brushes.Black, leftMargin, 150, new StringFormat());
            }
            else if (!string.IsNullOrEmpty(OriginalText.Text))
            {
                e.Graphics.DrawString("Original text:\n" + OriginalText.Text + "\n\nRSA pub key:\n" + public_key + "\n\nRSA private key:\n" + private_key, myFont, Brushes.Black, leftMargin, 150, new StringFormat());
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            public_key = rSA.ToXmlString(false);
            private_key = rSA.ToXmlString(true);

            richTextBox2.Text = public_key;
            richTextBox1.Text = private_key;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            public_key = richTextBox2.Text;
            private_key = richTextBox1.Text;
        }
    }
}
