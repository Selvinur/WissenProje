﻿using MixErp302.Data;
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
    public partial class frmUrunSatisListe : Form
    {
        MixErpDbEntities db = new MixErpDbEntities();
        int secimId = -1;
        public bool Secim = false;
        public frmUrunSatisListe()
        {
            InitializeComponent();
        }

        private void frmUrunSatisListe_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void Listele()
        {
            Liste.Rows.Clear();
            int i = 0;
            var srg = (from s in db.tblUrunSatisUsts select s).ToList();
            foreach (var k in srg)
            {
                Liste.Rows.Add();
                Liste.Rows[i].Cells[0].Value = k.SatisGrupNo;
                Liste.Rows[i].Cells[1].Value = k.tblCari.CariAdi;
                Liste.Rows[i].Cells[2].Value = k.STarih;
                i++;

            }
            Liste.AllowUserToAddRows = false;
        }

        private void btnBul_Click(object sender, EventArgs e)
        {
            Listele();
        }


        private void btnKapat_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Liste_DoubleClick(object sender, EventArgs e)
        {
            Sec();
            if (Secim == true && secimId > 0)
            {
                frmAnaSayfa.AktarmaInt = secimId;
                Close();
            }
        }
        private void Sec()
        {
            try
            {
                secimId = Convert.ToInt32(Liste.CurrentRow.Cells[0].Value);
            }
            catch (Exception)
            {
                secimId = -1;
            }
        }
    }
}
