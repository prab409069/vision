using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MultiFaceRec
{
    public partial class splash : Form
    {
        public splash()
        {
            InitializeComponent();
        }
        Timer tmr;
        private void splash_Shown(object sender, EventArgs e)
        {
            tmr = new Timer();
            
            tmr.Interval = 2000;
          
            tmr.Start();
            tmr.Tick += tmr_Tick;
        }
        void tmr_Tick(object sender, EventArgs e)
        {
            
            tmr.Stop();
           
            FrmPrincipal frm = new FrmPrincipal();
            frm.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        
    }
}
