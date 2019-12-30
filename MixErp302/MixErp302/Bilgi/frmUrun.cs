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
    public partial class frmUrun : Form
    {
        MixErpDbEntities db = new MixErpDbEntities();
        int secimId=-1;
        bool edit = false;
        Numaralar N = new Numaralar();
        public frmUrun()
        {
            InitializeComponent();
        }

        private void frmUrun_Load(object sender, EventArgs e)
        {
            Combo();
            Listele();
        }

        private void Combo()
        {
            txtMenseiId.DataSource = db.bMenseis.ToList();
            txtMenseiId.ValueMember = "Id";
            txtMenseiId.DisplayMember = "MenseiAdi";
            txtMenseiId.SelectedIndex = 0;

            txtBirim.DataSource = db.bBirims.ToList();
            txtBirim.ValueMember = "Id";
            txtBirim.DisplayMember = "BirimAdi";
            txtBirim.SelectedIndex = 0;

            txtKategoriId.DataSource = db.bKategoris.ToList();
            txtKategoriId.ValueMember = "Id";
            txtKategoriId.DisplayMember = "KategoriAdi";
            txtKategoriId.SelectedIndex = 0;

            txtCariId.DataSource = db.tblCaris.ToList();
            txtCariId.ValueMember = "Id";
            txtCariId.DisplayMember = "CariAdi";
            txtCariId.SelectedIndex = -1;
        }
        private void YeniKaydet()
        {
            var ukontrol = db.tblUrunlers.Where(x => x.UrunKodu.ToLower() == txtUrunKodu.Text.ToLower()).ToList();

            if (ukontrol.Count()==0)
            {
                tblUrunler urn = new tblUrunler();
                urn.UrunKodu = txtUrunKodu.Text;
                urn.UrunAciklama = txtUrunAciklama.Text;
                urn.MenseiId = db.bMenseis.First(x => x.MenseiAdi == txtMenseiId.Text).Id;
                urn.KategoriId = db.bKategoris.First(x => x.KategoriAdi == txtKategoriId.Text).Id;
                urn.CariId = db.tblCaris.First(x => x.CariAdi == txtCariId.Text).Id;
                urn.Birim = db.bBirims.First(x => x.BirimAdi == txtBirim.Text).Id;

                db.tblUrunlers.Add(urn);
                db.SaveChanges();

                tblStokDurum stk = new tblStokDurum();
                stk.Ambar = 0;
                stk.Barkod = txtUrunKodu.Text + "/" + txtUrunAciklama.Text;
                stk.Depo = 0;
                stk.Raf = 0;
                stk.StokKodu = N.StokKod();
                stk.UrunId = db.tblUrunlers.First(x => x.UrunKodu == txtUrunKodu.Text).Id;
                db.tblStokDurums.Add(stk);
                db.SaveChanges();

                MessageBox.Show("Kayıt Başarılı");                
            }
            else
            {
                MessageBox.Show("Bu ürün daha önce kaydedilmiştir..Lütfen Kontrol Ediniz ..!");
                txtUrunKodu.Text = "";
                return;
            }
            Listele();
            Temizle();
        }

        private void Temizle()
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
            txtUrunKodu.Text ="";
            txtUrunAciklama.Text ="";
            txtMenseiId.SelectedIndex = 0;
            txtKategoriId.SelectedIndex = 0;
            txtBirim.SelectedIndex = 0;
            txtCariId.SelectedIndex = 0;
        }

        private void Listele()
        {
            Liste.Rows.Clear();
            int i = 0;
            var srg = (from s in db.tblUrunlers
                       where s.UrunKodu.Contains(txtBul.Text) || s.UrunAciklama.Contains(txtBul.Text) || s.bKategori.KategoriAdi.Contains(txtBul.Text) || s.tblCari.CariAdi.Contains(txtBul.Text)
                       select s).ToList();
            foreach (var k in srg)
            {
                Liste.Rows.Add();
                Liste.Rows[i].Cells[0].Value = k.Id;
                Liste.Rows[i].Cells[1].Value = k.UrunKodu;
                Liste.Rows[i].Cells[2].Value = k.UrunAciklama;
                Liste.Rows[i].Cells[3].Value = k.tblCari.CariAdi;
                Liste.Rows[i].Cells[4].Value = k.bKategori.KategoriAdi;
                i++;
            }
            Liste.AllowUserToAddRows = false;
        }

        private void Guncelle()
        {
            tblUrunler urn = db.tblUrunlers.Find(secimId);
            urn.UrunKodu = txtUrunKodu.Text;
            urn.UrunAciklama = txtUrunAciklama.Text;
            urn.MenseiId = db.bMenseis.First(x => x.MenseiAdi == txtMenseiId.Text).Id;
            urn.KategoriId = db.bKategoris.First(x => x.KategoriAdi == txtKategoriId.Text).Id;
            urn.CariId = db.tblCaris.First(x => x.CariAdi == txtCariId.Text).Id;
            urn.Birim = db.bBirims.First(x => x.BirimAdi == txtBirim.Text).Id;

            db.SaveChanges();
            MessageBox.Show("Güncelleme Başarılı");
            Listele();
            Temizle();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (edit && secimId > 0)   Guncelle();

            
            else if (edit == false)     YeniKaydet();
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
            tblUrunler urn = db.tblUrunlers.Find(secimId);
            txtUrunKodu.Text = urn.UrunKodu;
            txtUrunAciklama.Text = urn.UrunAciklama;
            txtMenseiId.Text = urn.bMensei.MenseiAdi;
            txtBirim.Text = urn.bBirim.BirimAdi;
            txtKategoriId.Text = urn.bKategori.KategoriAdi;
            txtCariId.Text = urn.tblCari.CariAdi;
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
                db.tblUrunlers.Remove(db.tblUrunlers.Find(secimId));
                db.SaveChanges();
                MessageBox.Show($"'{secimId}'Kayıt Silinmiştir");

                Listele();
                Temizle();      
        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnBul_Click(object sender, EventArgs e)
        {
            Listele();
        }
    }
}
