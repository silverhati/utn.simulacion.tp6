using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Simulacion_TP6
{
    public partial class Form1 : Form
    {
        private clSimu simulacion;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtCantS.KeyPress += new KeyPressEventHandler(onlyNumeric_KeyPress);
            txtCantSS.KeyPress += new KeyPressEventHandler(onlyNumeric_KeyPress);
            txtCantJ.KeyPress += new KeyPressEventHandler(onlyNumeric_KeyPress);
        }

        static public void onlyNumeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Permitir sólo datos numéricos
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar))
            {
                    e.Handled = true;
            }
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            simulacion = new clSimu(Convert.ToInt32(txtCantS.Text),
                                    Convert.ToInt32(txtCantSS.Text),
                                    Convert.ToInt32(txtCantJ.Text));

            simulacion.Iniciar();
        }
    }
}
