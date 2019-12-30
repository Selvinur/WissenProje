using MixErp302.Bilgi;
using MixErp302.Urun;
using MixErp302.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MixErp302.Fonksiyonlar
{
    public class Formlar
    {
        #region Bilgi Giriş
        public void PersGiris()
        {
            frmPersonelGiris pers = new frmPersonelGiris();
            pers.MdiParent = frmAnaSayfa.ActiveForm;
            pers.WindowState = FormWindowState.Maximized;
            pers.Show();
        }
        public void CariGiris()
        {
            frmCariGiris cari = new frmCariGiris();
            cari.MdiParent = frmAnaSayfa.ActiveForm;
            cari.WindowState = FormWindowState.Maximized;
            cari.Show();
        }
        public void UrunGiris()
        {
            frmUrun urn = new frmUrun();
            urn.MdiParent = frmAnaSayfa.ActiveForm;
            urn.WindowState = FormWindowState.Maximized;
            urn.Show();
        }
        #endregion
        #region Ürün İşlemleri
        public void UrunAlis()
        {
            frmUrunAlis ualis = new frmUrunAlis();
            ualis.MdiParent = frmAnaSayfa.ActiveForm;
            ualis.WindowState = FormWindowState.Maximized;
            ualis.Show();
        }
        public void UrunSatis()
        {
            frmUrunSatis usatis = new frmUrunSatis();
            usatis.MdiParent = frmAnaSayfa.ActiveForm;
            usatis.WindowState = FormWindowState.Maximized;
            usatis.Show();
        }
        public void KulGiris()
        {
            frmKulGiris kgir = new frmKulGiris();
            kgir.MdiParent = frmAnaSayfa.ActiveForm;
            kgir.WindowState = FormWindowState.Maximized;
            kgir.Show();
        }
        public int UrunAlisNo(bool secim = false)
        {
            var frm = new frmUrunAlisListe();
            if (secim)
            {
                frm.Secim = true;
                frm.ShowDialog();
            }
            else
            {
                frm.MdiParent = Form.ActiveForm;
                frm.WindowState = FormWindowState.Maximized;
                frm.Show();
            }
            return frmAnaSayfa.AktarmaInt;
        }
        public int UrunSatisNo(bool secim = false)
        {
            var frm = new frmUrunSatisListe();
            if (secim)
            {
                frm.Secim = true;
                frm.ShowDialog();
            }
            else
            {
                frm.MdiParent = Form.ActiveForm;
                frm.WindowState = FormWindowState.Maximized;
                frm.Show();
            }
            return frmAnaSayfa.AktarmaInt;
        }
        #endregion

    }
}

