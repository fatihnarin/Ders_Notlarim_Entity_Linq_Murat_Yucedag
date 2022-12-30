using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using EntityORnek.Model;

namespace EntityORnek
{
    public partial class Form1 : Form
    {
        DbSinavOgrenciEntities db = new DbSinavOgrenciEntities();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnDersListesi_Click(object sender, EventArgs e)
        {
            SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-3KJ91FM;Initial Catalog=DbSinavOgrenci;Integrated Security=True");
            SqlCommand komut = new SqlCommand("Select * From Tblders",baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void btnOgrListele_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.TblOgrenci.ToList();
            dataGridView1.Columns[3].Visible = false;
            dataGridView1.Columns[4].Visible = false;
        }

        private void btnNotListesi_Click(object sender, EventArgs e)
        {
            var query = from item in db.TblNotlar
                        select new { item.NotId, item.Ogr, item.Ders, item.Sinav1,
                        item.Sinav2,item.Sinav3,item.Ortalama,item.Durum};
            dataGridView1.DataSource = query.ToList();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            TblOgrenci t = new TblOgrenci();
            t.Ad = txtOgrAd.Text;
            t.Soyad = txtOgrSoyad.Text;
            db.TblOgrenci.Add(t);
            db.SaveChanges();
            MessageBox.Show("Öğrenci Listeye Eklenmiştir");
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtOgrID.Text);
            var x = db.TblOgrenci.Find(id);
            db.TblOgrenci.Remove(x);
            MessageBox.Show("Öğrenci sistemden silindi");
        }

        private void btnGüncelle_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtOgrID.Text);
            var x = db.TblOgrenci.Find(id);
            x.Ad = txtOgrAd.Text;
            x.Soyad = txtOgrSoyad.Text;
            x.Fotograf = txtOgrFoto.Text;
            db.SaveChanges();
            MessageBox.Show("Öğrenci Bilgileri Güncellendi");
        }

        private void btnPrasedur_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.NOTLISTESI();
        }

        private void btnBul_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.TblOgrenci.Where(x => x.Ad == txtOgrAd.Text &
            x.Soyad==txtOgrSoyad.Text).ToList();
        }

        private void txtOgrAd_TextChanged(object sender, EventArgs e)
        {
            string aranan = txtOgrAd.Text;
            var degerler = from item in db.TblOgrenci
                           where item.Ad.Contains(aranan)
                           select item;
            dataGridView1.DataSource = degerler.ToList();
        }

        private void btnLinqEntity_Click(object sender, EventArgs e)
        {
            if (rdbtnAdaGoreSirala.Checked==true)
            {
                List<TblOgrenci> liste1 = db.TblOgrenci.OrderBy(p => p.Ad).ToList();
                dataGridView1.DataSource = liste1;
            }
            if (rdbtnAdaGoreSiralaTers.Checked==true)
            {
                List<TblOgrenci> liste2 = db.TblOgrenci.OrderByDescending(p => p.Ad).ToList();
                dataGridView1.DataSource = liste2;
            }
            if (rdbtnIlkUcKayit.Checked == true)
            {
                List<TblOgrenci> liste3 = db.TblOgrenci.OrderBy(p => p.Id).Take(3).ToList();
                dataGridView1.DataSource = liste3;
            }
            if (rdbtnIsminBasHarfiA.Checked==true)
            {
                List<TblOgrenci> liste4 = db.TblOgrenci.Where(p => p.Ad.StartsWith("a")).ToList();
                dataGridView1.DataSource = liste4;
            }
            if (radioButton1.Checked == true)
            {
                List<TblOgrenci> liste5 = db.TblOgrenci.Where(p => p.Ad.EndsWith("a")).ToList();
                dataGridView1.DataSource = liste5;
            }
            if (radioButton2.Checked==true)
            {
                bool deger = db.TblOgrenci.Any();
                MessageBox.Show(deger.ToString(),"Bilgi", MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            if (radioButton3.Checked==true)
            {
                int toplam = db.TblOgrenci.Count();
                MessageBox.Show(toplam.ToString(), "Toplam Öğrenci Sayısı", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            if (radioButton4.Checked==true)
            {
                var toplam = db.TblNotlar.Sum(p => p.Sinav1);
                MessageBox.Show("1. Sınav Toplam Puan: " + toplam.ToString());
            }
            if (radioButton5.Checked == true)
            {
                var toplam = db.TblNotlar.Sum(p => p.Sinav3);
                MessageBox.Show("2. Sınav Toplam Puan: " + toplam.ToString());
            }
            if (radioButton6.Checked == true)
            {
                var toplam = db.TblNotlar.Sum(p => p.Sinav3);
                MessageBox.Show("3. Sınav Toplam Puan: " + toplam.ToString());
            }
            if (radioButton7.Checked == true)
            {
                var enyuksek = db.TblNotlar.Max(p => p.Sinav1);
                MessageBox.Show("1. Sınavın en yüksek notu :" + enyuksek);
            }
        }

        private void btnJoinileGetir_Click(object sender, EventArgs e)
        {
            var sorgu = from d1 in db.TblNotlar
                        join d2 in db.TblOgrenci
                        on d1.Ogr equals d2.Id
                        join d3 in db.TblDers
                        on d1.Ders equals d3.DersID
                        select new
                        {
                            Öğrenci=d2.Ad+" "+d2.Soyad,
                            Ders=d3.DersAd,
                            Sınav1=d1.Sinav1,
                            Sınav2=d1.Sinav2,
                            Sınav3=d1.Sinav3,
                            Ortalama=d1.Ortalama
                        };
            dataGridView1.DataSource = sorgu.ToList();
        }
    }
}
