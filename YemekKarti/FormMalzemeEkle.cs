using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace YemekKarti
{
    public partial class FormMalzemeEkle : Form
    {
        int yemekID;

        public FormMalzemeEkle(int id)
        {
            InitializeComponent();
            yemekID = id;
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (txtAd.Text == "" || txtMiktar.Text == "")
            {
                MessageBox.Show("Boş bırakma!");
                return;
            }

            using (SqlConnection con = Db.Baglanti())
            {
                string sorgu = @"INSERT INTO Malzeme (YemekID, Ad, Birim, Miktar)
                                 VALUES (@yemekID, @ad, @birim, @miktar)";

                SqlCommand cmd = new SqlCommand(sorgu, con);

                cmd.Parameters.AddWithValue("@yemekID", yemekID);
                cmd.Parameters.AddWithValue("@ad", txtAd.Text);
                cmd.Parameters.AddWithValue("@birim", txtBirim.Text);
                cmd.Parameters.AddWithValue("@miktar", txtMiktar.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            MessageBox.Show("Malzeme eklendi!");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}