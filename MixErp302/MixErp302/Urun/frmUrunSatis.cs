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

namespace MixErp302.Urun
{
    public partial class frmUrunSatis : Form
    {
        MixErpDbEntities db = new MixErpDbEntities();
        Numaralar N = new Numaralar();
        int secimId = -1;
        bool edit = false;
        int UrnSatisId = -1;
        public string[] MyArray { get; set; }
        public frmUrunSatis()
        {
            InitializeComponent();
        }

        private void frmUrunSatis_Load(object sender, EventArgs e)
        {
            txtSatisGrupNo.Text = N.SatisGrupNo();
            Combo();
        }

        private void Combo()
        {
            txtCari.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtCari.AutoCompleteMode = AutoCompleteMode.Suggest;
            AutoCompleteStringCollection veri = new AutoCompleteStringCollection();
            var lst = db.tblCaris.Select(x => x.CariAdi).Distinct();
            foreach (var cari in lst)
            {
                veri.Add(cari);
                txtCari.Items.Add(cari);
            }
            txtCari.AutoCompleteCustomSource = veri;

            txtOdeme.DataSource = db.bOdemeTurleris.ToList();
            txtOdeme.ValueMember = "Id";
            txtOdeme.DisplayMember = "OdemeTipi";

            var srg = db.tblUrunlers.Select(x => x.UrunKodu);
            foreach (var k in srg)
            {
                txtUkod.Items.Add(k);
            }
            int dgv;
            dgv = txtUkod.Items.Count;
            MyArray = new string[dgv];

            for (int i = 0; i < dgv; i++)
            {
                MyArray[i] = txtUkod.Items[i].ToString();
            }

        }

        private void Liste_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox txt = e.Control as TextBox;
            if (Liste.CurrentCell.ColumnIndex == 0 && txt != null)
            {
                txt.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txt.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txt.AutoCompleteCustomSource.AddRange(MyArray);
            }
            else if (Liste.CurrentCell.ColumnIndex != 0 && txt != null)
            {
                txt.AutoCompleteMode = AutoCompleteMode.None;
            }
        }

        private void Liste_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                string a = Liste.CurrentRow.Cells[0].Value.ToString();
                var lst = (from s in db.tblUrunlers
                           where s.UrunKodu == a
                           select s).First();
                Liste.CurrentRow.Cells[1].Value = lst.UrunAciklama;
                Liste.CurrentRow.Cells[2].Value = lst.bBirim.BirimAdi;
            }
            if (e.ColumnIndex == 4)
            {
                if (Liste.CurrentRow.Cells[3].Value != null)
                {
                    Rhesapla();
                }
            }
            if (e.ColumnIndex == 3)
            {
                if (Liste.CurrentRow.Cells[4].Value != null)
                {
                    Rhesapla();
                }
            }
        }

        private void Rhesapla()
        {
            
            if (Liste.CurrentRow.Cells[4].Value != null && Liste.CurrentRow.Cells[5].Value != null)
            {
                decimal a, b;
                a = Convert.ToDecimal(Liste.CurrentRow.Cells[4].Value.ToString());
                b = Convert.ToDecimal(Liste.CurrentRow.Cells[5].Value.ToString());

                Liste.CurrentRow.Cells[6].Value = a * b * 0.18M;// 0.18M buradaki M 

                decimal aratoplam = 0;
                decimal kdvtoplam = 0;
                
                for (int i = 0; i < Liste.RowCount; i++)
                {
                    aratoplam += Convert.ToDecimal(Liste.Rows[i].Cells[4].Value) * Convert.ToInt32(Liste.Rows[i].Cells[5].Value);
                    kdvtoplam += Convert.ToDecimal(Liste.Rows[i].Cells[6].Value);
                }
                txtKdvToplam.Text = kdvtoplam.ToString();
                txtAraToplam.Text = aratoplam.ToString();
                txtGenelToplam.Text = (aratoplam + kdvtoplam).ToString();

            }
            else
            {
                MessageBox.Show("Lütfen bir değer giriniz.");
                Liste.CurrentRow.Cells[6].Value = "";
            }
        }
        void YeniKaydet()
        {

            var srch = new tblUrunSatisUst();
            srch.SatisGrupNo = txtSatisGrupNo.Text;
            srch.AraToplam = Convert.ToDecimal(txtAraToplam.Text);
            srch.STarih = Convert.ToDateTime(txtATarih.Text);
            srch.CariId = db.tblCaris.First(x => x.CariAdi == txtCari.Text).Id;
            srch.Vade = Convert.ToInt32(txtVade.Text);
            srch.OdemeId = db.bOdemeTurleris.First(x => x.OdemeTipi == txtOdeme.Text).Id;
            srch.KdvToplam = Convert.ToDecimal(txtKdvToplam.Text);
            srch.GenelToplam = Convert.ToDecimal(txtGenelToplam.Text);
            srch.Durum = false;

            db.tblUrunSatisUsts.Add(srch);
            db.SaveChanges();

            Liste.AllowUserToAddRows = false;
            tblUrunSatisAlt[] ualt = new tblUrunSatisAlt[Liste.RowCount];
            for (int i = 0; i < Liste.RowCount; i++)
            {
                ualt[i] = new tblUrunSatisAlt();
                ualt[i].Miktar = Convert.ToInt32(Liste.Rows[i].Cells[5].Value.ToString());
                ualt[i].SatisGrupNo = txtSatisGrupNo.Text;
                ualt[i].SFiyat = Convert.ToInt32(Liste.Rows[i].Cells[3].Value.ToString());
                ualt[i].BFiyat = Convert.ToDecimal(Liste.Rows[i].Cells[4].Value.ToString());
                string brm = Liste.Rows[i].Cells[2].Value.ToString();
                ualt[i].BirimId = db.bBirims.First(x => x.BirimAdi == brm).Id;
                string urn = Liste.Rows[i].Cells[1].Value.ToString();
                ualt[i].UrunId = db.tblUrunlers.First(x => x.UrunAciklama == urn).Id;
                ualt[i].AToplam = Convert.ToDecimal(Liste.Rows[i].Cells[4].Value) * Convert.ToInt32(Liste.Rows[i].Cells[5].Value);
                ualt[i].Kdv = Convert.ToDecimal(Liste.Rows[i].Cells[6].Value);

                db.tblUrunSatisAlts.Add(ualt[i]);

                string uBarkod = Liste.Rows[i].Cells[0].Value.ToString() + "/" + Liste.Rows[i].Cells[1].Value.ToString();
                var sKontrol = db.tblStokDurums.First(x => x.Barkod == uBarkod);
                sKontrol.Ambar += 0;
                sKontrol.Depo -= Convert.ToInt32(Liste.Rows[i].Cells[5].Value.ToString());
                sKontrol.Raf -= Convert.ToInt32(Liste.Rows[i].Cells[5].Value.ToString());
                ;
            }
            db.SaveChanges();
            MessageBox.Show("Başarıyla kaydedildi.");
        }
        void Guncelle()
        {
            var srch = db.tblUrunSatisUsts.First(x => x.SatisGrupNo == txtSatisGrupNo.Text);
            srch.SatisGrupNo = txtSatisGrupNo.Text;
            srch.AraToplam = Convert.ToDecimal(txtAraToplam.Text);
            srch.STarih = Convert.ToDateTime(txtATarih.Text);
            srch.CariId = db.tblCaris.First(x => x.CariAdi == txtCari.Text).Id;
            srch.Vade = Convert.ToInt32(txtVade.Text);
            srch.OdemeId = db.bOdemeTurleris.First(x => x.OdemeTipi == txtOdeme.Text).Id;
            srch.KdvToplam = Convert.ToDecimal(txtKdvToplam.Text);
            srch.GenelToplam = Convert.ToDecimal(txtGenelToplam.Text);
            srch.Durum = false;
            db.SaveChanges();

            Liste.AllowUserToAddRows = false;

            tblUrunSatisAlt[] ualt = new tblUrunSatisAlt[Liste.RowCount];
            for (int i = 0; i < Liste.RowCount; i++)
            {
                var altId = Convert.ToInt32(Liste.Rows[i].Cells[7].Value);
                ualt[i] = db.tblUrunSatisAlts.First(x => x.SatisGrupNo == txtSatisGrupNo.Text && x.Id == altId);
                ualt[i].Miktar = Convert.ToInt32(Liste.Rows[i].Cells[5].Value.ToString());
                ualt[i].SatisGrupNo = txtSatisGrupNo.Text;
                ualt[i].BFiyat = Convert.ToDecimal(Liste.Rows[i].Cells[4].Value.ToString());
                ualt[i].SFiyat = Convert.ToDecimal(Liste.Rows[i].Cells[3].Value.ToString());
                string brm = Liste.Rows[i].Cells[2].Value.ToString();
                ualt[i].BirimId = db.bBirims.First(x => x.BirimAdi == brm).Id;
                string urn = Liste.Rows[i].Cells[1].Value.ToString();
                ualt[i].UrunId = db.tblUrunlers.First(x => x.UrunAciklama == urn).Id;
                ualt[i].AToplam = Convert.ToDecimal(Liste.Rows[i].Cells[4].Value) * Convert.ToInt32(Liste.Rows[i].Cells[5].Value);
                ualt[i].Kdv = Convert.ToDecimal(Liste.Rows[i].Cells[6].Value);
            }
            db.SaveChanges();
            MessageBox.Show("Başarıyla güncellendi.");
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (edit == true && UrnSatisId > 0) Guncelle();
            else if (!edit) YeniKaydet();
        }
        protected override void OnLoad(EventArgs e)
        {
            //Buraya öyle bir kod yazacağım ki değiştirmek istediğim kodun üzerime yazılarak onu manipüle etmiş olacağım.
            var btnUrunSatisNo = new Button();
            btnUrunSatisNo.Size = new Size(25, txtSatisGrupNo.ClientSize.Height + 2);
            btnUrunSatisNo.Location = new Point(btnUrunSatisNo.ClientSize.Width - btnUrunSatisNo.Width, -1);
            btnUrunSatisNo.Cursor = Cursors.Default;
            btnUrunSatisNo.Image = Properties.Resources.arroww;
            btnUrunSatisNo.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
            txtSatisGrupNo.Controls.Add(btnUrunSatisNo);


            base.OnLoad(e);

            btnUrunSatisNo.Click += btnUrunSatisNo_Click;
        }
        Formlar F = new Formlar();
        private void btnUrunSatisNo_Click(object sender, EventArgs e)
        {
            int id = F.UrunSatisNo(true);
            if (id > 0)
            {
                Ac(id);
            }
            frmAnaSayfa.AktarmaInt = -1;
        }
        private void Ac(int id)
        {
            edit = true;
            UrnSatisId = id;
            string ustNo = id.ToString().PadLeft(7, '0');
            tblUrunSatisUst ust = db.tblUrunSatisUsts.First(x => x.SatisGrupNo == ustNo);
            txtSatisGrupNo.Text = ust.SatisGrupNo;
            txtAraToplam.Text = ust.AraToplam.ToString();
            txtATarih.Text = ust.STarih.ToString();
            txtCari.Text = ust.tblCari.CariAdi;
            txtGenelToplam.Text = ust.GenelToplam.ToString();
            txtKdvToplam.Text = ust.KdvToplam.ToString();
            txtOdeme.Text = ust.bOdemeTurleri.OdemeTipi;
            txtVade.Text = ust.Vade.ToString();

            Liste.Rows.Clear();
            Liste.AllowUserToAddRows = false;
            int i = 0;
            var alt = (from s in db.tblUrunSatisAlts where s.SatisGrupNo == ustNo select s).ToList();
            foreach (var k in alt)
            {
                Liste.Rows.Add();
                Liste.Rows[i].Cells[0].Value = k.tblUrunler.UrunKodu;
                Liste.Rows[i].Cells[1].Value = k.tblUrunler.UrunAciklama;
                Liste.Rows[i].Cells[2].Value = k.bBirim.BirimAdi;
                Liste.Rows[i].Cells[3].Value = k.SFiyat;
                Liste.Rows[i].Cells[4].Value = k.BFiyat;
                Liste.Rows[i].Cells[5].Value = k.Miktar;
                Liste.Rows[i].Cells[6].Value = k.Kdv;
                Liste.Rows[i].Cells[7].Value = k.Id;
                i++;
            }
        }

    }
}
