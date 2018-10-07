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

        private void txtCantS_TextChanged(object sender, EventArgs e)
        {
            this.SetButton();
        }
        private void SetButton()
        {
            btnIniciar.Enabled = (txtCantS.Text != "") && (txtCantSS.Text != "") && (txtCantJ.Text != "");
        }

        private void txtCantSS_TextChanged(object sender, EventArgs e)
        {
            this.SetButton();
        }

        private void txtCantJ_TextChanged(object sender, EventArgs e)
        {
            this.SetButton();
        }

        private void btnBuscarMejor_Click(object sender, EventArgs e)
        {
            txtResultados.Clear();
            txtResultados.Enabled = false;
            txtCantJ.Enabled = false;
            txtCantSS.Enabled = false;
            txtCantS.Enabled = false;
            btnIniciar.Enabled = false;
            btnBuscarMejor.Text = "Buscando...";
            btnBuscarMejor.Enabled = false;

            List<Tuple<Int32, Int32, Int32>> configuraciones = this.obtengoPosiblesConfiguraciones();

            foreach (Tuple<Int32, Int32, Int32> configuracion in configuraciones)
            {
                //Correr la simulación
                simulacion = new clSimu(configuracion.Item1, configuracion.Item2, configuracion.Item3);
                clSimuResultados resultados = simulacion.Iniciar();
            }

            txtResultados.Enabled = true;
            //Mostrar resultados
            string msg = "Finalizo la busqueda, buscar el archivo C:\\Resultados.CSV";
            txtResultados.Text = msg; // MessageBox.Show(msg);

            txtCantJ.Enabled = true;
            txtCantSS.Enabled = true;
            txtCantS.Enabled = true;
            this.SetButton();
            btnBuscarMejor.Enabled = true;
            btnBuscarMejor.Text = "Buscar mejor";
        }

        //Basicamente obtengo todas las posible configuraciones entre 6 y 12 empleados, con al menos uno de ellos. 210 configuraciones
        private List<Tuple<Int32, Int32, Int32>> obtengoPosiblesConfiguraciones()
        {
            List<Tuple<Int32, Int32, Int32>> results = new List<Tuple<Int32, Int32, Int32>>();

            List<Int32> seniors = new List<Int32> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            List<Int32> semiSeniors = new List<Int32> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            List<Int32> juniors = new List<Int32> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            foreach (Int32 s in seniors)
            {
                foreach (Int32 ss in semiSeniors)
                {
                    foreach (Int32 j in juniors)
                    {
                        if (s + ss + j == 6 || s + ss + j == 7 || s + ss + j == 8 || s + ss + j == 9 || s + ss + j == 10 || s + ss + j == 11 || s + ss + j == 12)
                        {
                            results.Add(new Tuple<int, int, int>(s, ss, j));
                        }
                    }
                }
            }

            return results;
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
            txtResultados.Clear();
            txtResultados.Enabled = false;
            btnIniciar.Text = "Calculando...";
            btnIniciar.Enabled = false;

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

            txtResultados.Enabled = true;
            //Mostrar resultados
            string msg = "Cantidad Sr: " + cantS.ToString() + " -  Cantidad Ssr:" + cantSS.ToString() + " Cantidad Jr: " + cantJ.ToString() + Environment.NewLine +
                         "TMEA:" + resultados.TMEA + " // TMEN:" + resultados.TMEN + " // TMEB: " + resultados.TMEB + Environment.NewLine +
                         "TMAA:" + resultados.TMAA + " // TMAN:" + resultados.TMAN + " // TMAB: " + resultados.TMAB + Environment.NewLine +
                         "PTOS:" + resultados.PTOS + " // PTOSS:" + resultados.PTOSS + " // PTOJ: " + resultados.PTOJ + " // PTOGral: " + resultados.PtoGral;
            txtResultados.Text = msg; // MessageBox.Show(msg);

            btnIniciar.Enabled = true;
            btnIniciar.Text = "Iniciar Simulación";
        }
    }
}
