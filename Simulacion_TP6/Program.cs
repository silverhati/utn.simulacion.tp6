using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Simulacion_TP6
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            //clSimu simu = new clSimu(0,0,0);
            //simu.SumarTiempos(new DateTime(2018,10,5,15,26,22),180);
        }
    }
}
