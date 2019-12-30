using MixErp302.Data;
using MixErp302.Fonksiyonlar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MixErp302.Bilgi
{
    public partial class frmCariGiris : Form
    {
        MixErpDbEntities db = new MixErpDbEntities();
        Numaralar N = new Numaralar();     
        int secimId = -1;
        bool edit = false;

        public frmCariGiris()
        {
            InitializeComponent();
        }
        private void frmCariGiris_Load(object sender, EventArgs e)
        {
            Combo();
            Listele();
            txtCariNo.Text = N.CariNo();
        }
        void Temizle()
        {
            foreach (Control con in splitContainer2.Panel1.Controls)
            {
                if (con is TextBox || con is ComboBox)
                {
                    con.Text = "";
                }
            }
            secimId = -1;
            edit = false;
            txtCariNo.Text = N.CariNo();
            txtCariTip.SelectedIndex = 0;
            txtSehir.SelectedIndex = 0;
            txtIlce.SelectedIndex = 0;
        }

        private void Listele()
        {
            Liste.Rows.Clear();
            int i = 0;
            var srg = (from s in db.tblCaris
                       select s).ToList();
            foreach (var k in srg)
            {
                Liste.Rows.Add();
                Liste.Rows[i].Cells[0].Value = k.Id;
                Liste.Rows[i].Cells[1].Value = k.CariNo;
                Liste.Rows[i].Cells[2].Value = k.CariAdi;
                Liste.Rows[i].Cells[3].Value = k.Tel;
                Liste.Rows[i].Cells[4].Value = k.blgCariTipi.CariTipi;
                i++;
            }
            Liste.AllowUserToAddRows = false;
        }

        private void Combo()
        {
            txtCariTip.DataSource = db.blgCariTipis.ToList();
            txtCariTip.ValueMember = "Id";
            txtCariTip.DisplayMember = "CariTipi";
            txtCariTip.SelectedIndex = -1;

            txtSehir.Items.Clear();
            var srg = (from s in db.illers select s).ToList();
            foreach(var k in srg)
            {
                txtSehir.Items.Add(k.isim);
            }
        }
        private void YeniKaydet()
        {
            tblCari cari = new tblCari();
            cari.CariNo = txtCariNo.Text;
            cari.CariAdi = txtCariAdi.Text;
            cari.CariTipId = db.blgCariTipis.First(x => x.CariTipi == txtCariTip.Text).Id;
            cari.SehirId = db.illers.First(x => x.isim == txtSehir.Text).il_no;
            cari.IlceId = db.ilcelers.First(x => x.isim == txtIlce.Text).ilce_no;
            cari.Tel = txtTelefon.Text;
            cari.Adres = txtAdres.Text;
            cari.VergiD = txtVergiDairesi.Text;
            cari.VergiNoTc = txtVergiNoTc.Text;

            db.tblCaris.Add(cari);
            db.SaveChanges();
            MessageBox.Show("Kayıt Başarılı");
            Listele();
            Temizle();
        }
        void Guncelle()
        {
            tblCari cari = db.tblCaris.Find(secimId);
            cari.CariNo = txtCariNo.Text;
            cari.CariAdi = txtCariAdi.Text;
            cari.CariTipId = db.blgCariTipis.First(x => x.CariTipi == txtCariTip.Text).Id;
            cari.SehirId = db.illers.First(x => x.isim == txtSehir.Text).il_no;
            cari.IlceId = db.ilcelers.First(x => x.isim == txtIlce.Text).ilce_no;
            cari.Tel = txtTelefon.Text;
            cari.Adres = txtAdres.Text;
            cari.VergiD = txtVergiDairesi.Text;
            cari.VergiNoTc = txtVergiNoTc.Text;

            db.SaveChanges();
            MessageBox.Show("Güncelleme Başarılı");
            Listele();
            Temizle();
        }

      

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (edit && secimId > 0) Guncelle();
            else if (edit == false)
                
            YeniKaydet();
        }

        private void Liste_DoubleClick(object sender, EventArgs e)
        {
            Sec();
            if (secimId > 0)
            {
                Ac(secimId);
            }
        }

        private void Ac(int secimId)
        {
            edit = true;
            tblCari cari = db.tblCaris.Find(secimId);
            txtCariNo.Text = cari.CariNo;
            txtAdres.Text = cari.Adres;
            txtCariAdi.Text = cari.CariAdi;
            txtTelefon.Text = cari.Tel;
            txtVergiNoTc.Text = cari.VergiNoTc;
            txtVergiDairesi.Text = cari.VergiD;
            txtCariTip.Text = cari.blgCariTipi.CariTipi;
            txtSehir.Text = cari.iller.isim;
            txtIlce.Text = cari.ilceler.isim;           
        }

        private void Sec()
        {
            try
            {
                secimId = Convert.ToInt32(Liste.CurrentRow.Cells[0].Value.ToString());
            }
            catch (Exception)
            {

                secimId = -1;
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (edit && secimId > 0)
            {
                Sil();
            }
        }

        private void Sil()
        {
            db.tblCaris.Remove(db.tblCaris.Find(secimId));
            db.SaveChanges();
            MessageBox.Show($"'{secimId}'Kayıt Silinmiştir");

            Listele();
            Temizle();
        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtSehir_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtIlce.Text = "";
            txtIlce.Items.Clear();
            int a = db.illers.First(x => x.isim == txtSehir.Text).il_no;
            var srg = (from s in db.ilcelers where s.il_no == a select s).ToList();
            foreach(var k in srg)
            {
                txtIlce.Items.Add(k.isim);
            }
        }
    }
}
