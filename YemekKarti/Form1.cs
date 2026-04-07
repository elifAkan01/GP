using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YemekKarti
{
    public partial class Form1 : Form
    {
        int seciliYemekID = 0;
        string seciliGorselYolu = "";
        public Form1()
        {
            InitializeComponent();
        }
        void YemekleriListele()
        {
            using (SqlConnection con = Db.Baglanti())
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Tarif", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        string MalzemeleriGetir(int yemekID)
        {
            StringBuilder sb = new StringBuilder();

            using (SqlConnection con = Db.Baglanti())
            {
                string sorgu = "SELECT Ad, Birim, Miktar FROM Malzeme WHERE YemekID=@YemekID";
                SqlCommand cmd = new SqlCommand(sorgu, con);
                cmd.Parameters.AddWithValue("@YemekID", yemekID);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    decimal miktar = Convert.ToDecimal(dr["Miktar"]);
                    string miktarYazi = miktar % 1 == 0 ? miktar.ToString("0") : miktar.ToString("0.##");

                    sb.AppendLine("- " + dr["Ad"].ToString() + " : " +
                                  miktarYazi + " " +
                                  dr["Birim"].ToString());
                }

                con.Close();
            }

            return sb.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            YemekleriListele();
            printDocument1.DefaultPageSettings.Landscape = true;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (dataGridView1.Rows[e.RowIndex].Cells["ID"].Value == DBNull.Value ||
                dataGridView1.Rows[e.RowIndex].Cells["ID"].Value == null)
                return;

            seciliYemekID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);

            string ad = dataGridView1.Rows[e.RowIndex].Cells["Ad"].Value?.ToString() ?? "";
            string yapilisi = dataGridView1.Rows[e.RowIndex].Cells["Yapilisi"].Value?.ToString() ?? "";
            string hazirlik = dataGridView1.Rows[e.RowIndex].Cells["HazirlikSuresi"].Value?.ToString() ?? "";
            string pisirme = dataGridView1.Rows[e.RowIndex].Cells["PisirmeSuresi"].Value?.ToString() ?? "";
            string enerji = dataGridView1.Rows[e.RowIndex].Cells["Enerji"].Value?.ToString() ?? "";
            seciliGorselYolu = dataGridView1.Rows[e.RowIndex].Cells["GorselYolu"].Value?.ToString() ?? "";

            if (!string.IsNullOrEmpty(seciliGorselYolu) && System.IO.File.Exists(seciliGorselYolu))
                pictureBox1.Image = Image.FromFile(seciliGorselYolu);
            else
                pictureBox1.Image = null;

            rtbDetay.Clear();
            rtbDetay.AppendText("Yemek Adı: " + ad + Environment.NewLine + Environment.NewLine);
            rtbDetay.AppendText("Malzemeler:" + Environment.NewLine);
            rtbDetay.AppendText(MalzemeleriGetir(seciliYemekID) + Environment.NewLine);
            rtbDetay.AppendText("Yapılışı:" + Environment.NewLine);
            rtbDetay.AppendText(yapilisi + Environment.NewLine + Environment.NewLine);
            rtbDetay.AppendText("Hazırlık Süresi: " + hazirlik + " dk" + Environment.NewLine);
            rtbDetay.AppendText("Pişirme Süresi: " + pisirme + " dk" + Environment.NewLine);
            rtbDetay.AppendText("Enerji: " + enerji + " kalori");
        }

        private void btnYeniYemek_Click(object sender, EventArgs e)
        {
            FormYemekEkle frm = new FormYemekEkle();

            if (frm.ShowDialog() == DialogResult.OK)
            {
                YemekleriListele();
            }
        }

        private void btnMalzemeEkle_Click(object sender, EventArgs e)
        {
            if (seciliYemekID == 0)
            {
                MessageBox.Show("Önce yemek seç!");
                return;
            }

            FormMalzemeEkle frm = new FormMalzemeEkle(seciliYemekID);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                YemekleriListele();
            }
        }

        private void btnYazdir_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.WindowState = FormWindowState.Maximized;
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;

            Font baslikFont = new Font("Arial", 18, FontStyle.Bold);
            Font normalFont = new Font("Arial", 10);

            int sayfaGenislik = e.PageBounds.Width;
            int sayfaYukseklik = e.PageBounds.Height;

            // SOL TARAF (GÖRSEL)
            Rectangle resimAlani = new Rectangle(20, 20, sayfaGenislik / 2 - 40, sayfaYukseklik - 40);

            if (pictureBox1.Image != null)
            {
                g.DrawImage(pictureBox1.Image, resimAlani);
            }

            // SAĞ TARAF (YAZILAR)
            int x = sayfaGenislik / 2 + 10;
            int y = 30;

            // Başlık
            g.DrawString("Yemek Kartı", baslikFont, Brushes.Black, x, y);
            y += 40;

            // Detay (rtb içeriği)
            g.DrawString(rtbDetay.Text, normalFont, Brushes.Black,
                new RectangleF(x, y, sayfaGenislik / 2 - 40, sayfaYukseklik - 100));
        }
    }
}
