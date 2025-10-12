using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotePad1
{
    public partial class Form1 : Form
    {
        private string currentFilePath = ""; // Mevcut açık dosyanın yolu
        private PrintDocument printDocument = new PrintDocument(); // Yazdırma işlemi için
        public Form1()
        {
            InitializeComponent();
       
        }
        //Dosya Sekmesi
        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear(); // Metin kutusunu temizler
            currentFilePath = ""; // Dosya yolunu sıfırlar
            this.Text = "Not Defteri - Yeni Belge"; // Form başlığını günceller
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Metin Dosyaları|*.txt|Tüm Dosyalar|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                // Dosyayı oku ve RichTextBox'a yaz
                richTextBox1.Text = File.ReadAllText(ofd.FileName);
                currentFilePath = ofd.FileName;
                this.Text = "Not Defteri - " + Path.GetFileName(currentFilePath);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                saveAsToolStripMenuItem_Click(sender, e); // Farklı Kaydet'e yönlendir
            }
            else
            {
                File.WriteAllText(currentFilePath, richTextBox1.Text);
                MessageBox.Show("Dosya başarıyla kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Metin Dosyaları|*.txt|Tüm Dosyalar|*.*";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sfd.FileName, richTextBox1.Text);
                currentFilePath = sfd.FileName;
                this.Text = "Not Defteri - " + Path.GetFileName(currentFilePath);
                MessageBox.Show("Dosya kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            pd.Document = printDocument;

            if (pd.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print(); // Yazdırma işlemini başlat
            }
        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintPreviewDialog previewDialog = new PrintPreviewDialog();
            previewDialog.Document = printDocument;
            previewDialog.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(richTextBox1.Text, richTextBox1.Font, Brushes.Black, 100, 100);
        }

        // Düzen Sekmesi
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Eğer bir işlem geri alınabiliyorsa
            if (richTextBox1.CanUndo)
            {
                richTextBox1.Undo(); // Geri al
                richTextBox1.ClearUndo(); // Undo tamponunu temizle (opsiyonel)
            }
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Redo işlemi RichTextBox'ta sınırlıdır.");
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectedText != "")
            {
                richTextBox1.Cut(); // Seçili metni kes ve clipboard'a kopyala
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectedText != "")
            {
                richTextBox1.Copy(); // Seçili metni clipboard'a kopyala
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Clipboard'da metin varsa yapıştır
            if (Clipboard.ContainsText())
            {
                richTextBox1.Paste();
            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll(); // Tüm metni seç
        }


        //Araçlar Sekmesi
        private void toolsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void customizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Özelleştirme seçenekleri daha sonra eklenecek!", "Customize", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Program ayarları ydaha sonra eklenecek!", "Options", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void appearanceToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //Görünüm Sekmesi
        private void coverScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized && this.FormBorderStyle == FormBorderStyle.None)
            {
                // Normal pencere boyutuna dön
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                // Tam ekran moduna geç
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void minimiseToIconStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void horizontallyFurnisingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer1.Visible = true; // Bölünmeyi aktif et
            splitContainer1.Orientation = Orientation.Horizontal;
        }

        private void verticalFurnisingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer1.Visible = true;
            splitContainer1.Orientation = Orientation.Vertical;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Başlangıçta sadece tek editör göster
            splitContainer1.Visible = false;
            richTextBox1.Dock = DockStyle.Fill;
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
        }

        private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filePath = "help.txt"; // artık proje ile birlikte gelecek

            if (File.Exists(filePath))
            {
                Form helpForm = new Form();
                helpForm.Text = "Yardım";
                helpForm.Size = new Size(600, 400);

                RichTextBox rtb = new RichTextBox
                {
                    Dock = DockStyle.Fill,
                    ReadOnly = true,
                    ScrollBars = RichTextBoxScrollBars.Both,
                    Text = File.ReadAllText(filePath)
                };

                helpForm.Controls.Add(rtb);
                helpForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("help.txt bulunamadı!");
            }

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutForm = new AboutBox1(); // Hakkımızda formundan bir nesne oluştur
            aboutForm.ShowDialog(); // Formu diyalog olarak göster (küçük pencere gibi)
        }
    }
}
