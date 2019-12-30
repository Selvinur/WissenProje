using MixErp302.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MixErp302.User
{
    public partial class frmKulGiris : Form
    {
        MixErpDbEntities db = new MixErpDbEntities();
        int secimId = -1;
        bool edit = false;
        public frmKulGiris()
        {
            InitializeComponent();
        }

        private void frmKulGiris_Load(object sender, EventArgs e)
        {
            Listele();
            txtRole.SelectedIndex = 0;
        }

        private void Listele()
        {
            Liste.Rows.Clear();
            int i = 0;
            var srg = (from s in db.tblUsers
                       where s.KulAdi.Contains(txtBul.Text)
                       select s).ToList();
            foreach (var k in srg)
            {
                Liste.Rows.Add();
                Liste.Rows[i].Cells[0].Value = k.Id;
                Liste.Rows[i].Cells[1].Value = k.KulAdi;
                Liste.Rows[i].Cells[2].Value = k.Sifre;
                Liste.Rows[i].Cells[3].Value = k.Role;
                i++;
            }
            Liste.AllowUserToAddRows = false;
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
            txtKulAdi.Text = "";
            txtSifre.Text = "";
            txtRole.SelectedIndex = 0;
        }
        private void Guncelle()
        {
            tblUser usr = db.tblUsers.Find(secimId);
             usr.KulAdi = txtKulAdi.Text;
             usr.Sifre = txtSifre.Text;
            if (txtRole.Text == "0 Belirtilmemiş")
                usr.Role = 0;
            else if (txtRole.Text == "1 Admin")
                usr.Role = 1;
            else if (txtRole.Text == "2 Standart")
                usr.Role = 2;
            else if (txtRole.Text == "3 Misafir")
                usr.Role = 3;

            db.SaveChanges();
            MessageBox.Show("Güncelleme Başarılı");
            Listele();
            Temizle();
        }
        private void YeniKaydet()
        {
            tblUser usr = new tblUser();
            usr.KulAdi = txtKulAdi.Text;
            usr.Sifre = txtSifre.Text;
            if (txtRole.Text == "0 Belirtilmemiş")
                usr.Role = 0;
            else if (txtRole.Text == "1 Admin")
                usr.Role = 1;
            else if (txtRole.Text == "2 Standart")
                usr.Role = 2;
            else if (txtRole.Text == "3 Misafir")
                usr.Role = 3;

            db.tblUsers.Add(usr);
            db.SaveChanges();
            MessageBox.Show("Kayıt Başarılı");
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
            tblUser usr = db.tblUsers.Find(secimId);
            txtKulAdi.Text = usr.KulAdi;
            txtSifre.Text = usr.Sifre;
            for (int i = 0; i < txtRole.Items.Count; i++)
            {
                if(usr.Role == i)
                {
                    txtRole.SelectedIndex = i;
                }
            }
            //if (usr.Role == 0)
            //    txtRole.SelectedIndex = 0;               
            //else if (usr.Role == 1)
            //    txtRole.SelectedIndex = 1;
            //else if (usr.Role == 2)
            //    txtRole.SelectedIndex = 2;
            //else if (usr.Role == 3)
            //    txtRole.SelectedIndex = 3;
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
            db.tblUsers.Remove(db.tblUsers.Find(secimId));
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
