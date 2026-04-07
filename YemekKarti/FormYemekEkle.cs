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
    public partial class FormYemekEkle : Form
    {
        string secilenResimYolu = "";
        public FormYemekEkle()
        {
            InitializeComponent();
        }

        private void txtHazirlik_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnResimSec_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Resim Dosyaları|*.jpg;*.png;*.jpeg";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                secilenResimYolu = openFileDialog1.FileName;
                pictureBox1.ImageLocation = secilenResimYolu;
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (txtAd.Text == "" || rtbYapilisi.Text == "")
            {
                MessageBox.Show("Boş bırakma!");
                return;
            }

            using (SqlConnection con = Db.Baglanti())
            {
                string sorgu = @"INSERT INTO Tarif
                     (Ad, Yapilisi, HazirlikSuresi, PisirmeSuresi, Enerji, GorselYolu)
                     VALUES
                     (@ad,@yap,@haz,@pis,@ene,@gor)";

                SqlCommand cmd = new SqlCommand(sorgu, con);

                cmd.Parameters.AddWithValue("@ad", txtAd.Text);
                cmd.Parameters.AddWithValue("@yap", rtbYapilisi.Text);
                cmd.Parameters.AddWithValue("@haz", txtHazirlik.Text);
                cmd.Parameters.AddWithValue("@pis", txtPisirme.Text);
                cmd.Parameters.AddWithValue("@ene", txtEnerji.Text);
                cmd.Parameters.AddWithValue("@gor", secilenResimYolu);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            MessageBox.Show("Yemek kaydedildi!");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
