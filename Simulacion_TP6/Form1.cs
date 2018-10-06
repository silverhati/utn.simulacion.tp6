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
        private int cantS, cantSS, cantJ;

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
            //Leer valores de las variables de control
            if (txtCantS.Text == "") { cantS = 0; }
            else { cantS = Convert.ToInt32(txtCantS.Text); }

            if (txtCantSS.Text == "") { cantSS = 0; }
            else { cantSS = Convert.ToInt32(txtCantSS.Text); }

            if (txtCantJ.Text == "") { cantJ = 0; }
            else { cantJ = Convert.ToInt32(txtCantJ.Text); }

            //Correr la simulación
            simulacion = new clSimu(cantS, cantSS, cantJ);
            clSimuResultados resultados = simulacion.Iniciar();

            //Mostrar resultados
            string msg = "Cantidad Sr: " + cantS.ToString() + " -  Cantidad Ssr:" + cantSS.ToString() + " Cantidad Jr: " + cantJ.ToString() + Environment.NewLine +
                         "TMEA:" + resultados.TMEA + " // TMEN:" + resultados.TMEN + " // TMEB: " + resultados.TMEB + Environment.NewLine +
                         "TMAA:" + resultados.TMAA + " // TMAN:" + resultados.TMAN + " // TMAB: " + resultados.TMAB + Environment.NewLine +
                         "PTOS:" + resultados.PTOS + " // PTOSS:" + resultados.PTOSS + " // PTOJ: " + resultados.PTOJ + " // PTOGral: " + resultados.PtoGral;
            txtResultados.Text = msg; // MessageBox.Show(msg);
        }
    }
}
